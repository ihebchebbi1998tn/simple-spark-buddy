import {namespace} from "@Main/namespace";
import {HelperLookup} from "@Main/helper/Helper.Lookup";

export class MaintenancePlanEditModalViewModel extends window.Main.ViewModels.ViewModelBase {
	maintenancePlan = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_MaintenancePlan>(null);
	errors = ko.validation.group(this.maintenancePlan, {deep: true});
	maintenancePlanTimeUnits = ko.observableArray<string>(window._.compact(window.Crm.Service.Settings.ServiceContract.MaintenancePlan.AvailableTimeUnits.split(',')));
	lookups: LookupType = {
		serviceOrderTypes: {$tableName: "CrmService_ServiceOrderType"}
	};
	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		let maintenancePlan: Crm.Service.Rest.Model.CrmService_MaintenancePlan;
		if (id) {
			maintenancePlan = await window.database.CrmService_MaintenancePlan
				.find(id);

			window.database.attachOrGet(maintenancePlan);
		} else {
			maintenancePlan = window.database.CrmService_MaintenancePlan.defaultType.create();
			maintenancePlan.ServiceContractId = params.serviceContractId;
			maintenancePlan.LastActivity = new Date();
			window.database.add(maintenancePlan);
		}
		this.maintenancePlan(maintenancePlan.asKoObservable());
		await HelperLookup.getLocalizedArrayMaps(this.lookups);
	};

	dispose(): void {
		window.database.detach(this.maintenancePlan().innerInstance);
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
			$(".modal:visible").modal("hide");
		} catch {
			this.loading(false);
			window.swal(window.Helper.String.getTranslatedString("UnknownError"),
				window.Helper.String.getTranslatedString("Error_InternalServerError"),
				"error");
		}
	};

	getServiceOrderTemplateAutocompleteFilter(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderHead>, term: string): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderHead> {
		query = query.filter("it.IsTemplate === true && it.TypeKey in this.keys", {keys: this.lookups.serviceOrderTypes.$array.filter(t => t.MaintenanceOrder).map(l => l.Key)});
		query = query.filter(it => {
			return it.IsTemplate === true;
		});
		if (term) {
			query = window.Helper.String.contains(query, term, ["OrderNo", "ErrorMessage"]);
		}
		return query;
	}

	rhythmUnitFilter(query: $data.Queryable<Main.Rest.Model.Lookups.Main_TimeUnit>): $data.Queryable<Main.Rest.Model.Lookups.Main_TimeUnit> {
		return window.Helper.Lookup.queryLookup(query.filter("it.Key === null || it.Key in this.maintenancePlanTimeUnits",
			{maintenancePlanTimeUnits: this.maintenancePlanTimeUnits()}));
	};
}

namespace("Crm.Service.ViewModels").MaintenancePlanEditModalViewModel = MaintenancePlanEditModalViewModel;