const generateCssTask = require("../../../gulp-core/gulptaskCore").generateCssTask;

module.exports = function() {
	// Original location : .\
	let cssFiles = [
		"Content/style/components/knockout.component.barcodeFormat.css",
	];

	generateCssTask(__filename, cssFiles);
};
