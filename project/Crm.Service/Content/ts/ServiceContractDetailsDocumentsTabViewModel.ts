import type {ServiceContractDetailsViewModel} from "./ServiceContractDetailsViewModel";
import {namespace} from "@Main/namespace";

export class ServiceContractDetailsDocumentsTabViewModel extends window.Crm.ViewModels.DocumentAttributeListIndexViewModel {
	constructor(parentViewModel: ServiceContractDetailsViewModel) {
		super();
		const serviceContractId = parentViewModel.serviceContract().Id();
		this.bulkActions([]);
		this.getFilter("ReferenceKey").extend({filterOperator: "==="})(serviceContractId);
		this.showContactLink(false);
	}
}

namespace("Crm.Service.ViewModels").ServiceContractDetailsDocumentsTabViewModel = ServiceContractDetailsDocumentsTabViewModel;