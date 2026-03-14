import type {CompanyDetailsViewModel} from "@Crm/CompanyDetailsViewModel";
import {namespace} from "@Main/namespace";

export class CompanyDetailsServiceOrdersTabViewModel extends window.Crm.Service.ViewModels.ServiceOrderHeadListIndexViewModel {
	companyId = ko.observable<string>(null);

	constructor(parentViewModel: CompanyDetailsViewModel) {
		super();
		this.isTabViewModel(true);
		this.bookmark(this.bookmarks().find(x => x.Key === "All"));
		this.bulkActions([]);
		const companyId = parentViewModel.company().Id();
		this.companyId(companyId);
		this.infiniteScroll(true);
		this.joins.remove("Company");
	}

	applyFilters(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderHead>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderHead> {
		query = super.applyFilters(query);
		query = query.filter("it.CustomerContactId === this.companyId", {companyId: this.companyId()});
		return query;
	};
}

namespace("Crm.ViewModels").CompanyDetailsServiceOrdersTabViewModel = CompanyDetailsServiceOrdersTabViewModel;