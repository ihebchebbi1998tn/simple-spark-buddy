import {HelperString} from "./helper/Helper.String";

export type Map = {
	getLocationPoint(): L.LatLng | null
	getSearchRadiusCircleRadiusKm(): number
	removeSearchRadiusCircle(): void
	setSearchRadiusCircle(distanceKm: number): void
	addCustomLocationMarker(latlng: L.LatLng, zoom?: number): void
	locationMarker: L.Marker
} & L.Map

; (function(ko, L: any) {
	ko.bindingHandlers.map = {
		init: function (element, valueAccessor, allBindingsAccessor, context, viewModel) {
			const value = valueAccessor(), allBindings = allBindingsAccessor();
			if (!!$(element).data('map')) {
				return;
			}
			// create unique ids to support multiple maps
			const mapId = 'map-' + (new Date()).getTime().toString();
			$(element).attr('id', mapId);
			// create a map in the "map" div, set the view to a given place and zoom
			const map: Map = L.map(mapId, {
				dragging: !L.Browser.mobile,
				tap: !L.Browser.mobile
			}).setView([49.002421, 9.491715], 13);
			// add a tile layer
			const tileLayerUrl = value.tileLayerUrl;
			if (tileLayerUrl.toLowerCase().indexOf('google') > -1) {
				L.tileLayer(value.tileLayerUrl.replace(/{and}/g, '&'), {
					subdomains: ['mt0', 'mt1', 'mt2', 'mt3']
				}).addTo(map);
			} else {
				L.tileLayer(tileLayerUrl, {
					attribution: '&copy; <a href="https://openstreetmap.org/copyright">OpenStreetMap</a> contributors'
				}).addTo(map);
			}

			let isLocationSelectMode = false;
			let locationMarker: L.Marker | null = null;
			let locationCircle = null;
			let locationSearchRadiusCircle = null;
			let searchRadiusKm: number | null = null;

			map.getLocationPoint = function () {
				if (map.locationMarker && map.hasLayer(locationMarker)) {
					return map.locationMarker?.getLatLng();
				} else return null;
			}
			map.getSearchRadiusCircleRadiusKm = function () {
				if (locationSearchRadiusCircle != null && map.hasLayer(locationSearchRadiusCircle)) {
					return searchRadiusKm;
				}
				else {
					return -1;
				}
			}
			map.removeSearchRadiusCircle = function () {
				if (locationSearchRadiusCircle != null) {
					map.removeLayer(locationSearchRadiusCircle);
				}
			}
			map.setSearchRadiusCircle = function (distanceKm: number) {
				if (!map.locationMarker || map.getSearchRadiusCircleRadiusKm() == distanceKm || distanceKm < 1)
				{
					return;
				}
				map.removeSearchRadiusCircle();
				let loc = map.locationMarker.getLatLng();
				locationSearchRadiusCircle = L.circle([loc.lat, loc.lng], distanceKm * 1000);
				locationSearchRadiusCircle.bindPopup('<div class="f-12" style="font-style: italic"><i class="zmdi zmdi-long-arrow-left"></i> ' + distanceKm + ' km <i class="zmdi zmdi-long-arrow-right"></i></div>');
				locationSearchRadiusCircle.addTo(map);
				map.fitBounds(locationSearchRadiusCircle.getBounds());
				searchRadiusKm = distanceKm;
			}

			map.addCustomLocationMarker = function (latlng: L.LatLng, zoom?: number) {
				map.removeSearchRadiusCircle();
				if (locationMarker != null) {
					map.removeLayer(locationMarker);
				}
				if (locationCircle != null) {
					map.removeLayer(locationCircle);
				}
				map.setView(latlng, zoom || map.getZoom());
				locationMarker = new L.marker(latlng, { icon: ko.bindingHandlers.map.getYouAreHereIcon() });
				$.get('https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat=' + latlng.lat + '&lon=' + latlng.lng, function (data) {
					locationMarker.bindPopup(data.display_name).openPopup();
				});
				locationMarker.addTo(map);
				map.locationMarker = locationMarker;
			}
		
			function onLocationFound(e) {
				map.removeSearchRadiusCircle();
				if (locationMarker != null) {
					map.removeLayer(locationMarker);
				}
				if (locationCircle != null) {
					map.removeLayer(locationCircle);
				}
				const radius = e.accuracy / 2;
				locationMarker = L.marker(e.latlng, { icon: ko.bindingHandlers.map.getYouAreHereIcon() });
				const youAreHereMessage = window.Helper.String.getTranslatedString("YouAreHereWithinXMeters").replace("{0}", Math.round(radius).toString());
				locationMarker.addTo(map).bindPopup(youAreHereMessage).openPopup();
				locationCircle = L.circle(e.latlng, radius);
				locationCircle.addTo(map);
				map.locationMarker = locationMarker;
			}

			function onLocationError(e) {
				window.Log.error("bindingHandlers.map: " + e.message);
			}

			map.on('click', (e) => {
				if (isLocationSelectMode) {
					map.addCustomLocationMarker(e.latlng);
					isLocationSelectMode = false;
					$('.leaflet-container').css('cursor', '');
					$('#locationselectbtn').addClass('btn-primary').removeClass('btn-default');
				}
			});
			map.on('locationfound', onLocationFound);
			map.on('locationerror', onLocationError);

			function addCurrentLocationButton() {
				const locateControl = L.control({position: "bottomright"});
				locateControl.onAdd = function(leafletMap) {
					const button = $(`<button data-bind="tooltip: true" title='${HelperString.getTranslatedString("CurrentLocation")}' data-placement="left" class="btn btn-primary btn-icon waves-effect waves-circle waves-float"><i class="zmdi zmdi-gps-dot fa fa-home"></i></button>`);
					$(button).on("click", function() {
						map.locate({ setView: true, maxZoom: 16 });
					});
					return button.get(0);
				};
				locateControl.addTo(map);
				ko.applyBindings(viewModel, locateControl.getContainer());
			}
			function addLocationSelectButton() {
				const locateControl = L.control({position: "bottomright"});
				locateControl.onAdd = function (leafletMap) {
					const button = $(`<button data-bind="visible: isCustomLocationSelectionEnabled, tooltip: true" title='${HelperString.getTranslatedString("SelectLocation")}' data-placement="left" id="locationselectbtn" class="btn btn-primary btn-icon waves-effect waves-circle waves-float"><i class="zmdi zmdi-flag fa fa-home"></i></button>`);
					$(button).on("click", function () {
						if (isLocationSelectMode) {
							isLocationSelectMode = false;
							$('.leaflet-container').css('cursor', '');
							$('#locationselectbtn').addClass('btn-primary').removeClass('btn-default');
						}
						else {
							setTimeout(() => {
								isLocationSelectMode = !isLocationSelectMode;
								if (isLocationSelectMode) {
									$('.leaflet-container').css('cursor', 'crosshair');
									$('#locationselectbtn').addClass('btn-default').removeClass('btn-primary');
								}
							}, 0);
						}
					});
					return button.get(0);
				};
				locateControl.addTo(map);
				ko.applyBindings(viewModel, locateControl.getContainer());
			}
			if (navigator.geolocation) {
				const options = {enableHighAccuracy: true, maximumAge: 0, timeout: 10000};
				navigator.geolocation.getCurrentPosition(function () {
					addCurrentLocationButton();
				},
					function(error) {
						window.Log.warn("bindingHandlers.map getCurrentPosition: " + error.message);
					}, options);
			}
			if (viewModel.$data?.isCustomLocationSelectionEnabled?.call()) {
				setTimeout(() => { addLocationSelectButton(); }, 0);
			}
			$(element).data('map', map);
		},
		update: function (element, valueAccessor, allBindingsAccessor, context, viewModel) {
			const value = valueAccessor(), allBindings = allBindingsAccessor();
			const mapElements = ko.utils.unwrapObservable(value.elements);
			const map = $(element).data('map');
			const oldMarkers = $(element).data('markers');
			const markerClusterGroup = new L.MarkerClusterGroup();
			let markers = false;
			if (!!mapElements && $.isArray(mapElements)) {
				for (let i = 0; i < mapElements.length; i++) {
					let mapElement = mapElements[i];
					const latitude = ko.unwrap(mapElement.Latitude);
					const longitude = ko.unwrap(mapElement.Longitude);
					if (latitude && longitude) {
						const marker = L.marker([latitude, longitude], {icon: ko.bindingHandlers.map.GetDefaultIcon(mapElement)});
						const iconName = ko.unwrap(mapElement.IconName) || "marker_pin3";
						marker.options.icon.options.iconUrl = window.Helper.resolveUrl("~/Plugins/Main/Content/img/" + iconName + ".png");
						const markerColor = ko.unwrap(mapElement.MarkerColor);
						const markerContent = ko.unwrap(mapElement.MarkerContent);
						const popupInformation = ko.unwrap(mapElement.PopupInformation);
						if (map.locationMarker && context?.searchOnMap()) {
							let distance = map.locationMarker.getLatLng().distanceTo(marker.getLatLng());
							$(popupInformation).append('<div class="f-11" style="font-style: italic"> ' + (distance / 1000).toFixed(0) + ' km</div>');
						}
						if (markerColor){
							marker.options.icon.options.html = '<div class="leaflet-marker-pin" style="background-color: ' + markerColor + '"></div>';
						}
						if (markerContent){
							marker.options.icon.options.html += '<div class="leaflet-div-icon-content">' + markerContent + '</div>';
						}
						if (popupInformation) {
							$(popupInformation).on('click', function(e){
								let element = $(`#${ko.unwrap(mapElement.Id)}`)[0]
								element.scrollIntoView({behavior: "smooth", block: "center"});
								$(element).toggleClass('lv-item-target');
								setTimeout(() => {
									$(element).toggleClass('lv-item-target');
								}, 850);
							});
							marker.bindPopup(popupInformation, {
								minWidth: 200
							});
						}
						$(marker).data('mapElement', mapElement);
						$(marker).on('click', function (e) {
							$(map).trigger('markerclick', e);
						});
						$(marker).on('dblclick', function (e) {
							$(map).trigger('markerdblclick', e);
						});
						markerClusterGroup.addLayer(marker);
						markers = true;
					}
				}
			}
			if (!!oldMarkers) {
				map.removeLayer(oldMarkers);
			}
			map.addLayer(markerClusterGroup);
			if (markers == true) {
				$(element).data('markers', markerClusterGroup);
			}
		}
	};
	ko.bindingHandlers.map.GetDefaultIcon = function(mapElement) {
		return L.divIcon({
			iconUrl: window.Helper.Url.resolveUrl('~/Plugins/Main/Content/img/marker_pin3.png'),
			shadowUrl: window.Helper.Url.resolveUrl('~/Plugins/Main/Content/img/marker_shadow.png'),
			
			className: "",
			html: "<div class='leaflet-marker-pin' style='background-color: grey'></div><div class='leaflet-div-icon-content'>?</div>",

			iconSize: [32, 37], // size of the icon
			shadowSize: [51, 37], // size of the shadow
			iconAnchor: [16, 37], // point of the icon which will correspond to marker's location
			shadowAnchor: [25, 37],  // the same for the shadow
			popupAnchor: [0, -30] // point from which the popup should open relative to the iconAnchor
		});
	};
	ko.bindingHandlers.map.getYouAreHereIcon = function() {
		const $profileImg = $(".profile-pic img");
		if ($profileImg.length > 0) {
			return L.divIcon({
				html: '<img class="bgm-white leaflet-youarehere" src="' + $profileImg.attr("src") + '">'
			});
		}
		const defaultIcon = ko.bindingHandlers.map.GetDefaultIcon();
		return L.icon({
			iconUrl: window.Helper.Url.resolveUrl('~/Plugins/Main/Content/img/marker_youarehere.png'),
			shadowUrl: defaultIcon.options.shadowUrl,
			iconSize: defaultIcon.options.iconSize,
			shadowSize: defaultIcon.options.shadowSize,
			iconAnchor: defaultIcon.options.iconAnchor,
			shadowAnchor: defaultIcon.options.shadowAnchor,
			popupAnchor: defaultIcon.options.popupAnchor
		});
	};
})(window.ko, window.L);