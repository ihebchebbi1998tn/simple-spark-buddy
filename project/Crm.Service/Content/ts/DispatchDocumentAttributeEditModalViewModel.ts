import {namespace} from "@Main/namespace";
import type {DispatchDetailsViewModel} from "./DispatchDetailsViewModel";

export class DispatchDocumentAttributeEditModalViewModel extends window.Crm.ViewModels.BaseDocumentAttributeEditModalViewModel {
	dispatch: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch>;
	serviceOrder: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead>;

	constructor(parentViewModel: DispatchDetailsViewModel) {
		super();
		this.dispatch = parentViewModel.dispatch;
		this.serviceOrder = parentViewModel.serviceOrder;
	}

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		if (!id) {
			this.documentAttribute().ContactName(window.Helper.ServiceOrder.getDisplayName(this.serviceOrder()));
			this.documentAttribute().ContactType("ServiceOrder");
			this.documentAttribute().ExtensionValues().DispatchId(ko.unwrap(this.dispatch) ? this.dispatch().Id() : null);
			this.documentAttribute().ReferenceKey(this.serviceOrder().Id());
			this.documentAttribute().ExtensionValues().ServiceOrderTimeId(params.serviceOrderTimeId ||
				(ko.unwrap(this.dispatch) ? this.dispatch().CurrentServiceOrderTimeId() : null) ||
				null);
		}
	};

	getServiceOrderTimeAutocompleteDisplay(serviceOrderTime: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime): string {
		if (this.dispatch && this.dispatch()) {
			return window.Helper.ServiceOrderTime.getAutocompleteDisplay(serviceOrderTime, this.dispatch().CurrentServiceOrderTimeId());
		}
		return window.Helper.ServiceOrderTime.getAutocompleteDisplay(serviceOrderTime, null);
	};

	getServiceOrderTimeAutocompleteFilter(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTime>, term: string): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTime> {
		query = query.filter(function (it) {
				return it.OrderId === this.orderId;
			},
			{orderId: this.serviceOrder().Id()});
		if (term) {
			query = window.Helper.String.contains(query, term, ["Description.toLowerCase()", "ItemNo.toLowerCase()", "PosNo.toLowerCase()"]);
		}
		return query;
	};
}

namespace("Crm.Service.ViewModels").DispatchDocumentAttributeEditModalViewModel = DispatchDocumentAttributeEditModalViewModel;