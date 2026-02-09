import {EventStore, Panel, PanelConfig, TimeAxis} from "@bryntum/schedulerpro";
import type MapComponent from "./knockout.component.Map";
import {ImapView, MapMarker, MapMarkerType, MapMessage, MapMessageType} from "./Model/MapMessage";
import type {FeatureCollection} from "geojson";
import type {ClusterGroup} from "../@types";

export class MapPanel extends Panel {
	//public map = null;
	public eventStore!: EventStore;
	public timeAxis!: TimeAxis;
	mapBroadcastChannel: BroadcastChannel = null;
	allMarkers: KnockoutObservableArray<KnockoutObservable<MapMarker[]>>;
	mapComponent: KnockoutObservable<MapComponent> = ko.observable<MapComponent>(null);
	mapView: KnockoutObservable<ImapView> = ko.observable<ImapView>(null);
	defaultTaskDurationInPixel: KnockoutObservable<number>;
	mapMarkers: KnockoutComputed<MapMarker[]>;
	geoJsonFeatureCollection: KnockoutObservable<FeatureCollection> = ko.observable<FeatureCollection>(null);
	isUndocked: KnockoutObservable<boolean> = ko.observable<boolean>(false);
	clusterGroups: ClusterGroup[] = [];
	hasActiveRoute: boolean = false;
	mapWindow: Window;
	constructor(config: Partial<PanelConfig> & {
		eventStore: EventStore,
		timeAxis: TimeAxis
	}, allMarkers: KnockoutObservableArray<KnockoutObservable<MapMarker[]>>, defaultTaskDurationInPixel: KnockoutObservable<number>) {
		super(config);
		const me = this;

		me.onMarkerClick = me.onMarkerClick.bind(me);
		me.onMapLoaded = me.onMapLoaded.bind(me);

		me.eventStore = config.eventStore;
		me.timeAxis = config.timeAxis;
		me.defaultTaskDurationInPixel = defaultTaskDurationInPixel;

		me.allMarkers = allMarkers;

		me.mapMarkers = ko.computed(() => {
			let markers = Array<MapMarker>();
			let allMarkers = me.allMarkers();
			allMarkers.forEach(e => markers.push(...e()));

			return markers;
		});
		me.clusterGroups = window.Helper.Scheduler.ClusterGroups();

		let helperElement = document.createElement("div");
		helperElement.style.height = "100%";
		helperElement.innerHTML = `<map-widget params="
markers: mapMarkers,
mapView :mapView,
onMarkerClick: onMarkerClick,
modelForParent: mapComponent,
onMapLoaded: onMapLoaded,
clusterGroups: clusterGroups
"></map-widget>`;

		ko.applyBindings(this, helperElement);
		document.body.appendChild(helperElement);

		ko.bindingEvent.subscribe(helperElement.firstChild, 'descendantsComplete', function (node) {
			//@ts-ignore
			me.bodyElement.appendChild(helperElement);

			me.mapComponent().setView({lat: 49.002421, lng: 9.491715, zoom: 13});
			me.mapMarkers.subscribe(me.onMapMarkersChange, me);

			me.defaultTaskDurationInPixel.subscribe(value => {
				me.setDefaultTaskDurationInPixel(value);
			});
		});
	}

	// Factoryable type name
	static get type() {
		return 'mappanel';
	}

	// Required to store class name for IdHelper and bryntum.query in IE11
	static get $name() {
		return 'MapPanel';
	}

	getState() {
		let state = super.getState();
		return {
			...state,
			...{
				isVisible: this.isVisible,
				width: this.width
			}
		};
	}

	async applyState(state: Object & { width: number, isVisible: boolean }) {
		super.applyState(state);

		this.width = state.width;
		if (state.isVisible) {
			await this.show();
		} else {
			await this.hide(false);
		}
	}

	composeBody() {
		//@ts-ignore
		const result = super.composeBody();

		return result;
	}

	scrollMarkerIntoView(marker: MapMarker) {
		if (marker) {
			this.mapComponent()?.setView({lat: marker.Latitude, lng: marker.Longitude});
			this.broadcastMessage({type: MapMessageType.scrollMarkerIntoView, data: marker});
		}
	}

	setDefaultTaskDurationInPixel(length: number) {
		this.broadcastMessage({type: MapMessageType.defaultTaskDurationInPixel, data: length});
	}

	scrollMarkerIntoViewById(id: string, type: MapMarkerType) {
		let marker: MapMarker = this.mapMarkers().find(m => m.id == id && m.type == type);
		this.scrollMarkerIntoView(marker);
	}

	onMarkerClick(marker: MapMarker) {
		this.trigger('markerclick', {marker});
	}

	onMapLoaded() {
		this.addUndockButton();
		this.addLayerControl();
	}

	onResize = () => {
		// This widget was resized, so refresh the Mapbox map
		this.mapComponent()?.invalidateSize();
		this.saveState();
	}

	addUndockButton() {
		const me = this;
		//@ts-ignore
		const locateControl = window.L.control({position: "topright"});
		locateControl.onAdd = function (leafletMap) {
			const button = $('<button class="btn btn-primary btn-icon waves-effect waves-circle waves-float"><i class="zmdi zmdi-window-restore fa fa-home"></i></button>');
			button.attr("title", "Undock into separate window");
			$(button).on("click", me.undockButtonClick.bind(me));
			return button.get(0);
		};
		locateControl.addTo(me.mapComponent().map);
	}

	addLayerControl() {
		const me = this;
		me.mapComponent().layerControl.addTo(me.mapComponent().map);
	}

	undockButtonClick() {
		const me = this;
		if (me.mapBroadcastChannel == null) {
			me.mapBroadcastChannel = new BroadcastChannel(crypto.randomUUID());
			me.mapBroadcastChannel.onmessage = event => me.onMessage(event);

			window.onbeforeunload = function () {
				me.broadcastMessage({type: MapMessageType.unloading, data: null});
			};
		}

		let center = me.mapComponent().map.getCenter();
		let zoom = me.mapComponent().map.getZoom();

		me.hide(true);
		me.isUndocked(true);

		queueMicrotask(() => {
			this.mapWindow = window.open(`${window.Helper.Url.resolveUrl("~/Home/MaterialIndex#/Sms.Scheduler/Scheduler/Map/")}${me.mapBroadcastChannel.name}?lat=${center.lat}&lng=${center.lng}&zoom=${zoom}`, '_blank', 'location=0')
		});
	}

	dispose() {
		if (this.isUndocked()) {
			this.mapBroadcastChannel.close();
			this.mapWindow.close();
		}
		super.destroy()
	}

	broadcastMessage(message: MapMessage) {
		if (this.mapBroadcastChannel != null) {
			this.mapBroadcastChannel.postMessage(message);
		}
	}

	onMessage(event: MessageEvent) {
		let message: MapMessage = event.data;

		switch (message.type) {
			case MapMessageType.requestMarkers:
				this.broadcastMessage({type: MapMessageType.markers, data: this.mapMarkers()})
				break;
			case MapMessageType.markerClick:
				this.onMarkerClick(message.data);
				break;
			case MapMessageType.unloading:
				this.mapComponent().setView(message.data);
				this.show();
				this.isUndocked(false);
				break;
			case MapMessageType.mapView:
				this.mapComponent().setView(message.data);
				break;
			case MapMessageType.requestDefaultTaskDurationInPixel:
				this.broadcastMessage({
					type: MapMessageType.defaultTaskDurationInPixel,
					data: this.defaultTaskDurationInPixel()
				});
				break;
			case MapMessageType.requestRoute:
				if (this.geoJsonFeatureCollection() !== null) {
					this.broadcastMessage({type: MapMessageType.route, data: this.geoJsonFeatureCollection()});
				}
				break;
		}
	}

	onMapMarkersChange(newMarkers: MapMarker[]) {
		if(newMarkers == null) return;
		this.broadcastMessage({type: MapMessageType.markers, data: newMarkers})
	}

	showRouteOnMap(geoJsonFeatureCollection: FeatureCollection) {
		this.mapComponent().addFeatures(geoJsonFeatureCollection);
		this.geoJsonFeatureCollection(geoJsonFeatureCollection);
		if (this.hidden && !this.isUndocked()) {
			this.show();
		}
		this.broadcastMessage({type: MapMessageType.route, data: geoJsonFeatureCollection});
		this.hasActiveRoute = true;
	}

	clearRoute() {
		this.mapComponent().clearGeoJsonLayers();
		const numberedDispatchMarkers = this.mapMarkers().filter(m => m.type === MapMarkerType.ServiceOrderDispatch && m.MarkerContent !== 'D');
		numberedDispatchMarkers.forEach(m => m.MarkerContent = 'D');
		this.mapMarkers.notifySubscribers();
		this.broadcastMessage({type: MapMessageType.clearRoute, data: null});
		this.hasActiveRoute = false;
	}

	async toggle(showPanel: boolean) {
		if (showPanel) {
			await this.show();
		} else {
			if (this.isVisible) {
				await this.hide(true);
			}
			if (this.isUndocked()) {
				this.broadcastMessage({type: MapMessageType.unloading, data: null});
			}
		}
		this.saveState();
	}
}