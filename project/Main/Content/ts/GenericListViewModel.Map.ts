import type { Map } from "./knockout.custom.map";
import type { GenericListViewModel } from "./GenericListViewModel";
import {namespace} from "./namespace";
import {GenericListGeolocationViewModel} from "@Crm/GenericListViewModel.Geolocation";

type ParentViewModel<TModel extends $data.Entity, TObservableModel extends $data.Entity, TItemViewModel extends TObservableModel = TObservableModel> = {
	getLatitude?(item: TObservableModel): number
	getLongitude?(item: TObservableModel): number
	getMarkerColor?(item: TObservableModel): string
	getPopupInformation?(item: TObservableModel): string | HTMLElement
	getIconName?(item: TObservableModel): string
	getMarkerContent?(item: TObservableModel): string
} & GenericListViewModel<TModel, TObservableModel, TItemViewModel>

class MapItem {
	IconName: KnockoutComputed<string>;
	PopupInformation: KnockoutComputed<string>;
	MarkerColor: KnockoutComputed<string>;
	MarkerContent: KnockoutComputed<string>;
	Latitude: KnockoutComputed<number>;
	Longitude: KnockoutComputed<number>;
}

export class GenericListMapViewModel<TModel extends $data.Entity, TObservableModel extends $data.Entity, TItemViewModel extends TObservableModel = TObservableModel> {

	isCustomLocationSelectionEnabled = ko.observable<boolean>(false);
	items = ko.pureComputed<TItemViewModel[]>(() => {
		return this.parentViewModel.items().map(x => this.initItem(x));
	});
	mapEnabled: KnockoutComputed<boolean>;
	parentViewModel: ParentViewModel<TModel, TObservableModel, TItemViewModel>;
	searchOnMap = ko.observable<boolean>(false);
	renderMap = ko.observable<boolean>(false);
	showMap = ko.observable<boolean>(false);
	leafletMap = ko.observable<Map | null>(null);

	constructor(parentViewModel: ParentViewModel<TModel, TObservableModel, TItemViewModel>) {
		this.parentViewModel = parentViewModel;
		this.mapEnabled = window.ko.pureComputed(function () {
			return true;
		});
		this.showMap.subscribe(showMap => {
			if (showMap && !this.renderMap()) {
				this.renderMap(true);
			}
		});
	}

	toggleMap = (): void => {
		if (this.parentViewModel.viewMode().Key !== "List") {
			this.parentViewModel.viewMode(ko.utils.arrayFirst(this.parentViewModel.viewModes(), function (x) {
				return x.Key === "List";
			}));
		}
		this.showMap(!this.showMap());
	}

	initItem = (item: TItemViewModel): (TItemViewModel & MapItem) => {
		let result = item as (TItemViewModel & MapItem); 
		result.IconName = ko.pureComputed(() => this.getIconName(result));
		result.PopupInformation = ko.pureComputed(() => this.getPopupInformation(result));
		result.MarkerColor = ko.pureComputed(() => this.getMarkerColor(result)); 
		result.MarkerContent = ko.pureComputed(() => this.getMarkerContent(result));
		result.Latitude = ko.pureComputed(() => this.getLatitude(result));
		result.Longitude = ko.pureComputed(() => this.getLongitude(result));
		return result;
	}

	getLatitude(item: TItemViewModel): number {
		return this.parentViewModel.getLatitude(item);
	}

	getLongitude(item: TItemViewModel): number {
		return this.parentViewModel.getLongitude(item);
	}

	getMarkerColor(item: TItemViewModel): string {
		return this.parentViewModel.getMarkerColor ? this.parentViewModel.getMarkerColor(item) : "grey";
	}

	getMarkerContent(item: TItemViewModel): string {
		return this.parentViewModel.getMarkerColor ? this.parentViewModel.getMarkerContent(item) : "?";
	}
	
	getPopupInformation(item: TItemViewModel): string {
		return this.parentViewModel.getPopupInformation ? this.parentViewModel.getPopupInformation(item) : ko.unwrap((item as any).Name);
	}

	getIconName(item: TItemViewModel): string {
		return this.parentViewModel.getIconName ? this.parentViewModel.getIconName(item) : "marker_pin3";
	}

}

namespace("Main.ViewModels").GenericListMapViewModel = GenericListMapViewModel;

function updateMapHeight(element, maxSize) {
	const $map = $(element);
	const currentSize = $map.innerHeight();
	$map.css("height", Math.min(window.innerHeight, Math.max(currentSize, maxSize)));
	const map = $map.data("map");
	if (!!map) {
		map.invalidateSize(true);
	}
}

function resetMap(element) {
	const $map = $(element);
	const map = $map.data("map");
	const markers = $map.data("markers");
	if (!!map && !!markers) {
		map.fitBounds(markers.getBounds());
	}
}

// extend knockout map binding handler to interact with generic list viewmodel
export function extendMapBindingHandler() {
	const baseMapInit = ko.bindingHandlers.map.init;
	ko.bindingHandlers.map.init = function (element, valueAccessor, allBindingsAccessor, context, viewModel) {
		baseMapInit(element, valueAccessor, allBindingsAccessor, context, viewModel);
		const $mapContainer = $(element).closest(".map-container");

		const map = $(element).data("map");
		(viewModel.$data as GenericListMapViewModel<$data.Entity, $data.Entity>).leafletMap(map);
		const filterByMap = new window.L.Control();filterByMap.onAdd = function (leafletMap) {
			const button = document.createElement("div");
			button.innerHTML = $(".search-on-map-template").html();
			return button;
		};
		filterByMap.addTo(map);
		const triggerSearch = function () {
			if (viewModel.$data.searchOnMap()) {
				const bounds = map.getBounds();
				const latitudeMin = bounds._southWest.lat;
				const latitudeMax = bounds._northEast.lat;
				const longitudeMin = bounds._southWest.lng;
				const longitudeMax = bounds._northEast.lng;
				viewModel.$root.getFilter("LatitudeMin")({
					Column: viewModel.$root.latitudeFilterColumn || "Latitude",
					Operator: ">=",
					Value: latitudeMin.toPrecision(15)
				});
				viewModel.$root.getFilter("LatitudeMax")({
					Column: viewModel.$root.latitudeFilterColumn || "Latitude",
					Operator: "<=",
					Value: latitudeMax.toPrecision(15)
				});
				viewModel.$root.getFilter("LongitudeMin")({
					Column: viewModel.$root.longitudeFilterColumn || "Longitude",
					Operator: ">=",
					Value: longitudeMin.toPrecision(15)
				});
				viewModel.$root.getFilter("LongitudeMax")({
					Column: viewModel.$root.longitudeFilterColumn || "Longitude",
					Operator: "<=",
					Value: longitudeMax.toPrecision(15)
				});
				viewModel.$root.filter();
			} else {
				const previousFiltered = viewModel.$root.getFilter("LatitudeMin")() != null &&
					viewModel.$root.getFilter("LatitudeMax")() != null &&
					viewModel.$root.getFilter("LongitudeMin")() != null &&
					viewModel.$root.getFilter("LongitudeMax")() != null;
				viewModel.$root.getFilter("LatitudeMin")(null);
				viewModel.$root.getFilter("LatitudeMax")(null);
				viewModel.$root.getFilter("LongitudeMin")(null);
				viewModel.$root.getFilter("LongitudeMax")(null);
				if (previousFiltered) {
					viewModel.$root.filter();
				}
			}
		};
		const onSearchOnMapClick = function () {
			viewModel.$data.searchOnMap(!viewModel.$data.searchOnMap());
			triggerSearch();
		};
		$mapContainer
			.on("click",
				".search-on-map",
				onSearchOnMapClick);
		map.on("moveend",
			function () {
				triggerSearch();
			});
		const maxHeight = $(element).innerHeight();
		const eventListener = function () {
			updateMapHeight(element, maxHeight);
		}
		window.addEventListener("resize", eventListener);
		updateMapHeight(element, maxHeight);
		ko.utils.domNodeDisposal.addDisposeCallback(element,
			function () {
				window.removeEventListener("resize", eventListener);
				$mapContainer.off("click", ".search-on-map", onSearchOnMapClick);
			});
	};


	const baseMapUpdate = ko.bindingHandlers.map.update;
	ko.bindingHandlers.map.update = function (element, valueAccessor, allBindingsAccessor, context, viewModel) {
		baseMapUpdate(element, valueAccessor, allBindingsAccessor, context, viewModel);
		if (!viewModel.$data.searchOnMap()) {
			resetMap(element);
		}
	};
}

namespace("Main.ViewModels").GenericListMapViewModel = GenericListMapViewModel;