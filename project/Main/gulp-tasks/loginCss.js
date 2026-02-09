const generateCssTask = require("../../../gulp-core/gulptaskCore").generateCssTask;

module.exports = function() {
    // Original location : .\
    let cssFiles = [
        "../../node_modules/animate.css/animate.css",
        "../../node_modules/material-design-iconic-font/dist/css/material-design-iconic-font.css",
        "content/style/material/app.min.1.css",
        "content/style/material/app.min.2.css",
        "content/style/material/app.custom.css"
    ];

    generateCssTask(__filename, cssFiles);

};
