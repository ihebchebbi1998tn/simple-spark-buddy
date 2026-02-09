import {namespace} from "@Main/namespace";
import type {GenericListGeolocationViewModel} from "@Crm/GenericListViewModel.Geolocation";
import type {GenericListMapViewModel} from "@Main/GenericListViewModel.Map";

export class InstallationListIndexViewModel extends window.Crm.ViewModels.ContactListViewModel<Crm.Service.Rest.Model.CrmService_Installation, Crm.Service.Rest.Model.ObservableCrmService_Installation> {
	geolocationViewModel: GenericListGeolocationViewModel;
	mapViewModel: GenericListMapViewModel<Crm.Service.Rest.Model.CrmService_Installation, Crm.Service.Rest.Model.ObservableCrmService_Installation>;
	lookups: LookupType = {
		countries: {$tableName: "Main_Country"},
		regions: {$tableName: "Main_Region"},
		installationHeadStatuses: {$tableName: "CrmService_InstallationHeadStatus"},
		installationTypes: {$tableName: "CrmService_InstallationType"},
		statisticsKeyProductTypes: { $tableName: "CrmService_StatisticsKeyProductType"}
	};
	latitudeFilterColumn = "Address.Latitude";
	longitudeFilterColumn = "Address.Longitude";

	constructor() {
		const joinTags = {
			Selector: "Tags",
			Operation: "orderBy(function(t) { return t.Name; })"
		};
		super(
			"CrmService_Installation",
			["InstallationNo", "Description"],
			["ASC", "ASC"],
			["Company", "ResponsibleUserUser", "ServiceObject", "Address", joinTags]);
		this.geolocationViewModel = new window.Crm.ViewModels.GenericListGeolocationViewModel();
		this.mapViewModel = new window.Main.ViewModels.GenericListMapViewModel(this);
	}

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		let user = await window.Helper.User.getCurrentUser();
		this.currentUser(user);
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		await super.init(id, params);
		await this.geolocationViewModel.init();
	};

	applyFilters(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_Installation>): $data.Queryable<Crm.Service.Rest.Model.CrmService_Installation> {
		if (this.filters["LocationAddress.City"]) {
			this.filters["Address.City"] = this.filters["LocationAddress.City"];
			delete this.filters["LocationAddress.City"];
		}
		if (this.filters["LocationAddress.Street"]) {
			this.filters["Address.Street"] = this.filters["LocationAddress.Street"];
			delete this.filters["LocationAddress.Street"];
		}
		if (this.filters["LocationAddress.ZipCode"]) {
			this.filters["Address.ZipCode"] = this.filters["LocationAddress.ZipCode"];
			delete this.filters["LocationAddress.ZipCode"];
		}
		let itemNoFilter = null;
		if (this.filters["Article.ItemNo"]) {
			itemNoFilter = this.filters["Article.ItemNo"];
			delete this.filters["Article.ItemNo"];
		}
		query = super.applyFilters(query);
		if (ko.unwrap(itemNoFilter)) {
			query = query.filter("it.Article.ItemNo.contains(this.itemNo) || it.CustomItemNo.contains(this.itemNo)", { itemNo: ko.unwrap(itemNoFilter).Value });
			this.filters["Article.ItemNo"] = itemNoFilter;
		}
		if (this.filters["Address.City"]) {
			this.filters["LocationAddress.City"] = this.filters["Address.City"];
			delete this.filters["Address.City"];
		}
		if (this.filters["Address.Street"]) {
			this.filters["LocationAddress.Street"] = this.filters["Address.Street"];
			delete this.filters["Address.Street"];
		}
		if (this.filters["Address.ZipCode"]) {
			this.filters["LocationAddress.ZipCode"] = this.filters["Address.ZipCode"];
			delete this.filters["Address.ZipCode"];
		}
		return query;
	};

	dispose(): void {
		this.geolocationViewModel.dispose();
	};

	getDirection(installation: Crm.Service.Rest.Model.ObservableCrmService_Installation): number {
		return this.geolocationViewModel.getDirection(installation);
	};

	getAddress(item: Crm.Service.Rest.Model.ObservableCrmService_Installation): Crm.Rest.Model.ObservableCrm_Address {
		return ko.unwrap(item.Address);
	};

	getDistance(installation: Crm.Service.Rest.Model.ObservableCrmService_Installation): KnockoutObservable<string> {
		return this.geolocationViewModel.getDistance(installation);
	};

	getPopupInformation = window.Helper.Map.getPopupItemTemplateInformation;

	getMarkerColor = window.Helper.Map.getMapMarkerColor;

	getMarkerContent = window.Helper.Map.getMapMarkerContent;

	getIconName(item: Crm.Service.Rest.Model.ObservableCrmService_Installation): string {
		return "marker_pin3";
	};

	getLatitude(item: Crm.Service.Rest.Model.ObservableCrmService_Installation): number {
		const address = this.getAddress(item);
		return address ? ko.unwrap(address.Latitude) : null;
	};

	getLongitude(item: Crm.Service.Rest.Model.ObservableCrmService_Installation): number {
		const address = this.getAddress(item);
		return address ? ko.unwrap(address.Longitude) : null;
	};
}

namespace("Crm.Service.ViewModels").InstallationListIndexViewModel = InstallationListIndexViewModel;