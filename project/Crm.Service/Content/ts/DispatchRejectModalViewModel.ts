import {namespace} from "@Main/namespace";
import type {DispatchDetailsViewModel} from "./DispatchDetailsViewModel";
import type {ServiceOrderDispatchListIndexViewModel} from "./ServiceOrderDispatchListIndexViewModel";

export class DispatchRejectModalViewModel extends window.Main.ViewModels.ViewModelBase {
	dispatch = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch>(null);
	lookups: LookupType = {
		serviceOrderDispatchRejectReasons: {}
	};
	serviceOrder: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead>;
	errors = ko.validation.group(this.dispatch, {deep: true});
	parentViewModel: DispatchDetailsViewModel | ServiceOrderDispatchListIndexViewModel;

	constructor(parentViewModel: DispatchDetailsViewModel | ServiceOrderDispatchListIndexViewModel) {
		super();
		this.parentViewModel = parentViewModel;
		if (parentViewModel instanceof window.Crm.Service.ViewModels.DispatchDetailsViewModel) {
			this.serviceOrder = parentViewModel.serviceOrder;
		}
	}

	async refreshParentViewModel() {
		if (this.parentViewModel instanceof window.Crm.Service.ViewModels.DispatchDetailsViewModel) {
			await this.parentViewModel.init(this.dispatch().Id());
		}
	};

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		let dispatch = await window.database.CrmService_ServiceOrderDispatch
			.include("ServiceOrder")
			.find(id);
		this.dispatch(dispatch.asKoObservable());
		this.dispatch().StatusKey("Rejected");
		this.dispatch().RejectReasonKey.extend({
			required: {
				params: true,
				message: window.Helper.String.getTranslatedString("RuleViolation.Required")
					.replace("{0}", window.Helper.String.getTranslatedString("RejectReason"))
			}
		});
		this.dispatch().RejectReasonKey.subscribe(rejectReasonKey => {
			const rejectReason = this.lookups.serviceOrderDispatchRejectReasons[rejectReasonKey];
			const serviceOrderStatusKey = rejectReason.ServiceOrderStatusKey;
			if (serviceOrderStatusKey) {
				this.dispatch().ServiceOrder().StatusKey(serviceOrderStatusKey);
			}
		});
		window.database.attachOrGet(dispatch);
		this.lookups.serviceOrderDispatchRejectReasons = await window.Helper.Lookup.getLocalizedArrayMap("CrmService_ServiceOrderDispatchRejectReason");
		this.lookups.serviceOrderDispatchRejectReasons.$array = this.lookups.serviceOrderDispatchRejectReasons.$array.filter(x => x.Key !== null);
	};

	dispose(): void {
		window.database.detach(this.dispatch().innerInstance);
	};

	async save(): Promise<void> {
		this.loading(true);
		if (this.errors().length > 0) {
			this.loading(false);
			this.errors.showAllMessages();
			return;
		}

		try {
			await window.database.saveChanges();
			this.loading(false);
			if (this.serviceOrder) {
				this.serviceOrder().StatusKey(ko.unwrap(this.dispatch().ServiceOrder().StatusKey));
			}
			await this.refreshParentViewModel();
			$(".modal:visible").modal("hide");
		} catch (e) {
			this.loading(false);
			window.swal(window.Helper.String.getTranslatedString("UnknownError"),
				window.Helper.String.getTranslatedString("Error_InternalServerError"),
				"error");
		}
	};
}

namespace("Crm.Service.ViewModels").DispatchRejectModalViewModel = DispatchRejectModalViewModel;