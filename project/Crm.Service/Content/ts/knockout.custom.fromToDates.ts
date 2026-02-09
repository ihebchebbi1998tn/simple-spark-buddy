window.ko.bindingHandlers.fromToDates = {
	init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
		const value = ko.unwrap(valueAccessor());
		let cssClass = ko.unwrap(value.cssClass);
		if (!ko.unwrap(value.from) && !ko.unwrap(value.to)) {
			if (!!cssClass) {
				$(element).addClass(cssClass);
			}
			$(element).text(window.Helper.String.getTranslatedString("Unspecified"));
		} else {
			let from = moment(ko.unwrap(value.from));
			let to = moment(ko.unwrap(value.to));
			var skeleton = "d";
			if (from.month() !== to.month()) {
				skeleton = "MMMd";
			}
			if (from.year() !== to.year()) {
				skeleton = "yMMMd";
			}
			var dateRange = window.Globalize.formatDate(from.toDate(), {skeleton: skeleton}) + " - " + window.Globalize.formatDate(to.toDate(), {skeleton: "yMMMd"});
			$(element).text(dateRange);
		}
	}
};
