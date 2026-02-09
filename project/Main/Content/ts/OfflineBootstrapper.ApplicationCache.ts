(function (bootstrapper) {
	// @ts-ignore
	const cache = window.jakeCache || (navigator.userAgent.indexOf("AppBoundDomains") === -1 ? window.applicationCache : null);
	if (!cache) {
		return bootstrapper;
	}
	let applicationDataTotalCount = null;
	let applicationDataCount;

	let waitForApplicationCacheDeferred = $.Deferred();
	// notify/resolve/reject will be called from application cache event listeners
	bootstrapper.waitForApplicationCache = function () {
		const isResolved = waitForApplicationCacheDeferred.state() === "resolved";
		const isRejected = waitForApplicationCacheDeferred.state() === "rejected";
		if (isResolved || isRejected) {
			const oldDeferred = waitForApplicationCacheDeferred;
			waitForApplicationCacheDeferred = $.Deferred();
			// iOS 6 won't be able to fetch anything while a connection is active (ios6 bug - connection limit 1, see http://www.realsoftwareblog.com/2012/09/ios-6-mobile-safari-web-applications.html )
			// seems to happen on ios 7 too
			// skip updating appcache since signalr might be active
			if (navigator.userAgent.match(/(iPad|iPhone|iPod).*(OS 6_)/g) || navigator.userAgent.match(/(iPad|iPhone|iPod).*(OS 7_)/g)) {
				oldDeferred.always(function() {
				if (isResolved) {
						waitForApplicationCacheDeferred.resolve();
				} else {
					waitForApplicationCacheDeferred.reject();
				}
				});
			} else {
				if (cache.status == 1 || cache.status > 3) {
					cache.update();
				}
			}
		}
		return waitForApplicationCacheDeferred.promise();
	}

	cache.addEventListener('error', async function () {
		window.Log.info('OfflineBootstrapper.js applicationCache error event triggered, checking online status by sending a custom ajax request to the server');

		try {
			await $.ajax({
				type: "HEAD",
				url: window.Helper.resolveUrl("~/Main/Account/Login"),
				cache: false
			});
			window.Log.warn('Successful Ajax request after failed fetching of cache manifest - most likely our login expired');
			waitForApplicationCacheDeferred.reject(200);
		} catch (e: any) {
			if (e.status == 0) {
				window.Log.info('Ajax request failed with status code 0 - this happens when we are offline');
				waitForApplicationCacheDeferred.reject(0);
			} else {
				window.Log.warn('Ajax request failed with status code ' + e.status + ' after failed fetching of cache manifest');
				waitForApplicationCacheDeferred.reject(0);
			}
		}
	}, false);
	cache.addEventListener('downloading', function () {
		if (applicationDataTotalCount == null) {
			const manifestUrl = $('html').attr('manifest');
			$.get(manifestUrl).then(function (result) {
				const manifestUrls = result.match(/([$\/].*[^\r\n])+/g);
				applicationDataTotalCount = manifestUrls.length;
			});
		}
	}, false);
	cache.addEventListener('progress', function (e) {
		applicationDataCount = (e && e.loaded) || applicationDataCount || 0;
		applicationDataTotalCount = (e && e.total) || applicationDataTotalCount;
		if (applicationDataTotalCount != null && applicationDataTotalCount > 0) {
			const progress = Math.round(applicationDataCount * 100 / applicationDataTotalCount);
			waitForApplicationCacheDeferred.notify(progress, applicationDataCount, applicationDataTotalCount);
		}
		applicationDataCount++;
	}, false);

	cache.addEventListener('updateready', cacheUpToDateEvent, false);
	cache.addEventListener('cached', cacheUpToDateEvent, false);
	cache.addEventListener('noupdate', cacheUpToDateEvent, false);
	function cacheUpToDateEvent() {
		applicationDataTotalCount = null;
		window.Log.debug('OfflineBootstrapper.js cache up to date event triggered, checking version');
		const manifestUrl = $('html').attr('manifest');
		if (!manifestUrl) {
			waitForApplicationCacheDeferred.resolve();
			return;
		}
		$.get(manifestUrl).then(function (result) {
			const versionMatches = result.match(/([$(#)](.*)( rev )(.*)[^\r\n])+/g);
			if (!!versionMatches && versionMatches.length > 0) {
				window.Log.info('Current Version: ' + versionMatches[0]);
			} else {
				window.Log.warn("Couldn't find version information in cache manifest");
			}
			waitForApplicationCacheDeferred.resolve();
		});
	}

	cache.addEventListener('checking', function () {
		applicationDataCount = 0;
		if ((navigator.userAgent.match(/(iPad|iPhone|iPod).*(OS 6_)/g) || navigator.userAgent.match(/(iPad|iPhone|iPod).*(OS 7_)/g)) && $.connection && $.connection.ProfilingHub && $.connection.ProfilingHub.state === "Connected") {
			// iOS 6 won't be able to fetch anything while a connection is active (ios6 bug - connection limit 1, see http://www.realsoftwareblog.com/2012/09/ios-6-mobile-safari-web-applications.html )
			// seems to happen on ios 7 too
			window.Log.warn('browser is trying to fetch application cache manifest, while signalR is already connected');
			setTimeout(function () {
				window.location.href = window.Helper.Url.resolveUrl("~/Main/Account/Login");
			}, 1000);
		}
	}, false);

	// make sure we are running with the latest application data in our cache
	cache.addEventListener('updateready', function () {
		window.Log.info('OfflineBootstrapper.js: applicationCache updateready -> reloading page');
		window.location.reload();
	}, false);
	
	return bootstrapper;
})(window.Crm.Offline.Bootstrapper);