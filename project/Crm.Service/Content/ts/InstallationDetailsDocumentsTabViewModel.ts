import type {InstallationDetailsViewModel} from "./InstallationDetailsViewModel";
import {namespace} from "@Main/namespace";

export class InstallationDetailsDocumentsTabViewModel extends window.Crm.ViewModels.DocumentAttributeListIndexViewModel {
	constructor(parentViewModel: InstallationDetailsViewModel) {
		super();
		const installationId = parentViewModel.installation().Id();
		this.bulkActions([]);
		this.getFilter("ReferenceKey").extend({filterOperator: "==="})(installationId);
		this.showContactLink(false);
	}
}

namespace("Crm.Service.ViewModels").InstallationDetailsDocumentsTabViewModel = InstallationDetailsDocumentsTabViewModel;