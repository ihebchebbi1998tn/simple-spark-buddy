import {HelperUrl} from "./Helper.Url";

export function setupErrorLogging(): void {
	window.onerror = function (msg, url, line, columnNo, error) {
		if (url.includes("Crm/log")) {
			return;
		}
		let errorMessage = JSON.stringify(error);
		if (errorMessage === "{}") {
			const error2 = {...error};
			errorMessage = JSON.stringify(error2);
		}
		url = JSON.stringify(url);
		if (window.location && window.location.href !== url) {
			url += " || " + window.location.href;
		}
		msg = JSON.stringify(msg);
		const currentUser = document.getElementById("meta.CurrentUser") as HTMLMetaElement;
		if (currentUser) {
			msg += " || current user: " + currentUser.content;
		}
		$.ajax({
			url: HelperUrl.resolveUrl("~/Main/Log/LogError"),
			data: {
				message: JSON.stringify(msg),
				url: url,
				line: line,
				column: columnNo,
				error: errorMessage
			},
			type: "POST",
			success: function () {
				console.log("error logged");
			}
		});
	};
	
}

