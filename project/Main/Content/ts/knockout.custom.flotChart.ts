;
(function(ko) {
	ko.bindingHandlers.flotChart = {};
	// @ts-ignore
	ko.bindingHandlers.flotChart.defaults = {
		axisLabels: {
			show: true
		},
		xaxes: [
			{
				axisLabel: ""
			}
		],
		yaxes: [
			{
				position: "left",
				axisLabel: ""
			}
		],
		series: {
			stack: true,
			lines: {
				show: false,
				fill: true,
				steps: false
			},
			bars: {
				show: true,
				align: "center",
				barWidth: 0.6,
				lineWidth: 0
			}
		},
		grid: {
			borderWidth: 1,
			borderColor: "#eee",
			show: true,
			hoverable: true,
			clickable: true
		},
		yaxis: {
			tickColor: "#eee",
			tickDecimals: 0,
			tickFormatter: function(val, axis) {
				return window.Globalize.formatNumber(val);
			},
			font: {
				lineHeight: 13,
				style: "normal",
				color: "#9f9f9f"
			},
			shadowSize: 0
		},
		xaxis: {
			tickColor: "#fff",
			tickDecimals: 0,
			font: {
				lineHeight: 13,
				style: "normal",
				color: "#9f9f9f"
			},
			shadowSize: 0
		},
		legend: {
			container: ".flc-bar",
			backgroundOpacity: 0.5,
			noColumns: 0,
			backgroundColor: "white",
			lineWidth: 0
		}
	};
	ko.bindingHandlers.flotChart.init = function(element, valueAccessor) {
		$(element).bind("plothover", function(event, pos, item) {
			if (item) {
				const y = window.Globalize.formatNumber(item.datapoint[1] - item.datapoint[2], {
					minimumFractionDigits: 2,
					maximumFractionDigits: 2
				});
				let tooltip = item.series.label + ": " + y;
				if (item.series.unit) {
					tooltip += " " + item.series.unit;
				}
				$(".flot-tooltip").html(tooltip).css({ top: item.pageY + 5, left: item.pageX + 5 }).show();
			} else {
				$(".flot-tooltip").hide();
			}
		});
	};
	ko.bindingHandlers.flotChart.update = function(element, valueAccessor) {
		const data = ko.unwrap(valueAccessor().data);
		// @ts-ignore
		const options = $.extend({}, ko.bindingHandlers.flotChart.defaults, ko.unwrap(valueAccessor().options));
		$.plot(element, data, options);
	};

})(window.ko);