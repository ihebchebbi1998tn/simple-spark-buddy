ko.bindingHandlers.fullCalendar = {
	defaults: {
		displayEventEnd: true,
		editable: false,
		eventRender: function(event, element, calendar) {
			let descriptionLimit = 30;
			if (calendar.name === "month" && event.title.length > descriptionLimit) {
				let toggle = $("<a></a>");
				toggle.append(document.createTextNode(window.Helper.String.getTranslatedString("more")));
				toggle.attr("href", "javascript:void(0);");
				let titleSpan = $(".fc-title", element);
				titleSpan.attr("id", new Date().getTime());
				toggle.attr("id", "toggle-" + titleSpan.attr("id"));
				toggle.css({
					color: 'white',
					fontWeight: 'bold'
				});
				let desc = event.title.slice(0, descriptionLimit);
				titleSpan.text(desc + ' ');
				titleSpan.append(toggle);
				toggle.click(function (e) {
					let target = $(e.currentTarget);
					e.preventDefault();
					let collapsed = target.contents()[0].nodeValue === ' ' + window.Helper.String.getTranslatedString("less");
					if (collapsed) {
						target.contents()[0].nodeValue = window.Helper.String.getTranslatedString("more");
						titleSpan.contents().filter(function () {
							return this.nodeType === 3;
						})[0].nodeValue = desc + ' ';
					} else {
						target.contents()[0].nodeValue = " " + window.Helper.String.getTranslatedString("less");
						titleSpan.contents().filter(function () {
							return this.nodeType === 3;
						})[0].nodeValue = event.title + ' ';
					}
				});
			}
			var renderDescription = calendar.name === "agendaDay" || calendar.name === "agendaWeek";
			if (renderDescription && event.description) {
				var description = $(document.createElement("div")).addClass("pre-line").text(event.description);
				$(".fc-title", element).append(description);
			}
			if (event.description) {
				element.attr("title", event.title + "\r\n\r\n" + event.description);
			} else {
				element.attr("title", event.title);
			}
			ko.bindingHandlers.tooltip.init(element, function() { return true; }, undefined, undefined, undefined);
			if (event.hatched === true) {
				$(".fc-content", element).css("background",
					"repeating-linear-gradient(-45deg, rgba(255, 255, 255, 0.255), rgba(255, 255, 255, 0.255) 10px, rgba(0,0,0,0) 10px, rgba(0,0,0,0) 20px)");
			}
			if(event.secondaryColor && event.secondaryColor !== "#AAAAAA"){
				$(".fc-title", element).prepend("<i class='zmdi zmdi-circle zmdi-hc-sm' style='color: " + event.secondaryColor + "; display: inline; margin-right: 2px; margin-left: -2px; font-size: 0.9em;'></i>");
			}
			if(event.entityType && event.entityType.length > 0){
				$(".fc-title", element).prepend("<span style='display: block; font-size: 0.9em;'>[" + event.entityType + "]</span>");
			}
			$(".fc-time", element).addClass("w-100 p-t-5");
			$(".fc-time", element).css("display", "block");
			$(".fc-time", element).addClass("p-t-5");
			$(".fc-time", element).css("display", "block");
		},
		eventAfterRender: function () {
			// Class `fc-short` causes unwanted behaviour
			// Only displays start time if duration is <= 40 minutes 
			// For instance 13:00 - 13:45 results to `13:00 - `
			$(".fc-time-grid-event").removeClass("fc-short")
		},
		eventTimeFormat: window.moment.localeData((document.getElementById("meta.CurrentCulture") as HTMLMetaElement).content).longDateFormat("LT"),
		firstHour: 7,
		header: {
			right: "",
			center: "prev, title, next",
			left: ""
		},
		lang: (document.getElementById("meta.CurrentLanguage") as HTMLMetaElement).content,
		locale: (document.getElementById("meta.CurrentCulture") as HTMLMetaElement).content,
		selectable: false,
		selectHelper: false,
		theme: true,
		weekends: true,
		weekNumbers: true,
		defaultView: "month"
	},
	init: function(element, valueAccessor) {
		var options = valueAccessor();

		var getEvents = function(start, end, timezone, callback) {
			var returnEvents = () => {
				var events = ko.unwrap(options.items)
					.map(function(x) { return options.getEvent(x); })
					.filter(function(x) { return x.start != null && x.end != null; });
				callback(events);
			}
			if (options.loading()){
				var waitForLoading = options.loading.subscribe(loading => {
					if (loading == false) {
						waitForLoading.dispose();
						returnEvents();
					}
				})
			} else {
				returnEvents();
			}
		};

		// @ts-ignore
		$(element).fullCalendar($.extend({}, ko.bindingHandlers.fullCalendar.defaults, {
			handleWindowResize: ko.unwrap(options.handleWindowResize),
			editable: ko.unwrap(options.editable),
			events: getEvents,
			eventClick: options.eventClick,
			selectable: ko.unwrap(options.selectable),
			selectHelper: ko.unwrap(options.selectHelper),
			viewRender: function () {
				// @ts-ignore
				var start = window.moment($(element).fullCalendar("getView").start.toDate());
				options.start(start.subtract(start.utcOffset(), 'm').toDate());
				// @ts-ignore
				var end = window.moment($(element).fullCalendar("getView").end.toDate());
				options.end(end.subtract(start.utcOffset(), 'm').toDate());
				// @ts-ignore
				var contentHeight = $(element).fullCalendar("getView").name.indexOf("agenda") !== -1 ? "auto" : null;
				// @ts-ignore
				$(element).fullCalendar("option", "contentHeight", contentHeight);
			},
			defaultView: ko.unwrap(options.defaultView)
		}, options.options || {}));

		var actionMenu = $(element).parents(".full-calendar").find(".toolbar-template").html();

		const fcToolbar = $(element).find(".fc-toolbar");
		fcToolbar.find(".fc-right").append(actionMenu);
		$(element).on("click", "[data-view]", function (e) {
			e.preventDefault();
			const dataView = $(this).attr("data-view");
			$("[data-view]", element).closest("li").removeClass("active");
			$(this).closest("li").addClass("active");
			// @ts-ignore
			$(element).fullCalendar("changeView", dataView);
		});
	},
	update: function(element, valueAccessor) {
		// @ts-ignore
		$(element).fullCalendar("refetchEvents");
	}
};