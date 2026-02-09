import {namespace} from "@Main/namespace";
import type {ServiceContractDetailsViewModel} from "./ServiceContractDetailsViewModel";

export class ServiceContractDetailsServiceOrdersTabViewModel extends window.Crm.Service.ViewModels.ServiceOrderHeadListIndexViewModel {
	serviceContractId = ko.observable<string>(null);

	constructor(parentViewModel: ServiceContractDetailsViewModel) {
		super();
		this.bookmark(this.bookmarks().find(x => x.Key === "All"));
		const serviceContractId = parentViewModel.serviceContract().Id();
		this.serviceContractId(serviceContractId);
		this.infiniteScroll(true);
	}

	async init(): Promise<void> {
		await super.init();
		this.bulkActions([]);
	}

	applyFilters(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderHead>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderHead> {
		query = super.applyFilters(query);
		query = query.filter("it.ServiceContractId === this.serviceContractId",
			{serviceContractId: this.serviceContractId()});
		return query;
	};
}

namespace("Crm.Service.ViewModels").ServiceContractDetailsServiceOrdersTabViewModel = ServiceContractDetailsServiceOrdersTabViewModel;