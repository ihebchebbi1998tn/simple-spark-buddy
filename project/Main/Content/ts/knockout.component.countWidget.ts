import {registerComponent} from "./componentRegistrar";

registerComponent({
	componentName: "count-widget",
	template: "Main/Template/CountWidget",
	viewModel: {
		createViewModel: function (params, componentInfo) {
			return new CountWidgetViewModel(params);
		}
	}
});

class CountWidgetViewModel {
	loading: KnockoutObservable<boolean>;
	caption: KnockoutComputed<string>;
	query: $data.Queryable<any>;
	groupBy: string;
	lookupTable: string;
	urlTemplate: (priorityKey: any) => string;
	lookups: KnockoutObservableArray<any> = window.ko.observableArray([]);
	data: KnockoutObservable<any> = window.ko.observable({});
	min: KnockoutObservable<number> = window.ko.observable(0);
	max: KnockoutObservable<number> = window.ko.observable(0);
	
	constructor(params = null) {
		this.loading = window.ko.observable(true);
		this.caption = window.ko.pureComputed(function() {
			return window.Helper.String.getTranslatedString(ko.unwrap(params.caption), ko.unwrap(params.caption)) ||
				ko.unwrap(params.caption);
		});
		this.init(params).then(() => {});
	}
	async init(params: any) {
		this.query = params.query;
		this.groupBy = params.groupBy;
		this.lookupTable = params.lookupTable;
		this.urlTemplate = params.urlTemplate;

		if(params.options) {
			const options = await params.options();
			this.query = options.query;
			this.groupBy = options.groupBy;
			this.lookupTable = options.lookupTable;
			this.urlTemplate = options.urlTemplate;
		}

		this.lookups(await window.Helper.Lookup.getLocalized(this.lookupTable));
		if(this.lookups().find(x => x.Key === null)) {
			this.lookups().find(x => x.Key === null).Value = window.Helper.String.getTranslatedString("Unspecified");
		}

		for(let lookup of this.lookups()) {
			this.data()[lookup.Value] = 0;
		}

		const data = await this.query.map(`it.${this.groupBy}`).toArray();
		for(let entry of data) {
			const lookupValue = this.lookups().find((x) => x.Key === entry).Value;
			this.data()[lookupValue]++;
			if(this.data()[lookupValue] > this.max()) {
				this.max(this.data()[lookupValue]);
			}
		}
		this.data.valueHasMutated();
		this.loading(false);
	};
	getValue(value: string): number {
		return this.data()[value];
	};
	getUrl(value: string): string {
		if(!this.urlTemplate) {
			return null;
		}
		const key = this.lookups().find(x => x.Value === value).Key;
		if(key == null) {
			return this.urlTemplate(key).replace("{1}", window.Helper.String.getTranslatedString("Unspecified"));
		}
		return this.urlTemplate(key).replace("{1}", value);
	};
	getProgressBarWidth(value: string): number {
		return this.getValue(value)/this.max()*100;
	};
	getColor(value: string): string {
		return this.lookups().find(x => x.Value === value)?.Color || "gray";
	};
}