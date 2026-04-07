const datatables = {};
const datatableOptions = {};
const elementObservers = {};

function normalizeNulls(obj) {
    function walk(value) {
        if (value === "null" || value === "__NULL__") return null;

        if (Array.isArray(value)) return value.map(walk);

        if (value !== null && typeof value === "object") {
            for (const key in value) {
                value[key] = walk(value[key]);
            }
        }

        return value;
    }

    return walk(obj);
}

export function create(element, elementId, options, dotNetCallback) {
    let _datatable;

    if (options) {
        const opt = normalizeNulls(JSON.parse(options));
        opt.initComplete = async () => {
            await dotNetCallback.invokeMethodAsync("OnInitializedJs");
        };

        if (opt.serverSide) {
            if (opt.processing !== false) {
                opt.processing = true;
            }
            opt.searching = true;
            opt.paging = true;
            opt.info = true;
            opt.deferRender = true;

            const processingElement = document.getElementById(elementId + '-processing');
            if (processingElement) {
                const hasContent = processingElement.children.length > 0 || processingElement.textContent.trim().length > 0;

                if (hasContent) {
                    if (opt.processing === false) {
                        opt.hasCustomProcessingIndicator = true;
                    } else {
                        opt.processing = false;
                        opt.hasCustomProcessingIndicator = true;
                    }
                } else {
                    opt.processing = false;
                    opt.hasCustomProcessingIndicator = false;
                }
            }

            opt.ajax = async function(data, callback, settings) {
                try {
                    if (opt.hasCustomProcessingIndicator) {
                        const processingElement = document.getElementById(elementId + '-processing');
                        if (processingElement) {
                            processingElement.style.display = 'block';
                        }
                    }

                    const result = await dotNetCallback.invokeMethodAsync("OnServerSideRequestJs", JSON.stringify(data));

                    if (opt.hasCustomProcessingIndicator) {
                        const processingElement = document.getElementById(elementId + '-processing');
                        if (processingElement) {
                            processingElement.style.display = 'none';
                        }
                    }

                    const { data: tableData, totalRecords, totalFilteredRecords, continuationToken } = JSON.parse(result);

                    callback({
                        draw: data.draw,
                        recordsTotal: totalRecords,
                        recordsFiltered: totalFilteredRecords,
                        data: tableData,
                        continuationToken: continuationToken
                    });
                } catch (error) {
                    if (opt.hasCustomProcessingIndicator) {
                        const processingElement = document.getElementById(elementId + '-processing');
                        if (processingElement) {
                            processingElement.style.display = 'none';
                        }
                    }

                    console.error('Error in server-side processing:', error);
                    callback({
                        draw: data.draw,
                        recordsTotal: 0,
                        recordsFiltered: 0,
                        data: []
                    });
                }
            };

            $.fn.dataTable.ext.pager.numbers_length = 10;
        }

        _datatable = new DataTable('#' + elementId, opt);
        datatableOptions[elementId] = opt;
    } else {
        _datatable = new DataTable('#' + elementId, {
            initComplete: async () => {
                await dotNetCallback.invokeMethodAsync("OnInitializedJs");
            }
        });
    }

    datatables[elementId] = _datatable;
}

export function destroy(element) {
    const elementId = element.id;

    const dataTable = datatables[elementId];
    if (dataTable) {
        dataTable.destroy();
        delete datatables[elementId];
    }

    if (elementObservers[elementId]) {
        elementObservers[elementId].disconnect();
        delete elementObservers[elementId];
    }
}

export async function addEventListener(elementId, eventName, dotNetCallback) {
    const dataTable = datatables[elementId];

    dataTable.on(eventName, async (...args) => {
        try {
            if (eventName === "item_select") {
                await dotNetCallback.invokeMethodAsync("Invoke", args[0].textContent);
            } else {
                const processedArgs = args.map(arg => {
                    if (typeof arg === 'object' && arg !== null) {
                        if (arg instanceof DataTable.Api) {
                            return {
                                page: arg.page.info().page + 1,
                                length: arg.page.info().length,
                                recordsTotal: arg.page.info().recordsTotal,
                                recordsDisplay: arg.page.info().recordsDisplay
                            };
                        }
                        const result = {};
                        for (const key in arg) {
                            if (typeof arg[key] !== 'object' || arg[key] === null) {
                                result[key] = arg[key];
                            }
                        }
                        return result;
                    }
                    return arg;
                });
                await dotNetCallback.invokeMethodAsync("Invoke", JSON.stringify(processedArgs));
            }
        } catch (error) {
            console.error(`Error handling ${eventName} event:`, error);
        }
    });
}

export function refresh(elementId) {
    const dt = datatables[elementId];
    if (dt) {
        dt.clear();
        dt.rows.add($(dt.table().body()).children());
        dt.draw();
    }
}

export function createObserver(element) {
    const elementId = element.id;

    if (elementObservers[elementId]) {
        elementObservers[elementId].disconnect();
        delete elementObservers[elementId];
    }

    const observer = new MutationObserver((mutations) => {
        const targetRemoved = mutations.some(mutation =>
            Array.from(mutation.removedNodes).includes(element)
        );

        if (targetRemoved) {
            destroy(element);
        }
    });

    observer.observe(element.parentNode, { childList: true });

    elementObservers[elementId] = observer;
}
