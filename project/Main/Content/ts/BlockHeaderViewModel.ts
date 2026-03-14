import { namespace } from "./namespace";

export class BlockHeaderViewModel {

	customCss?: string;
	icon?: string;
	viewContext: any;

	constructor(params: {
		customCss?: string;
		icon?: string;
		context?: any;
	}) {
		this.customCss = params.customCss;
		this.icon = params.icon;
		this.viewContext = ko.unwrap(params.context) ?? this;
	}
}

namespace("Main.ViewModels").BlockHeaderViewModel = BlockHeaderViewModel;