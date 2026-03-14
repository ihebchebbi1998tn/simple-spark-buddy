import { namespace } from "./namespace";

export class CollapsibleBlockContentViewModel {
	viewContext: any;

	constructor(params: {
		context?: any;
	}) {
		this.viewContext = ko.unwrap(params.context) ?? this;
	}
}

namespace("Main.ViewModels").CollapsibleBlockContentViewModel = CollapsibleBlockContentViewModel;