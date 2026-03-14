import { registerComponent } from "./componentRegistrar";

registerComponent({
	componentName: "block-header",
	template: "Main/Template/BlockHeader",
	viewModel: {
		createViewModel: function (params) {
			return new window.Main.ViewModels.BlockHeaderViewModel(params);
		}
	}
});