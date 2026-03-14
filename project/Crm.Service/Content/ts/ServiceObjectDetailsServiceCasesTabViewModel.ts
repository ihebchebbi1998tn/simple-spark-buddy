import type { ServiceObjectDetailsViewModel } from "./ServiceObjectDetailsViewModel";
import { namespace } from "@Main/namespace";

export class ServiceObjectDetailsServiceCasesTabViewModel extends window.Crm.Service.ViewModels.ServiceCaseListIndexViewModel {
	serviceObjectId = ko.observable<string>(null);

	constructor(parentViewModel: ServiceObjectDetailsViewModel) {
		super();
		this.isTabViewModel(true);
		this.bulkActions([]);
		const serviceObjectId = parentViewModel.serviceObject().Id();
		this.serviceObjectId(serviceObjectId);
		this.infiniteScroll(true);
		this.joins.remove("ServiceObject");
	}

	applyFilters(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceCase>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceCase> {
		query = super.applyFilters(query);
		query = query.filter("it.ServiceObjectId === this.serviceObjectId", { serviceObjectId: this.serviceObjectId() });
		return query;
	};
}

namespace("Crm.Service.ViewModels").ServiceObjectDetailsServiceCasesTabViewModel = ServiceObjectDetailsServiceCasesTabViewModel;