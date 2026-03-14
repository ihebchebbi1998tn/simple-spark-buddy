ko.bindingHandlers.popover = {
	update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
		var value = valueAccessor();
		var $modal = $(".modal.in .modal-body");
		var container = $modal.length === 1 ? $modal : "body";
		var options: any = { container: container, placement: "auto left", trigger: "focus" };
		var content = ko.unwrap(value.content);
		if (content) {
			options.content = window.Helper.String.getTranslatedString(content, content);
		}
		var title = ko.unwrap(value.title);
		if (title) {
			options.title = window.Helper.String.getTranslatedString(title, title);
		}
		var placement = ko.unwrap(value.placement);
		if (placement) {
			options.placement = placement;
		}
		var trigger = ko.unwrap(value.trigger);
		if (trigger) {
			options.trigger = trigger;
		}
		var html = ko.unwrap(value.html);
		if (html) {
			options.html = html;
		}
		// @ts-ignore
		$(element).popover("destroy");
		// @ts-ignore
		$(element).popover(options);
		$(element).on("click", function(e) {
			e.preventDefault();
			return false;
		});
	}
};