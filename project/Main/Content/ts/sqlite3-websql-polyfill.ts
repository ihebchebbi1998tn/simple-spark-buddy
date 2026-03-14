function SQLiteDatabase(name) {
    this.handlers = {}; // map id -> function
    this.lastCallbackId = 0;
    this.name = name;
    this.worker = new Worker(window.Helper.Url.resolveUrl("~/Plugins/Main/Content/js/sqlite-wasm/sqlite3-websql-polyfill-worker.js"), {name: this.name});
    this.worker.onmessage = ({data}) => {
        if ("callback" in data) {
            this.handlers[data.callback](data.err, data.results);
            delete this.handlers[data.callback];
            return;
        }
        if (data.type === "log") {
            window.Log[data.level](data.message);
            // @ts-ignore
            if (data.level === "error" && window.database && !window.database._isOK) {
                // @ts-ignore
                (window.database.onReadyFunction || []).forEach(onReadyFunction => onReadyFunction.error(data.message));
            }
            return;
        }
        throw new Error("Unhandled message: " + JSON.stringify(data));
    };
}

SQLiteDatabase.prototype.exec = function exec(queries, readOnly, callback) {
    const callbackId = this.lastCallbackId++;
    this.handlers[callbackId] = callback;
    const mappedQueries = queries.map((query) => {
        return {sql: query.sql, args: query.args || []}
    });
    this.worker.postMessage({
        queries: mappedQueries,
        readOnly: readOnly,
        callback: callbackId
    });
};

const customOpenDatabase = require("websql/custom");
const polyfillOpenDatabase = customOpenDatabase(SQLiteDatabase);
const nativeOpenDatabase = window.openDatabase;
const opfsSupported = !!navigator?.storage?.getDirectory;
const databases = {};
window.openDatabase = (name, version, displayName, estimatedSize, creationCallback) => {
    migrateToWebSqlPolyfill();
    const useWebSqlPolyfill = opfsSupported && (!nativeOpenDatabase || window.Helper.Database.getFromLocalStorage("useWebSqlPolyfill") === "true");
    if (useWebSqlPolyfill) {
        databases[name.toString()] ||=  polyfillOpenDatabase(name, version, displayName, estimatedSize, creationCallback);
        return databases[name.toString()];
    } else {
        return nativeOpenDatabase(name, version, displayName, estimatedSize, creationCallback);
    }
};

function migrateToWebSqlPolyfill(): boolean {
    const migrateToPolyfill = opfsSupported && !!nativeOpenDatabase && window.Helper.Database.getStorageOptions().provider === "local" && window.Helper.Database.getFromLocalStorage("useWebSqlPolyfill") !== "true" && JSON.parse(window.Helper.Database.getFromLocalStorage("transientItems") || "[]").length === 0;
    if (migrateToPolyfill) {
        window.Helper.Database.getLocalStorageKeys().filter(x => x.endsWith("_clientId")).forEach(x => window.Helper.Database.removeFromLocalStorage(x));
        window.Helper.Database.saveToLocalStorage("useWebSqlPolyfill", "true");
        return true;
    }
    return false;
}

$(document).one("Initialized", function () {
    if (migrateToWebSqlPolyfill()) {
        window.location.reload();
    }
});