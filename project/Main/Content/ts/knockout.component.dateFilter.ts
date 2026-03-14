import {registerComponent} from "./componentRegistrar";

registerComponent({
	componentName: "date-filter",
	template: "Main/Template/DateFilter",
	viewModel: function (params) {
		const viewModel = this;
		viewModel.captionKey = ko.observable(params.caption);
		viewModel.caption = ko.observable(window.Helper.String.getTranslatedString(params.caption));
		viewModel.dateFrom = ko.observable(null);
		viewModel.dateTo = ko.observable(null);
		viewModel.keepTimeZone = ko.unwrap(params.keepTimeZone) as boolean;
		viewModel.pickDateTime = ko.unwrap(params.pickDateTime) as boolean;
		viewModel.options = ko.observableArray([]);
		viewModel.options.push({Key: null, Value: window.Helper.String.getTranslatedString("All")});
		if (params.allowPastDates) {
			viewModel.options.push({Key: "LastMonth", Value: window.Helper.String.getTranslatedString("LastMonth")});
		}
		if (params.allowFutureDates) {
			viewModel.options.push({
				Key: "Next3Months",
				Value: window.Helper.String.getTranslatedString("Next3Months")
			});
		}
		viewModel.options.push({Key: "ThisMonth", Value: window.Helper.String.getTranslatedString("ThisMonth")});
		if (params.allowFutureDates) {
			viewModel.options.push({Key: "NextMonth", Value: window.Helper.String.getTranslatedString("NextMonth")});
		}
		viewModel.options.push({Key: "ThisYear", Value: window.Helper.String.getTranslatedString("ThisYear")});
		if (params.allowPastDates) {
			viewModel.options.push({Key: "LastYear", Value: window.Helper.String.getTranslatedString("LastYear")});
		}
		viewModel.options.push({Key: "FromTo", Value: window.Helper.String.getTranslatedString("FromTo")});

		viewModel.selectedOption = window.ko.observable("All");
		if (params.value() !== null && params.value().Value !== null) {
			viewModel.selectedOption(params.value().Value);
		}
		if (viewModel.selectedOption() === "FromTo") {
			if (params.value().DateFrom)
				viewModel.dateFrom(params.value().DateFrom);
			if (params.value().DateTo)
				viewModel.dateTo(params.value().DateTo);
			if (!params.value().DateTo && !params.value().DateFrom)
				viewModel.selectedOption("All");
		}
		viewModel.showDatePickers = ko.pureComputed(function () {
			return viewModel.selectedOption() === "FromTo";
		});
		viewModel.value = ko.pureComputed(function () {
			let dateFrom: moment.Moment = null;
			let dateTo: moment.Moment = null;
			const result = {
				Value: viewModel.selectedOption(),
				displayedValue: undefined,
				Type: "date",
				DateFrom: undefined,
				DateTo: undefined,
				caption: viewModel.captionKey()
			};
			const momentFn = viewModel.keepTimeZone ? window.moment : window.moment.utc;
			if (viewModel.selectedOption() === "FromTo") {
				if (viewModel.dateFrom() !== null) {
					dateFrom = momentFn(viewModel.dateFrom());
					if (!viewModel.pickDateTime) {
						dateFrom = dateFrom.startOf("day");
					}
					result.DateFrom = dateFrom;
				}

				if (viewModel.dateTo() !== null) {
					dateTo = momentFn(viewModel.dateTo());
					if (!viewModel.pickDateTime) {
						dateTo = dateTo.endOf("day");
					}
					result.DateTo = dateTo;
				}
			}

			result.caption = viewModel.captionKey();
			if (viewModel.selectedOption() === "FromTo") {
				const displayFormat = viewModel.pickDateTime ? window.moment.localeData().longDateFormat('L') + " " + window.moment.localeData().longDateFormat('LT') : window.moment.localeData().longDateFormat('L');
				if (dateFrom) {
					if (dateTo) {
						result.displayedValue = momentFn(dateFrom).format(displayFormat) + " - " + momentFn(dateTo).format(displayFormat);
					} else {
						result.displayedValue = momentFn(dateFrom).format(displayFormat) + " - " + window.Helper.String.getTranslatedString("TimeAgoRightNow");
					}
				} else if (dateTo) {
					result.displayedValue = window.Helper.String.getTranslatedString("TimeAgoRightNow") + " - " + momentFn(dateTo).format(displayFormat);
				}
			} else {
				result.displayedValue = window.Helper.String.getTranslatedString(result.Value);
			}
			return result;
		});
		viewModel.value.subscribe(function (x) {
			params.value(x);
		});
	}
});