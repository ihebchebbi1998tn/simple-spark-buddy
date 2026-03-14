import {namespace} from "@Main/namespace";
import type {ServiceOrderTemplateDetailsViewModel} from "./ServiceOrderTemplateDetailsViewModel";

export class ServiceOrderTemplateDetailsJobsTabViewModel extends window.Crm.Service.ViewModels.ServiceOrderDetailsJobsTabViewModelBase<ServiceOrderTemplateDetailsViewModel> {
	timesCanBeAdded = ko.pureComputed<boolean>(() => {
		return this.parentViewModel.serviceOrderTemplateIsEditable() &&
			window.Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode === "JobPerInstallation";
	});

	canDeleteServiceOrderTime(serviceOrderTime: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime): boolean {
		return this.parentViewModel.serviceOrderTemplateIsEditable();
	};

	canEditServiceOrderTime(serviceOrderTime: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime): boolean {
		return this.parentViewModel.serviceOrderTemplateIsEditable();
	};
}

namespace("Crm.Service.ViewModels").ServiceOrderTemplateDetailsJobsTabViewModel = ServiceOrderTemplateDetailsJobsTabViewModel;