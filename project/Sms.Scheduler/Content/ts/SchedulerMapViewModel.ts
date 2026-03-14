import {namespace} from "@Main/namespace";
import {ImapView, MapMarker, MapMarkerType, MapMessage, MapMessageType} from "./Model/MapMessage";
import {Breadcrumb} from "@Main/breadcrumbs";
import type MapComponent from "./knockout.component.Map";
import type {FeatureCollection} from "geojson";
import type {ClusterGroup} from "../@types";

export class SchedulerMapViewModel extends window.Main.ViewModels.ViewModelBase {
	mapBroadcastChannel: BroadcastChannel = null;
	markers: KnockoutObservableArray<MapMarker> = ko.observableArray<MapMarker>([]);
	mapHeight: KnockoutObservable<number> = ko.observable<number>(500);
	mapComponent: KnockoutObservable<MapComponent> = ko.observable<MapComponent>(null);
	mapView: KnockoutObservable<ImapView> = ko.observable(null);
	defaultTaskDurationInPixel: number = 100;
	clusterGroups: ClusterGroup[] = [];

	unloading = false;

	constructor() {
		super();

		let self = this;
		self.clusterGroups = window.Helper.Scheduler.ClusterGroups();
		window.setInterval(() => {
			let newMarkers: HTMLElement[] = Array.from(document.querySelectorAll("#mapcontainer .marker-serviceorder:not(.draggable-marker)"));

			for (let marker of newMarkers) {
				marker.classList.add("draggable-marker");
				const element = marker.querySelector(":scope > div") as HTMLElement;
				if (!element) {
					continue;
				}
				element.draggable = true;

				element.addEventListener('dragstart', function (e) {
					let mapMarker = $(this).parent().data("mapElement") as MapMarker;

					const proxy = document.createElement('div');
					proxy.style.top = "-10000px";
					document.body.appendChild(proxy);

					// Fake an event bar
					proxy.classList.add('b-sch-event-wrap', 'b-sch-event', 'b-unassigned-class', 'drag-proxy');
					proxy.innerHTML = `<div class="b-sch-event b-has-content b-sch-event-withicon">
				<div class="b-sch-event-content">
					<p>${mapMarker.PopupInformation}</p>
				</div>
			</div>`;

					proxy.style.height = '50px';
					proxy.style.width = `${mapMarker.DurationInPx}px`;

					e.dataTransfer.clearData();

					let uri = window.Helper.Url.qualifyURL(window.Helper.Url.resolveUrl(`~/Home/MaterialIndex#/Crm.Service/ServiceOrder/DetailsTemplate/${mapMarker.id}`));
					e.dataTransfer.setData("text/plain", uri);
					e.dataTransfer.setData("text/uri-list", uri);

					e.dataTransfer.setData("crm/serviceorder", mapMarker.id);
					e.dataTransfer.setDragImage(proxy, 0, 0);

				}, false);
			}
		}, 1000);

		document.addEventListener("dragend", () => $(".drag-proxy").remove(), false);
	}

	async init(id: string): Promise<void> {
		const me = this;

		//toggle menu
		$('body').removeClass('toggled sw-toggled');

		if (id) {
			me.mapBroadcastChannel = new BroadcastChannel(id);
			me.mapBroadcastChannel.onmessage = event => me.onMessage(event);

			me.broadcastMessage({type: MapMessageType.requestMarkers, data: null});
		}

		me.onMarkerClick = me.onMarkerClick.bind(me);

		me.loading.subscribe(function (loading) {
			if (!loading) {
				const params = new Proxy(new URLSearchParams(window.location.hash.substring(window.location.hash.lastIndexOf("?"))), {
					get: (searchParams, prop: string) => searchParams.get(prop),
				});

				//@ts-ignore
				let v = {lat: params.lat, lng: params.lng, zoom: params.zoom};
				me.mapView(v);

				me.mapView.subscribe(function (pos) {
					me.broadcastMessage({type: MapMessageType.mapView, data: pos});
				});

				window.onresize = function () {
					let mapcontainer = document.getElementById("mapcontainer");
					if (mapcontainer) {
						let bound = mapcontainer.getBoundingClientRect();

						let bottompad = mapcontainer.offsetLeft + bound.left;

						let height = window.innerHeight - bound.top - bottompad;
						me.mapHeight(height);
					}
				}
				window.dispatchEvent(new Event('resize'));

				me.broadcastMessage({type: MapMessageType.requestDefaultTaskDurationInPixel, data: null});
			}

			me.setBreadcrumbs();
		})

		me.mapComponent.subscribe((newValue) => {
			if (newValue && newValue.geoJsonLayer?.getLayers().length > 0) {
				me.broadcastMessage({type: MapMessageType.requestRoute, data: null});
			}
		});

		window.onbeforeunload = function () {
			if (!me.unloading) {
				me.broadcastMessage({type: MapMessageType.unloading, data: me.mapView()});
			}
		};
	}

	onMarkerClick(marker: MapMarker) {
		this.broadcastMessage({type: MapMessageType.markerClick, data: marker});
	}

	onMessage(event: MessageEvent) {
		let message: MapMessage = event.data;

		switch (message.type) {
			case MapMessageType.unloading:
				this.unloading = true;
				window.close();
				break;
			case MapMessageType.markers:
				this.markers(message.data);
				break;
			case MapMessageType.scrollMarkerIntoView:
				this.scrollMarkerIntoView(message.data);
				break;
			case MapMessageType.defaultTaskDurationInPixel:
				this.defaultTaskDurationInPixel = message.data;
				break;
			case MapMessageType.route:
				this.showRouteOnMap(message.data);
				break;
			case MapMessageType.clearRoute:
				this.clearRoute();
				break;
			case MapMessageType.mapView:
				this.mapComponent().setView(message.data);
				break;
		}
	}

	scrollMarkerIntoView(marker: MapMarker) {
		this.mapComponent().setView({lat: marker.Latitude, lng: marker.Longitude});
	}

	broadcastMessage(message: MapMessage) {
		if (this.mapBroadcastChannel != null) {
			this.mapBroadcastChannel.postMessage(message);
		}
	}

	showRouteOnMap(geoJsonFeatureCollection: FeatureCollection) {
		this.mapComponent().addFeatures(geoJsonFeatureCollection);
	}

	clearRoute() {
		this.mapComponent().clearGeoJsonLayers();
		const numberedDispatchMarkers = this.markers()?.filter(m => m.type === MapMarkerType.ServiceOrderDispatch && m.MarkerContent !== 'D');
		numberedDispatchMarkers?.forEach(m => m.MarkerContent = 'D');
		this.markers.notifySubscribers();
	}

	async setBreadcrumbs(): Promise<void> {
		window.breadcrumbsViewModel.setCustomBreadcrumbs([
			new Breadcrumb(window.Helper.String.getTranslatedString("WebScheduler"), "Scheduler::Read", "#/Sms.Scheduler/Scheduler/DetailsTemplate"),
			new Breadcrumb(window.Helper.String.getTranslatedString("Map"), "Scheduler::Read", window.location.hash)
		]);
	}
}

namespace("Sms.Scheduler.ViewModels").SchedulerMapViewModel = SchedulerMapViewModel;