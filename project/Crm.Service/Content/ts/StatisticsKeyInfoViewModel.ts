import {namespace} from "@Main/namespace";

export class StatisticsKeyInfoViewModel {
	entity: Crm.Service.Rest.Model.ObservableCrmService_ServiceCase | Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderStatisticsKey
	tableName: string;
	id: string;
	showHeader: boolean;
	condition: KnockoutComputed<boolean>;
	getEditRoute = ko.observable<string>(null);

	ProductTypeKey: KnockoutObservable<string>;
	MainAssemblyKey: KnockoutObservable<string>;
	SubAssemblyKey: KnockoutObservable<string>;
	AssemblyGroupKey: KnockoutObservable<string>;
	FaultImageKey: KnockoutObservable<string>;
	RemedyKey: KnockoutObservable<string>;
	CauseKey: KnockoutObservable<string>;
	WeightingKey: KnockoutObservable<string>;
	CauserKey: KnockoutObservable<string>;

	constructor(params) {
		this.entity = ko.unwrap(params.entity);
		this.tableName = ko.unwrap(params.tableName);
		this.showHeader = ko.unwrap(params.showHeader) ?? true;
		this.id = ko.unwrap(params.id) || ko.unwrap(this.entity.Id) || null;
		this.condition = ko.computed(() => ko.unwrap(params.condition) !== false);
		if (this.tableName) {
			this.getEditRoute("Crm.Service/StatisticsKey/EditTemplate/" + this.id + "?tableName=" + this.tableName);
			if (params.dispatchId){
				this.getEditRoute(this.getEditRoute() + "&dispatchId=" + ko.unwrap(params.dispatchId));
			}
			if (params.serviceOrderId){
				this.getEditRoute(this.getEditRoute() + "&serviceOrderId=" + ko.unwrap(params.serviceOrderId));
			}
		} else {
			this.getEditRoute("Crm.Service/StatisticsKey/EditTemplate");
		}
		if (this.entity instanceof Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderStatisticsKey) {
			this.ProductTypeKey = this.entity.ProductTypeKey;
			this.MainAssemblyKey = this.entity.MainAssemblyKey;
			this.SubAssemblyKey = this.entity.SubAssemblyKey;
			this.AssemblyGroupKey = this.entity.AssemblyGroupKey;
			this.FaultImageKey = this.entity.FaultImageKey;
			this.RemedyKey = this.entity.RemedyKey;
			this.CauseKey = this.entity.CauseKey;
			this.WeightingKey = this.entity.WeightingKey;
			this.CauserKey = this.entity.CauserKey;
		} else {
			this.ProductTypeKey = this.entity.StatisticsKeyProductTypeKey;
			this.MainAssemblyKey = this.entity.StatisticsKeyMainAssemblyKey;
			this.SubAssemblyKey = this.entity.StatisticsKeySubAssemblyKey;
			this.AssemblyGroupKey = this.entity.StatisticsKeyAssemblyGroupKey;
			this.FaultImageKey = this.entity.StatisticsKeyFaultImageKey;
			this.RemedyKey = this.entity.StatisticsKeyRemedyKey;
			this.CauseKey = this.entity.StatisticsKeyCauseKey;
			this.WeightingKey = this.entity.StatisticsKeyWeightingKey;
			this.CauserKey = this.entity.StatisticsKeyCauserKey;
		}
	}
}

namespace("Crm.Service.ViewModels").StatisticsKeyInfoViewModel = StatisticsKeyInfoViewModel