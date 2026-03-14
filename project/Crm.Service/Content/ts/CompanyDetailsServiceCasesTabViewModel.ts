import type {CompanyDetailsViewModel} from "@Crm/CompanyDetailsViewModel";
import {namespace} from "@Main/namespace";

export class CompanyDetailsServiceCasesTabViewModel extends window.Crm.Service.ViewModels.ServiceCaseListIndexViewModel {
	companyId = ko.observable<string>(null);

	constructor(parentViewModel: CompanyDetailsViewModel) {
		super();
		this.isTabViewModel(true);
		this.bookmark(this.bookmarks().find(x => x.Key === "All"));
		this.bulkActions([]);
		const companyId = parentViewModel.company().Id();
		this.companyId(companyId);
		this.infiniteScroll(true);
		this.joins.remove("AffectedCompany");
	}

	applyFilters(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceCase>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceCase> {
		query = super.applyFilters(query);
		query = query.filter("it.AffectedCompanyKey === this.companyId", {companyId: this.companyId()});
		return query;
	};
}

namespace("Crm.ViewModels").CompanyDetailsServiceCasesTabViewModel = CompanyDetailsServiceCasesTabViewModel;