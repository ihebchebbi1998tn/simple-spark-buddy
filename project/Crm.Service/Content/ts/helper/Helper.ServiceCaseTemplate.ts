export class HelperServiceCaseTemplate {
	static mapForSelect2Display(serviceCaseTemplate: Crm.Service.Rest.Model.CrmService_ServiceCaseTemplate): Select2AutoCompleterResult {
		return {
			id: serviceCaseTemplate.Id,
			item: serviceCaseTemplate,
			text: serviceCaseTemplate.Name
		};
	}
}