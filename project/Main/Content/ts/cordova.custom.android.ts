;(function() {
	function showLoadingIndicator(): void {
		if (window.cordova && window.cordova.define.moduleMap["org.pbernasconi.progressindicator.ProgressIndicator"]) {
			const progressIndicator = window.cordova.require("org.pbernasconi.progressindicator.ProgressIndicator");
			if (!!progressIndicator && !!progressIndicator.show) {
				let loadingMessage = "";
				if (!!window.Helper && !!window.Helper.String && !!window.Helper.String.getTranslatedString) {
					loadingMessage = window.Helper.String.getTranslatedString("loading");
					if (loadingMessage.indexOf("Translation Missing") !== -1) {
						loadingMessage = "";
					}
				} else if ($("#loading-message").length === 1) {
					loadingMessage = $("#loading-message").text();
				}
				progressIndicator.show(loadingMessage);
			}
		}
	}

	function hideLoadingIndicator(): void {
		if (window.cordova && window.cordova.define.moduleMap["org.pbernasconi.progressindicator.ProgressIndicator"]) {
			const progressIndicator = window.cordova.require("org.pbernasconi.progressindicator.ProgressIndicator");
			if (!!progressIndicator && !!progressIndicator.hide) {
				progressIndicator.hide();
			}
		}
	}

	function setRestoreLastPageAfterStartup(restore: boolean): void {
		localStorage["restoreLastPage"] = restore;
	}

	function registerEventhandler(): void {
		document.addEventListener("resume", function() {
			window.Log.info("debug: resumed app at " + (new Date()).toString());
			hideLoadingIndicator();
		}, false);

		document.addEventListener("pause", function() {
			window.Log.debug("debug: paused app at " + (new Date()).toString());
			setRestoreLastPageAfterStartup(true);
		}, false);
	}

	window.onbeforeunload = showLoadingIndicator;

	const deviceready = new Promise<void>((resolve, reject) => {
		document.addEventListener("deviceready", function () {
			if (window.sqlitePlugin && navigator.userAgent.indexOf("Crosswalk") === -1) {
				window.openDatabase = function (name, version, displayName, estimatedSize, creationCallback) {
					return window.sqlitePlugin.openDatabase({
						name: name,
						location: "default"
					});
				};
			}
			resolve();
		});
	});
	if (window.Helper && window.Helper.Database) {
		const baseHelperDatabaseInitialize = window.Helper.Database.initialize;
		window.Helper.Database.initialize = function(): Promise<void> {
			const self = this;
			return deviceready.then(function() {
				return baseHelperDatabaseInitialize.call(self);
			});
		};
	}

	document.addEventListener("deviceready", function() {
		if (window.cordova && window.cordova.define.moduleMap["phonegap-plugin-barcodescanner.BarcodeScanner"]) {
			window.barcodeScanner = window.cordova.require("phonegap-plugin-barcodescanner.BarcodeScanner");
		}
		if (window.cordova && window.cordova.InAppBrowser && window.cordova.InAppBrowser.open) {
			window.open = window.cordova.InAppBrowser.open;
		}
		registerEventhandler();
		hideLoadingIndicator();

		if (window.cordova.plugins && window.cordova.plugins.printer) {
			window.print = function() {
				$("body").addClass("printing");
				window.cordova.plugins.printer.print(function(res) {
					$("body").removeClass("printing");
					if (res) {
							window.Log.debug("cordova.plugins.printer.print done");
					} else {
							window.Log.debug("cordova.plugins.printer.print canceled");
					}
				});
			};
		}

		if (window.FileRepository && !(navigator.storage && navigator.storage.estimate)) {
			window.FileRepository.prototype.getFreeDiskSpace = function () {
				return new Promise(function (resolve, reject) {
					window.cordova.exec(function(result) {
							resolve(result * 1000);
						},
						reject,
						"File",
						"getFreeDiskSpace");
				});
			};
		}
	}, false);

	if (!!window.Main && !!window.Main.ViewModels && !!window.Main.ViewModels.HomeStartupViewModel) {
		const baseStartupInit = window.Main.ViewModels.HomeStartupViewModel.prototype.init;
		const baseStartupDispose = window.Main.ViewModels.HomeStartupViewModel.prototype.dispose || function () {
		};
		window.Main.ViewModels.HomeStartupViewModel.prototype.init = function(id?: string, params?: {[key:string]:string}): Promise<void> {
			if (window.powermanagement && window.powermanagement.acquire) {
				window.Log.debug("acquiring wakelock");
				window.powermanagement.acquire();
			}
			return baseStartupInit.call(this, id, params);
		};
		window.Main.ViewModels.HomeStartupViewModel.prototype.dispose = function() {
			if (window.powermanagement && window.powermanagement.release) {
				window.Log.debug("boostrapping done, releasing wakelock");
				window.powermanagement.release();
			}
			return baseStartupDispose.call(this);
		};
	}
})();
// file transfer plugin integration
$(function() {
	let helperFile = {
		getFile: function (dirEntry, uri, filename) {
			const xhr = new window.XMLHttpRequest();
			xhr.open("GET", uri, true);
			xhr.responseType = "blob";
			xhr.onload = function () {
				if (this.status === 200) {
					const blob = new Blob([this.response], {type: this.response.type});
					helperFile.saveFile(dirEntry, blob, filename);
				}
			};
			xhr.send();
		},

		saveFile: function(dirEntry, fileData, fileName) {
			dirEntry.getFile(fileName, { create: true, exclusive: false }, function(fileEntry) {
				helperFile.writeFile(fileEntry, fileData);
			}, function(createFileError) {
				window.Log.error(createFileError);
			});
		},
	
		writeFile: function(fileEntry, dataObj) {
			fileEntry.createWriter(function(fileWriter) {
				fileWriter.onwriteend = function() {
					window.Log.debug("Successful file write...");
					helperFile.readBinaryFile(fileEntry);
				};
				fileWriter.onerror = function(fileWriterError) {
					window.Log.error("Failed file write: " + fileWriterError.toString());
				};
				fileWriter.write(dataObj);
			});
		},
	
		getMimeType: function(file) {
			return file.type;
		},
	
		readBinaryFile: function(fileEntry) {
			fileEntry.file(function(file) {
				const reader = new FileReader();
				reader.onloadend = function() {
					window.Log.debug("Successful file write: " + this.result);
					const fileType = helperFile.getMimeType(file) || file.type;
					const path = decodeURIComponent(fileEntry.nativeURL);
					window.cordova.plugins.fileOpener2.open(path, fileType, {
						error: function(fileOpenError) {
							window.Log.error(fileOpenError);
							window.cordova.plugins.fileOpener2.showOpenWithDialog(path, fileType,
							{
								error: window.Log.error,
								success: function() { window.Log.debug("file showOpenWithDialog success"); }
							});
						},
						success: function() { window.Log.debug("file open success"); }
					});
				};
				reader.readAsArrayBuffer(file);
			}, function(readFileError) { window.Log.error(readFileError); });
		}
	}
	
	$(document).on("click", "a[download]", function(e) {
		e.preventDefault();
		const href = $(this).attr("href");
		const filename = $(this).attr("download");
		let uri = encodeURI(href.indexOf("http") === 0 ? href : window.Helper.Url.qualifyURL(href));
		if (uri.indexOf("blob") !== 0) {
			uri += uri.indexOf("?") === -1 ? "?" : "&";
			uri += "filename=/" + filename;
		}
		// @ts-ignore
		window.resolveLocalFileSystemURL(window.cordova.file.externalCacheDirectory, function(dirEntry) { helperFile.getFile(dirEntry, uri, filename); }, function(resolveLocalFileSystemUrlError) { window.Log.error(resolveLocalFileSystemUrlError); });
	});

	$(document).on("click", "a[target='_system']", function () {
		window.open(this.href, "_system");
		return false;
	});

	$("#content").on("click touchend", "a[target='_blank']", function () {
		window.open(this.href, "_blank", "location=no,top=20,closebuttoncaption=" + window.Helper.String.getTranslatedString("Close") + ",enableViewportScale=yes");
		return false;
	});
});