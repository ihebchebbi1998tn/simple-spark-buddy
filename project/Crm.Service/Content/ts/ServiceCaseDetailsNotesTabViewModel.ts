import {namespace} from "@Main/namespace";
import type {ServiceCaseDetailsViewModel} from "./ServiceCaseDetailsViewModel";

export class ServiceCaseDetailsNotesTabViewModel extends window.Crm.ViewModels.ContactDetailsNotesTabViewModel {
	constructor(parentViewModel: ServiceCaseDetailsViewModel) {
		super();
		this.contactId(parentViewModel.serviceCase().Id());
		this.contactName(parentViewModel.serviceCase().Name());
		this.contactType("ServiceCase");
		this.minDate(parentViewModel.serviceCase().CreateDate());
		this.plugin = "Crm.Service";
	}
}

namespace("Crm.Service.ViewModels").ServiceCaseDetailsNotesTabViewModel = ServiceCaseDetailsNotesTabViewModel;