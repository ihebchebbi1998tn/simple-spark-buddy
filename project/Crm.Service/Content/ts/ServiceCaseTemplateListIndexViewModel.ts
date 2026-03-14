import {namespace} from "@Main/namespace";

export class ServiceCaseTemplateListIndexViewModel extends window.Main.ViewModels.GenericListViewModel<Crm.Service.Rest.Model.CrmService_ServiceCaseTemplate, Crm.Service.Rest.Model.ObservableCrmService_ServiceCaseTemplate> {
	lookups: LookupType = {
		serviceCaseCategories: {$tableName: "CrmService_ServiceCaseCategory"}
	};

	constructor() {
		super(
			"CrmService_ServiceCaseTemplate",
			"Name",
			"ASC");
	}

	async init(id?: string, params?: {[key:string]:string}) {
		let user = await window.Helper.User.getCurrentUser();
		this.currentUser(user);
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		await super.init(id, params);
	};
}

namespace("Crm.Service.ViewModels").ServiceCaseTemplateListIndexViewModel = ServiceCaseTemplateListIndexViewModel;