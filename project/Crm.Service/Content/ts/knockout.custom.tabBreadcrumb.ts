window.ko.bindingHandlers.tabBreadcrumb = {
	update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
		const value = valueAccessor();
		const text = value.text;
		const length = value.length;
		const index = value.index;
		if (index < length - 1) {
			$(element).addClass('c-gray');
		}
		if (index < length - 1 && index > 0) {
			$(element).text("...");
			$(element).attr("data-placement", "top");
			$(element).attr("title", text);
		} else {
			$(element).text(text);
		}
	}
}