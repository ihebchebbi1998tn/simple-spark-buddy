import {registerComponent} from "./componentRegistrar";

registerComponent({
	componentName: "timeRange-filter",
	template: "Main/Template/TimeRangeFilter",
	viewModel: function (params) {
		const viewModel = this;
		viewModel.captionKey = ko.observable(params.caption);
		viewModel.caption = !!params.ignoreCaption ? params.caption : ko.observable(window.Helper.String.getTranslatedString(params.caption));
		viewModel.minInput = ko.observable(null);
		viewModel.maxInput = ko.observable(null);
		viewModel.options = ko.observableArray([]);

		if (params.value()) {
			viewModel.minInput(params.value().Min);
			viewModel.maxInput(params.value().Max);
		}

		viewModel.value = ko.pureComputed(function () {
			let from = viewModel.minInput();
			let to = viewModel.maxInput();
			let result: any = {Type: "timeRange"};

			if (!!from) {
				result.Min = window.moment.duration(from).format('hh:mm', {trim: false});
			}

			if (!!to) {
				result.Max = window.moment.duration(to).format('hh:mm', {trim: false});
			}

			if (!!result.Max && !!result.Max && result.Max < result.Min) {
				let tmp = result.Max;
				result.Max = result.Min;
				result.Min = tmp;
			}

			if (!result.hasOwnProperty("Min") && !result.hasOwnProperty("Max")) {
				return null;
			}

			result.caption = viewModel.captionKey();
			return result;
		});
		viewModel.value.subscribe(function (x) {
			params.value(x);
		});
	}
});