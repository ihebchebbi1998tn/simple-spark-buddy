import {registerComponent} from "@Main/componentRegistrar";
import {ViewModelBase} from "@Main/ViewModelBase";
import type {ImapView, MapMarker, MapMarkerType} from "./Model/MapMessage";
import type {GeoJSON, Layer, Map, MarkerClusterGroup} from "leaflet";
import type {FeatureCollection} from "geojson";
import type {ClusterGroup} from "../@types";

registerComponent({
	componentName: "map-widget",
	template: "Sms.Scheduler/Template/MapComponent",
	viewModel: {
		createViewModel: function (params, componentInfo) {
			return new MapComponent(params, componentInfo);
		}
	},
});

interface MapComponentParam {
	modelForParent: KnockoutObservable<ViewModelBase>,
	mapHeight: KnockoutObservable<number | string> | number | string,
	onMarkerClick: (marker: MapMarker) => void,
	markers: KnockoutObservableArray<MapMarker>,
	mapView: KnockoutObservable<ImapView>,
	onMapLoaded: () => void,
	clusterGroups: ClusterGroup[]
}

export default class MapComponent extends ViewModelBase {
	mapHeight: KnockoutObservable<number | string>;
	markers: KnockoutObservableArray<MapMarker>;
	map: Map = null;
	mapView: KnockoutObservable<ImapView>;
	geoJsonLayer: GeoJSON = null;
	layerControl: L.Control.Layers = null;
	clusterGroups: {
		layer: MarkerClusterGroup,
		title: string,
		markerTypes: MapMarkerType[]
	}[] = [];

	constructor(params: MapComponentParam, componentInfo: KnockoutComponentTypes.ComponentInfo) {
		super();
		let self = this;

		if (params.modelForParent) {
			params.modelForParent(self);
		}

		if (params.mapView) {
			this.mapView = params.mapView;
		} else {
			this.mapView = ko.observable<ImapView>();
		}

		if (params.mapHeight) {
			if (ko.isObservable(params.mapHeight))
				self.mapHeight = params.mapHeight;
			else
				self.mapHeight = ko.observable(ko.unwrap(params.mapHeight));
		} else {
			self.mapHeight = ko.observable<number | string>("100%");
		}

		self.markers = params.markers ?? ko.observableArray<MapMarker>([]);
		self.layerControl = window.L.control.layers(null, null, {collapsed: false});
		self.clusterGroups = ko.unwrap(params.clusterGroups).map(g => ({
			layer: new window.L.MarkerClusterGroup(),
			title: g.title,
			markerTypes: g.markerTypes
		}));

		ko.bindingEvent.subscribe(componentInfo.element, 'descendantsComplete', function (node) {

			self.map = $(componentInfo.element).find(".map").data('map');
			//overwriting focus function is needed to prevent the scroll on first click of the map bug
			self.map.getContainer().focus = () => {
			};

			if (params.onMarkerClick) {
				$(self.map).on('markerclick', function (e, originalEvent) {
					let marker = $(originalEvent.target).data("mapElement");
					if (marker) {
						params.onMarkerClick(marker);
					}
				});
			}

			let pos = self.mapView();
			if (pos)
				self.setView(pos);

			self.map.on('moveend', function (e) {
				let center = self.map.getCenter();
				let zoom = self.map.getZoom();

				self.mapView({lat: center.lat, lng: center.lng, zoom});
			});

			self.invalidateSize();

			if (params.onMapLoaded)
				params.onMapLoaded();

			self.geoJsonLayer = window.L.geoJSON(null, {
				onEachFeature: onEachFeature,
				style: function (feature: GeoJSON.Feature) {
					return {color: feature.properties.color ?? "#ff00ff"};
				}
			}).addTo(self.map);

			function onEachFeature(feature: GeoJSON.Feature, layer: Layer) {
				// does this feature have a property named popupContent?
				if (feature?.properties?.popupContent) {
					layer.bindPopup(feature.properties.popupContent, {
						maxWidth: 250,
						maxHeight: 150
					});
				}
			}
		});


	}

	public setView(p: ImapView) {
		if (this.map == null)
			return;

		if (p.zoom) {
			this.map.setView([p.lat, p.lng], p.zoom);
		} else {
			let latlng = window.L.latLng(p.lat, p.lng);
			this.map.panTo(latlng);
		}
	}

	invalidateSize() {
		this.map?.invalidateSize();
	}

	public addFeatures(features: FeatureCollection) {
		this.geoJsonLayer.addData(features)
	}

	public clearGeoJsonLayers() {
		this.geoJsonLayer.clearLayers();
	}

	public updateMarkerContent(markerId: string, value: string) {
		this.markers().find(r => r.id == markerId).MarkerContent = value;
		this.markers.notifySubscribers();
	}

	public getIconHtml(content: string, color: string): string {
		return `<div>
			${color !== null ? `<div class="leaflet-marker-pin" style="background-color: ${color}"></div>` : ''}
			${content !== null ? `<div class="leaflet-div-icon-content">${content}</div>` : ''}
			</div>`;
	}
}
