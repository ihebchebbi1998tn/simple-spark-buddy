import {namespace} from "@Main/namespace";

export class ServiceOrderCloseModalViewModel extends window.Main.ViewModels.ViewModelBase {

	serviceOrder = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead>(null);
	errors = ko.validation.group(this.serviceOrder, {deep: true});
	returnUrl: string;

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		this.returnUrl = params.returnUrl;
		let serviceOrder = await window.database.CrmService_ServiceOrderHead.find(id);
		window.database.attachOrGet(serviceOrder);
		this.serviceOrder(serviceOrder.asKoObservable());
		this.serviceOrder().CloseReason.extend({
			required: {
				params: true,
				message: window.Helper.String.getTranslatedString("RuleViolation.Required")
					.replace("{0}", window.Helper.String.getTranslatedString("Reason"))
			}
		});
	};

	dispose(): void {
		window.database.detach(this.serviceOrder().innerInstance);
	};

	async save(): Promise<void> {
		this.loading(true);
		if (this.errors().length > 0) {
			this.loading(false);
			this.errors.showAllMessages();
			return;
		}
		this.serviceOrder().StatusKey("Closed");
		try {
			await window.database.saveChanges();
			$(".modal:visible").modal("hide");
			if (this.returnUrl) {
				window.location.hash = this.returnUrl;
			}
		} catch {
			this.loading(false);
			window.swal(window.Helper.String.getTranslatedString("UnknownError"),
				window.Helper.String.getTranslatedString("Error_InternalServerError"),
				"error");
		}

	};
}

namespace("Crm.Service.ViewModels").ServiceOrderCloseModalViewModel = ServiceOrderCloseModalViewModel;