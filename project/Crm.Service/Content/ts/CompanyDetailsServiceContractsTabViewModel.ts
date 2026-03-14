import {namespace} from "@Main/namespace";
import type {CompanyDetailsViewModel} from "@Crm/CompanyDetailsViewModel";

export class CompanyDetailsServiceContractsTabViewModel extends window.Crm.Service.ViewModels.ServiceContractListIndexViewModel {
	constructor(parentViewModel: CompanyDetailsViewModel) {
		super();
		this.bulkActions([]);
		this.infiniteScroll(true);
		this.joins.remove("ParentCompany");
		const companyId = parentViewModel.company().Id();
		this.getFilter("ParentId").extend({filterOperator: "==="})(companyId);
		this.isTabViewModel(true);
	}

	async init(id?: string, params?: { [key: string]: string }): Promise<void> {
		await super.init(id, params);
		this.bookmark(this.bookmarks().find(x => x.Key === "All"));
	};
}

namespace("Crm.ViewModels").CompanyDetailsServiceContractsTabViewModel = CompanyDetailsServiceContractsTabViewModel;