import {registerComponent} from "./componentRegistrar";

export class MiniChartViewModel {
	loading = ko.observable<boolean>(true);
	caption: KnockoutComputed<string>;
	color: KnockoutObservable<string>;
	count = ko.observable<number>(null);
	url: KnockoutObservable<string>;

	constructor(params) {
		this.caption = ko.pureComputed(() => window.Helper.String.getTranslatedString(ko.unwrap(params.caption), ko.unwrap(params.caption)) ||
			ko.unwrap(params.caption));
		this.color = ko.observable(ko.unwrap(params.color));
		params.count().then(count => {
			this.count(count);
			this.loading(false);
		});
		this.url = ko.observable(ko.unwrap(params.url));
	}
}

registerComponent({
	componentName: "mini-chart",
	template: "Main/Template/MiniChart",
	viewModel: {
		createViewModel: function (params, componentInfo) {
			return new MiniChartViewModel(params);
		}
	}
});