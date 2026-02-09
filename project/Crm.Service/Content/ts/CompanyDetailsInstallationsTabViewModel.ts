import type {CompanyDetailsViewModel} from "@Crm/CompanyDetailsViewModel";
import {namespace} from "@Main/namespace";

export class CompanyDetailsInstallationsTabViewModel extends window.Crm.Service.ViewModels.InstallationListIndexViewModel {
	companyId = ko.observable<string>(null);

	constructor(parentViewModel: CompanyDetailsViewModel) {
		super();
		this.isTabViewModel(true);
		const companyId = parentViewModel.company().Id();
		this.companyId(companyId);
		this.infiniteScroll(true);
		this.joins.remove("Company");
	}

	applyFilters(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_Installation>): $data.Queryable<Crm.Service.Rest.Model.CrmService_Installation> {
		query = super.applyFilters(query);
		query = query.filter("it.LocationContactId === this.companyId", {companyId: this.companyId()});
		return query;
	};
}

namespace("Crm.ViewModels").CompanyDetailsInstallationsTabViewModel = CompanyDetailsInstallationsTabViewModel;