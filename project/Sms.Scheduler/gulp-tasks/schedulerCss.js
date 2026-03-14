const generateCssTask = require("../../../gulp-core/gulptaskCore").generateCssTask;

module.exports = function() {
	const cssFiles = [
		"Content/css/style.less"
	];

	generateCssTask(__filename, cssFiles);

};
