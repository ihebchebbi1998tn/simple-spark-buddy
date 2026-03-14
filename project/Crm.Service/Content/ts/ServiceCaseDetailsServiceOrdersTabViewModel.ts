import {namespace} from "@Main/namespace";

export class ServiceCaseDetailsServiceOrdersTabViewModel extends window.Crm.Service.ViewModels.ServiceOrderHeadListIndexViewModel {
	serviceCase: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceCase>;
	
	constructor(parentViewModel: any) {
		super();
		
		this.isTabViewModel(true);
		this.bookmark(this.bookmarks().find(x => x.Key === "All"));
		this.bulkActions([]);
		this.serviceCase = parentViewModel.serviceCase;
		this.infiniteScroll(true);
	}
	
	applyFilters(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderHead>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderHead> {
		query = super.applyFilters(query);
		
		return query.filter("it.ServiceCaseKey === this.serviceCaseId",
			{
				serviceCaseId: this.serviceCase().Id()
			});
	};
}

namespace("Crm.Service.ViewModels").ServiceCaseDetailsServiceOrdersTabViewModel = ServiceCaseDetailsServiceOrdersTabViewModel;