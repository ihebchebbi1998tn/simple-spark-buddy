import {namespace} from "@Main/namespace";
import type {ServiceOrderDetailsViewModel} from "./ServiceOrderDetailsViewModel";
import type {ServiceOrderTemplateDetailsViewModel} from "./ServiceOrderTemplateDetailsViewModel";
import type {DispatchDetailsViewModel} from "./DispatchDetailsViewModel";

export class ServiceOrderDetailsExpensePostingsTabViewModelBase<T extends ServiceOrderDetailsViewModel | ServiceOrderTemplateDetailsViewModel | DispatchDetailsViewModel> extends window.Main.ViewModels.GenericListViewModel<Crm.Service.Rest.Model.CrmService_ServiceOrderExpensePosting, Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderExpensePosting> {
	accumulatedTotalPrice = ko.pureComputed<number>(() => {
		return this.items().reduce((partialSum, item) => partialSum + item.Amount(), 0);
	})
	serviceOrder: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead>;
	expensePostingsCanBeAdded = ko.pureComputed<boolean>(() => {
		return this.parentViewModel.serviceOrderIsEditable();
	});
	parentViewModel: T;

	constructor(parentViewModel: T) {
		super("CrmService_ServiceOrderExpensePosting",
			[],
			[],
			["FileResource", "Article", "ResponsibleUserObject", "ServiceOrderTime", "ServiceOrderTime.Installation"]);
		this.parentViewModel = parentViewModel;
		this.lookups = parentViewModel.lookups;
		this.serviceOrder = parentViewModel.serviceOrder;
		this.infiniteScroll(true);
	}

	applyFilters(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderExpensePosting>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderExpensePosting> {
		query = super.applyFilters(query);
		return query.filter("it.OrderId === this.orderId", {orderId: this.serviceOrder().Id()});
	};

	getItemGroup(serviceOrderPosition: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderExpensePosting): ItemGroup {
		return window.Helper.ServiceOrder.getServiceOrderPositionItemGroup(serviceOrderPosition);
	};

	async deleteServiceOrderExpensePosting(serviceOrderExpensePosting: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderExpensePosting): Promise<void> {
		let confirm = await window.Helper.Confirm.confirmDeleteAsync();
		if (!confirm) {
			return;
		}
		this.loading(true);
		if (serviceOrderExpensePosting.FileResource()) {
			window.database.remove(serviceOrderExpensePosting.FileResource());
		}
		window.database.remove(serviceOrderExpensePosting);
		await window.database.saveChanges();

		this.filter();
	};

	expensePostingCanBeEdited(serviceOrderExpensePosting: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderExpensePosting): boolean {
		const editable = this.parentViewModel.serviceOrderIsEditable();
		if (editable) {
			return window.AuthorizationManager.currentUserHasPermission("ServiceOrderExpensePosting::Edit");
		}
		return editable && !serviceOrderExpensePosting.IsClosed();
	};

	expensePostingCanBeDeleted(serviceOrderExpensePosting: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderExpensePosting): boolean {
		const editable = this.parentViewModel.serviceOrderIsEditable();
		if (editable) {
			return window.AuthorizationManager.currentUserHasPermission("ServiceOrderExpensePosting::Delete");
		}
		return editable && !serviceOrderExpensePosting.IsClosed();
	};
}

export class ServiceOrderDetailsExpensePostingsTabViewModel extends ServiceOrderDetailsExpensePostingsTabViewModelBase<ServiceOrderDetailsViewModel>{
	
}

namespace("Crm.Service.ViewModels").ServiceOrderDetailsExpensePostingsTabViewModel = ServiceOrderDetailsExpensePostingsTabViewModel;