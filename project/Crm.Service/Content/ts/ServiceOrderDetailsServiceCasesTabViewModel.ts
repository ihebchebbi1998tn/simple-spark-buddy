import {namespace} from "@Main/namespace";
import type {ServiceOrderDetailsViewModel} from "./ServiceOrderDetailsViewModel";
import type {ServiceOrderTemplateDetailsViewModel} from "./ServiceOrderTemplateDetailsViewModel";
import type {DispatchDetailsViewModel} from "./DispatchDetailsViewModel";

export class ServiceOrderDetailsServiceCasesTabItemViewModel {
	CreateUserUser: KnockoutObservable<Main.Rest.Model.Main_User>
}
export class ServiceOrderDetailsServiceCasesTabViewModelBase<T extends ServiceOrderDetailsViewModel | ServiceOrderTemplateDetailsViewModel | DispatchDetailsViewModel> extends window.Main.ViewModels.GenericListViewModel<Crm.Service.Rest.Model.CrmService_ServiceCase, Crm.Service.Rest.Model.ObservableCrmService_ServiceCase> {
	serviceOrder: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead>;

	constructor(parentViewModel: T) {
		super("CrmService_ServiceCase",
			["ServiceOrderTime.PosNo", "ServiceCaseNo", "ErrorMessage"],
			["ASC", "ASC", "ASC"],
			["ServiceOrderTime", "ServiceOrderTime.Installation"]);

		this.lookups = parentViewModel.lookups || {};
		this.lookups.serviceCaseCategories = {$tableName: "CrmService_ServiceCaseCategory"};
		this.lookups.serviceCaseStatuses = {$tableName: "CrmService_ServiceCaseStatus"};
		this.lookups.servicePriorities = {$tableName: "CrmService_ServicePriority"};
		this.serviceOrder = parentViewModel.serviceOrder;
		this.infiniteScroll(true);
	}

	async init(): Promise<void> {
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		await super.init();
	}

	applyFilters(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceCase>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceCase> {
		query = super.applyFilters(query);
		return query.filter("it.Id === this.serviceCaseId || it.ServiceOrderTime.OrderId === this.serviceOrderId || it.OriginatingServiceOrderId === this.serviceOrderId",
			{
				serviceOrderId: this.serviceOrder().Id(),
				serviceCaseId: this.serviceOrder().ServiceCaseKey()
			});
	};

	getItemGroup =
		window.Crm.Service.ViewModels.DispatchDetailsViewModel.prototype.getServiceOrderPositionItemGroup;

	async initItems(items: (Crm.Service.Rest.Model.ObservableCrmService_ServiceCase & ServiceOrderDetailsServiceCasesTabItemViewModel)[]): Promise<(Crm.Service.Rest.Model.ObservableCrmService_ServiceCase & ServiceOrderDetailsServiceCasesTabItemViewModel)[]> {
		const queries = [];
		for (const serviceCase of items) {
			queries.push({
				queryable: window.database.Main_User.filter(function (it) {
						return it.Id === this.createUser;
					},
					{createUser: serviceCase.ServiceCaseCreateUser()}),
				method: "toArray",
				handler: function (users) {
					serviceCase.CreateUserUser = ko.observable(null);
					if (users.length > 0) {
						serviceCase.CreateUserUser(users[0]);
					}
					return items;
				}
			});
		}
		await window.Helper.Batch.Execute(queries);
		return items;
	};
	assignToMe =
		window.Crm.Service.ViewModels.ServiceCaseDetailsViewModel.prototype.assignToMe;
	onUserGroupSelect =
		window.Crm.Service.ViewModels.ServiceCaseDetailsViewModel.prototype.onUserGroupSelect;
}

export class ServiceOrderDetailsServiceCasesTabViewModel extends ServiceOrderDetailsServiceCasesTabViewModelBase<ServiceOrderDetailsViewModel>{	
}

namespace("Crm.Service.ViewModels").ServiceOrderDetailsServiceCasesTabViewModel = ServiceOrderDetailsServiceCasesTabViewModel;