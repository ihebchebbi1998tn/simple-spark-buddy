import {registerComponent} from "./componentRegistrar";

registerComponent({
	componentName: "flot-chart",
	template: "Main/Template/FlotChart",
	viewModel: function (params) {
		const viewModel = this;
		viewModel.data = window.ko.observable([]);
		viewModel.height = ko.unwrap(params.height) || "300px";
		viewModel.loading = ko.observable<boolean>(true);
		viewModel.options = ko.unwrap(params.options) || {};
		const getColor = ko.unwrap(params.color) || function () {
			return null;
		};
		const getDefaultLabel = function (labelKey) {
			return labelKey;
		};
		const getLabel = ko.unwrap(params.label) || function () {
			return null;
		};

		function updateData() {
			viewModel.loading(true);
			let query = ko.unwrap(params.source);
			query = params.mapAndGroupBy(query);
			query.toArray(function (results) {
				const xValues = window._.uniq(results.map(function (result) {
					return result.x;
				}));
				results = results.map(function (result) {
					return {label: result.d, data: [result.x, result.y]};
				});
				const groupedResults = window._.groupBy(results, function (result) {
					return result.label;
				});
				const labels = Object.getOwnPropertyNames(groupedResults);
				labels.forEach(function (label) {
					xValues.forEach(function (xValue) {
						const value = window._.find(groupedResults[label], function (groupedResult) {
							return groupedResult.data[0] === xValue;
						});
						if (!value) {
							groupedResults[label].push({data: [xValue, 0], label: label});
						}
					});
					groupedResults[label].sort(function (a, b) {
						if ($.isNumeric(a.data[0]) && $.isNumeric(b.data[0])) {
							return a.data[0] - b.data[0];
						}
						return a.data[0].localeCompare(b.data[0]);
					});
				});
				viewModel.data(labels.map(function (label) {
					return {
						color: getColor(label),
						label: getLabel(label) || getDefaultLabel(label),
						data: groupedResults[label].map(function (groupedResult) {
							return groupedResult.data;
						}),
						unit: ko.unwrap(params.unit) || null
					};
				}));
				viewModel.loading(false);
			});
		}

		function updateLabels() {
			if (ko.unwrap(params.axisXLabel)) {
				viewModel.options.xaxes = viewModel.options.xaxes || [{}];
				viewModel.options.xaxes[0].axisLabel = window.Helper.String.getTranslatedString(ko.unwrap(params.axisXLabel), ko.unwrap(params.axisXLabel));
			}
			if (ko.unwrap(params.axisYLabel)) {
				viewModel.options.yaxes = viewModel.options.yaxes || [{}];
				viewModel.options.yaxes[0].axisLabel = window.Helper.String.getTranslatedString(ko.unwrap(params.axisYLabel), ko.unwrap(params.axisYLabel));
				const unit = ko.unwrap(params.unit);
				if (unit) {
					viewModel.options.yaxes[0].axisLabel += " (" + unit + ")";
				}
			}
		}

		updateLabels();
		updateData();
		if (ko.isObservable(params.source)) {
			params.source.subscribe(updateData);
		}
		if (ko.isObservable(params.axisXLabel)) {
			params.axisXLabel.subscribe(updateLabels);
			params.axisXLabel.subscribe(updateData);
		}
		if (ko.isObservable(params.axisYLabel)) {
			params.axisYLabel.subscribe(updateLabels);
			params.axisYLabel.subscribe(updateData);
		}
	}
});