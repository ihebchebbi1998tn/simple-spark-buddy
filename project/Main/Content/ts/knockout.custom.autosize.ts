ko.bindingHandlers.autosize = {
	update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
		const autosize = ko.unwrap(valueAccessor());
		if (autosize) {
			window.autosize(element);
		} else {
			window.autosize.destroy(element);
		}
	}
};