const generateJsTask = require("../../../gulp-core/gulptaskCore").generateJsTask;

module.exports = function() {
    // Original location : .\
    let jsFiles = [
        "Content/js/jaydata/jaydata.js",
        "Content/js/jaydata/jaydata-compatibility.js",
        "Content/js/jaydata/jaydatamodules/knockout.js",
        "Content/js/jaydata/jaydatamodules/knockout.custom.js",
        "Content/js/jaydata/jaydatamodules/QueryCache.js",
        "Content/js/jaydata/jaydataproviders/IndexedDbProProvider.js",
        "Content/js/jaydata/jaydataproviders/SqLiteProProvider.js",
        "Content/js/jaydata/jaydataproviders/InMemoryProvider.js",
        "Content/js/jaydata/jaydataproviders/oDataProvider.js",
    ];

    generateJsTask(__filename, jsFiles);

};
