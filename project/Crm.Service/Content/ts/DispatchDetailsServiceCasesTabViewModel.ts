import {namespace} from "@Main/namespace";
import type {DispatchDetailsViewModel} from "./DispatchDetailsViewModel";
import {ServiceOrderDetailsServiceCasesTabViewModelBase} from "./ServiceOrderDetailsServiceCasesTabViewModel";

export class DispatchDetailsServiceCasesTabViewModel extends ServiceOrderDetailsServiceCasesTabViewModelBase<DispatchDetailsViewModel> {
	dispatch: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch>;
	constructor(parentViewModel: DispatchDetailsViewModel) {
		super(parentViewModel);
		this.dispatch = parentViewModel.dispatch;
	}

	applyOrderBy(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceCase>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceCase> {
		// @ts-ignore
		query = query.orderByDescending("orderByCurrentServiceOrderTime", { currentServiceOrderTimeId: this.dispatch().CurrentServiceOrderTimeId() });
		return super.applyOrderBy(query);
	};

	async complete(serviceCase: Crm.Service.Rest.Model.ObservableCrmService_ServiceCase): Promise<void> {
		this.loading(true);
		const completedServiceCaseStatus = this.lookups.serviceCaseStatuses.$array.find(x => {
			return x.Key === this.completedStatus;
		});
		await window.Crm.Service.ViewModels.ServiceCaseDetailsViewModel.prototype.setStatus.bind({
			loading: this.loading,
			serviceCase: window.ko.observable(serviceCase)
		})(completedServiceCaseStatus);
		this.loading(false);
	};

	completedStatus = 6;

	getItemGroup =
		window.Crm.Service.ViewModels.DispatchDetailsViewModel.prototype.getServiceOrderPositionItemGroup;
	initItems = window.Crm.Service.ViewModels.ServiceOrderDetailsServiceCasesTabViewModel.prototype.initItems;
}

namespace("Crm.Service.ViewModels").DispatchDetailsServiceCasesTabViewModel = DispatchDetailsServiceCasesTabViewModel;