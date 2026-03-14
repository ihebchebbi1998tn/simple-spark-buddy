import {namespace} from "@Main/namespace";

export class ServiceOrderNoInvoiceModalViewModel extends window.Main.ViewModels.ViewModelBase {
	serviceOrder = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead>(null);
	errors = ko.validation.group(this.serviceOrder, {deep: true});

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		let serviceOrder = await window.database.CrmService_ServiceOrderHead.find(id);
		window.database.attachOrGet(serviceOrder);
		this.serviceOrder(serviceOrder.asKoObservable());
		this.serviceOrder().NoInvoiceReasonKey.extend({
			required: {
				params: true,
				message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("Reason"))
			}
		});
	}

	async save(): Promise<void> {
		this.loading(true);
		if (this.errors().length > 0) {
			this.loading(false);
			this.errors.showAllMessages();
			return;
		}
		try {
			await window.database.CrmService_ServiceOrderMaterial
				.filter(function(it) { return it.OrderId === this.serviceOrderId && it.ServiceOrderMaterialType === window.Crm.Service.ServiceOrderMaterialType.Invoice; }, {serviceOrderId: this.serviceOrder().Id()})
				.forEach(serviceOrderMaterial => {
					window.database.remove(serviceOrderMaterial);
				});
			await window.database.CrmService_ServiceOrderTimePosting
				.filter(function(it) { return it.OrderId === this.serviceOrderId && it.ServiceOrderTimePostingType === window.Crm.Service.ServiceOrderTimePostingType.Invoice; }, {serviceOrderId: this.serviceOrder().Id()})
				.forEach(serviceOrderTimePosting => {
					window.database.remove(serviceOrderTimePosting);
				});
			await window.database.CrmService_ServiceOrderTime
				.filter("it.OrderId === this.serviceOrderId", {serviceOrderId: this.serviceOrder().Id()})
				.forEach(serviceOrderTime => {
					window.database.attachOrGet(serviceOrderTime);
					serviceOrderTime.InvoiceDuration = window.moment.duration(0).toString();
				});
			this.serviceOrder().StatusKey("Closed");
			await window.database.saveChanges();
			this.loading(false);
			$(".modal:visible").modal("hide");
		} catch (e) {
			this.loading(false);
			window.swal(window.Helper.String.getTranslatedString("UnknownError"), window.Helper.String.getTranslatedString("Error_InternalServerError"), "error");
		}
	}
}

namespace("Crm.Service.ViewModels").ServiceOrderNoInvoiceModalViewModel = ServiceOrderNoInvoiceModalViewModel;

