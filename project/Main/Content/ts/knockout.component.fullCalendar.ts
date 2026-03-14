import {registerComponent} from "./componentRegistrar";

registerComponent({
	componentName: "full-calendar",
	template: "Main/Template/FullCalendarWidget",
	viewModel: function (params) {
		const viewModel = this;
		Object.getOwnPropertyNames(params).forEach(function (param) {
			viewModel[param] = params[param];
		});
		if (!params.hasOwnProperty("loading")) {
			viewModel.loading = ko.observable(false);
		}
	}
});