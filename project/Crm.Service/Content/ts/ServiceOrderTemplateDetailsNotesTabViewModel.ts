import {namespace} from "@Main/namespace";
import type {ServiceOrderTemplateDetailsViewModel} from "@Crm.Service/ServiceOrderTemplateDetailsViewModel";

export class ServiceOrderTemplateDetailsNotesTabViewModel extends window.Crm.Service.ViewModels.ServiceOrderDetailsNotesTabViewModel<ServiceOrderTemplateDetailsViewModel> {
	constructor(parentViewModel: ServiceOrderTemplateDetailsViewModel) {
		super(parentViewModel);
		this.contactType("ServiceOrderTemplate");
	}
}

namespace("Crm.Service.ViewModels").ServiceOrderTemplateDetailsNotesTabViewModel = ServiceOrderTemplateDetailsNotesTabViewModel;