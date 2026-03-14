(function () {
	$(document)
		.on("focus", "input, textarea", function () {
    	$("body").addClass("fixfixed");
    })
    .on("blur", "input, textarea", function () {
    	$("body").removeClass("fixfixed");
    });

  // handle back button for ios
	$(function() {
		const $backTrigger = $("#back-trigger");
		if ($backTrigger.length === 0) {
			return;
		}

		function toggleBackTrigger() {
			if ($("#sidebar li.active").length > 0 || $(".sub-menu a.active").length > 0) {
				$("#back-trigger").hide();
				$("#menu-trigger").show();
			} else {
				$("#menu-trigger").hide();
				$("#back-trigger").show();
			}
		}

		$backTrigger.on("click",
			function(e) {
				e.preventDefault();
				window.history.back();
				return false;
			});
		$.sammy("#content",
			function() {
				this.around(function(callback) {
					toggleBackTrigger();
					callback();
				});
			});
	});

	$("#content").on("click touchend", "a[target='_blank']", function () {
	  window.open(this.href, "_blank", "location=no,top=20,closebuttoncaption=" + window.Helper.String.getTranslatedString("Close") + ",enableViewportScale=yes");
		return false;
	});

	if (!!window.Main && !!window.Main.ViewModels && !!window.Main.ViewModels.HomeStartupViewModel) {
		const baseStartupInit = window.Main.ViewModels.HomeStartupViewModel.prototype.init;
		const baseStartupDispose = window.Main.ViewModels.HomeStartupViewModel.prototype.dispose || function () {
		};
		window.Main.ViewModels.HomeStartupViewModel.prototype.init = function(id?: string, params?: {[key:string]:string}): Promise<void> {
			const powerManagement = window.cordova.require("cordova-plugin-chrome-apps-power.power");
			if (!!powerManagement && !!powerManagement.requestKeepAwake) {
				powerManagement.requestKeepAwake("system");
			}
			return baseStartupInit.call(this, id, params);
		};
		window.Main.ViewModels.HomeStartupViewModel.prototype.dispose = function () {
			const powerManagement = window.cordova.require("cordova-plugin-chrome-apps-power.power");
			if (!!powerManagement && !!powerManagement.releaseKeepAwake) {
				window.Log.debug("boostrapping done, releasing wakelock");
				powerManagement.releaseKeepAwake();
			}
			return baseStartupDispose.call(this);
		};
	}

	if (!window.Helper || !window.Helper.Database) {
		return;
	}
	const baseInitialize = window.Helper.Database.initialize;
	const dbInitDone = new Promise<void>((resolve, reject) => {
		document.addEventListener("deviceready", function () {
			const sqlitePlugin = window.sqlitePlugin;
			if (!!sqlitePlugin && !!sqlitePlugin.openDatabase) {
				window.openDatabase = function (name, version, displayName, estimatedSize, creationCallback) {
					return sqlitePlugin.openDatabase({
						name: name,
						iosDatabaseLocation: "Documents"
					});
				};
			}
			dbInitStart.then(() => {
				return baseInitialize();
			}).then(resolve).catch(reject);
		});
	});
	const dbInitStart = new Promise<void>((resolve, reject) => {
		window.Helper.Database.initialize = function(): Promise<void> {
			resolve();
			return dbInitDone;
		};
	});
	function setRestoreLastPageAfterStartup(restore) {
		localStorage["restoreLastPage"] = restore;
	}

	function registerEventhandler() {
		document.addEventListener("resume", function () {
			window.Log.info('debug: resumed app at ' + (new Date()).toString());
		}, false);

		document.addEventListener("pause", function () {
			window.Log.debug('debug: paused app at ' + (new Date()).toString());
			setRestoreLastPageAfterStartup(true);
		}, false);
	}

	document.addEventListener("deviceready", function () {
		if (window.cordova && window.cordova.define.moduleMap["phonegap-plugin-barcodescanner.BarcodeScanner"]) {
			window.barcodeScanner = window.cordova.require("phonegap-plugin-barcodescanner.BarcodeScanner");
		}
		const printToPdf = window.cordova.require("cordova-plugin-printtopdf.PrintToPdf");
		if (!!printToPdf && !!printToPdf.print) {
			window.print = function() {
				var scrollX = window.scrollX;
				var scrollY = window.scrollY;
				window.scrollTo(0, 0);
				$(".repeating-header, .repeating-footer").css("position", "static").css("padding-left", "0").css("padding-right", "0");
				$(".repeating-header-placeholder, .repeating-footer-placeholder").hide();
				printToPdf.print();
				window.scrollTo(scrollX, scrollY);
			};
		}
		if (window.FileRepository && !(navigator.storage && navigator.storage.estimate)) {
			window.FileRepository.prototype.getFreeDiskSpace = function() {
				return new Promise(function(resolve, reject) {
					window.cordova.exec(resolve,
						reject,
						"File",
						"getFreeDiskSpace");
				});
			};
		}
		registerEventhandler();
	}, false);

	$(document).on("click", "a[target='_system']", function() {
		window.open(this.href, "_system");
		return false;
	});
	$(document).on("click", "a[href^='mailto:'],a[href^='tel:']", function() {
		window.Log.info('handling external link ' + this.href);
		window.open(this.href.replace(/\s/g, ""), "_system");
		return false;
	});

	if (window.ko?.bindingHandlers?.autosize) {
		window.ko.bindingHandlers.autosize.update = function (element, valueAccessor, allBindings, viewModel, bindingContext) {
			const autosize = ko.unwrap(valueAccessor());
			if (autosize) {
				$(element).css("-webkit-overflow-scrolling", "touch");
				$(element).css("overflow-y", "scroll");
				let defaultRows = 10;
				let minRows = 2;
				let maxViewportRows = Math.floor(window.visualViewport.height / parseFloat($(element).css("line-height"))) - 3;
				let rows = Math.max(Math.min(maxViewportRows, defaultRows), minRows);
				if (rows.toString() !== $(element).attr("rows")) {
					$(element).attr("rows", rows);
				}
			}
		}
	}
})();