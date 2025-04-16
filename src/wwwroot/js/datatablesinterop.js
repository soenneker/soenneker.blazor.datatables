export class DataTablesInterop {
    constructor() {
        this.datatables = {};
        this.options = {};
    }

    async create(element, elementId, options, dotNetCallback) {
        let _datatable;

        if (options) {
            const opt = JSON.parse(options);

            opt.initComplete = async (settings, json) => {
                await dotNetCallback.invokeMethodAsync("OnInitializedJs");
            };

            _datatable = new DataTable('#' + elementId, opt);

            this.options[elementId] = opt;
        } else {
            _datatable = new DataTable('#' + elementId, {
                initComplete: async (settings, json) => {
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

            const tableElement = document.getElementById(elementId);

            if (tableElement)
                tableElement.remove();
        }
    }

    async addEventListener(elementId, eventName, dotNetCallback) {
        const dataTable = this.datatables[elementId];

        dataTable.on(eventName, async (...args) => {
            if (eventName === "item_select") {
                await dotNetCallback.invokeMethodAsync("Invoke", args[0].textContent);
            } else {
                const json = this.getJsonFromArguments(...args);
                await dotNetCallback.invokeMethodAsync("Invoke", json);
            }
        });
    }

    getJsonFromArguments(...args) {
        const processedArgs = args.map(arg => {
            if (typeof arg === 'object' && arg !== null) {
                return this.objectToStringifyable(arg);
            } else {
                return arg;
            }
        });

        return JSON.stringify(processedArgs);
    }

    objectToStringifyable(obj) {
        let objectJSON = {};
        const props = Object.getOwnPropertyNames(obj);

        props.forEach(prop => {
            const descriptor = Object.getOwnPropertyDescriptor(obj, prop);
            if (descriptor && typeof descriptor.get === 'function') {
                objectJSON[prop] = descriptor.get.call(obj);
            } else {
                const propValue = obj[prop];
                if (typeof propValue === 'object' && propValue !== null) {
                    objectJSON[prop] = this.objectToStringifyable(propValue);
                } else {
                    objectJSON[prop] = propValue;
                }
            }
        });

        return objectJSON;
    }

    createObserver(element) {
        this.observer = new MutationObserver((mutations) => {
            const targetRemoved = mutations.some(mutation => Array.from(mutation.removedNodes).indexOf(element) !== -1);

            if (targetRemoved) {
                this.destroy(element);

                this.observer && this.observer.disconnect();
                delete this.observer;
            }
        });

        this.observer.observe(element.parentNode, { childList: true });
    }
}

window.DataTablesInterop = new DataTablesInterop();