import type {FeatureCollection} from "geojson";

export interface ImapView {
	lat: number,
	lng: number,
	zoom?: number | null
}

export enum MapMessageType {
	//host
	scrollMarkerIntoView = "scrollMarkerIntoView",
	unloading = "unloading",
	markers = "markers",
	defaultTaskDurationInPixel = "defaultTaskDurationInPixel",
	route = "route",
	clearRoute = "clearRoute",

	//client
	requestMarkers = "requestMarkers",
	markerClick = "markerClick",
	mapView = "mapView",
	requestDefaultTaskDurationInPixel = "requestDefaultTaskDurationInPixel",
	requestRoute = "requestRoute",
}

export enum MapMarkerType {
	ServiceOrderDispatch = "ServiceOrderDispatch",
	ServiceOrder = "ServiceOrder",
	Resource = "Resource",
	TechnicianHome = "TechnicianHome"
}

export interface MapMarker {
	id: string,
	type: MapMarkerType,
	Latitude: number,
	Longitude: number,
	IconName?: string,
	PopupInformation?: string,
	DurationInPx?: number,
	MarkerContent?: string,
	MarkerColor?: string,
	ClassName?: string
}

type MapMessage_nullData = {
	type: MapMessageType.requestDefaultTaskDurationInPixel | MapMessageType.requestMarkers | MapMessageType.unloading | MapMessageType.requestRoute | MapMessageType.clearRoute,
	data: null
}

type MapMessage_numberData = {
	type: MapMessageType.defaultTaskDurationInPixel,
	data: number
}

type MapMessage_MapMarkerData = {
	type: MapMessageType.scrollMarkerIntoView | MapMessageType.markerClick,
	data: MapMarker
}

type MapMessage_MapMarkersData = {
	type: MapMessageType.markers,
	data: MapMarker[]
}

type MapMessage_ImapViewData = {
	type: MapMessageType.mapView | MapMessageType.unloading,
	data: ImapView
}

type MapMessage_RouteData = {
	type: MapMessageType.route,
	data: FeatureCollection
}

export type MapMessage =
	MapMessage_nullData
	| MapMessage_numberData
	| MapMessage_MapMarkerData
	| MapMessage_ImapViewData
	| MapMessage_MapMarkersData
	| MapMessage_RouteData;
