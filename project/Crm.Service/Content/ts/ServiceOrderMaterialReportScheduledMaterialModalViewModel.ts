import {namespace} from "@Main/namespace";

export class ServiceOrderMaterialReportScheduledMaterialModalViewModel extends window.Main.ViewModels.ViewModelBase {
	serviceOrder = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead>(null);
	dispatch = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch>(null);
	canEditActualQty = ko.observable<boolean>(false);
	currentUser = ko.observable<Main.Rest.Model.Main_User>(null);
	currentServiceOrderTimeId = ko.observable<string>(null);
	showReasons = ko.observableArray<{ materialId: string, showReason: KnockoutObservable<boolean> }>([]);
	validCostItemNosAfterCustomerSignature = ko.observableArray<string>([]);
	validMaterialItemNosAfterCustomerSignature = ko.observableArray<string>([]);
	lookups: LookupType = {
		quantityUnits: {$tableName: "CrmArticle_QuantityUnit"},
		currencies: {$tableName: "Main_Currency"}
	};
	serviceOrderMaterials = ko.observableArray<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial>(null);
	serviceOrderMaterialForServiceOrderTime = ko.pureComputed<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial[]>(() => {
		let materials = this.serviceOrderMaterials().filter(x => x.ServiceOrderTimeId() === this.serviceOrderTimeId());
		if (this.dispatch().StatusKey() === "SignedByCustomer") {
			return materials.filter(m => this.validMaterialItemNosAfterCustomerSignature().indexOf(m.ItemNo()) !== -1 || this.validCostItemNosAfterCustomerSignature().indexOf(m.ItemNo()) !== -1);
		}
		return materials;
	});
	serviceOrderTimeId = ko.observable<string>(null);
	errors: KnockoutValidationErrors;
	isThereAnyStore = ko.observable<boolean>(true);

	getStoreByIdQuery = Helper.Store.filterStoreQueryByStoreNo

	async init(id?: string, params?: { [key: string]: string }): Promise<void> {
		await super.init(id, params);
		if (!params.serviceOrderTimeId || params.serviceOrderTimeId === "null"){
			params.serviceOrderTimeId = null;
		}
		this.serviceOrderTimeId(params.serviceOrderTimeId);
		let dispatch = await window.database.CrmService_ServiceOrderDispatch
			.include("ServiceOrder")
			.find(params.dispatchId);
		this.dispatch(dispatch.asKoObservable());
		this.serviceOrder(this.dispatch().ServiceOrder())

		let user = await window.Helper.User.getCurrentUser();
		this.currentUser(user);
		await this.onServiceOrderTimeSelect(this.serviceOrderTimeId());
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		this.errors = ko.validation.group(this.serviceOrderMaterials, {deep: true});
		let result = await window.Helper.ServiceOrderMaterial.getValidItemNosAfterSignature();
		this.validMaterialItemNosAfterCustomerSignature(result.validMaterialItemNosAfterCustomerSignature);
		this.validCostItemNosAfterCustomerSignature(result.validCostItemNosAfterCustomerSignature);

		let count = await window.database.CrmArticle_Store.count();
		this.isThereAnyStore(count > 0);
	};

	articleIsWarehouseManaged(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial) {
		return serviceOrderMaterial.Article() && serviceOrderMaterial.Article().IsWarehouseManaged();
	}
	
	getServiceOrderTimeAutocompleteDisplay(serviceOrderTime: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime): string {
		return window.Helper.ServiceOrderTime.getAutocompleteDisplay(serviceOrderTime,
			this.currentServiceOrderTimeId());
	};

	getServiceOrderTimeAutocompleteFilter(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTime>, term: string): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTime> {
		query = query.filter(function (it) {
				return it.OrderId === this.orderId;
			},
			{orderId: this.serviceOrder().Id()});
		if (term) {
			query = window.Helper.String.contains(query, term, ["Description.toLowerCase()", "ItemNo.toLowerCase()", "PosNo.toLowerCase()"]);
		}
		return query;
	};
	
	async onServiceOrderTimeSelect(serviceOrderTimeId: string): Promise<void> {
		if (this.serviceOrderMaterials().some(x => x.ServiceOrderTimeId() === serviceOrderTimeId)){
			return;
		}
		let serviceOrderId = this.serviceOrder().Id();
		let dispatchId = this.dispatch().Id();
		let preplannedType = window.Crm.Service.ServiceOrderMaterialType.Preplanned;
		let serviceOrderMaterialsQuery = () =>
			window.database.CrmService_ServiceOrderMaterial
				.include("Article")
				.include("Article.QuantityUnitEntry")
				.include("DocumentAttributes")
				.include("DocumentAttributes.FileResource")
				.include("ReplenishmentOrderItem")
				.include("ServiceOrderHead")
				.filter("it.OrderId === this.serviceOrderId", { serviceOrderId })
				.filter("it.ServiceOrderTimeId === this.serviceOrderTimeId", { serviceOrderTimeId })
				.filter("it.DispatchId === this.dispatchId || it.DispatchId === null", { dispatchId })
				.filter("it.ServiceOrderMaterialType === this.preplannedType", { preplannedType })
				.orderBy("it.PosNo")
				.orderBy("it.ItemNo");
		let serviceOrderMaterials = await serviceOrderMaterialsQuery().toArray();
		await window.Helper.ServiceOrderMaterial.loadParentAndChildPositions(serviceOrderMaterials.map(x => x.asKoObservable()), serviceOrderMaterialsQuery);
		serviceOrderMaterials = serviceOrderMaterials.map(serviceOrderMaterial => {
			let existingMaterial = (serviceOrderMaterial.ChildServiceOrderMaterials || []).find(x => x.ServiceOrderMaterialType === window.Crm.Service.ServiceOrderMaterialType.Used);
			if (existingMaterial) {
				existingMaterial.ParentServiceOrderMaterial = serviceOrderMaterial;
				window.database.attachOrGet(existingMaterial);
				return existingMaterial;
			}
			let newServiceOrderMaterial = window.Helper.Database.createClone(serviceOrderMaterial);
			newServiceOrderMaterial.ChildServiceOrderMaterials = [];
			newServiceOrderMaterial.DispatchId = this.dispatch().Id();
			newServiceOrderMaterial.FromLocation = serviceOrderMaterial.ToLocation || this.currentUser().ExtensionValues.DefaultLocationNo;
			newServiceOrderMaterial.FromWarehouse = serviceOrderMaterial.ToWarehouse || this.currentUser().ExtensionValues.DefaultStoreNo;
			newServiceOrderMaterial.ToLocation = null;
			newServiceOrderMaterial.ToWarehouse = null;
			newServiceOrderMaterial.Id = window.$data.createGuid().toString().toLowerCase();
			newServiceOrderMaterial.ParentServiceOrderMaterial = serviceOrderMaterial;
			newServiceOrderMaterial.ParentServiceOrderMaterialId = serviceOrderMaterial.Id;
			newServiceOrderMaterial.ParentServiceOrderMaterialVersion = serviceOrderMaterial.Version;
			newServiceOrderMaterial.Quantity = serviceOrderMaterial.Quantity;
			newServiceOrderMaterial.ServiceOrderMaterialType = window.Crm.Service.ServiceOrderMaterialType.Used;
			window.database.add(newServiceOrderMaterial);
			return newServiceOrderMaterial;
		})
		for (const serviceOrderMaterial of serviceOrderMaterials) {
			this.showReasons.push({
				materialId: serviceOrderMaterial.Id,
				showReason: ko.observable(!!serviceOrderMaterial.NoPreviousSerialNoReasonKey)
			});
		}

		let observableServiceOrderMaterials = serviceOrderMaterials.map(x => x.asKoObservable());
		for (const serviceOrderMaterial of observableServiceOrderMaterials) {
			// @ts-ignore
			serviceOrderMaterial.initialQuantity = ko.observable<number>(serviceOrderMaterial.ParentServiceOrderMaterial().Quantity());
			// @ts-ignore
			serviceOrderMaterial.updateReplenishmentOrder = ko.observable<boolean>(this.articleIsWarehouseManaged(serviceOrderMaterial) && window.Crm.Service.Settings.ServiceOrderMaterial.CreateReplenishmentOrderItemsFromServiceOrderMaterial);
			// @ts-ignore
			serviceOrderMaterial.quantityUnit = ko.pureComputed<Crm.Article.Rest.Model.Lookups.CrmArticle_QuantityUnit>(() => {
				return this.lookups.quantityUnits.$array.find(x => x.Key === serviceOrderMaterial.QuantityUnitKey());
			});

			serviceOrderMaterial.Quantity.extend({
				validation: {
					validator: val => window.Helper.ServiceOrderMaterial.quantityValidator(val, serviceOrderMaterial),
					message: () => window.Helper.String.getTranslatedString("RuleViolation.RespectQuantityStep")
						.replace("{0}", window.Helper.String.getTranslatedString("Quantity"))
						.replace("{1}", serviceOrderMaterial.Article()?.QuantityStep() + ""),
					onlyIf: () => serviceOrderMaterial.Article()
				}
			});
		}
		this.serviceOrderMaterials(this.serviceOrderMaterials().concat(observableServiceOrderMaterials));
		this.serviceOrderTimeId(serviceOrderTimeId);
	}

	dispose(): void {
	};

	async save(): Promise<void> {
		this.loading(true);

		if (this.errors().length > 0) {
			const invalidServiceOrderMaterial = this.serviceOrderMaterials().find(x => ko.validation.group(x)().length > 0);
			this.serviceOrderTimeId(invalidServiceOrderMaterial.ServiceOrderTimeId());
			this.loading(false);
			this.errors.showAllMessages();
			this.errors.expandCollapsiblesWithErrors();
			return;
		}
		
		try {
			let updateInReplenishmentOrder: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial[] = [];
			for (const serviceOrderMaterial of this.serviceOrderMaterials()) {
				if (serviceOrderMaterial.Quantity() === 0){
					if (serviceOrderMaterial.innerInstance.entityState === $data.EntityState.Added){
						window.database.detach(serviceOrderMaterial.innerInstance);
					}
					else if (serviceOrderMaterial.innerInstance.entityState === $data.EntityState.Modified){
						window.database.remove(serviceOrderMaterial.innerInstance);
					}
				}
				// @ts-ignore
				if (serviceOrderMaterial.updateReplenishmentOrder()){
					updateInReplenishmentOrder.push(serviceOrderMaterial);
				}
			}
			await this.saveReplenishmentOrderItems(updateInReplenishmentOrder);
			await window.database.saveChanges();
			this.loading(false);
			$(".modal:visible").modal("hide");
		} catch {
			this.loading(false);
			window.swal(window.Helper.String.getTranslatedString("UnknownError"),
				window.Helper.String.getTranslatedString("Error_InternalServerError"),
				"error");
		}
	};

	async saveReplenishmentOrderItems(serviceOrderMaterials: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial[]): Promise<void> {
		for (const serviceOrderMaterial of serviceOrderMaterials) {
			// @ts-ignore
			if (!this.articleIsWarehouseManaged(serviceOrderMaterial)) {
				if (!!serviceOrderMaterial.ReplenishmentOrderItemId()) {
					let replenishmentOrderItem = await window.database.CrmService_ReplenishmentOrderItem
						.find(serviceOrderMaterial.ReplenishmentOrderItemId());
					serviceOrderMaterial.ReplenishmentOrderItemId(null);
					window.database.remove(replenishmentOrderItem);
					continue;
				}
			}
			let replenishmentOrder = await window.Helper.ReplenishmentOrder.getOrCreateCurrentReplenishmentOrder(this.currentUser().Id);
			const createNewReplenishmentOrderItem = !serviceOrderMaterial.ReplenishmentOrderItem() ||
				serviceOrderMaterial.ReplenishmentOrderItem().ReplenishmentOrderId() !== replenishmentOrder.Id();
			if (createNewReplenishmentOrderItem) {
				const newReplenishmentOrderItem = window.database.CrmService_ReplenishmentOrderItem.defaultType.create();
				newReplenishmentOrderItem.ReplenishmentOrderId = replenishmentOrder.Id();
				window.database.add(newReplenishmentOrderItem);
				serviceOrderMaterial.ReplenishmentOrderItem(newReplenishmentOrderItem.asKoObservable());
				serviceOrderMaterial.ReplenishmentOrderItemId(newReplenishmentOrderItem.Id);
			} else {
				window.database.attachOrGet(serviceOrderMaterial.ReplenishmentOrderItem());
				if (serviceOrderMaterial.ReplenishmentOrderItem().ArticleId() == serviceOrderMaterial.ArticleId()
					&& serviceOrderMaterial.ReplenishmentOrderItem().Quantity() == serviceOrderMaterial.Quantity()) {
					continue;
				}
			}
			if (serviceOrderMaterial.Quantity() === 0) {
				serviceOrderMaterial.ReplenishmentOrderItemId(null);
				window.database.remove(serviceOrderMaterial.ReplenishmentOrderItem());
				return;
			}
			serviceOrderMaterial.ReplenishmentOrderItem().ArticleId(serviceOrderMaterial.ArticleId());
			serviceOrderMaterial.ReplenishmentOrderItem().MaterialNo(serviceOrderMaterial.ItemNo());
			serviceOrderMaterial.ReplenishmentOrderItem().Description(serviceOrderMaterial.Description());
			serviceOrderMaterial.ReplenishmentOrderItem()
				.Quantity(serviceOrderMaterial.Quantity());
			serviceOrderMaterial.ReplenishmentOrderItem().QuantityUnitKey(serviceOrderMaterial.QuantityUnitKey());
			serviceOrderMaterial.ReplenishmentOrderItem().QuantityUnitEntryKey(serviceOrderMaterial.QuantityUnitEntryKey());
		}
	};

	locationFilter(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial, query: $data.Queryable<Crm.Article.Rest.Model.CrmArticle_Location>): $data.Queryable<Crm.Article.Rest.Model.CrmArticle_Location> {
		return query.filter('it.Store.StoreNo == this.storeNo', {storeNo: serviceOrderMaterial.FromWarehouse()});
	};

	async onStoreSelect(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial, store: Crm.Article.Rest.Model.CrmArticle_Store): Promise<void> {
		if (store == null || store.StoreNo != serviceOrderMaterial.FromWarehouse()) {
			serviceOrderMaterial.FromLocation(null);
		}
		serviceOrderMaterial.FromWarehouse(store != null ? store.StoreNo : null);
	};

	onLocationSelect(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial, location: Crm.Article.Rest.Model.CrmArticle_Location): void {
			serviceOrderMaterial.FromLocation(location != null ? location.LocationNo : null);
	};
}

namespace("Crm.Service.ViewModels").ServiceOrderMaterialReportScheduledMaterialModalViewModel = ServiceOrderMaterialReportScheduledMaterialModalViewModel;