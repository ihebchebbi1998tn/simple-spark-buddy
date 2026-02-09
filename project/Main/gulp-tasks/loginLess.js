const generateCssTask = require("../../../gulp-core/gulptaskCore").generateCssTask;

module.exports = function() {
    // Original location : .\
    let cssFiles = [
        "content/style/base/variables.less",
        "content/style/base/login.less"
    ];

    generateCssTask(__filename, cssFiles);

};
