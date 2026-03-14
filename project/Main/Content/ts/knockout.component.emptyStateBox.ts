import {registerComponent} from "./componentRegistrar";

registerComponent({
	componentName: "empty-state-box",
	template: "Main/Template/EmptyStateBox",
	viewModel: function (params) {
		const viewModel = this;
		viewModel.mood = ko.unwrap(params.mood);
		viewModel.title = ko.observable(params.title ? window.Helper.String.getTranslatedString(ko.unwrap(params.title), ko.unwrap(params.title)) : null);
		viewModel.text = ko.observable(params.text ? window.Helper.String.getTranslatedString(ko.unwrap(params.text), ko.unwrap(params.text)) : null);
	},
});