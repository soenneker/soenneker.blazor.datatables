export class DataTablesInterop {
    constructor() {
        this.datatables = {};
        this.options = {};
    }

    create(element, elementId, options, dotNetCallback) {
        let _datatable;

        if (options) {
            const opt = JSON.parse(options);
            opt.initComplete = (settings, json) => dotNetCallback.invokeMethodAsync("OnInitializedJs");

            _datatable = new DataTable('#' + elementId, options);

            this.options[elementId] = opt;
        } else {
            _datatable = new DataTable('#' + elementId, {
                initComplete: (settings, json) => dotNetCallback.invokeMethodAsync("OnInitializedJs")
            });
        }

        this.datatables[elementId] = _datatable;
    }

    destroy(element) {
        var elementId = element.id;
        const dataTable = this.datatables[elementId];

        if (dataTable) {
            dataTable.destroy();
            delete this.datatables[elementId];

            var tableElement = document.getElementById(elementId);

            if (tableElement)
                tableElement.remove();
        }
    }

    addEventListener(elementId, eventName, dotNetCallback) {
        const dataTable = this.datatables[elementId];

        dataTable.on(eventName, (...args) => {
            if (eventName === "item_select") {
                return dotNetCallback.invokeMethodAsync("Invoke", args[0].textContent);
            } else {
                const json = this.getJsonFromArguments(...args);
                return dotNetCallback.invokeMethodAsync("Invoke", json);
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

        const json = JSON.stringify(processedArgs);
        return json;
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