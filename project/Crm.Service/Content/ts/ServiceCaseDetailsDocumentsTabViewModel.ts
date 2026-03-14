import type {ServiceCaseDetailsViewModel} from "./ServiceCaseDetailsViewModel";
import {namespace} from "@Main/namespace";

export class ServiceCaseDetailsDocumentsTabViewModel extends window.Crm.ViewModels.DocumentAttributeListIndexViewModel {
	constructor(parentViewModel: ServiceCaseDetailsViewModel) {
		super();
		const serviceCaseId = parentViewModel.serviceCase().Id();
		this.bulkActions([]);
		this.getFilter("ReferenceKey").extend({filterOperator: "==="})(serviceCaseId);
		this.showContactLink(false);
	}
}

namespace("Crm.Service.ViewModels").ServiceCaseDetailsDocumentsTabViewModel = ServiceCaseDetailsDocumentsTabViewModel;