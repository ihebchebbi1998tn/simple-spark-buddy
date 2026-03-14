import {namespace} from "@Main/namespace";
export class ServiceContractListIndexItemViewModel {
	NextDate: KnockoutObservable<Date>;
}

export class ServiceContractListIndexViewModel extends window.Crm.ViewModels.ContactListViewModel<Crm.Service.Rest.Model.CrmService_ServiceContract, Crm.Service.Rest.Model.ObservableCrmService_ServiceContract> {
	constructor() {
		const joinTags = {
			Selector: "Tags",
			Operation: "orderBy(function(t) { return t.Name; })"
		};
		super("CrmService_ServiceContract",
			"ContractNo",
			"ASC",
			["ParentCompany", "ResponsibleUserUser", "ServiceObject", "Installations", "Installations.Child", joinTags]);
		this.lookups = {
			serviceContractStatuses: {$tableName: "CrmService_ServiceContractStatus"},
			serviceContractTypes: {$tableName: "CrmService_ServiceContractType"}
		};
	}

	applyFilters(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceContract>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceContract> {
		const dateFilter = this.filters["MaintenancePlans"];
		const date = ko.unwrap(dateFilter) || null;
		delete this.filters["MaintenancePlans"];
		query = super.applyFilters(query);
		return this.filterByNextFireDate(query, date, dateFilter);
	};

	filterByNextFireDate(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceContract>, date: { Value: Date }, filter): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceContract> {
		if (date && date.Value) {
			query = query.filter("filterByNextFireDate", {date: date});
		}
		this.filters["MaintenancePlans"] = filter;
		return query;
	};

	async getNextDate(serviceContractId: string): Promise<Date> {
		if (!window.AuthorizationManager.currentUserHasPermission("Sync::MaintenancePlan")) {
			return null;
		}
		let array = await window.database.CrmService_MaintenancePlan
			.filter("it.ServiceContractId == this.contractId",
				{contractId: ko.unwrap(serviceContractId)})
			.map(function (it) {
				return it.NextDate;
			})
			.toArray();
		if (array.length > 0) {
			return array[0];
		}
		return null;
	}

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		let user = await window.Helper.User.getCurrentUser();
		this.currentUser(user);
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		this.bookmarks.push({
			Category: window.Helper.String.getTranslatedString("Show"),
			Name: window.Helper.String.getTranslatedString("All"),
			Key: "All",
			Expression: query => query
		});
		this.lookups.serviceContractStatuses.$array
			.filter(serviceContractStatus => serviceContractStatus.Key !== null).forEach(serviceContractStatus => {
			const bookmark = {
				Category: window.Helper.String.getTranslatedString("Show"),
				Name: serviceContractStatus.Value,
				Key: serviceContractStatus.Key,
				Expression: function (query) {
					return query.filter(function (it) {
							return it.StatusKey === this.statusKey;
						},
						{statusKey: serviceContractStatus.Key});
				}
			};
			this.bookmarks.push(bookmark);
			if (!this.bookmark()) {
				this.bookmark(bookmark);
			}
			});
		await super.init(id, params);
	};

	async initItems(items: Crm.Service.Rest.Model.ObservableCrmService_ServiceContract[]): Promise<(Crm.Service.Rest.Model.ObservableCrmService_ServiceContract & ServiceContractListIndexItemViewModel)[]> {
		let result = await super.initItems(items) as (Crm.Service.Rest.Model.ObservableCrmService_ServiceContract & ServiceContractListIndexItemViewModel)[];
		for (const contract of result) {
			contract.NextDate = ko.observable(null);
			let date = await this.getNextDate(contract.Id())
			contract.NextDate(date);
		}
		return result;
	};
}

namespace("Crm.Service.ViewModels").ServiceContractListIndexViewModel = ServiceContractListIndexViewModel;