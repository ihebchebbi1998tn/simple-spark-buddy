import type {ServiceOrderDetailsViewModel} from "./ServiceOrderDetailsViewModel";
import {namespace} from "@Main/namespace";
import type {ContactDetailsViewModel} from "@Crm/ContactDetailsViewModel";
import type {ServiceOrderTemplateDetailsViewModel} from "./ServiceOrderTemplateDetailsViewModel";
import type {DispatchDetailsViewModel} from "./DispatchDetailsViewModel";

export class ServiceOrderDetailsDocumentsTabViewModelBase<T extends ServiceOrderDetailsViewModel | ServiceOrderTemplateDetailsViewModel | DispatchDetailsViewModel> extends window.Crm.ViewModels.ContactDetailsDocumentsTabViewModel{
	serviceOrder: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead>
	constructor(parentViewModel: T) {
		super(parentViewModel as ContactDetailsViewModel);
		this.serviceOrder = parentViewModel.serviceOrder;
		this.orderBy(["FileName"]);
		this.orderByDirection(["ASC"]);
		this.contactId(this.serviceOrder().Id());
	}

	applyJoins(query: $data.Queryable<Crm.Rest.Model.Crm_DocumentAttribute>): $data.Queryable<Crm.Rest.Model.Crm_DocumentAttribute> {
		return super.applyJoins(query);
	};

	applyFilters(query: $data.Queryable<Crm.Rest.Model.Crm_DocumentAttribute>): $data.Queryable<Crm.Rest.Model.Crm_DocumentAttribute> {
		query = super.applyFilters(query);
		query = query.filter("it.ReferenceKey === this.orderId", { orderId: this.serviceOrder().Id() });
		return query;
	};

	getItemGroup(item: Crm.Rest.Model.ObservableCrm_DocumentAttribute & {ServiceOrderTime:KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime>}): ItemGroup {
		if (!item.ServiceOrderTime) {
			// @ts-ignore
			item.ServiceOrderTime = ko.observable(null);
		}
		return window.Helper.ServiceOrder.getServiceOrderPositionItemGroup(item);
	}

	getDispatch(item: Crm.Rest.Model.Crm_DocumentAttribute & {ServiceOrderDispatch:KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch>}): Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch {
		return item.ServiceOrderDispatch ? item.ServiceOrderDispatch() : null;
	}
	
	async initItems(items: Crm.Rest.Model.ObservableCrm_DocumentAttribute[]): Promise<Crm.Rest.Model.ObservableCrm_DocumentAttribute[]>{
		const queries = [];
		items.forEach(documentAttribute => {
			if (documentAttribute.ExtensionValues().DispatchId() !== null) {
				queries.push({
					queryable: window.database.CrmService_ServiceOrderDispatch.include("DispatchedUser").filter(function (it) {
							return it.Id === this.dispatchId;
						},
						{dispatchId: documentAttribute.ExtensionValues().DispatchId()}),
					method: "first",
					handler: (result: Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch) => {
						// @ts-ignore
						documentAttribute.ServiceOrderDispatch = ko.observable(result.asKoObservable());
						return items;
					}
				});
			}
			if (documentAttribute.ExtensionValues().ServiceOrderTimeId() !== null) {
				queries.push({
					queryable: window.database.CrmService_ServiceOrderTime.include("Installation").filter(function (it) {
							return it.Id === this.serviceOrderTimeId;
						},
						{serviceOrderTimeId: documentAttribute.ExtensionValues().ServiceOrderTimeId()}),
					method: "first",
					handler: (result: Crm.Service.Rest.Model.CrmService_ServiceOrderTime) => {
						// @ts-ignore
						documentAttribute.ServiceOrderTime = ko.observable(result.asKoObservable());
						// @ts-ignore
						documentAttribute.itemGroup = this.getItemGroup(documentAttribute.asKoObservable());
						return items;
					}
				});
			}
		});
		await window.Helper.Batch.Execute(queries);
		await super.initItems(items);
		return items;
	}
}

export class ServiceOrderDetailsDocumentsTabViewModel extends ServiceOrderDetailsDocumentsTabViewModelBase<ServiceOrderDetailsViewModel>{
}

namespace("Crm.Service.ViewModels").ServiceOrderDetailsDocumentsTabViewModel = ServiceOrderDetailsDocumentsTabViewModel;