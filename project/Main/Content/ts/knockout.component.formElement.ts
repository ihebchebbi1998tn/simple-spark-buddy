import {registerComponent} from "./componentRegistrar";

registerComponent({
	componentName: "form-element",
	template: "Main/Template/FormElement",
	viewModel: function (params) {
		const viewModel = this;
		viewModel.hasFocus = !!ko.unwrap(params.hasFocus);
		viewModel.caption = ko.pureComputed(function () {
			return window.Helper.String.getTranslatedString(ko.unwrap(params.caption), ko.unwrap(params.caption) || "");
		});
		viewModel.placeholder = ko.pureComputed(function () {
			const placeholder = ko.unwrap(params.placeholder) || (viewModel.type === "select" || viewModel.type === "select2" ? (viewModel.isRequired() ? "PleaseSelect" : "Unspecified") : "");
			return window.Helper.String.getTranslatedString(placeholder, placeholder);
		});
		viewModel.isRequired = ko.pureComputed(function () {
			let isRequired = params.required ? params.required() : false;
			if (isRequired) {
				return isRequired;
			}
			if (!!viewModel.value && !!viewModel.value.rules) {
				viewModel.value.rules().forEach(function (rule) {
					if (rule.rule === "required" && rule.params === true && (!rule.condition || rule.condition())) {
						isRequired = true;
					}
				});
			}
			return isRequired;
		});
		viewModel.maxLength = ko.pureComputed(function () {
			let maxLength = "";
			if (!!viewModel.value && !!viewModel.value.rules) {
				viewModel.value.rules().forEach(function (rule) {
					if (rule.rule === "maxLength") {
						maxLength = rule.params;
					}
				});
			}
			return maxLength;
		});
		viewModel.name = ko.pureComputed(function () {
			return ko.unwrap(params.name) || ko.unwrap(params.caption) || null;
		});
		viewModel.loading = ko.observable(false);
		viewModel.type = params.type;
		viewModel.value = params.value;
		viewModel.accept = params.accept;
		viewModel.disable = params.disable || false;
		viewModel.datePickerOptions = ko.pureComputed(function () {
			const datePickerOptions = ko.unwrap(params.datePickerOptions || {});
			datePickerOptions.pickDuration = datePickerOptions.pickDuration || viewModel.type === "durationPicker";
			datePickerOptions.pickTime = datePickerOptions.pickTime || viewModel.type === "timePicker";
			datePickerOptions.onlyTime = datePickerOptions.onlyTime || false;
			return datePickerOptions;
		});
		viewModel.sliderOptions = params.sliderOptions;
		viewModel.tooltip = ko.observable();
		if (viewModel.type === "durationPicker") {
			var max = params.max || null;
			var options = ko.unwrap(viewModel.datePickerOptions || {});
			var config = options.config || {};
			viewModel.stepping = config.stepping || 5;
			viewModel.minutes = ko.pureComputed(function () {
				return window.moment.duration(viewModel.value()).minutes().toString().padStart(2,'0');
			});
			viewModel.hours = ko.pureComputed(function () {
				return (window.moment.duration(viewModel.value()).hours() + window.moment.duration(viewModel.value()).days() * 24).toString().padStart(2,'0');
			});
			viewModel.incrementHours = function () {
				var newValue = window.moment.duration(viewModel.value()).add(1, 'h');
				if (newValue <= max || max === null) {
					viewModel.value(newValue.toString());
				}
			}
			viewModel.decrementHours = function () {
				if (viewModel.hours() > 0) {
					viewModel.value(window.moment.duration(viewModel.value()).add(-1, 'h').toString());
				}
			}
			viewModel.incrementMinutes = function () {
				var newValue = window.moment.duration(viewModel.value()).add(viewModel.stepping, 'm');
				if (newValue <= max || max === null) {
					viewModel.value(newValue.toString());
				}
			}
			viewModel.decrementMinutes = function () {
				viewModel.value(window.moment.duration(viewModel.value()).add(-viewModel.stepping, 'm').toString());
			}
		}
		viewModel.quantityStep = ko.pureComputed(function () {
			let step = "";
			if (["number", "money"].indexOf(viewModel.type) > -1 && params.quantityStep) {
				step = ko.unwrap(params.quantityStep) || "any";
			}
			return step;
		});
		viewModel.min = params.min;
	}
});