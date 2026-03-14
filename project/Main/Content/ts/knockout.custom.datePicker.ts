;
(function (ko) {
	var getLocale = function (ignoreLocale) {
		var locale = null;
		if (ignoreLocale) {
			locale = "de";
		} else if (!window.Globalize.locale()) {
			window.Globalize.locale(window.Helper.getMetadata("CurrentCulture"));
			locale = window.Globalize.locale().locale;
		} else {
			locale = window.Globalize.locale().locale;
		}
		return locale;
	};

	var getFormat = function (pickDateTime, pickTime, locale) {
		var localeData = window.moment().locale(locale).localeData();
		if (pickDateTime) {
			return localeData.longDateFormat("L") + " " + localeData.longDateFormat("LT");
		} else if (pickTime) {
			return localeData.longDateFormat("LT");
		} else {
			return localeData.longDateFormat("L");
		}
	};

	ko.bindingHandlers.datePicker = {
		getDefaults: function() {
			return {
				focusOnShow: false,
				stepping: 5,
				icons: {
					clear: "zmdi zmdi-delete",
					close: "zmdi zmdi-close",
					today: new Date().getDate() < 15 ? "zmdi zmdi-calendar-alt" : "zmdi zmdi-calendar"
				},
				minDate: new Date(1900, 0, 1),
				maxDate: new Date(3000, 0, 1),
				showClear: true,
				showClose: true,
				tooltips: {
					today: window.Helper.String.getTranslatedString("Today"),
					clear: window.Helper.String.getTranslatedString("Clear"),
					close: window.Helper.String.getTranslatedString("Close"),
					selectMonth: window.Helper.String.getTranslatedString("SelectMonth"),
					prevMonth: window.Helper.String.getTranslatedString("PrevMonth"),
					nextMonth: window.Helper.String.getTranslatedString("NextMonth"),
					selectYear: window.Helper.String.getTranslatedString("SelectYear"),
					prevYear: window.Helper.String.getTranslatedString("PrevYear"),
					nextYear: window.Helper.String.getTranslatedString("NextYear")
				},
				useCurrent: false
			};
		},
		init: function initDatePicker(element, valueAccessor, allBindings, viewModel, bindingContext) {
			var el = $(element);
			var options = ko.unwrap(allBindings.get("datePickerOptions") || {});
			const timeZone = options.timeZone;
			if(ko.unwrap(timeZone)) {
				el.attr("timeZone", ko.unwrap(timeZone));
			}
			let pickDateTime = Boolean(options.pickDateTime);
			if (pickDateTime) {
				options.pickTime = true;
			}
			var pickTime = options.pickTime || options.pickDuration;
			var modal = el.parents(".modal");
			const ignoreLocale = options.pickDuration || false;

			var container = el.parents(".dtp-container").first();
			if (container.length === 1) {
				container.css("position", "relative");
			}

			const locale = getLocale(ignoreLocale);
			var config = $.extend({},
				ko.bindingHandlers.datePicker.getDefaults(),
				{
					stepping: pickTime ? 5 : 1,
					format: getFormat(pickDateTime, pickTime, locale),
					locale: locale,
					showTodayButton: options.pickDuration !== true,
					useCurrent: options.pickDuration === true ? "day" : false
				},
				options.config || {});
			if (el.offset().top < 402 || (modal.length && el.offset().top - modal.offset().top < 402)) {
				config.widgetPositioning = { vertical: "bottom" };
			}
			if (!!el.datetimepicker) {
				el.datetimepicker(config);
			}
			var show = function() {
				$(".date-picker").each(function(i, e) {
					var data = $(e).data("DateTimePicker");
					if (data) {
						data.hide();
					}
				});
				el.data("DateTimePicker").show();
			}
			// @ts-ignore
			if (window.Modernizr && window.Modernizr.touch) {
				el.on("focus",
					function () {
						$(this).blur();
						show();
						return false;
					})
					.on("mousedown",
						function () {
							$("input:focus").blur();
							show();
						})
					.on("touchstart",
						function () {
							$("input:focus").blur();
							show();
							return false;
						});
			} else {
				el.on("click",
					function () {
						el[0].select();
						return false;
					})
			}
			el.attr("autocomplete", "off");
			ko.utils.domNodeDisposal.addDisposeCallback(element,
				function() {
					el.data("DateTimePicker").destroy();
				});
			el.on("change",
				function() {
					var parsedDate = window.Globalize.parseDate(el.val());
					valueAccessor()(parsedDate);
				})
				.on("dp.change",
					function(e) {
						if (!e.date) {
							valueAccessor()(null);
						} else {
							var date;
							var currentValue = valueAccessor()();
							if (options.pickTime) {
								if (options.onlyTime) {
									date = e.date.hours().toString().padStart(2, '0') + ":" + e.date.minutes().toString().padStart(2, '0');
								} else {
									date = e.date.toDate();
									if (!pickDateTime) {
										date.setYear(currentValue instanceof Date ? currentValue.getFullYear() : 1);
										date.setMonth(currentValue instanceof Date ? currentValue.getMonth() : 0);
										date.setDate(currentValue instanceof Date ? currentValue.getDate() : 1);
									}
									date.setUTCSeconds(0, 0);
									if(options.timeZone) {
										date = Helper.Date.convertDateToLocal(date, ko.unwrap(timeZone));
									}
								}
							} else if (options.pickDuration) {
								var duration = window.moment.duration(e.date.clone().seconds(0).milliseconds(0).diff(e.date.clone().startOf("day")));
								date = duration.toString();
							} else {
								if (options.keepTimeZone) {
									date = e.date.toDate();
									date.setHours(0, 0, 0, 0);
								} else {
									// @ts-ignore
									e.date.stripZone();
									date = e.date.toDate();
									date.setUTCHours(0, 0, 0, 0);
								}
							}
							if (!window.moment(date).isSame(currentValue)) {
								valueAccessor()(date);
							}
						}
					})
				.on("dp.show", function() {
					var hide = function () {
						$(document).off("mousedown", hide);
						el.off("dp.hide", hide);
						el.data("DateTimePicker").hide();
					};
					$(document).on("mousedown", hide);
					el.on("dp.hide", hide);
				});
		},
		update: function(element, valueAccessor, allBindings, viewModel, bindingContext) {
			let $element = $(element);
			var options = ko.unwrap(allBindings.get("datePickerOptions") || {});
			let pickDateTime = Boolean(options.pickDateTime);
			if (pickDateTime) {
				options.pickTime = true;
			}
			var config = options.config || {};
			var value = ko.unwrap(valueAccessor());
			var timeZone = ko.unwrap(options.timeZone);
			const ignoreLocale = options.pickDuration || false;
			if (options.pickTime && options.onlyTime && value) {
				value = window.moment("1-1-1 " + value);
			}
			if (options.pickTime && value && value instanceof Date) {
				var stepping = config.stepping || ko.bindingHandlers.datePicker.getDefaults().stepping;
				var roundedUp = Math.ceil(value.getMinutes() / stepping) * stepping;
				if (value.getMinutes() !== roundedUp ||
					value.getSeconds() !== 0 ||
					value.getMilliseconds() !== 0) {
					value.setMinutes(roundedUp);
					value.setSeconds(0);
					value.setMilliseconds(0);
					valueAccessor()(value);
				}
			}
			var date = "";
			const locale = getLocale(ignoreLocale);
			var format = config.format || getFormat(pickDateTime, options.pickTime || options.pickDuration, locale);
			if (value && options.pickDuration) {
				date = window.moment.duration(value).format(format, { stopTrim: "h" });
			} else if (value && options.pickTime) {
				date = window.moment(value).locale(locale).format(format);
				if(timeZone) {
					const lastTimeZone = $element.attr("timeZone");
					if(timeZone == lastTimeZone) {
						const localTime = Helper.Date.convertLocalDateToTimeZone(value, timeZone);
						date = window.moment(localTime).locale(locale).format(format);
					} else {
						const toDisplay = Helper.Date.convertLocalDateToTimeZone(value, lastTimeZone);
						const toStore = Helper.Date.convertDateToLocal(toDisplay, timeZone);
						date = window.moment(toDisplay).locale(locale).format(format);
						valueAccessor()(toStore);
					}
				}
			} else if (value) {
				if (options.keepTimeZone)
					date = window.moment(value).format(format);
				else {
					date = window.moment(value).utc().format(format);
				}
			}

			$element?.data?.("DateTimePicker")?.minDate?.(options?.config?.minDate ?? false);
			$element?.data?.("DateTimePicker")?.maxDate?.(options?.config?.maxDate ?? false);

			$element.attr('timeZone', timeZone);
			$element.attr('value', date);
			$element.val(date);
		}
	};
})(window.ko);