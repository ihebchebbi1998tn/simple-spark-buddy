import {namespace} from "@Main/namespace";

export class ServiceOrderTemplateListIndexViewModel extends window.Main.ViewModels.GenericListViewModel<Crm.Service.Rest.Model.CrmService_ServiceOrderHead, Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead> {
	lookups: LookupType = {
		serviceOrderTypes: {$tableName: "CrmService_ServiceOrderType"},
		servicePriorities: {$tableName: "CrmService_ServicePriority"}
	};

	constructor() {
		super("CrmService_ServiceOrderHead",
			"OrderNo",
			"ASC",
			["ResponsibleUserUser", "UserGroup"]);
	}

	applyFilters(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderHead>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderHead> {
		query = super.applyFilters(query);
		query = query.filter(function (serviceOrder) {
			return serviceOrder.IsTemplate === true;
		});
		return query;
	};

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		await super.init(id, params);
	};
}

namespace("Crm.Service.ViewModels").ServiceOrderTemplateListIndexViewModel = ServiceOrderTemplateListIndexViewModel;