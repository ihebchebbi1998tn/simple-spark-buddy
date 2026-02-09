import { namespace } from "@Main/namespace";

export class ServiceOrderTimePostingDispatchRelinkModalViewModel extends window.Main.ViewModels.ViewModelBase {
	serviceOrderTimePosting = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting>(null);
	relinkDispatchId = ko.observable<string>(null);
	errors = ko.validation.group(this.relinkDispatchId, { deep: true });
	lookups: LookupType = {
		installationHeadStatuses: { $tableName: "CrmService_InstallationHeadStatus" },
		serviceOrderTypes: { $tableName: "CrmService_ServiceOrderType" },
		servicePriorities: { $tableName: "CrmService_ServicePriority" }
	}

	async init(id?: string, params?: { [key: string]: string }): Promise<void> {
		let serviceOrderTimePosting: Crm.Service.Rest.Model.CrmService_ServiceOrderTimePosting;
		if (id) {
			serviceOrderTimePosting = await window.database.CrmService_ServiceOrderTimePosting
				.include("ServiceOrderDispatch")
				.find(id)
			window.database.attachOrGet(serviceOrderTimePosting);
		}
		this.serviceOrderTimePosting(serviceOrderTimePosting.asKoObservable());
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		this.relinkDispatchId.extend({
			required: {
				params: true,
				message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("ServiceOrderDispatch"))
			}
		})
	}

	dispatchFilter(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch>, term: string): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch> {
		if (term) {
			query = window.Helper.String.contains(query, term, ["ServiceOrder.OrderNo"]);
		}
		if (this.serviceOrderTimePosting().Username()) {
			query = query.filter("it.Username === this.username", { username: this.serviceOrderTimePosting().Username() });
		}
		query = query.filter("it.StatusKey in this.statusKeys",
			{ statusKeys: ["Released", "Read", "InProgress", "SignedByCustomer"] });
		return query;
	}

	async save(): Promise<void> {
		this.loading(true);

		if (this.errors().length > 0) {
			this.loading(false);
			this.errors.showAllMessages();
			return;
		}

		try {
			if (this.relinkDispatchId) {
				let relinkDispatch = await window.database.CrmService_ServiceOrderDispatch.find(this.relinkDispatchId());
				this.serviceOrderTimePosting().DispatchId(relinkDispatch.Id);
				this.serviceOrderTimePosting().OrderId(relinkDispatch.OrderId);
			}
			await window.database.saveChanges();
			this.loading(false);
			$(".modal:visible").modal("hide");
			this.showSnackbar(window.Helper.String.getTranslatedString("TimePostingRelinkedMessage"), window.Helper.String.getTranslatedString("ViewDispatch"), () => {
				window.location.hash = "/Crm.Service/Dispatch/DetailsTemplate/" + this.relinkDispatchId() + "?tab=tab-time-postings";
			})
		} catch {
			this.loading(false);
			window.swal(window.Helper.String.getTranslatedString("UnknownError"),
				window.Helper.String.getTranslatedString("Error_InternalServerError"),
				"error");
		}
	};
}

namespace("Crm.Service.ViewModels").ServiceOrderTimePostingDispatchRelinkModalViewModel = ServiceOrderTimePostingDispatchRelinkModalViewModel;