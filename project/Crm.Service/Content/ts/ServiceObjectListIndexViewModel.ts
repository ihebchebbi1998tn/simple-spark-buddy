import {namespace} from "@Main/namespace";
import type {GenericListGeolocationViewModel} from "@Crm/GenericListViewModel.Geolocation";
import type {GenericListMapViewModel} from "@Main/GenericListViewModel.Map";

export class ServiceObjectListIndexItemViewModel {
	InstallationsCount: number;
}

export class ServiceObjectListIndexViewModel extends window.Crm.ViewModels.ContactListViewModel<Crm.Service.Rest.Model.CrmService_ServiceObject, Crm.Service.Rest.Model.ObservableCrmService_ServiceObject> {
	geolocationViewModel: GenericListGeolocationViewModel;
	mapViewModel: GenericListMapViewModel<Crm.Service.Rest.Model.CrmService_ServiceObject, Crm.Service.Rest.Model.ObservableCrmService_ServiceObject>;
	latitudeFilterColumn: string;
	longitudeFilterColumn: string;
	lookups = {
		countries: {$tableName: "Main_Country"},
		regions: {$tableName: "Main_Region"},
		serviceObjectCategories: {$tableName: "CrmService_ServiceObjectCategory"}
	};
	currentUserUsergroups = ko.pureComputed<string[]>(() => {
		return this.currentUser()
			? this.currentUser().Usergroups
			: [];
	});

	constructor() {
		const joinAddresses = {
			Selector: "Addresses",
			Operation: "filter(function(a) { return a.IsCompanyStandardAddress === true; })"
		};
		const joinTags = {
			Selector: "Tags",
			Operation: "orderBy(function(t) { return t.Name; })"
		};
		super(
			"CrmService_ServiceObject",
			["ObjectNo", "Name"],
			["ASC", "ASC"],
			["ResponsibleUserUser", joinAddresses, joinTags]);
		this.geolocationViewModel = new window.Crm.ViewModels.GenericListGeolocationViewModel();
		this.mapViewModel = new window.Main.ViewModels.GenericListMapViewModel(this);
		this.getFilter("StandardAddress.IsCompanyStandardAddress")(true);
		this.latitudeFilterColumn = "StandardAddress.Latitude";
		this.longitudeFilterColumn = "StandardAddress.Longitude";
	}

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		let user = await window.Helper.User.getCurrentUser();
		this.currentUser(user);
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		await super.init(id, params);
		await this.geolocationViewModel.init();
	};

	async initItems(items: (Crm.Service.Rest.Model.ObservableCrmService_ServiceObject)[]): Promise<(Crm.Service.Rest.Model.ObservableCrmService_ServiceObject & ServiceObjectListIndexItemViewModel)[]> {
		const queries = [];
		let result = await super.initItems(items) as (Crm.Service.Rest.Model.ObservableCrmService_ServiceObject & ServiceObjectListIndexItemViewModel)[];
		result.forEach(function (serviceObject) {
			queries.push({
				queryable: window.database.CrmService_Installation.filter(function (it) {
						return it.FolderId === this.serviceObjectId;
					},
					{serviceObjectId: serviceObject.Id}),
				method: "count",
				handler: function (count) {
					serviceObject.InstallationsCount = count;
					return items;
				}
			});
		});
		await window.Helper.Batch.Execute(queries);
		return result;
	};

	dispose(): void {
		return this.geolocationViewModel.dispose();
	};

	getAddress(item: Crm.Service.Rest.Model.ObservableCrmService_ServiceObject): Crm.Rest.Model.ObservableCrm_Address {
		return (ko.unwrap(item.Addresses) || [])[0];
	};

	getDirection(serviceObject: Crm.Service.Rest.Model.ObservableCrmService_ServiceObject): number {
		return this.geolocationViewModel.getDirection(serviceObject);
	};

	getDistance(serviceObject: Crm.Service.Rest.Model.ObservableCrmService_ServiceObject): KnockoutObservable<string> {
		return this.geolocationViewModel.getDistance(serviceObject);
	};

	getMarkerColor = window.Helper.Map.getMapMarkerColor;

	getMarkerContent = window.Helper.Map.getMapMarkerContent;

	getPopupInformation = window.Helper.Map.getPopupItemTemplateInformation;

	getIconName(item: Crm.Service.Rest.Model.ObservableCrmService_ServiceObject): string {
		return "marker_pin3";
	};

	getLatitude(item: Crm.Service.Rest.Model.ObservableCrmService_ServiceObject): number {
		const address = this.getAddress(item);
		return address ? ko.unwrap(address.Latitude) : null;
	};

	getLongitude(item: Crm.Service.Rest.Model.ObservableCrmService_ServiceObject): number {
		const address = this.getAddress(item);
		return address ? ko.unwrap(address.Longitude) : null;
	};
}

namespace("Crm.Service.ViewModels").ServiceObjectListIndexViewModel = ServiceObjectListIndexViewModel;