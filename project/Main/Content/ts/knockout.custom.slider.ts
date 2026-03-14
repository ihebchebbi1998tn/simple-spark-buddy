ko.bindingHandlers.slider = {
	defaults: {
		min: 0,
		max: 10,
		step: null,
		sliderArray: null,
		valueFormat: (val) => { return val; },
		markerFormat: null,
		tooltipFormat: null,
	},
	init: function(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
		var value = valueAccessor().value || valueAccessor();
		var options = $.extend({}, ko.bindingHandlers.slider.defaults, ko.unwrap(valueAccessor().options) || {});
		var sliderOptions = ko.bindingHandlers.slider.defaults;
		sliderOptions.start = [ko.unwrap(value)];
		sliderOptions.step = options.step;
		if (options.min && options.max) {
			sliderOptions.range = { min: options.min, max: options.max };
		}
		if (options.sliderArray) {
			sliderOptions.range = { min: 0, max: options.sliderArray.length - 1 };
			sliderOptions.step = 1;
			sliderOptions.format = {
				to: function(x) {
					var val = options.sliderArray[Math.round(x)];
					return options.valueFormat(val);
				},
				from: function (x) {
					return options.sliderArray.findIndex((val) => options.valueFormat(val) == x);
				}
			}
		}
		// @ts-ignore
		$(element).noUiSlider(sliderOptions);
		
		$(element).on('slide', function( event, value ){
			// @ts-ignore
			ko.bindingHandlers.slider.update(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext);
		});
		if (options.markers) {
			var pipsOptions = {
				mode: 'steps',
				density: 100 / sliderOptions.range.max - sliderOptions.range.min,
				format: null,
			}
			if (!options.markerFormat) {
				options.markerFormat = options.valueFormat;
			}
			pipsOptions.format = {
				to: function(x) {
					var val = options.sliderArray[Math.round(x)];
					return options.markerFormat(val);
				}
			}
			// @ts-ignore
			$(element).noUiSlider_pips(pipsOptions);
		}
	},
	update: function(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
		var newValue = $(element).val()
		valueAccessor().value(newValue);
		if (ko.unwrap(valueAccessor().options).tooltipFormat) {
			var options = $.extend({}, ko.bindingHandlers.slider.defaults, ko.unwrap(valueAccessor().options) || {});
			var value = options.sliderArray ? options.sliderArray.find((val) => { return options.valueFormat(val) === newValue; }) : newValue;
			viewModel.tooltip(options.tooltipFormat(value));
		}
	}
};