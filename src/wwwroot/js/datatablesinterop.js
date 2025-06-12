export class DataTablesInterop {
    constructor() {
        this.datatables = {};
        this.options = {};
        this._elementObservers = {}; // Only for element removal
    }

    create(element, elementId, options, dotNetCallback) {
        let _datatable;

        if (options) {
            const opt = this.normalizeNulls(JSON.parse(options));
            opt.initComplete = async () => {
                await dotNetCallback.invokeMethodAsync("OnInitializedJs");
            };

            // Enable server-side processing
            if (opt.serverSide) {
                opt.processing = true;
                opt.searching = true;
                opt.paging = true;
                opt.info = true;
                opt.deferRender = true;
                
                // Centralize all server-side operations in the ajax function
                opt.ajax = async function(data, callback, settings) {
                    try {
                        // Extract parameters from the DataTables request
                        const pageNumber = Math.floor(data.start / data.length) + 1;
                        const pageSize = data.length;
                        const searchTerm = data.search?.value || '';
                        const orderColumn = data.order?.[0]?.column;
                        const orderDirection = data.order?.[0]?.dir || 'asc';
                        
                        // Call our Blazor method to get the data
                        const result = await dotNetCallback.invokeMethodAsync("OnServerSideRequestJs",
                            pageNumber,
                            pageSize,
                            searchTerm,
                            orderColumn,
                            orderDirection
                        );
                        
                        // Parse the result
                        const { data: tableData, totalRecords, totalFilteredRecords } = JSON.parse(result);
                        
                        // Return the data in DataTables format
                        callback({
                            draw: data.draw,
                            recordsTotal: totalRecords,
                            recordsFiltered: totalFilteredRecords,
                            data: tableData
                        });
                    } catch (error) {
                        console.error('Error in server-side processing:', error);
                        callback({
                            draw: data.draw,
                            recordsTotal: 0,
                            recordsFiltered: 0,
                            data: []
                        });
                    }
                };
                
                // Configure pagination display
                $.fn.dataTable.ext.pager.numbers_length = 10;
            }

            _datatable = new DataTable('#' + elementId, opt);
            this.options[elementId] = opt;
        } else {
            _datatable = new DataTable('#' + elementId, {
                initComplete: async () => {
                    await dotNetCallback.invokeMethodAsync("OnInitializedJs");
                }
            });
        }

        this.datatables[elementId] = _datatable;
    }

    destroy(element) {
        const elementId = element.id;

        const dataTable = this.datatables[elementId];
        if (dataTable) {
            dataTable.destroy();
            delete this.datatables[elementId];
        }

        if (this._elementObservers[elementId]) {
            this._elementObservers[elementId].disconnect();
            delete this._elementObservers[elementId];
        }
    }

    normalizeNulls(obj) {
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

    async addEventListener(elementId, eventName, dotNetCallback) {
        const dataTable = this.datatables[elementId];

        dataTable.on(eventName, async (...args) => {
            try {
                if (eventName === "item_select") {
                    await dotNetCallback.invokeMethodAsync("Invoke", args[0].textContent);
                } else {
                    // For other events, we know the structure of the arguments
                    // and can handle them directly without recursive serialization
                    const processedArgs = args.map(arg => {
                        if (typeof arg === 'object' && arg !== null) {
                            // For DataTables API objects, extract only the properties we need
                            if (arg instanceof DataTable.Api) {
                                return {
                                    page: arg.page.info().page + 1,
                                    length: arg.page.info().length,
                                    recordsTotal: arg.page.info().recordsTotal,
                                    recordsDisplay: arg.page.info().recordsDisplay
                                };
                            }
                            // For other objects, just get their direct properties
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

    refresh(elementId) {
        const dt = this.datatables[elementId];
        if (dt) {
            dt.clear();         // Clear internal DataTables data
            dt.rows.add($(dt.table().body()).children()); // Re-add rows from DOM
            dt.draw();
        }
    }

    createObserver(element) {
        const elementId = element.id;

        // Clean up existing observer if any
        if (this._elementObservers[elementId]) {
            this._elementObservers[elementId].disconnect();
            delete this._elementObservers[elementId];
        }

        const observer = new MutationObserver((mutations) => {
            const targetRemoved = mutations.some(mutation =>
                Array.from(mutation.removedNodes).includes(element)
            );

            if (targetRemoved) {
                this.destroy(element); // This clears the observer
            }
        });

        observer.observe(element.parentNode, { childList: true });

        this._elementObservers[elementId] = observer;
    }
}

window.DataTablesInterop = new DataTablesInterop();