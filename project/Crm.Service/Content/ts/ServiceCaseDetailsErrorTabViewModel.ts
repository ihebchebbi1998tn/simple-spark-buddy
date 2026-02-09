import { namespace } from "@Main/namespace";
import type { ServiceCaseDetailsViewModel } from "@Crm.Service/ServiceCaseDetailsViewModel";
import { ServiceOrderDetailsErrorTabViewModelBase } from "@Crm.Service/ServiceOrderDetailsErrorTabViewModel";

export class ServiceCaseDetailsErrorTabViewModel extends ServiceOrderDetailsErrorTabViewModelBase<ServiceCaseDetailsViewModel> {
	parentViewModel: ServiceCaseDetailsViewModel;
	errorTypesCanBeAdded = ko.pureComputed<boolean>(() => {
		return this.parentViewModel.isEditable();
	});

	errorCausesCanBeAdded = ko.pureComputed<boolean>(() => {
		return this.parentViewModel.isEditable();
	});


	constructor(parentViewModel: ServiceCaseDetailsViewModel) {
		super(parentViewModel);
		this.parentViewModel = parentViewModel;
	}

	applyFilters(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderErrorType>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderErrorType> {
		query = super.applyFilters(query);
		query = query.filter("it.ServiceCaseId === this.serviceCaseId", { serviceCaseId: this.parentViewModel.serviceCase().Id() });
		return query;
	};

	async init(): Promise<void> {
		await super.init();
	};
}

namespace("Crm.Service.ViewModels").ServiceCaseDetailsErrorTabViewModel = ServiceCaseDetailsErrorTabViewModel;