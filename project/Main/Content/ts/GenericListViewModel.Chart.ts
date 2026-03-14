import {namespace} from "./namespace";

export class GenericListChartViewModel<TModel extends $data.Entity, TObservableModel extends $data.Entity> extends window.Main.ViewModels.GenericListViewModel<TModel, TObservableModel> {

	chartAxisXLabel = ko.observable<string>(null);
	chartAxisYLabel = ko.observable<string>(null);
	chartAxisYLabels = ko.observableArray<string>([]);
	showChart = ko.observable<boolean>(false);

	constructor(entityType, orderBy?, orderByDirection?, joins?, groupBys?, map?) {
		super(entityType, orderBy, orderByDirection, joins, groupBys, map);
	}

	getChartColor(item: any): string {
		return null;
	}

	getChartLabel(item: any): string {
		return null;
	}

	toggleChart(): void {
		if (this.viewMode().Key !== "List") {
			this.viewMode(ko.utils.arrayFirst(this.viewModes(), viewMode => viewMode.Key === "List"));
		}
		this.showChart(!this.showChart());
	}
}
namespace("Main.ViewModels").GenericListChartViewModel = GenericListChartViewModel;