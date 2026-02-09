cordova.define("cordova-plugin-printtopdf.PrintToPdf", function (require, exports, module) {
	var exec = require("cordova/exec");
	var defaultOptions = {
		filePath: "~/Documents/PDF.pdf",
		headerAlign: "left",
		headerFontName: "Helvetica",
		headerFontSize: 10,
		headerHeight: 0,
		headerInset: 20,
		headerMarginPadding: 5,
		headerStartIndex: 0,
		headerText: null,
		footerAlign: "right",
		footerFontName: "Helvetica",
		footerFontSize: 10,
		footerHeight: 20,
		footerInset: 20,
		footerMarginPadding: 5,
		footerStartIndex: 0,
		footerText: "%d / %d"
	};
	exports.print = function(printOptions) {
		var options = $.extend({}, defaultOptions, printOptions);
		exec(function() {}, function(e) {
			throw e;
		}, "PrintToPdf", "print", [
			options.filePath,
			options.headerAlign,
			options.headerFontName,
			options.headerFontSize,
			options.headerHeight,
			options.headerInset,
			options.headerMarginPadding,
			options.headerStartIndex,
			options.headerText,
			options.footerAlign,
			options.footerFontName,
			options.footerFontSize,
			options.footerHeight,
			options.footerInset,
			options.footerMarginPadding,
			options.footerStartIndex,
			options.footerText
		]);
	}
});