import { namespace } from "./namespace";

export class BlockContentViewModel {
	viewContext: any;

	constructor(params: {
		context?: any;
	}) {
		this.viewContext = ko.unwrap(params.context) ?? this;
	}
}

namespace("Main.ViewModels").BlockContentViewModel = BlockContentViewModel;