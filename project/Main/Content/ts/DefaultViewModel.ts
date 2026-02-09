import {HelperDOM} from "./helper/Helper.DOM";
import {namespace} from "./namespace";

export class DefaultViewModel {

	loading = ko.observable<boolean>(true);
	global: Window;

	constructor() {
		this.global = window;
	}

	closeModal(): void {
		HelperDOM.dismissModal();
	}
}

namespace("Main.ViewModels").DefaultViewModel = DefaultViewModel;