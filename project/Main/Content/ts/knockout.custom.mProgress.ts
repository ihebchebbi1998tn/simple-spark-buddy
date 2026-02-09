ko.bindingHandlers.mProgress = {
	init: function(element, valueAccessor) {
		var id = new Date().getTime();
		$(element).attr("data-m-progress-id", id);
		// @ts-ignore
		var progress = new window.Mprogress({ template: 3, parent: "[data-m-progress-id='" + id + "']" });
		$(element).data("m-progress", progress);
	},
	update: function(element, valueAccessor) {
		var loading = ko.unwrap(valueAccessor());
		var progress = $(element).data("m-progress");
		if (loading && progress.status === null) {
			progress.start();
		} else if (!loading && progress.status !== null) {
			window.requestAnimationFrame(progress.end.bind(progress));
		}
	}
};