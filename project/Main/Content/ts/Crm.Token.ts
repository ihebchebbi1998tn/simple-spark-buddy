;(function() {
	const token = $("#meta\\.CurrentToken").attr("content") || (function () {
		const tokentest = /(\?|&)token=([^&#]+)/.exec(window.location.search);
		return !!tokentest ? tokentest[2] : null;
	})();

	if (!token){
		return;
	}

	function appendToken(url) {
		if (url && url.search(/(\?|\&)+token=/) === -1) {
			const urlParts = url.split("?");
			const urlParams = new URLSearchParams(urlParts[1]);
			if (urlParams.get("token") === null) {
				urlParams.append("token", token);
			}
			url = urlParts[0] + "?" + urlParams.toString();
		}
		return url;
	}

	if (window.signalR) {
		const withUrl = window.signalR.HubConnectionBuilder.prototype.withUrl;
		window.signalR.HubConnectionBuilder.prototype.withUrl = function(url){
			// @ts-ignore
			return withUrl.call(this, appendToken(url));
		};
	}

	if ($.ajax) {
		$(document).ajaxSend(function(event, request, settings) {
			settings.url = appendToken(settings.url);
		});
	}

	$(function () {
		$.fx.off = true;

		$("form").each(function () {
			$(this)
	.append($("<input />")
			.attr({ "type": "hidden", "name": "token" })
			.val(token))
	.data("tokenAppended", true);
		});

		$(document.body).delegate("form", "focus", function () {
			const current = $(this);
			if (!current.data("tokenAppended")) {
				current
						.append($("<input />")
										.attr({ "type": "hidden", "name": "token" })
										.val(token))
						.data("tokenAppended", true);
			}
		});

		$(document.body).delegate("form", "submit", function () {
			const current = $(this);
			if (!current.data("tokenAppended")) {
				current
						.append($("<input />")
										.attr({ "type": "hidden", "name": "token" })
										.val(token))
						.data("tokenAppended", true);
			}
		});

		$("a").each(function () {

			const current = $(this);
			const href = current.attr("href");
			if (href != undefined && href[0] != "#" && href.search("token=") < 0) {
				const hrefParts = href.split("#");
				current.attr("href", hrefParts[0] + (hrefParts[0].search(/\?/) > 0 ? "&token=" : "?token=") + token + (hrefParts[1] ? "#" + hrefParts[1] : ""));
			}
		});

		$(document.body).delegate("a", "focus", function () {
			const current = $(this);
			const href = current.attr("href");
			if (href != undefined && href[0] != "#" && href.search("token=") < 0) {
				const hrefParts = href.split("#");
				current.attr("href", hrefParts[0] + (hrefParts[0].search(/\?/) > 0 ? "&token=" : "?token=") + token + (hrefParts[1] ? "#" + hrefParts[1] : ""));
			}
		});

		$(document.body).delegate("a", "click", function () {
			const current = $(this);
			const href = current.attr("href");
			if (href != undefined && href[0] != "#" && href.search("token=") < 0) {
				const hrefParts = href.split("#");
				current.attr("href", hrefParts[0] + (hrefParts[0].search(/\?/) > 0 ? "&token=" : "?token=") + token + (hrefParts[1] ? "#" + hrefParts[1] : ""));
			}
		});

		// @ts-ignore
		if (window.Sammy && window.Sammy.EventContext){
			// @ts-ignore
			const getUrl = window.Sammy.EventContext.prototype.getUrl;
			// @ts-ignore
			window.Sammy.EventContext.prototype.getUrl = function() {
				return appendToken(getUrl.apply(this, arguments));
			};
		}

		if (window.Helper && window.Helper.Database) {
			const prepareRequest = window.Helper.Database.prepareRequest;
			window.Helper.Database.prepareRequest = function(requestData) {
				const request = requestData[0];
				request.requestUri = appendToken(request.requestUri);
				return prepareRequest.call(this, requestData);
			}
		}

		if (window.ko && window.ko.components.loaders.length > 2) {
			const loadTemplate = window.ko.components.loaders[1].loadTemplate;
			window.ko.components.loaders[1].loadTemplate = function(name, templateConfig, callback) {
				templateConfig.url = appendToken(templateConfig.url);
				return loadTemplate.call(this, name, templateConfig, callback);
			}
		}

		if (window.fetch && window.Helper && window.Helper.Url) {
			const fetch = window.fetch;
			window.fetch = function() {
				const url = arguments[0];
				arguments[0] = url && window.Helper.Url.qualifyURL(url).indexOf(location.host) !== -1 ? appendToken(url) : url;
				// @ts-ignore
				return fetch.apply(this, arguments);
			}
		}
	});

	if (window.fetch && window.Helper && window.Helper.Url) {
		var fetch = window.fetch;
		window.fetch = function() {
			const url = arguments[0];
			arguments[0] = url && window.Helper.Url.qualifyURL(url).indexOf(location.host) !== -1 ? appendToken(url) : url;
			// @ts-ignore
			return fetch.apply(this, arguments);
		}
	}
})();