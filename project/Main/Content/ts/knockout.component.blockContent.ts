import { registerComponent } from "./componentRegistrar";

registerComponent({
	componentName: "block-content",
	template: "Main/Template/BlockContent",
	viewModel: {
		createViewModel: function (params) {
			return new window.Main.ViewModels.BlockContentViewModel(params);
		}
	}
});