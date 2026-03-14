import {namespace} from "@Main/namespace";
import type {GenericListGeolocationViewModel} from "@Crm/GenericListViewModel.Geolocation";
import type {GenericListMapViewModel} from "@Main/GenericListViewModel.Map";
import type {TimeLineEvent} from "@Main/DashboardCalendarWidgetViewModel";
import {HelperString} from "@Main/helper/Helper.String";
import {HelperLookup} from "@Main/helper/Helper.Lookup";
import { HelperBatch } from "@Main/helper/Helper.Batch";
import {DispatchDetailsRelatedOrdersTabViewModel} from "@Crm.Service/DispatchDetailsRelatedOrdersTabViewModel";
import {HelperDownload} from "@Main/helper/Helper.Download";

export class ServiceOrderDispatchListIndexItemViewModel {
	Installations: KnockoutObservableArray<Crm.Service.Rest.Model.ObservableCrmService_Installation>;
}

export class ServiceOrderDispatchListIndexViewModel extends window.Main.ViewModels.GenericListViewModel<Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch, Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch, Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch & ServiceOrderDispatchListIndexItemViewModel> {
	geolocationViewModel: GenericListGeolocationViewModel;
	mapViewModel: GenericListMapViewModel<Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch, Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch, Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch & ServiceOrderDispatchListIndexItemViewModel>;
	latitudeFilterColumn: string;
	longitudeFilterColumn: string;
	status: string;
	context: string;
	parentViewModel: any;

	constructor(parentViewModel: any) {
		let joins = [
			"ServiceOrder", "ServiceOrder.Installation", "ServiceOrder.Installation.Address",
			"ServiceOrder.Installation.Company", "ServiceOrder.Company", "ServiceOrder.ServiceObject",
			"DispatchedUser",
			"ServiceOrder.Station", "ServiceOrder.Initiator", "ServiceOrder.InitiatorPerson",
			"ServiceOrder.ResponsibleUserUser"
		];
		if (parentViewModel instanceof DispatchDetailsRelatedOrdersTabViewModel) {
			joins.push("ServiceOrder.ServiceOrderTimes");
		}
		super("CrmService_ServiceOrderDispatch",
			["Date"],
			["DESC"],
			joins);

		this.geolocationViewModel = new window.Crm.ViewModels.GenericListGeolocationViewModel();
		this.mapViewModel = new window.Main.ViewModels.GenericListMapViewModel<Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch, Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch, Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch & ServiceOrderDispatchListIndexItemViewModel>(this);
		this.latitudeFilterColumn = "ServiceOrder.Latitude";
		this.longitudeFilterColumn = "ServiceOrder.Longitude";
		this.lookups = {
			installationHeadStatuses: {$tableName: "CrmService_InstallationHeadStatus"},
			serviceOrderDispatchRejectReasons: { $tableName: "CrmService_ServiceOrderDispatchRejectReason" },
			serviceOrderDispatchCancellationReasons: { $tableName: "CrmService_ServiceOrderDispatchCancellationReason" },
			serviceOrderDispatchStatuses: {$tableName: "CrmService_ServiceOrderDispatchStatus"},
			countries: {$tableName: "Main_Country"},
			regions: {$tableName: "Main_Region"},
			serviceOrderTypes: {$tableName: "CrmService_ServiceOrderType"},
			servicePriorities: {$tableName: "CrmService_ServicePriority"}
		};
		this.timelineProperties.push({
			Start: "Date",
			End: "Date",
			Caption: window.Helper.String.getTranslatedString("Date")
		});
		const bookmarkMy: Bookmark<Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch> = {
			Category: window.Helper.String.getTranslatedString("Filter"),
			Name: window.Helper.String.getTranslatedString("MyDispatches"),
			Key: "MyDispatches",
			Expression: query => {
				return query.filter(function (it) {
					return it.Username === this.username;
				}, {username: this.currentUser().Id});
			}
		}
		this.bookmarks.push(bookmarkMy);
		this.bookmark(bookmarkMy);
		if (window.AuthorizationManager.isAuthorizedForAction("Dispatch", "SeeAllUsersDispatches")) {
			let dispatchedUsernameFilter = this.getFilter("DispatchedUsername").extend({ filterOperator: { additionalProperties: { Operator: '===', caption: 'Technician' } } });
			let bookmarkAll: Bookmark<Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch> = {
				Category: window.Helper.String.getTranslatedString("Filter"),
				Name: window.Helper.String.getTranslatedString("All"),
				Key: "AllDispatches",
				ApplyFilters: () => {
					dispatchedUsernameFilter(null);
				}
			};
			this.bookmarks.unshift(bookmarkAll);
			delete bookmarkMy.Expression;
			bookmarkMy.ApplyFilters = () => {
				dispatchedUsernameFilter(this.currentUser().Id);
				dispatchedUsernameFilter().displayedValue = window.Helper.User.getDisplayName(this.currentUser());
			}
			dispatchedUsernameFilter.subscribe((dispatchedUsername) => {
				if (dispatchedUsername === null) {
					this.bookmark(bookmarkAll);
				} else if (dispatchedUsername.Value === this.currentUser().Id) {
					this.bookmark(bookmarkMy);
				} else if (dispatchedUsername.Value !== this.currentUser().Id) {
					this.bookmark(null);
				}
			})
		}
		this.parentViewModel = parentViewModel;
	}
	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		if (params && params.status) {
			this.status = params.status;
		}
		switch (this.status) {
			case "scheduled":
				this.pageTitle = HelperString.getTranslatedString("ScheduledDispatches");
				break;
			case "closed":
				this.pageTitle = HelperString.getTranslatedString("ClosedDispatches");
				break;
			default:
				this.pageTitle = HelperString.getTranslatedString("UpcomingDispatches");
		}
		if (params && params.context) {
			this.context = params.context;
		}
		let user = await window.Helper.User.getCurrentUser();
		this.currentUser(user);
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		await this.geolocationViewModel.init();
		if (params && params.status === "closed") {
			this.orderByDirection(["DESC", "DESC"]);
		}
		await super.init(id, params);
	};

	async initItems(items: (Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch & ServiceOrderDispatchListIndexItemViewModel)[]): Promise<(Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch & ServiceOrderDispatchListIndexItemViewModel)[]> {
		const queries = [];
	    for (const dispatch of items) {
			dispatch.Installations = ko.observableArray<Crm.Service.Rest.Model.ObservableCrmService_Installation>(window.Helper.ServiceOrder.getRelatedInstallations(dispatch.ServiceOrder()));
		}
		await HelperBatch.Execute(queries);
		return items;
	};

	dispose(): void {
		this.geolocationViewModel.dispose();
	};

	applyFilters(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch> {
		if (this.filters["DispatchedUsername"]) {
			this.filters["Username"] = this.filters["DispatchedUsername"];
			delete this.filters["DispatchedUsername"];
		}
		if (this.filters["OrderHead.InstallationId"]) {
			this.filters["ServiceOrder.InstallationId"] = this.filters["OrderHead.InstallationId"];
			delete this.filters["OrderHead.InstallationId"];
		}
		if (this.filters["OrderHead.CustomerContactId"]) {
			this.filters["ServiceOrder.CustomerContactId"] = this.filters["OrderHead.CustomerContactId"];
			delete this.filters["OrderHead.CustomerContactId"];
		}
		if (this.filters["OrderHead.ServiceObjectId"]) {
			this.filters["ServiceOrder.ServiceObjectId"] = this.filters["OrderHead.ServiceObjectId"];
			delete this.filters["OrderHead.ServiceObjectId"];
		}
		if (this.filters["OrderHead.StationKey"]) {
			this.filters["ServiceOrder.StationKey"] = this.filters["OrderHead.StationKey"];
			delete this.filters["OrderHead.StationKey"];
		}
		if (this.filters["OrderHead.RegionKey"]) {
			this.filters["ServiceOrder.RegionKey"] = this.filters["OrderHead.RegionKey"];
			delete this.filters["OrderHead.RegionKey"];
		}
		if (this.filters["OrderHead.ZipCode"]) {
			this.filters["ServiceOrder.ZipCode"] = this.filters["OrderHead.ZipCode"];
			delete this.filters["OrderHead.ZipCode"];
		}
		if (this.filters["OrderHead.City"]) {
			this.filters["ServiceOrder.City"] = this.filters["OrderHead.City"];
			delete this.filters["OrderHead.City"];
		}
		if (this.filters["OrderHead.Street"]) {
			this.filters["ServiceOrder.Street"] = this.filters["OrderHead.Street"];
			delete this.filters["OrderHead.Street"];
		}
		if (this.filters["OrderHead.ZipCode"]) {
			this.filters["ServiceOrder.ZipCode"] = this.filters["OrderHead.ZipCode"];
			delete this.filters["OrderHead.ZipCode"];
		}
		if (this.filters["OrderHead.PurchaseOrderNo"]) {
			this.filters["ServiceOrder.PurchaseOrderNo"] = this.filters["OrderHead.PurchaseOrderNo"];
			delete this.filters["OrderHead.PurchaseOrderNo"];
		}
		if (this.filters["OrderHead.Id"]) {
			this.filters["ServiceOrder.Id"] = this.filters["OrderHead.Id"];
			delete this.filters["OrderHead.Id"];
		}
		query = super.applyFilters(query).filter("it.ServiceOrder.Id !== null");
		if (this.filters["Username"]) {
			this.filters["DispatchedUsername"] = this.filters["Username"];
			delete this.filters["Username"];
		}
		if (this.filters["ServiceOrder.InstallationId"]) {
			this.filters["OrderHead.InstallationId"] = this.filters["ServiceOrder.InstallationId"];
			delete this.filters["ServiceOrder.InstallationId"];
		}
		if (this.filters["ServiceOrder.CustomerContactId"]) {
			this.filters["OrderHead.CustomerContactId"] = this.filters["ServiceOrder.CustomerContactId"];
			delete this.filters["ServiceOrder.CustomerContactId"];
		}
		if (this.filters["ServiceOrder.ServiceObjectId"]) {
			this.filters["OrderHead.ServiceObjectId"] = this.filters["ServiceOrder.ServiceObjectId"];
			delete this.filters["ServiceOrder.ServiceObjectId"];
		}
		if (this.filters["ServiceOrder.StationKey"]) {
			this.filters["OrderHead.StationKey"] = this.filters["ServiceOrder.StationKey"];
			delete this.filters["ServiceOrder.StationKey"];
		}
		if (this.filters["ServiceOrder.RegionKey"]) {
			this.filters["OrderHead.RegionKey"] = this.filters["ServiceOrder.RegionKey"];
			delete this.filters["ServiceOrder.RegionKey"];
		}
		if (this.filters["ServiceOrder.ZipCode"]) {
			this.filters["OrderHead.ZipCode"] = this.filters["ServiceOrder.ZipCode"];
			delete this.filters["ServiceOrder.ZipCode"];
		}
		if (this.filters["ServiceOrder.City"]) {
			this.filters["OrderHead.City"] = this.filters["ServiceOrder.City"];
			delete this.filters["ServiceOrder.City"];
		}
		if (this.filters["ServiceOrder.Street"]) {
			this.filters["OrderHead.Street"] = this.filters["ServiceOrder.Street"];
			delete this.filters["ServiceOrder.Street"];
		}
		if (this.filters["ServiceOrder.ZipCode"]) {
			this.filters["OrderHead.ZipCode"] = this.filters["ServiceOrder.ZipCode"];
			delete this.filters["ServiceOrder.ZipCode"];
		}
		if (this.filters["ServiceOrder.PurchaseOrderNo"]) {
			this.filters["OrderHead.PurchaseOrderNo"] = this.filters["ServiceOrder.PurchaseOrderNo"];
			delete this.filters["ServiceOrder.PurchaseOrderNo"];
		}
		if (this.filters["ServiceOrder.Id"]) {
			this.filters["OrderHead.Id"] = this.filters["ServiceOrder.Id"];
			delete this.filters["ServiceOrder.Id"];
		}
		if (this.status === "upcoming") {
			query = query.filter(function (it) {
					return it.StatusKey in this.statusKeys;
				},
				{statusKeys: ["Released", "Read", "InProgress", "SignedByCustomer"]});
		} else if (this.status === "scheduled") {
			query = query.filter(function (it) {
				return it.StatusKey in this.statusKeys;
			}, {statusKeys: ["Scheduled"]});
		} else if (this.status === "closed") {
			query = query.filter(function (it) {
					return it.StatusKey in this.statusKeys;
				},
				{ statusKeys: ["ClosedNotComplete", "ClosedComplete", "Rejected", "Cancelled", "CancelledNotComplete"]});
		} else if (this.context === "dashboard") {
			const todayStart = window.moment().startOf("day").toDate();
			const todayEnd = window.moment().endOf("day").toDate();
			query = query.filter(function (it) {
					return it.StatusKey in this.statusKeys && it.Date >= this.todayStart && it.Date <= this.todayEnd;
				},
				{
					statusKeys: ["Released", "Read", "InProgress", "SignedByCustomer"],
					todayStart: todayStart,
					todayEnd: todayEnd
				});
		}
		return query;
	};

	async confirm(dispatch: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch): Promise<void> {
		window.database.attachOrGet(dispatch.innerInstance);
		this.loading(true);
		dispatch.StatusKey("Released");
		await window.database.saveChanges();
		if (this.parentViewModel instanceof window.Crm.Service.ViewModels.ServiceOrderDetailsViewModel) {
			await this.parentViewModel.init(this.parentViewModel.serviceOrder()?.Id());
		}
		this.showSnackbar(window.Helper.String.getTranslatedString("DispatchConfirmed"), window.Helper.String.getTranslatedString("ViewDispatch"), function () {
			window.location.hash = "/Crm.Service/Dispatch/DetailsTemplate/" + dispatch.Id();
		});
	};

	getDirection(dispatch: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch): number {
		return this.geolocationViewModel.getDirection(dispatch.ServiceOrder());
	};

	getDistance(dispatch: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch): KnockoutComputed<string> {
		return this.geolocationViewModel.getDistance(dispatch.ServiceOrder());
	};

	getItemGroup(dispatch: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch): ItemGroup {
		if (this.context === "dashboard") {
			return null;
		}
		const mDispatchDate = window.moment(dispatch.Date());
		if (this.status === "upcoming" || this.status === "scheduled") {
			if (mDispatchDate.isBefore(window.moment())) {
				return {title: window.Helper.String.getTranslatedString("T_DueTasks"), css: "c-red"};
			}
			if (mDispatchDate.clone().startOf("day").isSame(window.moment().startOf("day"))) {
				return {title: window.Helper.String.getTranslatedString("Today"), css: "c-green"};
			}
			if (mDispatchDate.clone().startOf("day").isSame(window.moment().startOf("day").add(1, "day"))) {
				return {title: window.Helper.String.getTranslatedString("T_TomorrowTasks"), css: "c-blue"};
			}
			if (mDispatchDate.clone().startOf("month").isSame(window.moment().startOf("month"))) {
				return {title: window.Helper.String.getTranslatedString("ThisMonth")};
			}
			if (mDispatchDate.clone().startOf("month").isSame(window.moment().startOf("month").add(1, "month"))) {
				return {title: window.Helper.String.getTranslatedString("NextMonth")};
			}
			return {title: window.Helper.String.getTranslatedString("T_LaterTasks")};
		}
		if (this.status === "closed") {
			if (mDispatchDate.isAfter(window.moment())) {
				return null;
			}
			if (mDispatchDate.clone().startOf("day").isSame(window.moment().startOf("day"))) {
				return {title: window.Helper.String.getTranslatedString("Today"), css: "c-green"};
			}
			if (mDispatchDate.clone().startOf("day").isSame(window.moment().startOf("day").add(-1, "day"))) {
				return {title: window.Helper.String.getTranslatedString("Yesterday"), css: "c-blue"};
			}
			if (mDispatchDate.clone().startOf("month").isSame(window.moment().startOf("month"))) {
				return {title: window.Helper.String.getTranslatedString("ThisMonth")};
			}
			if (mDispatchDate.clone().startOf("month").isSame(window.moment().startOf("month").add(-1, "month"))) {
				return {title: window.Helper.String.getTranslatedString("LastMonth")};
			}
			return {title: window.Helper.String.getTranslatedString("T_Earlier")};
		}
		return null;
	};

	getTimelineEvent(dispatch: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch): TimeLineEvent {
		const startTime = new Date(dispatch.Date());

		const timeLineEvent: TimeLineEvent = {
			title: window.Helper.String.getTranslatedString("Dispatch"),
			start: startTime,
			end: new Date(dispatch.EndDate()),
			allDay: false,
			url: "#/Crm.Service/Dispatch/DetailsTemplate/" + window.ko.unwrap(dispatch.Id)
		};
		if (dispatch.StatusKey() === "Scheduled") {
			timeLineEvent.hatched = true;
		}
		if (dispatch.ServiceOrder()) {
			timeLineEvent.title = dispatch.ServiceOrder().OrderNo();
			timeLineEvent.title += "\r\n" + dispatch.ServiceOrder().ErrorMessage();
			timeLineEvent.entityType = window.Helper.getTranslatedString("Dispatch");
			const address =
				[
					dispatch.ServiceOrder().Name1(),
					dispatch.ServiceOrder().Name2(),
					dispatch.ServiceOrder().Name3(),
					dispatch.ServiceOrder().Street(),
					((dispatch.ServiceOrder().ZipCode() || "") + " " + (dispatch.ServiceOrder().City() || "")).trim(),
					dispatch.ServiceOrder().RegionKey() ? window.Helper.Lookup.getLookupValue(this.lookups.regions, dispatch.ServiceOrder().RegionKey()) : null,
					dispatch.ServiceOrder().CountryKey() ? window.Helper.Lookup.getLookupValue(this.lookups.countries, dispatch.ServiceOrder().CountryKey()) : null
				].filter(Boolean).join("\r\n");
			if (address) {
				timeLineEvent.description = address;
			}
			timeLineEvent.backgroundColor = dispatch.StatusKey() === "ClosedNotComplete" || dispatch.StatusKey() === "ClosedComplete"
				? "#aaaaaa"
				: window.Helper.Lookup.getLookupColor(this.lookups.serviceOrderTypes, dispatch.ServiceOrder().TypeKey());
		}
		if (["ClosedComplete", "ClosedNotComplete"].indexOf(dispatch.StatusKey()) === -1) {
			timeLineEvent.secondaryColor = window.Helper.Lookup.getLookupColor(this.lookups.serviceOrderDispatchStatuses, dispatch.StatusKey);
		}
		return timeLineEvent;
	};

	getIcsLink(): string {
		let link = super.getIcsLink();
		const containsQueryParameters = link.indexOf("?") !== -1;
		link += containsQueryParameters ? "&" : "?";
		link += "DF_DispatchedUsername=" + encodeURIComponent(this.currentUser().Id);
		if (this.status === "upcoming" || this.context === "context") {
			link += "&bookmark=Status%2fDispatchesTitle";
		} else if (this.status === "scheduled") {
			link += "&bookmark=Status%2fScheduledDispatches";
		} else if (this.status === "closed") {
			link += "&bookmark=Status%2fClosedDispatches";
		}
		return link;
	};

	getIcsLinkAllowed(): boolean {
		return window.AuthorizationManager.isAuthorizedForAction("ServiceOrderDispatch", "Ics");
	};

	downloadIcs(): void {
		const cal = window.ics();
		for (const dispatch of this.items()) {
			const zipCode = window.Helper.String.trim(dispatch.ServiceOrder().ZipCode());
			const city = window.Helper.String.trim(dispatch.ServiceOrder().City());
			const street = window.Helper.String.trim(dispatch.ServiceOrder().Street());
			let location = zipCode + " " + city;
			if (street) {
				location = location + ", " + street;
			}
			const startDate = dispatch.Date();
			const endDate = dispatch.EndDate();
			const title = window.Helper.Company.getDisplayName(dispatch.ServiceOrder().Company()) + " - " + dispatch.ServiceOrder().OrderNo();
			const description = window.Helper.Dispatch.getCalendarBodyText(dispatch);
			cal.addEvent(title, description, location, startDate, endDate);
		}

		cal.download("ServiceOrderDispatches");
	};

	getLatitude(item: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch): number {
		const order = ko.unwrap(item.ServiceOrder);
		return order ? ko.unwrap(order.Latitude) : null;
	};

	getLongitude(item: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch): number {
		const order = ko.unwrap(item.ServiceOrder);
		return order ? ko.unwrap(order.Longitude) : null;
	};

	getPopupInformation(item: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch): string {
		const order = ko.unwrap(item.ServiceOrder);
		return order ? ko.unwrap(order.OrderNo) : null;
	};

	getIconName(item: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch): string {
		return "marker_repair";
	};

	CrmService_ServiceOrderDispatchStatusCustomFilter(lookupName: string | $data.EntitySet<any, any> | $data.Queryable<any>, key: string = null, filterExpression: string = null, filterParameters: {} = null): $data.Queryable<any> {
		let viewModel = this;
		if (viewModel.status === "upcoming") {
			return HelperLookup.queryLookup(lookupName, key, "it.Key in this.keys", {keys: ["Released", "Read", "InProgress", "SignedByCustomer"]})
		}
		if (viewModel.status === "scheduled") {
			return HelperLookup.queryLookup(lookupName, key, "it.Key in this.keys", {keys: ["Scheduled"]})
		}
		if (viewModel.status === "closed") {
			return HelperLookup.queryLookup(lookupName, key, "it.Key in this.keys", { keys: ["ClosedNotComplete", "ClosedComplete", "Rejected", "Cancelled","CancelledNotComplete"]})
		}
		return HelperLookup.queryLookup(lookupName, key, filterExpression, filterParameters);
	};

	async downloadDispatchReport(dispatch: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch): Promise<void> {
		const viewModel = this;
		viewModel.loading(true);
		const reportName = `${dispatch.OrderNo()} - ${dispatch.Date().toISOString().substring(0, 10)} - ${Helper.User.getDisplayName(dispatch.DispatchedUser())}.pdf`;
		await HelperDownload.downloadPdf(`~/Crm.Service/Dispatch/GetReportPdf?dispatchId=${dispatch.Id()}`, reportName);
		viewModel.loading(false);
	};
	applyOrderBy(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch> {
		if (this.orderBy() === "OrderHead.OrderNo") {
			return this.orderByDirection() === "ASC" ? query.orderBy("it.ServiceOrder.OrderNo") : query.orderByDescending("it.ServiceOrder.OrderNo");
		}
		return super.applyOrderBy(query);
	};

	getLocalTimeZoneText(dispatch: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch): string {
		return `${window.Helper.String.getTranslatedString("CurrentTimeZoneIs")} ${window.Helper.Date.getTimeZoneWithUtcText(window.moment.tz.guess())}<br>
		${window.Helper.String.getTranslatedString("From")}: ${window.Globalize.formatDate(ko.unwrap(dispatch.Date), {"datetime": "short"})}`;
	}
}

namespace("Crm.Service.ViewModels").ServiceOrderDispatchListIndexViewModel = ServiceOrderDispatchListIndexViewModel;