$(function () {
	const language = $("#meta\\.CurrentLanguage").attr("content");
	const token = $("#meta\\.CurrentToken").attr("content");
	if (window.Helper && window.Helper.Database && token) {
		function addToken(request) {
			const uri = request.requestUri.split("?");
			const urlParams = new URLSearchParams(uri[1]);
			if (urlParams.get("token") === null) {
				urlParams.append("token", token);
			}
			request.requestUri = uri[0] + "?" + urlParams.toString();
		}

		const prepareRequest = window.Helper.Database.prepareRequest;
		window.Helper.Database.prepareRequest = function (requestData) {
			const request = requestData[0];
			addToken(request);
			if (request.requestUri.indexOf("$batch") > 0) {
				request.data.__batchRequests.forEach(function(batchRequest) {
					addToken(batchRequest);
				});
			}
			return prepareRequest.call(this, requestData);
		};
	}

	if (window.Helper && window.Helper.FileResource && token) {
		window.appendTokenToDownloadLink = true;
	}

	if (window.Helper && window.Helper.Lookup && language) {
		window.Helper.Lookup.setDefaultLanguage(language);
	}

	window.Helper.Culture.initialize().then(function () {
		try {
			// @ts-ignore
			window.ko.punches.enableAll();
			// @ts-ignore
			window.ko.applyBindings(window.viewModel);
		} catch (e: any) {
			$(document.body).append(e.message);
		}
	});
});