import type {DispatchDetailsViewModel} from "./DispatchDetailsViewModel";
import {namespace} from "@Main/namespace";
import {ServiceOrderDetailsDocumentsTabViewModelBase} from "./ServiceOrderDetailsDocumentsTabViewModel";

export class DispatchDetailsDocumentsTabViewModel extends ServiceOrderDetailsDocumentsTabViewModelBase<DispatchDetailsViewModel>{
	dispatch: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch>;
	constructor(parentViewModel: DispatchDetailsViewModel) {
		super(parentViewModel);
		this.canAddDocument(false);
		this.dispatch = parentViewModel.dispatch;
	}

	applyFilters(query: $data.Queryable<Crm.Rest.Model.Crm_DocumentAttribute>): $data.Queryable<Crm.Rest.Model.Crm_DocumentAttribute> {
		query = super.applyFilters(query);
		let dispatchId = null;
		if (this.dispatch()) {
			dispatchId = this.dispatch().Id();
		}
		query = query.filter("it.ExtensionValues.DispatchId === null || it.ExtensionValues.DispatchId === this.dispatchId",
			{ dispatchId: dispatchId });
		return query;
	};

	applyOrderBy(query:$data.Queryable<Crm.Rest.Model.Crm_DocumentAttribute>): $data.Queryable<Crm.Rest.Model.Crm_DocumentAttribute> {
		let id = null;
		if (this.dispatch() && this.dispatch().CurrentServiceOrderTimeId()) {
			id = this.dispatch().CurrentServiceOrderTimeId();
		}
		// @ts-ignore
		query = query.orderByDescending("orderByCurrentServiceOrderTime", { currentServiceOrderTimeId: id });
		return super.applyOrderBy(query);
	};

	getItemGroup(item: Crm.Rest.Model.ObservableCrm_DocumentAttribute & {ServiceOrderTime:KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime>}): ItemGroup {
		if (!item.ServiceOrderTime) {
			// @ts-ignore
			item.ServiceOrderTime = ko.observable(null);
		}
		return window.Crm.Service.ViewModels.DispatchDetailsViewModel.prototype.getServiceOrderPositionItemGroup.call(this,
			item);
	};
}

namespace("Crm.Service.ViewModels").DispatchDetailsDocumentsTabViewModel = DispatchDetailsDocumentsTabViewModel;