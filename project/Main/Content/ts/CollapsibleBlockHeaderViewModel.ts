import { namespace } from "./namespace";

export class CollapsibleBlockHeaderViewModel {

	customCss?: string;
	icon?: string;
	swapIcons: boolean;
	viewContext: any;

	constructor(params: {
		customCss?: string;
		icon?: string;
		swapIcons?: boolean;
		context?: any;
	}) {
		this.customCss = params.customCss;
		this.icon = params.icon;
		this.swapIcons = params.swapIcons ?? false;
		this.viewContext = ko.unwrap(params.context) ?? this;
	}
}

namespace("Main.ViewModels").CollapsibleBlockHeaderViewModel = CollapsibleBlockHeaderViewModel;