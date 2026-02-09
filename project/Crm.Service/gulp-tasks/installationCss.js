const generateCssTask = require("../../../gulp-core/gulptaskCore").generateCssTask;

module.exports = function() {
	// Original location : .\Plugins\Crm.Service
	const cssFiles = [
		"Content/css/Installation.less"
	];

	generateCssTask(__filename, cssFiles);

};
