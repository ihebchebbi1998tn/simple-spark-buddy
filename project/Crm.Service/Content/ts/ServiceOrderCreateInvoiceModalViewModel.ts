import type {ServiceOrderDetailsViewModel} from "@Crm.Service/ServiceOrderDetailsViewModel";
import type {ServiceOrderDetailsErrorTabViewModel} from "@Crm.Service/ServiceOrderDetailsErrorTabViewModel";
import type {ServiceOrderDetailsMaterialsTabViewModel} from "@Crm.Service/ServiceOrderDetailsMaterialsTabViewModel";
import type {
	ServiceOrderDetailsTimePostingsTabViewModel
} from "@Crm.Service/ServiceOrderDetailsTimePostingsTabViewModel";
import {namespace} from "@Main/namespace";

export class ServiceOrderCreateInvoiceModalViewModel extends window.Main.ViewModels.ViewModelBase {

	serviceOrder = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead>(null);
	serviceOrderErrors: ServiceOrderDetailsErrorTabViewModel;
	serviceOrderMaterials: ServiceOrderDetailsMaterialsTabViewModel;
	serviceOrderTimePostings: ServiceOrderDetailsTimePostingsTabViewModel;
	invoicePreplannedCosts = ko.observable<boolean>(false);
	invoicePreplannedMaterials = ko.observable<boolean>(false);
	invoicePreplannedTimePostings = ko.observable<boolean>(false);
	invoiceUsedCosts = ko.observable<boolean>(false);
	invoiceUsedMaterials = ko.observable<boolean>(false);
	invoiceUsedTimePostings = ko.observable<boolean>(false);
	lookups: LookupType = {
		statisticsKeyCauses: { $tableName: "CrmService_StatisticsKeyCause" },
		statisticsKeyFaultImages: { $tableName: "CrmService_StatisticsKeyFaultImage" }
	};
	selectedServiceOrderErrors = ko.observableArray<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderErrorType>(null);
	selectedServiceOrderStatisticsKey = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderStatisticsKey>(null);

	preplannedCostsPresent: KnockoutComputed<boolean> ;
	preplannedMaterialsPresent: KnockoutComputed<boolean>;
	preplannedTimePostingsPresent: KnockoutComputed<boolean>;
	costsPresent: KnockoutComputed<boolean>;
	materialsPresent: KnockoutComputed<boolean>;
	timePostingsPresent: KnockoutComputed<boolean>;

	constructor(parentViewModel: ServiceOrderDetailsViewModel) {
		super();
		this.serviceOrderErrors = new window.Crm.Service.ViewModels.ServiceOrderDetailsErrorTabViewModel(parentViewModel);
		let serviceOrderErrorsApplyFilters = this.serviceOrderErrors.applyFilters;
		this.serviceOrderErrors.applyFilters = (query) => {
			return serviceOrderErrorsApplyFilters.call(this.serviceOrderErrors, query).filter(it => it.ParentServiceOrderErrorTypeId !== null);
		}
		this.serviceOrderErrors.bulkActions.push({Name: null});
		this.serviceOrderErrors.joins.push("ServiceOrderDispatch");
		this.serviceOrderErrors.joins.push("ServiceOrderErrorCauses.ServiceOrderDispatch");
		this.serviceOrderMaterials = new window.Crm.Service.ViewModels.ServiceOrderDetailsMaterialsTabViewModel(parentViewModel);
		this.serviceOrderMaterials.bulkActions.push({Name: null});
		this.serviceOrderMaterials.hideActions = true;
		let serviceOrderMaterialsApplyFilters = this.serviceOrderMaterials.applyFilters;
		this.serviceOrderMaterials.applyFilters = (query) => {
			return serviceOrderMaterialsApplyFilters.call(this.serviceOrderMaterials, query).filter(it => it.ServiceOrderMaterialType !== window.Crm.Service.ServiceOrderMaterialType.Invoice);
		}
		this.serviceOrderMaterials.isVisible = (serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): boolean => {
			return this.serviceOrderMaterials.getInvoiceQuantity(serviceOrderMaterial) === 0;
		}
		this.serviceOrderTimePostings = new window.Crm.Service.ViewModels.ServiceOrderDetailsTimePostingsTabViewModel(parentViewModel);
		this.serviceOrderTimePostings.bulkActions.push({Name: null});
		this.serviceOrderTimePostings.hideActions = true;
		let serviceOrderTimePostingsApplyFilters = this.serviceOrderTimePostings.applyFilters;
		this.serviceOrderTimePostings.applyFilters = (query) => {
			return serviceOrderTimePostingsApplyFilters.call(this.serviceOrderTimePostings, query).filter(it => it.ServiceOrderTimePostingType !== window.Crm.Service.ServiceOrderTimePostingType.Invoice);
		}
		this.serviceOrderTimePostings.isVisible = (serviceOrderTimePosting: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting): boolean => {
			return window.moment.duration(this.serviceOrderTimePostings.getInvoiceDuration(serviceOrderTimePosting)).asMinutes() === 0;
		}

		this.invoicePreplannedCosts.subscribe(val => {
			this.serviceOrderMaterials.selectedItems.remove((x: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial) => x.ArticleTypeKey() === "Cost" && x.ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Preplanned);
			if (val === true) {
				this.serviceOrderMaterials.selectedItems.push(...this.serviceOrderMaterials.items().filter((x: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial) => this.serviceOrderMaterials.isVisible(x) && x.ArticleTypeKey() === "Cost" && x.ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Preplanned));
				this.invoiceUsedCosts(false);
			}
		});
		this.invoicePreplannedMaterials.subscribe(val => {
			this.serviceOrderMaterials.selectedItems.remove((x: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial) => x.ArticleTypeKey() === "Material" && x.ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Preplanned);
			if (val === true) {
				this.serviceOrderMaterials.selectedItems.push(...this.serviceOrderMaterials.items().filter((x: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial) => this.serviceOrderMaterials.isVisible(x) && x.ArticleTypeKey() === "Material" && x.ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Preplanned));
				this.invoiceUsedMaterials(false);
			}
		});
		this.invoicePreplannedTimePostings.subscribe(val => {
			this.serviceOrderTimePostings.selectedItems.remove((x: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting) => x.ServiceOrderTimePostingType() === window.Crm.Service.ServiceOrderTimePostingType.Preplanned);
			if (val === true) {
				this.serviceOrderTimePostings.selectedItems.push(...this.serviceOrderTimePostings.items().filter((x: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting) => this.serviceOrderTimePostings.isVisible(x) && x.ServiceOrderTimePostingType() === window.Crm.Service.ServiceOrderTimePostingType.Preplanned));
				this.invoiceUsedTimePostings(false);
			}
		});
		this.invoiceUsedCosts.subscribe(val => {
			this.serviceOrderMaterials.selectedItems.remove((x: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial) => x.ArticleTypeKey() === "Cost" && x.ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Used);
			if (val === true) {
				this.serviceOrderMaterials.selectedItems.push(...this.serviceOrderMaterials.items().filter((x: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial) => this.serviceOrderMaterials.isVisible(x) && x.ArticleTypeKey() === "Cost" && x.ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Used));
				this.invoicePreplannedCosts(false);
			}
		});
		this.invoiceUsedMaterials.subscribe(val => {
			this.serviceOrderMaterials.selectedItems.remove((x: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial) => x.ArticleTypeKey() === "Material" && x.ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Used);
			if (val === true) {
				this.serviceOrderMaterials.selectedItems.push(...this.serviceOrderMaterials.items().filter((x: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial) => this.serviceOrderMaterials.isVisible(x) && x.ArticleTypeKey() === "Material" && x.ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Used));
				this.invoicePreplannedMaterials(false);
			}
		});
		this.invoiceUsedTimePostings.subscribe(val => {
			this.serviceOrderTimePostings.selectedItems.remove((x: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting) => x.ServiceOrderTimePostingType() === window.Crm.Service.ServiceOrderTimePostingType.Used);
			if (val === true) {
				this.serviceOrderTimePostings.selectedItems.push(...this.serviceOrderTimePostings.items().filter((x: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting) => this.serviceOrderTimePostings.isVisible(x) && x.ServiceOrderTimePostingType() === window.Crm.Service.ServiceOrderTimePostingType.Used));
				this.invoicePreplannedTimePostings(false);
			}
		});

		this.preplannedCostsPresent = ko.pureComputed( () => this.serviceOrderMaterials?.items().some((x: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial) => this.serviceOrderMaterials.isVisible(x) && x.ArticleTypeKey() === "Cost" && x.ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Preplanned));
		this.preplannedMaterialsPresent = ko.pureComputed( () => this.serviceOrderMaterials?.items().some((x: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial) => this.serviceOrderMaterials.isVisible(x) && x.ArticleTypeKey() === "Material" && x.ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Preplanned));
		this.preplannedTimePostingsPresent = ko.pureComputed( () => this.serviceOrderTimePostings?.items().some((x: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting) => this.serviceOrderTimePostings.isVisible(x) && x.ServiceOrderTimePostingType() === window.Crm.Service.ServiceOrderTimePostingType.Preplanned));

		this.costsPresent = ko.pureComputed( () => this.serviceOrderMaterials?.items().some((x: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial) => this.serviceOrderMaterials.isVisible(x) && x.ArticleTypeKey() === "Cost" && x.ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Used));
		this.materialsPresent = ko.pureComputed( () => this.serviceOrderMaterials?.items().some((x: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial) => this.serviceOrderMaterials.isVisible(x) && x.ArticleTypeKey() === "Material" && x.ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Used));
		this.timePostingsPresent = ko.pureComputed( () => this.serviceOrderTimePostings?.items().some((x: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting) => this.serviceOrderTimePostings.isVisible(x) && x.ServiceOrderTimePostingType() === window.Crm.Service.ServiceOrderTimePostingType.Used));

	}

	async init(id?: string, params?: { [key: string]: string }): Promise<void> {
		let serviceOrder = await window.database.CrmService_ServiceOrderHead
			.include("ServiceOrderStatisticsKeys")
			.include("ServiceOrderStatisticsKeys.Dispatch")
			.include("ServiceOrderStatisticsKeys.Dispatch.DispatchedUser")
			.find(id);
		this.serviceOrder(serviceOrder.asKoObservable());
		await window.Helper.StatisticsKey.getAvailableLookups(this.lookups, ...this.serviceOrder().ServiceOrderStatisticsKeys());
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		await this.serviceOrderErrors.init();
		await this.serviceOrderMaterials.init();
		await this.serviceOrderTimePostings.init();
		if (serviceOrder.IsCostLumpSum) {
			this.invoicePreplannedCosts(true);
		} else {
			this.invoiceUsedCosts(true);
		}
		if (serviceOrder.IsMaterialLumpSum) {
			this.invoicePreplannedMaterials(true);
		} else {
			this.invoiceUsedMaterials(true);
		}
		if (serviceOrder.IsTimeLumpSum) {
			this.invoicePreplannedTimePostings(true);
		} else {
			this.invoiceUsedTimePostings(true);
		}
	}

	async save(): Promise<void> {
		this.loading(true);
		window.database.attachOrGet(this.serviceOrder().innerInstance);

		for (const selectedServiceOrderMaterial of this.serviceOrderMaterials.selectedItems()) {
			let invoiceServiceOrderMaterial = window.Helper.ServiceOrderMaterial.createInvoicePosition(selectedServiceOrderMaterial);
			window.database.add(invoiceServiceOrderMaterial);
		}

		for (const selectedServiceOrderTimePosting of this.serviceOrderTimePostings.selectedItems()) {
			let invoiceServiceOrderTimePosting = window.Helper.ServiceOrderTimePosting.createInvoicePosition(selectedServiceOrderTimePosting);
			window.database.add(invoiceServiceOrderTimePosting);
		}

		for (const selectedServiceOrderError of this.selectedServiceOrderErrors()) {
			let serviceOrderErrorType = await window.database.CrmService_ServiceOrderErrorType.find(selectedServiceOrderError.ParentServiceOrderErrorTypeId());
			window.database.attachOrGet(serviceOrderErrorType);
			serviceOrderErrorType.Description = selectedServiceOrderError.Description();
			serviceOrderErrorType.InternalRemark = selectedServiceOrderError.InternalRemark();
			serviceOrderErrorType.IsConfirmed = selectedServiceOrderError.IsConfirmed();
			serviceOrderErrorType.IsMainErrorType = selectedServiceOrderError.IsMainErrorType();
			serviceOrderErrorType.IsSuspected = selectedServiceOrderError.IsSuspected();
			serviceOrderErrorType.StatisticsKeyFaultImageKey = selectedServiceOrderError.StatisticsKeyFaultImageKey();
			for (const selectedServiceOrderErrorCause of selectedServiceOrderError.ServiceOrderErrorCauses()) {
				if (selectedServiceOrderErrorCause.ParentServiceOrderErrorCauseId() !== null){
					let serviceOrderErrorCause = await window.database.CrmService_ServiceOrderErrorCause.find(selectedServiceOrderErrorCause.ParentServiceOrderErrorCauseId());
					window.database.attachOrGet(serviceOrderErrorCause);
					serviceOrderErrorCause.Description = selectedServiceOrderErrorCause.Description();
					serviceOrderErrorCause.InternalRemark = selectedServiceOrderErrorCause.InternalRemark();
					serviceOrderErrorCause.IsConfirmed = selectedServiceOrderErrorCause.IsConfirmed();
					serviceOrderErrorCause.IsSuspected = selectedServiceOrderErrorCause.IsSuspected();
					serviceOrderErrorCause.StatisticsKeyCauseKey = selectedServiceOrderErrorCause.StatisticsKeyCauseKey();
				} else {
					let newServiceOrderErrorCause = window.Helper.Database.createClone(selectedServiceOrderErrorCause.innerInstance);
					newServiceOrderErrorCause.Id = window.$data.createGuid().toString().toLowerCase()
					newServiceOrderErrorCause.ChildServiceOrderErrorCauses = [];
					newServiceOrderErrorCause.DispatchId = null;
					newServiceOrderErrorCause.ParentServiceOrderErrorCauseId = null;
					newServiceOrderErrorCause.ServiceOrderErrorTypeId = serviceOrderErrorType.Id;
					window.database.add(newServiceOrderErrorCause);
				}
			}
		}

		if (!!this.selectedServiceOrderStatisticsKey()) {
			let serviceOrderStatisticsKey = this.serviceOrder().ServiceOrderStatisticsKeys().find(x => x.DispatchId() === null);
			if (serviceOrderStatisticsKey) {
				window.database.attachOrGet(serviceOrderStatisticsKey.innerInstance);
				serviceOrderStatisticsKey.ProductTypeKey(this.selectedServiceOrderStatisticsKey().ProductTypeKey());
				serviceOrderStatisticsKey.MainAssemblyKey(this.selectedServiceOrderStatisticsKey().MainAssemblyKey());
				serviceOrderStatisticsKey.SubAssemblyKey(this.selectedServiceOrderStatisticsKey().SubAssemblyKey());
				serviceOrderStatisticsKey.AssemblyGroupKey(this.selectedServiceOrderStatisticsKey().AssemblyGroupKey());
				serviceOrderStatisticsKey.FaultImageKey(this.selectedServiceOrderStatisticsKey().FaultImageKey());
				serviceOrderStatisticsKey.RemedyKey(this.selectedServiceOrderStatisticsKey().RemedyKey());
				serviceOrderStatisticsKey.CauseKey(this.selectedServiceOrderStatisticsKey().CauseKey());
				serviceOrderStatisticsKey.WeightingKey(this.selectedServiceOrderStatisticsKey().WeightingKey());
				serviceOrderStatisticsKey.CauserKey(this.selectedServiceOrderStatisticsKey().CauserKey());
			} else {
				let newServiceOrderStatisticsKey = window.Helper.Database.createClone(this.selectedServiceOrderStatisticsKey().innerInstance) as Crm.Service.Rest.Model.CrmService_ServiceOrderStatisticsKey;
				newServiceOrderStatisticsKey.Id = window.$data.createGuid().toString().toLowerCase();
				newServiceOrderStatisticsKey.DispatchId = null;
				window.database.add(newServiceOrderStatisticsKey);
			}
		}

		let status = await window.Helper.Lookup.getLookupByKeyQuery("CrmService_ServiceOrderStatus", "PostProcessing").first();
		// @ts-ignore
		window.Helper.ServiceOrder.setStatus(this.serviceOrder(), status);
		try {
			await window.database.saveChanges();
			this.loading(false);
			$(".modal:visible").modal("hide");
		} catch (e) {
			this.loading(false);
			window.Log.error(e);
			window.swal(window.Helper.String.getTranslatedString("Error"), (e as Error).message, "error");
		}
	}
}

namespace("Crm.Service.ViewModels").ServiceOrderCreateInvoiceModalViewModel = ServiceOrderCreateInvoiceModalViewModel;