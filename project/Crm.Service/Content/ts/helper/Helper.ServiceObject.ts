export class HelperServiceObject {
	static getCategoryAbbreviation(serviceObject: Crm.Service.Rest.Model.CrmService_ServiceObject | Crm.Service.Rest.Model.ObservableCrmService_ServiceObject, serviceObjectCategories: {[key:string]: Crm.Service.Rest.Model.Lookups.CrmService_ServiceObjectCategory}): string {
		if (!serviceObject){
			return "";
		}
		serviceObject = ko.unwrap(serviceObject);
		const serviceObjectCategoryKey = ko.unwrap(serviceObject.CategoryKey);
		if (serviceObjectCategoryKey) {
			const serviceObjectCategory = (serviceObjectCategories || {})[serviceObjectCategoryKey];
			if (serviceObjectCategory && serviceObjectCategory.Value) {
				return serviceObjectCategory.Value[0];
			}
		}
		return "";
	}

	static getDisplayName(serviceObject: Crm.Service.Rest.Model.CrmService_ServiceObject | Crm.Service.Rest.Model.ObservableCrmService_ServiceObject): string {
		if (!serviceObject) {
			return "";
		}
		return [ko.unwrap(serviceObject.ObjectNo), ko.unwrap(serviceObject.Name)].filter(Boolean).join(" - ");
	}

	static mapForSelect2Display(serviceObject: Crm.Service.Rest.Model.CrmService_ServiceObject): Select2AutoCompleterResult {
		return {
			id: serviceObject.Id,
			item: serviceObject,
			text: Helper.ServiceObject.getDisplayName(serviceObject)
		};
	}
}