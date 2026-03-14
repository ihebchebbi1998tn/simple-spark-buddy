import {registerComponent} from "./componentRegistrar";

registerComponent({
	componentName: "scale-filter",
	template: "Main/Template/ScaleFilter",
	viewModel: function (params) {
		const viewModel = this;
		viewModel.captionKey = ko.observable(params.caption);
		viewModel.caption = !!params.ignoreCaption ? params.caption : ko.observable(window.Helper.String.getTranslatedString(params.caption));
		viewModel.min = ko.observable(params.min);
		const max = params.max === 0 ? null : params.max;
		viewModel.max = ko.observable(max);
		const step = params.step === 0 ? 1 : params.step;
		viewModel.step = ko.observable(step);
		viewModel.hasLimits = ko.observable(!(params.step === 0 || (params.min === params.max)))
		viewModel.minInput = ko.observable(null);
		viewModel.minInput.subscribe((val) => {
			let newVal = val;
			if (!!viewModel.maxInput() && parseInt(val, 10) > parseInt(viewModel.maxInput(), 10)) {
				newVal = viewModel.maxInput();
			} else if (viewModel.max() && parseInt(val, 10) > parseInt(viewModel.max(), 10)) {
				newVal = viewModel.max();
			} else if (parseInt(val, 10) < parseInt(viewModel.min(), 10)) {
				newVal = viewModel.min();
			}
			viewModel.minInput(newVal);
		});
		viewModel.maxInput = ko.observable(null);
		viewModel.maxInput.subscribe((val) => {
			let newVal = val;
			if (!!viewModel.minInput() && parseInt(val, 10) < parseInt(viewModel.minInput(), 10)) {
				newVal = viewModel.minInput();
			} else if (parseInt(val, 10) < parseInt(viewModel.min(), 10)) {
				newVal = viewModel.min();
			} else if (viewModel.max() && parseInt(val, 10) > parseInt(viewModel.max(), 10)) {
				newVal = viewModel.max();
			}
			viewModel.maxInput(newVal);
		});
		const operator = params.operator || '>';
		viewModel.operator = ko.observable(operator);
		viewModel.selectedOption = ko.observable(null);
		viewModel.options = ko.observableArray([]);
		if (viewModel.hasLimits()) {
			viewModel.options.push({Key: "All", Value: window.Helper.String.getTranslatedString("All")});
			for (let i = viewModel.min(); i <= viewModel.max(); i += viewModel.step()) {
				viewModel.options.push({Key: i, Value: viewModel.operator() + " " + i});
			}
			viewModel.options.push({Key: "Custom", Value: window.Helper.String.getTranslatedString("Custom")});
			viewModel.selectedOption("All");
		}
		if (params.value() && params.value().Min !== null && params.value().Max !== null) {
			viewModel.minInput(params.value().Min);
			viewModel.maxInput(params.value().Max);
		}
		if (params.value() !== null && params.value().Value !== null) {
			viewModel.selectedOption(params.value().Value);
		}

		viewModel.showCustomInputs = ko.pureComputed(function () {
			return viewModel.selectedOption() === "Custom";
		});

		viewModel.value = ko.pureComputed(function () {
			const result: any = {Type: "scale", Value: viewModel.selectedOption()};

			if (viewModel.selectedOption() === "All")
				return null;

			if (viewModel.selectedOption() === "Custom" || !viewModel.hasLimits()) {
				const from = viewModel.minInput();
				const to = viewModel.maxInput();
				if (!!from && from !== "") {
					result.Min = from;
				}

				if (!!to && to !== "") {
					result.Max = to;
				}

				if (!result.hasOwnProperty("Min") && !result.hasOwnProperty("Max")) {
					return null;
				}
					
					if(!!from && from !== "" && !!to && to !== ""){
						result.displayedValue = from + " - " + to;
					}else{
						if(!!from && from !== ""){
							result.displayedValue = "> " + from;
						}else{
							result.displayedValue = "< " + to;
						}
					}
			} else {
				result.Operator = viewModel.operator();
			}

			result.caption = viewModel.captionKey();
				if(!result.displayedValue){
					result.displayedValue = viewModel.options().filter((option) =>{
						return option.Key == viewModel.selectedOption();
					})[0].Value;
				}
				
			return result;
		});
		viewModel.value.subscribe(function (x) {
			params.value(x);
		});
	}
});