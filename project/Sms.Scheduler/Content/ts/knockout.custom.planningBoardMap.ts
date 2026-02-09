import type MapComponent from "./knockout.component.Map";
import type {MapMarker} from "./Model/MapMessage";

;(function (ko, L: any) {
	ko.bindingHandlers.planningBoardMap = {
		init: function (element, valueAccessor, allBindingsAccessor, context: MapComponent, viewModel) {
			let value = valueAccessor();
			if ($(element).data('map')) {
				return;
			}
			// create unique ids to support multiple maps
			let mapId = 'map-' + (new Date()).getTime().toString();
			$(element).attr('id', mapId);
			// create a map in the "map" div, set the view to a given place and zoom
			let map: L.Map = new L.Map(mapId, {
				dragging: !L.Browser.mobile,
				tap: !L.Browser.mobile
			}).setView([49.002421, 9.491715], 13);
			// add a tile layer
			let tileLayerUrl: string = value.tileLayerUrl;
			if (tileLayerUrl.toLowerCase().indexOf('google') > -1) {
				new L.TileLayer(value.tileLayerUrl.replace(/{and}/g, '&'), {
					subdomains: ['mt0', 'mt1', 'mt2', 'mt3']
				}).addTo(map);
			} else {
				new L.TileLayer(tileLayerUrl, {
					attribution: '&copy; <a href="https://openstreetmap.org/copyright">OpenStreetMap</a> contributors'
				}).addTo(map);
			}
			context.clusterGroups.forEach(group => {
				context.layerControl.addOverlay(group.layer, group.title);
				map.addLayer(group.layer);
			});
			context.layerControl.addTo(map);
			$(element).data('map', map);
		},
		update: function (element, valueAccessor, allBindingsAccessor, context: MapComponent, viewModel) {
			let value = valueAccessor();
			let mapElements = ko.utils.unwrapObservable(value.elements) as MapMarker[];
			let map: L.Map = $(element).data('map');
			if (!Array.isArray(mapElements)) {
				return;
			}
				context.clusterGroups.forEach(group => {
					group.layer.clearLayers();
				});
				for (let i = 0; i < mapElements.length; i++) {
					let mapElement = mapElements[i];
					let latitude: number = ko.unwrap(mapElement.Latitude);
					let longitude: number = ko.unwrap(mapElement.Longitude);
					if (latitude && longitude) {
						const markerColor = ko.unwrap(mapElement.MarkerColor);
						const markerContent = ko.unwrap(mapElement.MarkerContent);
						const isDivIcon = (!!markerColor || !!markerContent);
						let marker: L.Marker = new L.Marker([latitude, longitude], {icon: ko.bindingHandlers.planningBoardMap.GetDefaultIcon(mapElement, isDivIcon)});
						if (!isDivIcon) {
							let iconName: string = ko.unwrap(mapElement.IconName) || "marker_pin3";
							marker.options.icon.options.iconUrl = window.Helper.resolveUrl("~/Plugins/Main/Content/img/" + iconName + ".png");
						} else {
							(marker.options.icon as L.DivIcon).options.html = context.getIconHtml(markerContent, markerColor);
						}
						const className = ko.unwrap(mapElement.ClassName);
						if (className) {
							marker.options.icon.options.className = className;
						}

						let popupInformation: HTMLElement | string = ko.unwrap(mapElement.PopupInformation);
						if (popupInformation) {
							marker.bindPopup(popupInformation, {
								minWidth: 250
							});
						}
						$(marker).data('mapElement', mapElement);
						$(marker)
							.on('click', function (e) {
								$(map).trigger('markerclick', e);
							})
							.on('mousedown', () => {
								//This is to prevent map move on mousedown of marker which makes problem for drag drops
								map.dragging.disable();
								map.dragging.enable();
							});

						$(marker).on('dblclick', function (e) {
							$(map).trigger('markerdblclick', e);
						});
						context.clusterGroups.forEach(group => {
							if (group.markerTypes.includes(mapElement.type)) {
								group.layer.addLayer(marker);
							}
						});
					}
				}
		}
	}

	ko.bindingHandlers.planningBoardMap.GetDefaultIcon = function (mapElement, isDivIcon: boolean) {
		const defaultIconConfig = {
			iconUrl: window.Helper.Url.resolveUrl('~/Plugins/Main/Content/img/marker_pin3.png'),
			shadowUrl: window.Helper.Url.resolveUrl('~/Plugins/Main/Content/img/marker_shadow.png'),

			iconSize: [32, 37], // size of the icon
			shadowSize: [51, 37], // size of the shadow
			iconAnchor: [16, 37], // point of the icon which will correspond to marker's location
			shadowAnchor: [25, 37],  // the same for the shadow
			popupAnchor: [0, -30] // point from which the popup should open relative to the iconAnchor
		};
		const icon = isDivIcon ? new L.DivIcon(defaultIconConfig) : new L.Icon(defaultIconConfig);

		if (mapElement) {
			let createIcon = icon.createIcon;
			icon.createIcon = function (oldIcon) {
				let _icon = createIcon.call(this, oldIcon);
				$(_icon).data("mapElement", mapElement);
				return _icon;
			};
		}

		return icon;
	}
})(window.ko, window.L);