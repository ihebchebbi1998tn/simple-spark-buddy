import {namespace} from "@Main/namespace";
import type {ServiceContractDetailsViewModel} from "./ServiceContractDetailsViewModel";

export class ServiceContractDetailsNotesTabViewModel extends window.Crm.ViewModels.ContactDetailsNotesTabViewModel {
	constructor(parentViewModel: ServiceContractDetailsViewModel) {
		super();
		this.contactId(parentViewModel.serviceContract().Id());
		this.contactName(window.Helper.ServiceContract.getDisplayName(parentViewModel.serviceContract()));
		this.contactType("ServiceContract");
		this.minDate(parentViewModel.serviceContract().CreateDate());
		this.plugin = "Crm.Service";
	}
}

namespace("Crm.Service.ViewModels").ServiceContractDetailsNotesTabViewModel = ServiceContractDetailsNotesTabViewModel;