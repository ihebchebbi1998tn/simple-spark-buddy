ko.bindingHandlers.durationText = {
	update: function(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
		let value = ko.unwrap(valueAccessor());
		let template = "hh:mm";
		let defaultText = "";
		if (value) {
			if (value.hasOwnProperty("template")) {
				template = ko.unwrap(value.template) || template;
			}
			if (value.hasOwnProperty("defaultText")) {
				defaultText = ko.unwrap(value.defaultText) || defaultText;
			}
			if (value.hasOwnProperty("value")) {
				value = ko.unwrap(value.value);
			}
		}
		const duration = window.moment.duration(value);
		if (duration.isValid()) {
			$(element).text(duration.format(template, { stopTrim: "h" }));
		} else {
			$(element).text(defaultText);
		}
	}
};