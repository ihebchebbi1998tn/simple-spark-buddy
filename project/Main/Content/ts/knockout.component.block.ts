import { registerComponent } from "./componentRegistrar";

registerComponent({
	componentName: "block",
	template: "Main/Template/Block",
	viewModel: {
		createViewModel: function () {
			return new window.Main.ViewModels.BlockViewModel();
		}
	}
});