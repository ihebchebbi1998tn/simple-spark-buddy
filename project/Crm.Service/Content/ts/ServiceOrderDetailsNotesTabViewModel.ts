import {namespace} from "@Main/namespace";
import type {ServiceOrderDetailsViewModel} from "./ServiceOrderDetailsViewModel";
import type {DispatchDetailsViewModel} from "@Crm.Service/DispatchDetailsViewModel";
import type {ServiceOrderTemplateDetailsViewModel} from "@Crm.Service/ServiceOrderTemplateDetailsViewModel";

export class ServiceOrderDetailsNotesTabViewModel<T extends ServiceOrderDetailsViewModel | DispatchDetailsViewModel | ServiceOrderTemplateDetailsViewModel> extends window.Crm.ViewModels.ContactDetailsNotesTabViewModel {
	parentViewModel: T;

	constructor(parentViewModel: T) {
		super();
		this.contactId(parentViewModel.serviceOrder().Id());
		this.contactName(parentViewModel?.serviceOrder()?.Name() || parentViewModel?.serviceOrder()?.OrderNo());
		this.contactType("ServiceOrder");
		this.minDate(parentViewModel?.serviceOrder()?.CreateDate());
		this.plugin = "Crm.Service";
		this.parentViewModel = parentViewModel;
		delete this.filters.ContactId;
	}

	applyFilters(query: $data.Queryable<Crm.Rest.Model.Crm_Note>): $data.Queryable<Crm.Rest.Model.Crm_Note> {
		query = super.applyFilters(query);
		return query;
	};

	getDispatch(item: Crm.Rest.Model.Crm_Note & {ServiceOrderDispatch:KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch>}): Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch {
		return item.ServiceOrderDispatch ? item.ServiceOrderDispatch() : null;
	} 
	
	async initItems(items: Crm.Rest.Model.ObservableCrm_Note[]) : Promise<Crm.Rest.Model.ObservableCrm_Note[]> {
		const queries = [];
		items.forEach(note => {
			if (note.ExtensionValues().DispatchId() !== null) {
				queries.push({
					queryable: window.database.CrmService_ServiceOrderDispatch.include("DispatchedUser").filter(function (it) {
							return it.Id === this.dispatchId;
						},
						{dispatchId: note.ExtensionValues().DispatchId()}),
					method: "first",
					handler: (result: Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch) => {
						// @ts-ignore
						note.ServiceOrderDispatch = ko.observable(result.asKoObservable());
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

namespace("Crm.Service.ViewModels").ServiceOrderDetailsNotesTabViewModel = ServiceOrderDetailsNotesTabViewModel;
