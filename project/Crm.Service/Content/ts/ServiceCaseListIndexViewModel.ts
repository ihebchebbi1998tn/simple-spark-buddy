import {namespace} from "@Main/namespace";

export class ServiceCaseListIndexItemViewModel {
	CreateUserUser: KnockoutObservable<Main.Rest.Model.Main_User>;
}  
export class ServiceCaseListIndexViewModel extends window.Crm.ViewModels.ContactListViewModel<Crm.Service.Rest.Model.CrmService_ServiceCase, Crm.Service.Rest.Model.ObservableCrmService_ServiceCase> {
	context: string;
	constructor() {
		const joinTags = {
			Selector: "Tags",
			Operation: "orderBy(function(t) { return t.Name; })"
		};
		super("CrmService_ServiceCase",
			"ServiceCaseCreateDate",
			"DESC",
			["AffectedCompany", "AffectedInstallation", "ResponsibleUserUser", "ServiceObject", joinTags, "UserGroup", "ServiceOrderErrorTypes"]);
		let statusKeyFilter = this.getFilter("StatusKey").extend({ filterOperator: { operator: "in",
				additionalProperties: {
					caption: "Status",
					getDisplayedValue: (keys) => this.getDisplayedValueFromLookups("CrmService_ServiceCaseStatus", keys)
				}
			}
		});
		let bookmarkAll = {
			Category: window.Helper.String.getTranslatedString("View"),
			Name: window.Helper.String.getTranslatedString("All"),
			Key: "All",
			ApplyFilters: () => {
				statusKeyFilter(null);
			}
		};
		this.bookmarks.push(bookmarkAll);
		let bookmarkOpen = {
			Category: window.Helper.String.getTranslatedString("View"),
			Name: window.Helper.String.getTranslatedString("OpenServiceCases"),
			Key: "OpenServiceCases",
			ApplyFilters: () => {
				statusKeyFilter(this.openStatuses());
			}
		};
		this.bookmarks.push(bookmarkOpen);
		this.bookmarks.push({
			Category: window.Helper.String.getTranslatedString("View"),
			Name: window.Helper.String.getTranslatedString("ServiceCasesWithoutAssignment"),
			Key: "ServiceCasesWithoutAssignment",
			Expression: query => {
				return window.Helper.ServiceCase.getServiceCasesWithoutAssignmentQuery(query, this.openStatuses());
			}
		});
		this.bookmarks.push({
			Category: window.Helper.String.getTranslatedString("View"),
			Name: window.Helper.String.getTranslatedString("EntityWithoutActivityDays").replace("{0}", window.Helper.String.getTranslatedString("ServiceCases")).replace("{1}", "3"),
			Key: "EntityWithoutActivityDays3",
			Expression: query => {
				return window.Helper.ServiceCase.getServiceCasesWithoutActivityQuery(query, this.openStatuses());
			}
		});
		let bookmarkClosed = {
			Category: window.Helper.String.getTranslatedString("View"),
			Name: window.Helper.String.getTranslatedString("ClosedServiceCases"),
			Key: "ClosedServiceCases",
			ApplyFilters: () => {
				statusKeyFilter(this.closedStatuses());
			}
		};
		this.bookmarks.push(bookmarkClosed);
		this.bookmark(bookmarkOpen);
		statusKeyFilter.subscribe((statusKey) => {
			if (statusKey === null){
				this.bookmark(bookmarkAll);
			} else if (window._.isEqual(this.openStatuses().sort(), statusKey.Value.sort())) {
				this.bookmark(bookmarkOpen);
			} else if (window._.isEqual(this.closedStatuses().sort(), statusKey.Value.sort())) {
				this.bookmark(bookmarkClosed);
			} else {
				this.bookmark(null);
			}
		});
		this.lookups = {
			errorCodes: { $tableName: "CrmService_ErrorCode" },
			serviceCaseCategories: {$tableName: "CrmService_ServiceCaseCategory"},
			serviceCaseStatuses: {$tableName: "CrmService_ServiceCaseStatus"},
			servicePriorities: {$tableName: "CrmService_ServicePriority"}
		};

		if (window.Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode === "JobPerInstallation") {
			const createServiceOrderBulkAction = {
				Name: "CreateServiceOrder",
				Modal: {
					Target: "#modal",
					Route: "Crm.Service/ServiceCase/CreateServiceOrderTemplate"
				}
			};
			this.bulkActions.push(createServiceOrderBulkAction);
			const addToServiceOrderBulkAction = {
				Name: "AddToServiceOrder",
				Modal: {
					Target: "#modal",
					Route: "Crm.Service/ServiceCase/AddToServiceOrder"
				}
			};
			this.bulkActions.push(addToServiceOrderBulkAction);
		}

		if (window.AuthorizationManager.isAuthorizedForAction("ServiceCase", "SetStatusMultiple")) {
			const setStatusBulkAction = {
				Name: "SetStatus",
				Modal: {
					Target: "#smModal",
					Route: "Crm.Service/ServiceCase/SetStatusTemplate"
				}
			};
			this.bulkActions.push(setStatusBulkAction);
		}
	}
	
	applyFilters(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceCase>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceCase> {
		if (ko.unwrap(this.filters["ResponsibleUser-Usergroup"])) {
			const responsibleUserGroup = this.filters["ResponsibleUser-Usergroup"]();
			query = query.filter(function (it) {
				return it.UserGroupKey === this.usergroup;
			}, {usergroup: responsibleUserGroup.Value});
		}
		query = window.Crm.ViewModels.ContactListViewModel.prototype.applyFilters.call(this, query);
		return query;
	};

	applyOrderBy(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceCase>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceCase> {
		if (this.orderBy() === "Priority" && this.orderByDirection() === "ASC") {
			return query.orderBy("it.PriorityKey");
		}
		if (this.orderBy() === "Priority") {
			return query.orderByDescending("it.PriorityKey");
		}
		if (this.orderBy() === "Status" && this.orderByDirection() === "DESC") {
			return query.orderByDescending("it.StatusKey");
		}
		if (this.orderBy() === "Status") {
			return query.orderBy("it.StatusKey");
		}
		return super.applyOrderBy(query);
	};

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		if (params && params.context) {
			this.context = params.context;
		}
		let user = await window.Helper.User.getCurrentUser();
		this.currentUser(user);
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		await super.init(id, params);
	};

	closedStatuses(): string[] {
		return this.lookups
			.serviceCaseStatuses.$array.filter(function (x) {
				return x.Groups === "Closed"
			}).map(function (x) {
				return x.Key
			});
	}

	openStatuses(): string[] {
		return this.lookups
			.serviceCaseStatuses.$array.filter(function (x) {
				return x.Key !== null && x.Groups !== "Closed"
			}).map(function (x) {
				return x.Key
			});
	}

	async initItems(items: (Crm.Service.Rest.Model.ObservableCrmService_ServiceCase)[]): Promise<(Crm.Service.Rest.Model.ObservableCrmService_ServiceCase & ServiceCaseListIndexItemViewModel)[]> {
		const queries = [];
		let result = await super.initItems(items) as (Crm.Service.Rest.Model.ObservableCrmService_ServiceCase & ServiceCaseListIndexItemViewModel)[];
		result.forEach(serviceCase => {
			queries.push({
				queryable: window.database.Main_User.filter(function (it) {
						return it.Id === this.createUser;
					},
					{createUser: serviceCase.ServiceCaseCreateUser}),
				method: "toArray",
				handler: function (users) {
					serviceCase.CreateUserUser = ko.observable(null);
					if (users.length > 0) {
						serviceCase.CreateUserUser(users[0]);
					}
					return items;
				}
			});
		});
		await window.Helper.Batch.Execute(queries)
		return result;
	};

	assignToMe =
		window.Crm.Service.ViewModels.ServiceCaseDetailsViewModel.prototype.assignToMe;
	onUserGroupSelect =
		window.Crm.Service.ViewModels.ServiceCaseDetailsViewModel.prototype.onUserGroupSelect;
}

namespace("Crm.Service.ViewModels").ServiceCaseListIndexViewModel = ServiceCaseListIndexViewModel;