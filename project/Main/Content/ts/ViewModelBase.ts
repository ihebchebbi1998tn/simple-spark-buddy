import {namespace} from "./namespace";

export abstract class ViewModelBase {
	loading = ko.observable<boolean>(true);

	async init(id?: string | number, params?: {[key:string]:string}): Promise<void> {
	}

	dispose(): void{};

	snackbarAction: () => void;

	showSnackbar(text: string, actionText?: string, actionFunction?: Function, noRemove?: boolean) {
		if (noRemove !== true) {
			$("div[data-growl]").remove();
		}
		let message = text;
		if (!!actionText) {
			message += '<button class="m-l-30 btn btn-default waves-effect" data-bind="click: snackbarAction">' + actionText + "</button>";
		}

		const growl = $.growl({
			message: message
		}, {
			type: "inverse",
			allow_dismiss: false,
			placement: {
				from: "bottom",
				align: "center"
			},
			delay: 6000,
			animate: {
				enter: "animated fadeInUp",
				exit: "animated fadeOut"
			}
		});
		this.snackbarAction = () => {
			growl.close();
			if (typeof actionFunction === "function") {
				actionFunction();
			}
		};
		ko.applyBindings(this, growl.$template.get(0));
	}
}

namespace("Main.ViewModels").ViewModelBase = ViewModelBase;
