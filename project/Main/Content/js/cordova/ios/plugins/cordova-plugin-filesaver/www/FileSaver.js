cordova.define("cordova-plugin-filesaver.FileSaver", function (require, exports, module) {
	var exec = require("cordova/exec");
	exports.save = function (path) {
		exec(function() {}, function(e) {
			throw e;
		}, "FileSaver", "save", [
			path
		]);
	};
});