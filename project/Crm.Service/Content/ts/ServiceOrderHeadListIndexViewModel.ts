import {namespace} from "@Main/namespace";
import type {GenericListGeolocationViewModel} from "@Crm/GenericListViewModel.Geolocation";
import type {GenericListMapViewModel} from "@Main/GenericListViewModel.Map";
import moment from "moment";

export class ServiceOrderHeadListIndexItemViewModel {
	Installations: KnockoutObservableArray<Crm.Service.Rest.Model.ObservableCrmService_Installation>;
}

export class ServiceOrderHeadListIndexViewModel extends window.Crm.ViewModels.ContactListViewModel<Crm.Service.Rest.Model.CrmService_ServiceOrderHead, Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead> {
	geolocationViewModel: GenericListGeolocationViewModel;
	mapViewModel: GenericListMapViewModel<Crm.Service.Rest.Model.CrmService_ServiceOrderHead, Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead>;
	currentUserUsergroupIds: KnockoutComputed<string[]>;
	fastLanePriorityKeys: { [key: string]: boolean };

	constructor() {
		const joinServiceOrderStatisticsKeys = {
			Selector: "ServiceOrderStatisticsKeys",
			Operation: "filter(function(it2) { return it2.DispatchId === null; })"
		};
		const joinTags = {
			Selector: "Tags",
			Operation: "orderBy(function(t) { return t.Name; })"
		};
		super("CrmService_ServiceOrderHead",
			["Planned", "PlannedTime", "OrderNo"],
			["DESC", "DESC", "DESC"],
			["Installation", "Installation.Address", "Installation.Company", "Company", "ServiceObject", "ServiceOrderTemplate", "ServiceOrderTimes", "ServiceOrderTimes.Installation", "Station", joinServiceOrderStatisticsKeys, joinTags]);
		this.geolocationViewModel = new window.Crm.ViewModels.GenericListGeolocationViewModel();
		this.mapViewModel = new window.Main.ViewModels.GenericListMapViewModel(this);
		this.contactType = "ServiceOrder";
		this.lookups = {
			countries: {$tableName: "Main_Country"},
			errorCodes: { $tableName: "CrmService_ErrorCode" },
			installationHeadStatuses: {$tableName: "CrmService_InstallationHeadStatus"},
			regions: {$tableName: "Main_Region"},
			serviceOrderNoInvoiceReasons: {$tableName: "CrmService_ServiceOrderNoInvoiceReason"},
			serviceOrderTypes: {$tableName: "CrmService_ServiceOrderType"},
			servicePriorities: {$tableName: "CrmService_ServicePriority"},
			serviceOrderStatuses: {$tableName: "CrmService_ServiceOrderStatus"},
			statisticsKeyCause: {$tableName: "CrmService_StatisticsKeyCause"},
			statisticsKeyFaultImage: {$tableName: "CrmService_StatisticsKeyFaultImage"}
		};
		let statusKeyFilter = this.getFilter("StatusKey").extend({ filterOperator: { operator: "in",
				additionalProperties: {
					caption: "Status",
					getDisplayedValue: (keys) => this.getDisplayedValueFromLookups("CrmService_ServiceOrderStatus", keys)
				}
			}
		});
		const activeBookmark = {
			Category: window.Helper.String.getTranslatedString("Filter"),
			Name: window.Helper.String.getTranslatedString("All"),
			Key: "All",
			ApplyFilters: () => {
				statusKeyFilter(null);
			}
		}
		this.bookmarks.push(activeBookmark);
		this.bookmark(activeBookmark);
		let bookmarkStatusKeys= {
			"All": null,
			"ReadyForScheduling": ["ReadyForScheduling", "PartiallyCompleted"],
			"OpenOrders": ["New", "ReadyForScheduling", "Scheduled", "PartiallyReleased", "Released", "InProgress", "PartiallyCompleted", "Completed", "PostProcessing", "ReadyForInvoice", "Invoiced"],
			"ClosedServiceOrders": ["Closed"],
			"PostProcessing": ["Completed", "PostProcessing","ReadyForInvoice"]
		}
		this.bookmarks.push({
			Category: window.Helper.String.getTranslatedString("Filter"),
			Name: window.Helper.String.getTranslatedString("ReadyForScheduling"),
			Key: "ReadyForScheduling",
			ApplyFilters: () => {
				statusKeyFilter(bookmarkStatusKeys["ReadyForScheduling"]);
			}
		});
		this.bookmarks.push({
			Category: window.Helper.String.getTranslatedString("Filter"),
			Name: window.Helper.String.getTranslatedString("OpenOrders"),
			Key: "OpenOrders",
			ApplyFilters: () => {
				statusKeyFilter(bookmarkStatusKeys["OpenOrders"]);
			}
		});
		this.bookmarks.push({
			Category: window.Helper.String.getTranslatedString("Filter"),
			Name: window.Helper.String.getTranslatedString("ClosedServiceOrders"),
			Key: "ClosedServiceOrders",
			ApplyFilters: () => {
				statusKeyFilter(bookmarkStatusKeys["ClosedServiceOrders"]);
			}
		});
		this.bookmarks.push({
			Category: window.Helper.String.getTranslatedString("Filter"),
			Name: window.Helper.String.getTranslatedString("PostProcessing"),
			Key: "PostProcessing",
			ApplyFilters: () => {
				statusKeyFilter(bookmarkStatusKeys["PostProcessing"]);
			}
		});
		statusKeyFilter.subscribe((statusKey) => {
			let matchingBookmark = this.bookmarks().find(bookmark => {
				return window._.isEqual((bookmarkStatusKeys[bookmark.Key] || []).sort(), (statusKey?.Value || []).sort())
			}) || null;
			this.bookmark(matchingBookmark);
		});
		if (!window.AuthorizationManager.isAuthorizedForAction("ServiceOrder", "SeeAllUsersServiceOrders")) {
			this.bookmark(this.bookmarks().find(x => x.Key == "ReadyForScheduling"))
		}
		this.bookmarks.push({
			Category: window.Helper.String.getTranslatedString("Filter"),
			Name: window.Helper.String.getTranslatedString("NoActivityForOneWeek"),
			Key: "NoActivityForOneWeek",
			Expression: query => query.filter("it.LastActivity <= this.date", { date: moment().add(-7, "d") })
		});
		this.replicationHintInfo = { SettingName : "ClosedServiceOrderHistory", HintTranslationKey : "IncompleteServiceOrderHistoryHint" };
		this.currentUserUsergroupIds = ko.pureComputed(() => {
			return this.currentUser()
				? this.currentUser().UsergroupIds
				: [];
		});
	}

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		let user = await window.Helper.User.getCurrentUser();
		this.currentUser(user);
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		this.fastLanePriorityKeys = this.lookups.servicePriorities.$array.reduce(function (map, x) {
			if (x.IsFastLane === true) {
				map[x.Key] = true;
			}
			return map;
		}, {});
		await super.init(id, params);
		await this.geolocationViewModel.init();
	};

	dispose(): void {
		this.geolocationViewModel.dispose();
	};

	getDirection(serviceOrder: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead): number {
		return this.geolocationViewModel.getDirection(serviceOrder);
	};

	getDistance(serviceOrder: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead): KnockoutComputed<string> {
		return this.geolocationViewModel.getDistance(serviceOrder);
	};

	getItemGroup(serviceOrder: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead): ItemGroup {
		const priorityKey = serviceOrder.PriorityKey();
		if (this.fastLanePriorityKeys && this.fastLanePriorityKeys.hasOwnProperty(priorityKey)) {
			return {title: window.Helper.String.getTranslatedString("FastLane"), css: "c-red"};
		}
		return null;
	};

	applyOrderBy(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderHead>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderHead> {
		const keys = Object.keys(this.fastLanePriorityKeys);
		if (keys.length > 0) {
			// @ts-ignore
			query = query.orderByDescending("orderByFastLanePriority", {keys: keys});
		}
		return super.applyOrderBy(query);
	};

	applyFilters(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderHead>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderHead> {
		if (this.filters["ServiceCase.ServiceCaseNo"]) {
			this.filters["ServiceCaseNo"] = this.filters["ServiceCase.ServiceCaseNo"];
			delete this.filters["ServiceCase.ServiceCaseNo"];
		}
		if (ko.unwrap(this.filters["ResponsibleUser-Usergroup"])) {
			const responsibleUserGroup = this.filters["ResponsibleUser-Usergroup"]();
			delete this.filters["ResponsibleUser-Usergroup"];
			if (!ko.unwrap(this.filters["ResponsibleUser"])) {
				query = query.filter(function (it) {
					return it.UserGroupKey === this.usergroup;
				}, {usergroup: responsibleUserGroup.Value});
			}
		}
		query = super.applyFilters(query);
		if (!window.AuthorizationManager.isAuthorizedForAction("ServiceOrder", "SeeAllUsersServiceOrders")) {
			query = query.filter(function (serviceOrder) {
					return (serviceOrder.CreateUser === this.username ||
						serviceOrder.PreferredTechnician === this.username ||
						(serviceOrder.PreferredTechnician === null && serviceOrder.PreferredTechnicianUsergroupKey in this.usergroups));
				},
				{
					username: this.currentUser().Id,
					usergroups: this.currentUserUsergroupIds()
				});
		}
		query = query.filter(function (serviceOrder) {
			return serviceOrder.IsTemplate === false;
		});
		return query;
	};

	getMarkerColor = window.Helper.Map.getMapMarkerColor;
	
	getMarkerContent = window.Helper.Map.getMapMarkerContent;

	getPopupInformation = window.Helper.Map.getPopupItemTemplateInformation;

	getIconName(item: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead): string {

		return "marker_repair";
	};

	async initItems(items: (Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead)[]): Promise<(Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead & ServiceOrderHeadListIndexItemViewModel)[]> {
		let result = await super.initItems(items) as (Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead & ServiceOrderHeadListIndexItemViewModel)[];
		for (const order of result) {
			order.Installations = ko.observableArray<Crm.Service.Rest.Model.ObservableCrmService_Installation>(window.Helper.ServiceOrder.getRelatedInstallations(order));
		}
		return result;
	};
}

namespace("Crm.Service.ViewModels").ServiceOrderHeadListIndexViewModel = ServiceOrderHeadListIndexViewModel;