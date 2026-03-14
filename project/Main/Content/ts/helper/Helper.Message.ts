
export class HelperMessage {
	static Template = $("<div />")
		.addClass("message")
		.append($("<a />")
		.addClass("close-button")
		.attr("href", "#"));

	static Types = {
		error: {
			cssClass: "error-message",
			cantHide: false
		},
		information: {
			cssClass: "information-message",
			cantHide: false
		},
		warning: {
			cssClass: "warning-message",
			cantHide: true
		}
	}

	static Close(messageElement: JQuery<HTMLElement>, force = false): void {
		if (force) {
			messageElement.remove();
			return;
		}
		messageElement.find("a.close-button:visible").click();
	}

	static Inline(message: string, type: string, timeOut: number | null = null): void {
		const target = $("body");
		const typeSettings = HelperMessage.Types[type || "information"];
		const cssClass = typeSettings.cssClass;
		const cantHide = typeSettings.cantHide;

		const messageElement = $(HelperMessage.Template.clone())
		.addClass(cssClass + (cantHide ? " cant-hide" : ""))
		.append(message)
		.prependTo(target)
		.hide()
		.slideDown("medium", function () {
			HelperMessage.Close($(this).siblings("." + cssClass));
		});

		if (!!timeOut) window.setTimeout(function() { HelperMessage.Close(messageElement); }, timeOut);
	}

	static Error(message: string): void {
		HelperMessage.Inline(message, "error");
	}

	static Warning(message: string): void {
		HelperMessage.Inline(message, "warning");
	}

	static Information(message: string): void {
		HelperMessage.Inline(message, "information");
	}	
}

