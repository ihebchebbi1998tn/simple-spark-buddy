import { registerComponent } from "./componentRegistrar";

registerComponent({
	componentName: "inline-editor",
	template: "Main/Template/InlineEditor",
	viewModel: {
		createViewModel: function (params) {
			return new window.Main.ViewModels.InlineEditorViewModel(params);
		}
	},
})
