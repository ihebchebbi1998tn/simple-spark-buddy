import { param } from "jquery";
import { registerComponent } from "./componentRegistrar";

registerComponent({
	componentName: "collapsible-block-content",
	template: "Main/Template/CollapsibleBlockContent",
	viewModel: {
		createViewModel: function (params) {
			return new window.Main.ViewModels.CollapsibleBlockContentViewModel(params);
		}
	}
});