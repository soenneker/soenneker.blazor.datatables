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
                dotNetCallback.invokeMethodAsync("OnInitializedJs");
            };

            _datatable = new DataTable('#' + elementId, opt);
            this.options[elementId] = opt;
        } else {
            _datatable = new DataTable('#' + elementId, {
                initComplete: async () => {
                    dotNetCallback.invokeMethodAsync("OnInitializedJs");
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
            if (eventName === "item_select") {
                await dotNetCallback.invokeMethodAsync("Invoke", args[0].textContent);
            } else {
                const json = this.getJsonFromArguments(...args);
                await dotNetCallback.invokeMethodAsync("Invoke", json);
            }
        });
    }

    getJsonFromArguments(...args) {
        const processedArgs = args.map(arg =>
            typeof arg === 'object' && arg !== null ? this.objectToStringifyable(arg) : arg
        );
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
                objectJSON[prop] = typeof propValue === 'object' && propValue !== null
                    ? this.objectToStringifyable(propValue)
                    : propValue;
            }
        });

        return objectJSON;
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