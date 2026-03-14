ko.bindingHandlers.colorpicker = {
	defaults: {
		customClass: "colorpicker-2x",
		format: "rgb",
		sliders: {
			saturation: {
				maxLeft: 200,
				maxTop: 200
			},
			hue: {
				maxTop: 200
			},
			alpha: {
				maxTop: 200
			}
		}
	},
	init: function(element, valueAccessor) {
		var value = valueAccessor().value || valueAccessor();
		var options = $.extend({}, ko.bindingHandlers.colorpicker.defaults, valueAccessor().options || {});
		// @ts-ignore
		$(element).colorpicker(options);
		$(element).on("changeColor",
			function(e: any) {
				var newValue = e.color.toHex();
				if (value && (!value() || (value() && value().toLowerCase() !== newValue.toLowerCase()))) {
					value(newValue);
				}
			});
		ko.utils.domNodeDisposal.addDisposeCallback(element,
			function() {
				// @ts-ignore
				$(element).colorpicker("destroy");
			});
	},
	update: function(element, valueAccessor) {
		var value = ko.unwrap(valueAccessor().value || valueAccessor());
		// @ts-ignore
		$(element).colorpicker("setValue", value);
	}
};