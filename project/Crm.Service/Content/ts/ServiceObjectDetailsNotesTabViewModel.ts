import {namespace} from "@Main/namespace";
import type {ServiceObjectDetailsViewModel} from "./ServiceObjectDetailsViewModel";

export class ServiceObjectDetailsNotesTabViewModel extends window.Crm.ViewModels.ContactDetailsNotesTabViewModel {
	constructor(parentViewModel: ServiceObjectDetailsViewModel) {
		super();
		this.contactId(parentViewModel.serviceObject().Id());
		this.contactName(window.Helper.ServiceObject.getDisplayName(parentViewModel.serviceObject()));
		this.contactType("ServiceObject");
		this.minDate(parentViewModel.serviceObject().CreateDate());
		this.plugin = "Crm.Service";
	}
}

namespace("Crm.Service.ViewModels").ServiceObjectDetailsNotesTabViewModel = ServiceObjectDetailsNotesTabViewModel;