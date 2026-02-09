import { registerComponent } from "./componentRegistrar";

registerComponent({
	componentName: "collapsible-block",
	template: "Main/Template/CollapsibleBlock",
	viewModel: {
		createViewModel: function (params) {
			return new window.Main.ViewModels.CollapsibleBlockViewModel(params);
		}
	}
});