ko.bindingHandlers.dateRange = {
	update: function(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
		var value = ko.unwrap(valueAccessor());
		var start = moment(ko.unwrap(value.start));
		var end = moment(ko.unwrap(value.end));
		var skeleton = "d";
		if (start.month() !== end.month()) {
			skeleton = "Md";
		}
		if (start.year() !== end.year()) {
			skeleton = "yMd";
		}
		var dateRange = window.Globalize.formatDate(start.toDate(), { skeleton: skeleton }) + " - " + window.Globalize.formatDate(end.toDate(), { skeleton: "yMd" });
		$(element).text(dateRange);
	}
};