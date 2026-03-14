import {namespace} from "./namespace";

export class ErrorViewModel extends window.Main.ViewModels.ViewModelBase {
	status = ko.observable<number>(null);
	errorTitle = ko.observable<string>(null);
	errorText = ko.observable<string>(null);

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		let status = parseInt(id);
		this.status(status);
		if (status === 404) {
			this.errorTitle(window.Helper.String.getTranslatedString("NotFoundTitle"));
			this.errorText(window.Helper.String.getTranslatedString("NotFoundMessage"));
		} else {
			this.errorTitle(window.Helper.String.getTranslatedString("UnknownError"));
			this.errorText(window.Helper.String.getTranslatedString("Error_InternalServerError"));
		}
	}

	back = function () {
		window.history.go(-2);
	}
}

namespace("Main.ViewModels").ErrorViewModel = ErrorViewModel;