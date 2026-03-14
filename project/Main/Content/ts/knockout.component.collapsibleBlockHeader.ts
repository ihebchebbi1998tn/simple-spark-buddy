import { registerComponent } from "./componentRegistrar";

registerComponent({
	componentName: "collapsible-block-header",
	template: "Main/Template/CollapsibleBlockHeader",
	viewModel: {
		createViewModel: function (params) {
			return new window.Main.ViewModels.CollapsibleBlockHeaderViewModel(params);
		}
	}
});