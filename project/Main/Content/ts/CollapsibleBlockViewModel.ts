import { namespace } from "./namespace";

export class CollapsibleBlockViewModel {

	id: string;
	collapsed = ko.observable<boolean>(false);

	constructor(params: {
		id: string,
		collapsed?: boolean,
		onLoad?: any
	}) {
		this.id = params.id;
		this.collapsed(params.collapsed ?? false);

		if (params.onLoad) {
			params.onLoad(this);
		}
	}
}

namespace("Main.ViewModels").CollapsibleBlockViewModel = CollapsibleBlockViewModel;