import {namespace} from "./namespace";

export class Bootstrapper {
	static initializeTranslations() {
		window.Log.debug('OfflineBootstrapper.js: initializeTranslations called');
		const deferred = $.Deferred();
		const url = window.Helper.resolveUrl('~/Main/Resources.json');
		$.ajax(url,
			{
				success: function(result) {
					window.Helper.Culture.languageCulture()
						.then(function (defaultLanguage) {
							window.Helper.String.setDefaultLanguage(defaultLanguage);
							if (window.Helper.Lookup) {
								window.Helper.Lookup.setDefaultLanguage(defaultLanguage);
							}
							for (let language in result) {
								for (let key in result[language]) {
									window.Helper.String.setStringTranslation(key, result[language][key], language);
								}
								window.Log.debug('OfflineBootstrapper.js: initializeTranslations successfull');
								deferred.resolve();
							}
						});
				}
			});
		return deferred.promise();
	};

	static initializeSettingsDeferred;
	static initializeSettings = function () {
		if (Bootstrapper.initializeSettingsDeferred) {
			return Bootstrapper.initializeSettingsDeferred.promise();
		}
		window.Log.debug('OfflineBootstrapper.js: initializeSettings called');
		Bootstrapper.initializeSettingsDeferred = $.Deferred();
		const url = window.Helper.resolveUrl('~/Main/Settings.json');
		$.ajax(url, {
			success: function (result) {
				for (let setting in result) {
					const index = setting.lastIndexOf('.');
					if (index != -1) {
						const settingNamespace = setting.substr(0, index);
						const settingName = setting.substr(index + 1);
						namespace(settingNamespace)[settingName] = result[setting];
					}
				}
				window.Log.debug('OfflineBootstrapper.js: initializeSettings successfull');
				Bootstrapper.initializeSettingsDeferred.resolve();
			}
		});
		return Bootstrapper.initializeSettingsDeferred.promise();
	};

	static loadLicensing = function () {
		window.Log.debug('OfflineBootstrapper.js: loadLicensing called');
		const deferred = $.Deferred();
		const url = window.Helper.resolveUrl('~/Main/Licensing.json');
		$.ajax(url, {
			success: function (result) {
				window.Helper.Database.saveToLocalStorage("license", JSON.stringify(result));
				window.Log.debug('OfflineBootstrapper.js: loadLicensing successfull');
				deferred.resolve();
			},
			error: function (error){
				window.Log.error("Couldn't load license", error);
				deferred.reject();
			}
		});
		return deferred.promise();
	};

	static registerNumberingService = function () {
		window.Log.debug('OfflineBootstrapper.js: registerNumberingService called');
		const deferred = $.Deferred();
		window.NumberingService.registerNumberingService().then(function () {
			window.Log.debug('OfflineBootstrapper.js: registerNumberingService successfull');
			deferred.resolve();
		}).catch(deferred.reject);
		return deferred.promise();
	};

	static registerDeviceForPushNotifications = function () {
		window.Log.debug('OfflineBootstrapper.js: registerDeviceForPushNotifications called');
		const deferred = $.Deferred();
		// @ts-ignore
		if (!!window.plugins && !!window.plugins.pushNotification && !!window.plugins.pushNotification.registerDevice) {
			const registerDeviceTimeout = setTimeout(function () {
				window.Log.error('OfflineBootstrapper.js: registerDeviceForPushNotifications failed: timeout');
				deferred.resolve();
			}, 5000);
			// @ts-ignore
			window.plugins.pushNotification.registerDevice({ alert: true, badge: true, sound: true }, function (status) {
				clearTimeout(registerDeviceTimeout);
				if (!!status && !!status.deviceToken) {
					$.ajax({
						type: 'POST',
						contentType: "application/json; charset=utf-8",
						url: window.Helper.resolveUrl('~/Main/User/SetDeviceToken/' + status.deviceToken),
						success: function () {
							window.Log.info('OfflineBootstrapper.js: registerDeviceForPushNotifications successfull');
							deferred.resolve();
						},
						error: function (error) {
							window.Log.error('OfflineBootstrapper.js: registerDeviceForPushNotifications failed to invoke server method setDeviceToken: ' + error);
							deferred.resolve();
						}
					});
				} else {
					window.Log.info('OfflineBootstrapper.js: registerDeviceForPushNotifications failed');
					deferred.resolve();
				}
			});
		} else {
			window.Log.debug('OfflineBootstrapper.js: registerDeviceForPushNotifications skipped - no pushNotifaction plugin');
			deferred.resolve();
		}
		return deferred.promise();
	};
}

namespace("window.Crm.Offline").Bootstrapper ||= Bootstrapper;