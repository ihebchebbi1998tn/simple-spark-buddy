import {namespace} from "@Main/namespace";
import type {DispatchDetailsViewModel} from "./DispatchDetailsViewModel";
import { ServiceOrderDetailsExpensePostingsTabViewModelBase } from "./ServiceOrderDetailsExpensePostingsTabViewModel";
import {HelperDispatch} from "@Crm.Service/helper/Helper.Dispatch";

export class DispatchDetailsExpensePostingsTabViewModel extends ServiceOrderDetailsExpensePostingsTabViewModelBase<DispatchDetailsViewModel> {
	dispatch: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch>;
	expensePostingsCanBeAdded = ko.pureComputed<boolean>(() => {
		return HelperDispatch.dispatchIsEditable(this.dispatch()) || this.dispatch().StatusKey() === "SignedByCustomer";
	});
	currentJobItemGroup = ko.pureComputed<ItemGroup>(() => window.Helper.Dispatch.getCurrentJobItemGroup(this));
	parentViewModel: DispatchDetailsViewModel;

	constructor(parentViewModel: DispatchDetailsViewModel) {
		super(parentViewModel);
		this.parentViewModel = parentViewModel;
		this.dispatch = parentViewModel.dispatch;
	}

	applyFilters(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderExpensePosting>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderExpensePosting> {
		query = super.applyFilters(query);
		query = query.filter("it.DispatchId === this.dispatchId", {dispatchId: this.dispatch().Id()});
		return query;
	};

	getItemGroup = window.Crm.Service
		.ViewModels.DispatchDetailsViewModel.prototype.getServiceOrderPositionItemGroup;

	async init(): Promise<void> {
		await super.init();
	};

	expensePostingCanBeEdited(serviceOrderExpensePosting: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderExpensePosting): boolean {
		return (HelperDispatch.dispatchIsEditable(this.dispatch()) || this.dispatch().StatusKey() === "SignedByCustomer") && !serviceOrderExpensePosting.IsClosed();
	};

	expensePostingCanBeDeleted(serviceOrderExpensePosting: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderExpensePosting): boolean {
		const editable = this.expensePostingCanBeEdited(serviceOrderExpensePosting);
		if (editable) {
			return window.AuthorizationManager.currentUserHasPermission("ServiceOrderExpensePosting::Edit");
		}
		return editable;
	};
}

namespace("Crm.Service.ViewModels").DispatchDetailsExpensePostingsTabViewModel = DispatchDetailsExpensePostingsTabViewModel;