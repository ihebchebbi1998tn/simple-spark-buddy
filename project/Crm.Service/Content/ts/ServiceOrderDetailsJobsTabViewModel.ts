import type {ServiceOrderDetailsViewModel} from "./ServiceOrderDetailsViewModel";
import {namespace} from "@Main/namespace";
import type {DispatchDetailsViewModel} from "./DispatchDetailsViewModel";
import type {ServiceOrderTemplateDetailsViewModel} from "./ServiceOrderTemplateDetailsViewModel";

export class ServiceOrderDetailsJobsTabItemViewModel {
	closedServiceCasesCount: KnockoutObservable<number>;
	itemGroup: ItemGroup;
	preplannedPostingsCount: KnockoutObservable<number>;
	preplannedServiceOrderMaterialsCount: KnockoutObservable<number>;
	postingsCount: KnockoutObservable<number>;
	serviceCasesCount: KnockoutObservable<number>;
	serviceOrderMaterialsCount: KnockoutObservable<number>;
	addableMaterialsAfterSignatureCount: KnockoutObservable<number>;
	totalPricePreplanned: KnockoutObservable<number>;
	totalPriceUsed: KnockoutObservable<number>;
	totalPriceInvoice: KnockoutObservable<number>;
	isEmpty: KnockoutObservable<boolean>
}

export class ServiceOrderDetailsJobsTabViewModelBase<T extends ServiceOrderDetailsViewModel | ServiceOrderTemplateDetailsViewModel | DispatchDetailsViewModel> extends window.Main.ViewModels.GenericListViewModel<Crm.Service.Rest.Model.CrmService_ServiceOrderTime, Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime, Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime & ServiceOrderDetailsJobsTabItemViewModel> {
	lookups: LookupType;
	serviceOrder: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead>;
	timesCanBeAdded = ko.pureComputed<boolean>(() => {
		return this.parentViewModel.serviceOrderIsEditable() &&
			window.Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode === "JobPerInstallation";
	});
	accumulatedTotalPricePreplanned = ko.pureComputed<number>(() => {
		return this.items().reduce((partialSum, item) => partialSum + item.totalPricePreplanned(), 0);
	})
	accumulatedTotalPriceUsed = ko.pureComputed<number>(() => {
		return this.items().reduce((partialSum, item) => partialSum + item.totalPriceUsed(), 0);
	})
	accumulatedTotalPriceInvoice = ko.pureComputed<number>(() => {
		return this.items().reduce((partialSum, item) => partialSum + item.totalPriceInvoice(), 0);
	})
	parentViewModel: T;

	constructor(parentViewModel: T) {
		super("CrmService_ServiceOrderTime", ["PosNo"], ["ASC"], ["Installation", "Article"]);
		this.parentViewModel = parentViewModel;
		this.lookups = parentViewModel.lookups;
		this.lookups.serviceCaseStatuses = this.lookups.serviceCaseStatuses ||
			{$tableName: "CrmService_ServiceCaseStatus"};
		this.lookups.serviceOrderTimeStatuses = this.lookups.serviceOrderTimeStatuses ||
			{$tableName: "CrmService_ServiceOrderTimeStatus"};
		this.lookups.currencies = this.lookups.currencies || {$tableName: "Main_Currency"};
		this.serviceOrder = parentViewModel.serviceOrder;
		this.infiniteScroll(true);
	}

	canDeleteServiceOrderTime(serviceOrderTime: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime): boolean {
		const hasPermission = window.AuthorizationManager.isAuthorizedForAction("ServiceOrder", window.ko.unwrap(serviceOrderTime.CreateUser) === window.Helper.User.getCurrentUserName() ? "TimeDeleteSelfCreated" : "TimeDelete");
		return hasPermission && this.parentViewModel.serviceOrderIsEditable() && window.Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode === "JobPerInstallation";
	};

	canEditServiceOrderTime(serviceOrderTime: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime): boolean {
		return this.parentViewModel.serviceOrderIsEditable();
	};

	async init(): Promise<void> {
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		await super.init();
		let afterCreateEventHandler = async () => {
			this.loading(true);
			await this.initItems(this.items());
			this.loading(false);
		};
		window.Helper.Database.registerEventHandlers(this,
			{
				CrmService_ServiceOrderMaterial: {
					afterCreate: afterCreateEventHandler
				},
				CrmService_ServiceOrderTimePosting: {
					afterCreate: afterCreateEventHandler
				}
			});
	};

	initItem(item: Crm.Service.Rest.Model.CrmService_ServiceOrderTime): Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime & ServiceOrderDetailsJobsTabItemViewModel {
		let result = super.initItem(item) as Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime & ServiceOrderDetailsJobsTabItemViewModel;
		result.totalPricePreplanned = ko.observable<number>((item.Price - (item.DiscountType == Crm.Article.Model.Enums.DiscountType.Percentage ? item.Price * item.Discount / 100 : item.Discount)) * window.moment.duration(item.EstimatedDuration).asHours());
		result.totalPriceUsed = ko.observable<number>((item.Price - (item.DiscountType == Crm.Article.Model.Enums.DiscountType.Percentage ? item.Price * item.Discount / 100 : item.Discount)) * window.moment.duration(item.ActualDuration).asHours());
		result.totalPriceInvoice = ko.observable<number>((item.Price - (item.DiscountType == Crm.Article.Model.Enums.DiscountType.Percentage ? item.Price * item.Discount / 100 : item.Discount)) * window.moment.duration(item.InvoiceDuration).asHours());
		return result;
	}

	async initItems(items: (Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime & ServiceOrderDetailsJobsTabItemViewModel)[]): Promise<(Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime & ServiceOrderDetailsJobsTabItemViewModel)[]> {
		const queries = [];
		const closedServiceCaseStatusKeys = this.lookups.serviceCaseStatuses.$array.filter(function (serviceCaseStatus) {
			return window.Helper.ServiceCase.belongsToClosed(serviceCaseStatus);
		}).map(function (serviceCaseStatus) {
			return serviceCaseStatus.Key;
		});
		let result = await window.Helper.ServiceOrderMaterial.getValidItemNosAfterSignature();
		let validMaterialItemNosAfterCustomerSignature = result.validMaterialItemNosAfterCustomerSignature;
		let validCostItemNosAfterCustomerSignature = result.validCostItemNosAfterCustomerSignature;
		items.forEach(function (jobSummary: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime & ServiceOrderDetailsJobsTabItemViewModel) {
			queries.push({
				queryable: window.database.CrmService_ServiceCase.filter(function (it) {
						return it.ServiceOrderTimeId === this.serviceOrderTimeId && it.StatusKey in this.statusKeys;
					},
					{serviceOrderTimeId: jobSummary.Id(), statusKeys: closedServiceCaseStatusKeys}),
				method: "count",
				handler: function (count: number) {
					jobSummary.closedServiceCasesCount ||= ko.observable<number>(null);
					jobSummary.closedServiceCasesCount(count);
					return items;
				}
			});
			queries.push({
				queryable: window.database.CrmService_ServiceCase.filter(function (it) {
						return it.ServiceOrderTimeId === this.serviceOrderTimeId;
					},
					{serviceOrderTimeId: jobSummary.Id()}),
				method: "count",
				handler: function (count: number) {
					jobSummary.serviceCasesCount ||= ko.observable<number>(null);
					jobSummary.serviceCasesCount(count);
					return items;
				}
			});
			queries.push({
				queryable: window.database.CrmService_ServiceOrderMaterial.filter(function (it) {
						return it.ServiceOrderTimeId === this.serviceOrderTimeId && it.ServiceOrderMaterialType === window.Crm.Service.ServiceOrderMaterialType.Used;
					},
					{serviceOrderTimeId: jobSummary.Id()}),
				method: "toArray",
				handler: async function (materials: Crm.Service.Rest.Model.CrmService_ServiceOrderMaterial[]) {
					jobSummary.serviceOrderMaterialsCount ||= ko.observable<number>(null);
					jobSummary.serviceOrderMaterialsCount(materials.length);
					jobSummary.addableMaterialsAfterSignatureCount ||= ko.observable<number>(null);
					jobSummary.addableMaterialsAfterSignatureCount(materials.filter(m => validMaterialItemNosAfterCustomerSignature.indexOf(m.ItemNo) !== -1 || validCostItemNosAfterCustomerSignature.indexOf(m.ItemNo) !== -1).length);
					return items;
				}
			});
			queries.push({
				queryable: window.database.CrmService_ServiceOrderMaterial.filter(function (it) {
						return it.ServiceOrderTimeId === this.serviceOrderTimeId && it.ServiceOrderMaterialType === window.Crm.Service.ServiceOrderMaterialType.Preplanned;
					},
					{serviceOrderTimeId: jobSummary.Id()}),
				method: "count",
				handler: function (count: number) {
					jobSummary.preplannedServiceOrderMaterialsCount ||= ko.observable<number>(null);
					jobSummary.preplannedServiceOrderMaterialsCount(count);
					return items;
				}
			});
			queries.push({
				queryable: window.database.CrmService_ServiceOrderTimePosting.filter(function (it) {
						return it.ServiceOrderTimeId === this.serviceOrderTimeId && it.ServiceOrderTimePostingType === window.Crm.Service.ServiceOrderTimePostingType.Used;
					},
					{serviceOrderTimeId: jobSummary.Id()}),
				method: "count",
				handler: function (count: number) {
					jobSummary.postingsCount ||= ko.observable<number>(null);
					jobSummary.postingsCount(count);
					return items;
				}
			});
			queries.push({
				queryable: window.database.CrmService_ServiceOrderTimePosting.filter(function (it) {
						return it.ServiceOrderTimeId === this.serviceOrderTimeId && it.ServiceOrderTimePostingType === window.Crm.Service.ServiceOrderTimePostingType.Preplanned;
					},
					{serviceOrderTimeId: jobSummary.Id()}),
				method: "count",
				handler: function (count: number) {
					jobSummary.preplannedPostingsCount ||= ko.observable<number>(null);
					jobSummary.preplannedPostingsCount(count);
					return items;
				}
			});
			jobSummary.isEmpty = ko.pureComputed(() => {
				return jobSummary.serviceCasesCount() === 0 && jobSummary.serviceOrderMaterialsCount() === 0 && jobSummary.postingsCount() === 0;
			});
		});
		await window.Helper.Batch.Execute(queries)
		return items;
	};

	applyFilters(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTime>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTime> {
		query = super.applyFilters(query);
		query = query
			.filter(function (it) {
					return it.OrderId === this.orderId;
				},
				{orderId: this.serviceOrder().Id()});
		return query;
	};

	async confirmDeleteServiceOrderTime(serviceOrderTime: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime): Promise<void> {
		let confirm = await window.Helper.Confirm.confirmDeleteAsync();
		if (confirm) {
			this.loading(true);
			await this.deleteServiceOrderTime(serviceOrderTime);
			await this.filter();
		}
	}

	async deleteServiceOrderTime(serviceOrderTime: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime): Promise<void> {
		const serviceOrderTimeId = window.ko.unwrap(serviceOrderTime.Id);
		let serviceOrderMaterials = await window.database.CrmService_ServiceOrderMaterial.filter(
			function (it) {
				return it.ServiceOrderTimeId === this.serviceOrderTimeId;
			},
			{serviceOrderTimeId: serviceOrderTimeId})
			.toArray();
		for (const serviceOrderMaterial of serviceOrderMaterials) {
			window.database.remove(serviceOrderMaterial);
		}

		let serviceOrderTimePostings = await window.database.CrmService_ServiceOrderTimePosting.filter(
			function (it) {
				return it.ServiceOrderTimeId === this.serviceOrderTimeId;
			},
			{serviceOrderTimeId: serviceOrderTimeId})
			.toArray();
		for (const serviceOrderTimePosting of serviceOrderTimePostings) {
			window.database.remove(serviceOrderTimePosting);
		}


		let documentAttributes = await window.database.Crm_DocumentAttribute.filter(
			function (it) {
				return it.ExtensionValues.ServiceOrderTimeId === this.serviceOrderTimeId;
			},
			{serviceOrderTimeId: serviceOrderTimeId})
			.toArray();
		for (const documentAttribute of documentAttributes) {
			window.database.remove(documentAttribute);
		}
		window.database.remove(serviceOrderTime);
		await window.database.saveChanges();
	}

	getAvatarColor(serviceOrderTime: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime): string {
		return window.Helper.Lookup.getLookupColor(this.lookups.serviceOrderTimeStatuses, serviceOrderTime.StatusKey);
	};

	getInstallationAutocompleteFilter(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_Installation>, term: string): $data.Queryable<Crm.Service.Rest.Model.CrmService_Installation> {
		if (term) {
			query = window.Helper.String.contains(query, term, ["InstallationNo", "Description"]);
		}
		return query.filter("filterByServiceOrderTimes", { orderId: this.serviceOrder().Id() });
	}
}

namespace("Crm.Service.ViewModels").ServiceOrderDetailsJobsTabViewModelBase = ServiceOrderDetailsJobsTabViewModelBase;

export class ServiceOrderDetailsJobsTabViewModel extends ServiceOrderDetailsJobsTabViewModelBase<ServiceOrderDetailsViewModel> {
}

namespace("Crm.Service.ViewModels").ServiceOrderDetailsJobsTabViewModel = ServiceOrderDetailsJobsTabViewModel;