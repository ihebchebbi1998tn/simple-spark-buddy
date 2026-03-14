import {namespace} from "@Main/namespace";
import type {ServiceOrderDetailsViewModel} from "./ServiceOrderDetailsViewModel";

export class ServiceOrderDetailsDispatchesTabViewModel extends window.Main.ViewModels.GenericListViewModel<Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch, Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch> {
	lookups: LookupType = {
		serviceOrderDispatchRejectReasons: {$tableName: "CrmService_ServiceOrderDispatchRejectReason"},
		serviceOrderDispatchStatuses: {$tableName: "CrmService_ServiceOrderDispatchStatus"},
		serviceOrderStatuses: {$tableName: "CrmService_ServiceOrderStatus"}
	};
	serviceOrder: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead>;
	dispatchesCanBeAdded = ko.pureComputed<boolean>(() => {
		if (!this.serviceOrder()) {
			return false;
		}
		const serviceOrderStatus = this.lookups.serviceOrderStatuses.$array.find(x => x.Key === this.serviceOrder().StatusKey());
		return window._.intersection((serviceOrderStatus.Groups || "").split(","), [
			"Scheduling", "InProgress"
		]).length > 0;
	});

	constructor(parentViewModel: ServiceOrderDetailsViewModel) {
		super(
			"CrmService_ServiceOrderDispatch",
			["Date"],
			["ASC"],
			["DispatchedUser"]);
		this.serviceOrder = parentViewModel.serviceOrder;
		this.getFilter("OrderId").extend({filterOperator: "==="})(this.serviceOrder().Id());
		this.infiniteScroll(true);
	}

	applyFilters(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch> {
		if (this.filters["DispatchedUsername"]) {
			this.filters["Username"] = this.filters["DispatchedUsername"];
			delete this.filters["DispatchedUsername"];
		}
		if (this.filters["OrderHead.OrderNo"]) {
			this.filters["ServiceOrder.OrderNo"] = this.filters["OrderHead.OrderNo"];
			delete this.filters["OrderHead.OrderNo"];
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
		query = super.applyFilters(query).filter("it.ServiceOrder.Id !== null");
		if (this.filters["Username"]) {
			this.filters["DispatchedUsername"] = this.filters["Username"];
			delete this.filters["Username"];
		}
		if (this.filters["ServiceOrder.OrderNo"]) {
			this.filters["OrderHead.OrderNo"] = this.filters["ServiceOrder.OrderNo"];
			delete this.filters["ServiceOrder.OrderNo"];
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
		return query;
	};

	applyJoins(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch> {
		query = query.include("expandAvatar");
		return super.applyJoins(query);
	};

	confirm = window.Crm.Service.ViewModels.ServiceOrderDispatchListIndexViewModel.prototype.confirm;

	downloadDispatchReport = window.Crm.Service.ViewModels.ServiceOrderDispatchListIndexViewModel.prototype.downloadDispatchReport;

	async init(): Promise<void> {
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		await super.init();
	};
	
	getLocalTimeZoneText = window.Crm.Service.ViewModels.ServiceOrderDispatchListIndexViewModel.prototype.getLocalTimeZoneText;
}

namespace("Crm.Service.ViewModels").ServiceOrderDetailsDispatchesTabViewModel = ServiceOrderDetailsDispatchesTabViewModel;