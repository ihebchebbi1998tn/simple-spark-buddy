const generateCssTask = require("../../../gulp-core/gulptaskCore").generateCssTask;

module.exports = function() {
    // Original location : .\
    let cssFiles = [
        "Content/style/TemplateReport.less"
    ];

    generateCssTask(__filename, cssFiles);
};
