import {registerComponent} from "./componentRegistrar";

registerComponent({
	componentName: "pmb-block",
	template: "Main/Template/PmbBlock",
	viewModel: {
		createViewModel: function (params) {
			return new window.Main.ViewModels.PmbbViewModel(params);
		}
	}
});
registerComponent({
	componentName: "pmbb-edit",
	template: "Main/Template/PmbbEdit"
});
registerComponent({
	componentName: "pmbb-edit-entry",
	template: "Main/Template/PmbbEditEntry",
	viewModel: {
		createViewModel: function (params, componentInfo) {
			const viewModel: any = {};
			viewModel.caption = ko.observable(window.Helper.String.getTranslatedString(window.ko.unwrap(params.caption), window.ko.unwrap(params.caption)));
			viewModel.validationElement = params.validationElement || window.ko.observable(null);
			viewModel.hasCheckbox = ko.observable(componentInfo.templateNodes.filter(function (x) {
				return x.tagName === "INPUT" && x.getAttribute("type") === "checkbox"
			}).length === 1);
			viewModel.hasDatePicker = ko.observable(componentInfo.templateNodes.filter(function (x) {
				return x.tagName === "INPUT" && x.getAttribute("class").split(" ").indexOf("date-picker") !== -1
			}).length === 1);
			viewModel.hasInput = ko.observable(componentInfo.templateNodes.filter(function (x) {
				return x.tagName === "INPUT"
			}).length === 1);
			viewModel.hasSelect = ko.observable(componentInfo.templateNodes.filter(function (x) {
				return x.tagName === "SELECT"
			}).length === 1);
			viewModel.isRequired = ko.pureComputed(function () {
				let isRequired = false;
				if (!!viewModel.validationElement && !!viewModel.validationElement.rules) {
					viewModel.validationElement.rules().forEach(function (rule) {
						if (rule.rule === "required" && rule.params === true && (!rule.condition || rule.condition())) {
							isRequired = true;
						}
					});
				}
				return isRequired;
			});
			viewModel.placeholder = ko.pureComputed(function () {
				let placeholder = ko.unwrap(params.placeholder) || (viewModel.isRequired() ? "PleaseSelect" : "Unspecified");
				return window.Helper.String.getTranslatedString(placeholder, placeholder);
			});
			return viewModel;
		}
	}
});
registerComponent({
	componentName: "pmbb-view",
	template: "Main/Template/PmbbView"
});
registerComponent({
	componentName: "pmbb-view-entry",
	template: "Main/Template/PmbbViewEntry",
	viewModel: function (params) {
		const viewModel = this;
		viewModel.caption = ko.observable(window.Helper.String.getTranslatedString(window.ko.unwrap(params.caption), window.ko.unwrap(params.caption)));
		viewModel.hint = params.hint ? ko.observable(window.Helper.String.getTranslatedString(window.ko.unwrap(params.hint), window.ko.unwrap(params.hint))) : null;
		return viewModel;
	}
});