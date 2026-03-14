import type { ServiceObjectDetailsViewModel } from "./ServiceObjectDetailsViewModel";
import { namespace } from "@Main/namespace";

export class ServiceObjectDetailsServiceOrdersTabViewModel extends window.Crm.Service.ViewModels.ServiceOrderHeadListIndexViewModel {
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

	applyFilters(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderHead>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderHead> {
		query = super.applyFilters(query);
		query = query.filter("it.ServiceObjectId === this.serviceObjectId", { serviceObjectId: this.serviceObjectId() });
		return query;
	};
}

namespace("Crm.Service.ViewModels").ServiceObjectDetailsServiceOrdersTabViewModel = ServiceObjectDetailsServiceOrdersTabViewModel;