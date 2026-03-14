const generateCssTask = require("../../../gulp-core/gulptaskCore").generateCssTask;

module.exports = function() {
    // Original location : .\
    let cssFiles = [
        "content/style/base/print.css",
        "../../node_modules/fullcalendar/dist/fullcalendar.print.css",
    ];

    generateCssTask(__filename, cssFiles);
};
