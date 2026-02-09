const generateJsTask = require("../../../gulp-core/gulptaskCore").generateJsTask;

module.exports = function () {
  // Original location : .\
  let jsFiles = [
    "../../node_modules/cldrjs/dist/cldr.js",
    "../../node_modules/cldrjs/dist/cldr/event.js",
    "../../node_modules/globalize/dist/globalize.js",
    "../../node_modules/jquery-chained/jquery.chained.js",
    "../../node_modules/jquery-blockui/jquery.blockUI.js",
  ];

  generateJsTask(__filename, jsFiles);
};