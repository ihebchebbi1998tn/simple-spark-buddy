import {registerComponent} from "./componentRegistrar";

registerComponent({
	componentName: "floating-action-button",
	template: "Main/Template/FloatingActionButton",
	viewModel: {
		createViewModel: function (params, componentInfo) {
			const viewModel = new window.Main.ViewModels.FloatingActionButtonViewModel(params, componentInfo);
			viewModel.init();
			return viewModel;
		}
	}
});