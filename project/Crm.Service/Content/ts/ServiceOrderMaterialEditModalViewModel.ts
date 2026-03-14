import {namespace} from "@Main/namespace";
import type {ServiceOrderDetailsViewModel} from "./ServiceOrderDetailsViewModel";
import type {DispatchDetailsViewModel} from "./DispatchDetailsViewModel";
import {HelperQuantityUnit} from "../../../Crm.Article/Content/ts/helper/Helper.QuantityUnit";

export class ServiceOrderMaterialEditModalViewModel extends window.Main.ViewModels.ViewModelBase {
	isNewServiceOrderMaterial = ko.observable <boolean>(true)
	actualQuantity = ko.observable<number>(0);
	estimatedQuantity = ko.observable<number>(0);
	invoiceQuantity = ko.observable<number>(0);
	showActiveArticles = ko.observable<boolean>(true);
	showExpiredArticles = ko.observable<boolean>(false);
	showInStockArticles = ko.observable<boolean>(false);
	showAllArticles = ko.pureComputed<boolean>({
		read: () => {
			return this.showActiveArticles() && this.showExpiredArticles() && !this.showInStockArticles();
		},
		write: value => {
			this.showActiveArticles(value);
			this.showExpiredArticles(value);
			this.showInStockArticles(false);
		},
		owner: this
	});
	serviceOrder: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead>;
	dispatch: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch>;
	articleAutocomplete = ko.observable<string>("");
	articleType = ko.observable<string>("Material");
	articleIsWarehouseManaged = ko.observable<boolean>(false);
	canEditArticle = ko.observable<boolean>(false);
	quantityUnitGroupKey = ko.pureComputed<string>(() => {
		return this.serviceOrderMaterial().Article() ? this.serviceOrderMaterial().Article().QuantityUnitEntryKey() : null;
	});
	canEditActualQty = ko.observable<boolean>(false);
	canEditEstimatedQty = ko.observable<boolean>(false);
	canEditInvoiceQty = ko.observable<boolean>(false);
	canEditPrice = ko.pureComputed<boolean>(() => {
		return this.canEditInvoiceQty()
			|| (this.canEditEstimatedQty() && window.AuthorizationManager.isAuthorizedForAction("ServiceOrder", "EditMaterialPrePlannedPrice"))
			|| (this.canEditActualQty() && window.AuthorizationManager.isAuthorizedForAction("ServiceOrder", "EditMaterialUsedPrice"));
	});
	currentUser = ko.observable<Main.Rest.Model.Main_User>(null);
	currentServiceOrderTimeId = ko.observable<string>(null);
	documentAttribute = ko.observable<Crm.Rest.Model.ObservableCrm_DocumentAttribute>(null);
	fileResource = ko.observable<Crm.Rest.Model.ObservableCrm_FileResource>(null);
	initialQuantity = ko.observable<number>(0);
	storeQuantity = ko.observable<number>(0);
	locationQuantity = ko.observable<number>(0);
	storeLocationQuantity = ko.observable<number>(0);
	storageAreaLocationQuantity = ko.observable<number>(0);
	storageAreaQuantity = ko.observable<number>(0);
	selectedStore = ko.observable<string>(null);
	selectedStorageArea = ko.observable<string>(null);
	selectedLocation = ko.observable<string>(null);
	showNonWmWarning = ko.observable<boolean>(false);
	showSerialDropdown = window.ko.observable<boolean>(false);
	showReason = window.ko.observable<boolean>(false);
	showDispatchSelection = ko.observable<boolean>(false);
	lookups: LookupType = {
		quantityUnits: {$tableName: "CrmArticle_QuantityUnit"},
		currencies: {$tableName: "Main_Currency"},
		installationHeadStatuses: {$tableName: "CrmService_InstallationHeadStatus"},
		serviceOrderTypes: {$tableName: "CrmService_ServiceOrderType"},
		servicePriorities: {$tableName: "CrmService_ServicePriority"},
		articleGroups01: {$tableName: "CrmArticle_ArticleGroup01"},
		articleGroups02: {$tableName: "CrmArticle_ArticleGroup02"},
		articleGroups03: {$tableName: "CrmArticle_ArticleGroup03"},
		articleGroups04: {$tableName: "CrmArticle_ArticleGroup04"},
		articleGroups05: {$tableName: "CrmArticle_ArticleGroup05"},
		articleTypes: {$tableName: "CrmArticle_ArticleType"},
		serviceOrderStatuses: {$tableName: "CrmService_ServiceOrderStatus"},
		documentCategories: { $tableName: "Main_DocumentCategory" }
	};
	updateReplenishmentOrder = ko.observable<boolean>(false);
	serviceOrderMaterial = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial>(null);
	errors: KnockoutValidationErrors;
	documentAttributeErrors: KnockoutValidationErrors;

	quantityUnit = ko.pureComputed<Crm.Article.Rest.Model.Lookups.CrmArticle_QuantityUnit>(() => {
		return this.lookups.quantityUnits.$array.find(x => x.Key === this.serviceOrderMaterial().QuantityUnitKey());
	});
	
	stock = ko.pureComputed<number>(() => {
		return this.serviceOrderMaterial()?.Article()?.Stocks()?.find(s => s.StoreKey() == this.selectedStore() && s.StorageAreaKey() == this.selectedStorageArea() && s.LocationKey() == this.selectedLocation())?.Quantity() ?? -1;
	});

	constructor(parentViewModel: ServiceOrderDetailsViewModel | DispatchDetailsViewModel) {
		super();
		this.serviceOrder = parentViewModel.serviceOrder;
		this.dispatch = parentViewModel && parentViewModel.dispatch || ko.observable(null);
	}

	toggleDiscountType(): void {
		if (this.serviceOrderMaterial().DiscountType() === window.Crm.Article.Model.Enums.DiscountType.Percentage) {
			this.serviceOrderMaterial().DiscountType(window.Crm.Article.Model.Enums.DiscountType.Absolute);
		} else {
			this.serviceOrderMaterial().DiscountType(window.Crm.Article.Model.Enums.DiscountType.Percentage);
		}
	};

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		if (params.articleType) {
			this.articleType(params.articleType);
		}
		if (params.currentServiceOrderTimeId) {
			this.currentServiceOrderTimeId(params.currentServiceOrderTimeId);
		}
		let user = await window.Helper.User.getCurrentUser();
		this.currentUser(user);
		let serviceOrderMaterial: Crm.Service.Rest.Model.CrmService_ServiceOrderMaterial;
		if (id) {
			this.isNewServiceOrderMaterial(false)
			serviceOrderMaterial = await window.database.CrmService_ServiceOrderMaterial
				.include("Article")
				.include("Article.QuantityUnitEntry")
				.include("Article.Stocks")
				.include("QuantityUnitEntry")
				.include("DocumentAttributes")
				.include("DocumentAttributes.FileResource")
				.include("ParentServiceOrderMaterial")
				.include("ParentServiceOrderMaterial.ParentServiceOrderMaterial")
				.include("ReplenishmentOrderItem")
				.include("ServiceOrderHead")
				.include("ServiceOrderDispatch")
				.find(id);
			if (!this.serviceOrder().CurrencyKey()) {
				serviceOrderMaterial.DiscountType = window.Crm.Article.Model.Enums.DiscountType.Percentage;
			}
			window.database.attachOrGet(serviceOrderMaterial);
			this.initialQuantity(serviceOrderMaterial.Quantity);
			if (serviceOrderMaterial.Article) {
				this.articleType(serviceOrderMaterial.Article.ArticleTypeKey);
				this.articleIsWarehouseManaged(serviceOrderMaterial.Article.IsWarehouseManaged);
			}
			if (this.articleType() && this.articleType() === "Material" && !serviceOrderMaterial.Quantity) {
				serviceOrderMaterial.FromLocation = this.currentUser().ExtensionValues.DefaultLocationNo;
				serviceOrderMaterial.FromWarehouse = this.currentUser().ExtensionValues.DefaultStoreNo;
			}
		} else {
			this.isNewServiceOrderMaterial(true)
			serviceOrderMaterial = window.database.CrmService_ServiceOrderMaterial.defaultType.create();
			serviceOrderMaterial.DiscountType = Crm.Article.Model.Enums.DiscountType.Absolute;
			if (params.dispatchId) {
				serviceOrderMaterial.DispatchId = params.dispatchId;
				serviceOrderMaterial.ServiceOrderMaterialType = window.Crm.Service.ServiceOrderMaterialType.Used;
			} else {
				serviceOrderMaterial.ServiceOrderMaterialType = window.Crm.Service.ServiceOrderMaterialType.Preplanned;
			}
			if (this.articleType() && this.articleType() === "Material") {
				serviceOrderMaterial.FromLocation = this.currentUser().ExtensionValues.DefaultLocationNo;
				serviceOrderMaterial.FromStorageArea = this.currentUser().ExtensionValues.DefaultStorageAreaNo;
				serviceOrderMaterial.FromWarehouse = this.currentUser().ExtensionValues.DefaultStoreNo;
			}
			serviceOrderMaterial.OrderId = params.serviceOrderId;
			serviceOrderMaterial.Quantity = 1;
			serviceOrderMaterial.ServiceOrderTimeId = params.serviceOrderTimeId || params.currentServiceOrderTimeId || (this.dispatch() ? this.dispatch().CurrentServiceOrderTimeId() : null) || null;
			if (!this.serviceOrder().CurrencyKey()) {
				serviceOrderMaterial.DiscountType = window.Crm.Article.Model.Enums.DiscountType.Percentage;
			}
			window.database.add(serviceOrderMaterial);
		}

		if (params.dispatchId && serviceOrderMaterial.ServiceOrderMaterialType === window.Crm.Service.ServiceOrderMaterialType.Preplanned) {
			window.database.detach(serviceOrderMaterial);
			let newServiceOrderMaterial = window.Helper.Database.createClone(serviceOrderMaterial);
			newServiceOrderMaterial.DispatchId = params.dispatchId || null;
			if (this.articleType() && this.articleType() === "Material") {
				newServiceOrderMaterial.FromLocation = this.currentUser().ExtensionValues.DefaultLocationNo;
				newServiceOrderMaterial.FromWarehouse = this.currentUser().ExtensionValues.DefaultStoreNo;
			}
			newServiceOrderMaterial.Id = window.$data.createGuid().toString().toLowerCase();
			newServiceOrderMaterial.ParentServiceOrderMaterial = serviceOrderMaterial;
			newServiceOrderMaterial.ParentServiceOrderMaterialId = serviceOrderMaterial.Id;
			newServiceOrderMaterial.ParentServiceOrderMaterialVersion = serviceOrderMaterial.Version;
			newServiceOrderMaterial.Quantity = 0;
			newServiceOrderMaterial.ServiceOrderMaterialType = window.Crm.Service.ServiceOrderMaterialType.Used;
			window.database.add(newServiceOrderMaterial);
			serviceOrderMaterial = newServiceOrderMaterial;
		}

		if (serviceOrderMaterial.DocumentAttributes.length === 0) {
			const newFileResource = window.database.Crm_FileResource.defaultType.create();
			window.database.add(newFileResource);
			this.fileResource(newFileResource.asKoObservable());
			const newDocumentAttribute = window.database.Crm_DocumentAttribute.defaultType.create();
			newDocumentAttribute.ContactName = window.Helper.ServiceOrder.getDisplayName(this.serviceOrder());
			newDocumentAttribute.ContactType = "ServiceOrder";
			newDocumentAttribute.ExtensionValues.DispatchId = serviceOrderMaterial.DispatchId;
			newDocumentAttribute.FileResource = newFileResource;
			newDocumentAttribute.FileResourceKey = this.fileResource().Id();
			newDocumentAttribute.ReferenceKey = serviceOrderMaterial.OrderId;
			newDocumentAttribute.ExtensionValues.ServiceOrderMaterialId = serviceOrderMaterial.Id;
			window.database.add(newDocumentAttribute);
			this.documentAttribute(newDocumentAttribute.asKoObservable());
		} else {
			const documentAttribute = serviceOrderMaterial.DocumentAttributes[0];
			this.documentAttribute(documentAttribute.asKoObservable());
			window.database.attachOrGet(documentAttribute);
			this.fileResource(documentAttribute.FileResource.asKoObservable());
			window.database.attachOrGet(documentAttribute.FileResource);
		}
		if (serviceOrderMaterial.NoPreviousSerialNoReasonKey) {
			this.showReason(true)
		}
		if (serviceOrderMaterial.SerialId) {
			this.showSerialDropdown(true)
		}
		this.fileResource().Filename.subscribe(this.documentAttribute().Description);
		this.fileResource().Filename.subscribe(this.documentAttribute().FileName);
		this.fileResource().Id.subscribe(this.documentAttribute().FileResourceKey);
		this.fileResource().Length.subscribe(this.documentAttribute().Length);
		this.serviceOrderMaterial(serviceOrderMaterial.asKoObservable());
		this.serviceOrderMaterial().Quantity.extend({
			validation: {
				validator: val => {
					const stock = this.stock(); 
					const diff = val - this.initialQuantity();
					return diff <= 0 || stock === -1 || diff <= stock;
				},
				message: window.Helper.String.getTranslatedString("QuantityExceedsStock"),
				onlyIf: () => this.serviceOrderMaterial().ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Used
			}
		});
		this.documentAttribute().DocumentCategoryKey.extend({
			required: {
				params: true,
				message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("Category")),
				onlyIf: function () {
					return this.fileResource().Length() > 0;
				}
			}
		});
		this.updateReplenishmentOrder(!!this.serviceOrderMaterial().ReplenishmentOrderItemId() && this.articleIsWarehouseManaged());
		this.canEditEstimatedQty(this.serviceOrderMaterial().ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Preplanned);
		this.canEditActualQty(this.serviceOrderMaterial().ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Used);
		this.canEditInvoiceQty(this.serviceOrderMaterial().ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Invoice);
		if (this.serviceOrderMaterial().ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Preplanned) {
			this.actualQuantity = ko.observable(0);
			this.estimatedQuantity = this.serviceOrderMaterial().Quantity;
			this.invoiceQuantity = ko.observable(0);
		} else if (this.serviceOrderMaterial().ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Used) {
			this.actualQuantity = this.serviceOrderMaterial().Quantity;
			this.estimatedQuantity = this.serviceOrderMaterial().ParentServiceOrderMaterial() ? this.serviceOrderMaterial().ParentServiceOrderMaterial().Quantity : ko.observable(0);
			this.invoiceQuantity = ko.observable(0);
		} else if (this.serviceOrderMaterial().ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Invoice) {
			this.actualQuantity = this.serviceOrderMaterial().ParentServiceOrderMaterial() && this.serviceOrderMaterial().ParentServiceOrderMaterial().ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Used ? this.serviceOrderMaterial().ParentServiceOrderMaterial().Quantity : ko.observable(0);
			this.estimatedQuantity = this.serviceOrderMaterial().ParentServiceOrderMaterial() && this.serviceOrderMaterial().ParentServiceOrderMaterial().ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Preplanned ? this.serviceOrderMaterial().ParentServiceOrderMaterial().Quantity : ko.observable(0);
			this.invoiceQuantity = this.serviceOrderMaterial().Quantity;
		} else {
			throw new Error("Unknown ServiceOrderMaterialType: " + this.serviceOrderMaterial().ServiceOrderMaterialType());
		}
		if (this.serviceOrderMaterial().FromWarehouse()) {
			const store = await window.database.CrmArticle_Store.filter("it.StoreNo == this.storeNo", {storeNo: this.serviceOrderMaterial().FromWarehouse()}).first();
			this.selectedStore(store.Id);
		}
		if (this.serviceOrderMaterial().FromStorageArea()) {
			const storageArea = await window.database.CrmArticle_StorageArea.filter("it.StorageAreaNo == this.storageAreaNo", {storageAreaNo: this.serviceOrderMaterial().FromStorageArea()}).first();
			this.selectedStorageArea(storageArea.Id);
		}
		if (this.serviceOrderMaterial().FromLocation()) {
			const location = await window.database.CrmArticle_Location.filter("it.LocationNo == this.locationNo", {locationNo: this.serviceOrderMaterial().FromLocation()}).first();
			this.selectedLocation(location.Id);
		}
		this.errors = ko.validation.group(this.serviceOrderMaterial, {deep: this.articleType() === "Cost" ? false : true});
		this.documentAttributeErrors = ko.validation.group(this.documentAttribute(), { deep: true });
		const currentServiceOrderStatus = this.lookups.serviceOrderStatuses[this.serviceOrder().StatusKey()];
		const serviceOrderAlreadyScheduled = currentServiceOrderStatus.SortOrder >= 3;
		this.showDispatchSelection(!params.dispatchId && serviceOrderAlreadyScheduled); //shows when opened from service order
		this.canEditArticle(this.serviceOrderMaterial().ParentServiceOrderMaterialId() === null);
	};

	dispose(): void {
		window.database.detach(this.documentAttribute().innerInstance);
		window.database.detach(this.fileResource().innerInstance);
		window.database.detach(this.serviceOrderMaterial().innerInstance);
	};

	async save(): Promise<void> {
		this.loading(true);

		if (!this.fileResource().Content()) {
			if (this.documentAttribute().innerInstance.entityState === $data.EntityState.Added) {
				window.database.detach(this.documentAttribute().innerInstance);
			} else if (this.documentAttribute().innerInstance.entityState !== $data.EntityState.Detached && this.documentAttribute().innerInstance.entityState !== $data.EntityState.Unchanged) {
				window.database.remove(this.documentAttribute().innerInstance);
			}
			if (this.fileResource().innerInstance.entityState === $data.EntityState.Added) {
				window.database.detach(this.fileResource().innerInstance);
			} else if (this.fileResource().innerInstance.entityState !== $data.EntityState.Detached && this.fileResource().innerInstance.entityState !== $data.EntityState.Unchanged) {
				window.database.remove(this.fileResource().innerInstance);
			}
			this.documentAttributeErrors = ko.validation.group([], { deep: true });
		}

		if (this.errors().length > 0|| this.documentAttributeErrors().length > 0) {
			this.loading(false);
			this.errors.showAllMessages();
			this.documentAttributeErrors.showAllMessages();
			return;
		}
		if (window.Crm.Service.Settings.PosNoGenerationMethod == "MixedMaterialAndTimes") {
			try {
				await window.Helper.ServiceOrder.updatePosNo(this.serviceOrderMaterial());
				await this.saveReplenishmentOrderItem();
				this.serviceOrderMaterial().ParentServiceOrderMaterial(null);
				await window.database.saveChanges();
				this.loading(false);
				if (this.isNewServiceOrderMaterial()) {
					this.showSnackbar(window.Helper.String.getTranslatedString("Added"));
				} else {
					this.showSnackbar(window.Helper.String.getTranslatedString("Modified"));
				}
				$(".modal:visible").modal("hide");
			} catch {
				this.loading(false);
				window.swal(window.Helper.String.getTranslatedString("UnknownError"),
					window.Helper.String.getTranslatedString("Error_InternalServerError"),
					"error");
			}
		} else {
			try {
				await window.Helper.ServiceOrder.updateMaterialPosNo(this.serviceOrderMaterial());
				await this.saveReplenishmentOrderItem();
				await window.database.saveChanges();
				this.loading(false);
				this.showSnackbar(window.Helper.String.getTranslatedString("Added"));
				$(".modal:visible").modal("hide");
			} catch {
				this.loading(false);
				window.swal(window.Helper.String.getTranslatedString("UnknownError"),
					window.Helper.String.getTranslatedString("Error_InternalServerError"),
					"error");
			}
		}
	};

	async saveReplenishmentOrderItem(): Promise<void> {
		const serviceOrderMaterial = this.serviceOrderMaterial();
		if (!this.articleIsWarehouseManaged()) {
			if (!!serviceOrderMaterial.ReplenishmentOrderItemId()) {
				let replenishmentOrderItem = await window.database.CrmService_ReplenishmentOrderItem
					.find(serviceOrderMaterial.ReplenishmentOrderItemId());
				serviceOrderMaterial.ReplenishmentOrderItemId(null);
				window.database.remove(replenishmentOrderItem);
				return;
			}
		}
		if (!this.updateReplenishmentOrder()) {
			return;
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
				return;
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
	};

	showFileSelection(): boolean {
		return this.articleType() === "Cost";
	};

	getArticleSelect2Filter(query: $data.Queryable<Crm.Article.Rest.Model.CrmArticle_Article>, term: string): $data.Queryable<Crm.Article.Rest.Model.CrmArticle_Article> {
		if (this.showInStockArticles()) {
			const storeId = this.selectedStore() ? `'${this.selectedStore()}'` : null;
			const locationId = this.selectedLocation() ? `'${this.selectedLocation()}'` : null;
			const storageAreaId = this.selectedStorageArea() ? `'${this.selectedStorageArea()}'` : null;
			query = query.include2(`Stocks.filter(it2 => it2.StoreKey == ${storeId} && (${storageAreaId} === null || it2.StorageAreaKey == ${storageAreaId}) && (${locationId} === null || it2.LocationKey == ${locationId}))`);
			query = query.filter("filterArticleByAvailability", {
				store: this.selectedStore(),
				storageArea: this.selectedStorageArea(),
				location: this.selectedLocation()
			});
		}
		query = query.filter("it.ArticleTypeKey === this.articleType && it.ExtensionValues.IsHidden === false",
			{articleType: this.articleType()});
		if (this.dispatch() && ["SignedByCustomer", "ClosedNotComplete", "ClosedComplete"].indexOf(this.dispatch().StatusKey()) !== -1) {
			query = query.filter(function (it) {
				return it.ExtensionValues.CanBeAddedAfterCustomerSignature;
			});
		}
		return window.Helper.Article.getArticleAutocompleteFilter(query, term, this.currentUser().DefaultLanguageKey);
	};

	getServiceOrderTimeAutocompleteDisplay(serviceOrderTime: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime): string {
		return window.Helper.ServiceOrderTime.getAutocompleteDisplay(serviceOrderTime,
			this.currentServiceOrderTimeId());
	};

	getServiceOrderTimeAutocompleteFilter(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTime>, term: string): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTime> {
		query = query.filter(function (it) {
				return it.OrderId === this.orderId;
			},
			{orderId: this.serviceOrderMaterial().OrderId()});
		if (term) {
			query = window.Helper.String.contains(query, term, ["Description.toLowerCase()", "ItemNo.toLowerCase()", "PosNo.toLowerCase()"]);
		}
		return query;
	};

	onArticleSelect(article: Crm.Article.Rest.Model.CrmArticle_Article): void {
		let articleHasChanged = false;
		if (article) {
			articleHasChanged = (!this.serviceOrderMaterial().Article() || JSON.stringify(this.serviceOrderMaterial().Article().innerInstance) != JSON.stringify(article)) && (!this.serviceOrderMaterial().Article() || this.serviceOrderMaterial().Article().Id() !== article.Id);
			if (articleHasChanged) {
				this.serviceOrderMaterial().Article(article.asKoObservable());
				this.serviceOrderMaterial().ArticleId(article.Id);
				this.serviceOrderMaterial().ArticleTypeKey(article.ArticleTypeKey);
				this.serviceOrderMaterial().IsSerial(article.IsSerial);
				this.serviceOrderMaterial().SerialId(null);
				this.serviceOrderMaterial().Description(window.Helper.Article.getArticleDescription(article));
				this.serviceOrderMaterial().ItemNo(article.ItemNo);
				if (article.Price !== null) {
					this.serviceOrderMaterial().Price(article.Price);
				}
				this.serviceOrderMaterial().QuantityUnitEntryKey(article.QuantityUnitEntryKey);
				if (article.QuantityUnitEntry)
					this.serviceOrderMaterial().QuantityUnitKey(article.QuantityUnitEntry.QuantityUnitKey)
				else
					this.serviceOrderMaterial().QuantityUnitKey(article.QuantityUnitKey)
				this.articleIsWarehouseManaged(article.IsWarehouseManaged);
				this.updateReplenishmentOrder(!!this.serviceOrderMaterial().ReplenishmentOrderItemId() && this.articleIsWarehouseManaged());
				this.serviceOrderMaterial().IsBatch(article.IsBatch);
				if (!article.IsBatch) {
					this.serviceOrderMaterial().BatchNo(null);
				}
				if (!article.IsSerial) {
					this.serviceOrderMaterial().SerialNo(null);
				}
				this.showSerialDropdown(false)
			}
		} else {
			this.serviceOrderMaterial().Article(null);
			this.serviceOrderMaterial().ArticleId(null);
			this.serviceOrderMaterial().ArticleTypeKey(null);
			this.serviceOrderMaterial().IsSerial(false);
			this.serviceOrderMaterial().Description(null);
			this.serviceOrderMaterial().ItemNo(null);
			this.serviceOrderMaterial().Price(null);
			this.serviceOrderMaterial().QuantityUnitEntryKey(null);
			this.serviceOrderMaterial().QuantityUnitKey(null);
			this.articleIsWarehouseManaged(false);
			this.serviceOrderMaterial().IsBatch(false);
			this.updateReplenishmentOrder(false);
			this.serviceOrderMaterial().BatchNo(null);
			this.serviceOrderMaterial().SerialNo(null);
			this.serviceOrderMaterial().SerialId(null);
		}
		if (!!this.serviceOrderMaterial().ReplenishmentOrderItemId() && !this.articleIsWarehouseManaged()) {
			this.showNonWmWarning(true);
		} else {
			this.showNonWmWarning(false);
		}
	};

	onQuantityUnitEntrySelect(quantityUnitEntry: Crm.Article.Rest.Model.CrmArticle_QuantityUnitEntry) {
		if (quantityUnitEntry && this.serviceOrderMaterial().QuantityUnitEntry()) {
			const conversionRate = HelperQuantityUnit.getConversionRate(this.serviceOrderMaterial().QuantityUnitEntry, quantityUnitEntry);
			this.serviceOrderMaterial().Price(this.serviceOrderMaterial().Price() !== null ? (this.serviceOrderMaterial().Price() / conversionRate) : null);
			this.serviceOrderMaterial().Quantity(this.serviceOrderMaterial().Quantity() * conversionRate);
		}
		if (quantityUnitEntry == null) {
			if (this.serviceOrderMaterial().ArticleId == null)
				this.serviceOrderMaterial().QuantityUnitEntry(null)
			else
				this.serviceOrderMaterial().QuantityUnitEntryKey(this.serviceOrderMaterial().QuantityUnitEntry().Id())
		} else if (this.serviceOrderMaterial().QuantityUnitEntry() == null || this.serviceOrderMaterial().QuantityUnitEntry().Id() !== quantityUnitEntry.Id) {
			this.serviceOrderMaterial().QuantityUnitEntry(quantityUnitEntry.asKoObservable())
			this.serviceOrderMaterial().QuantityUnitKey(quantityUnitEntry.QuantityUnitKey)
		}
	}

	locationFilter(query: $data.Queryable<Crm.Article.Rest.Model.CrmArticle_Location>, term: string): $data.Queryable<Crm.Article.Rest.Model.CrmArticle_Location> {
		query = query.filter('it.StoreId == this.storeId', {storeId: this.selectedStore()});
		if (this.selectedStorageArea() != null) {
			query = query.filter('it.StorageAreaId == this.storageAreaId', {storageAreaId: this.selectedStorageArea()});
		}
		if (term) {
			query = window.Helper.String.contains(query, term, ["LocationNo"]);
		}
		return query;
	};

	storageAreaFilter(query: $data.Queryable<Crm.Article.Rest.Model.CrmArticle_StorageArea>, term: string): $data.Queryable<Crm.Article.Rest.Model.CrmArticle_StorageArea> {
		query = query.filter('it.StoreId == this.storeId', {storeId: this.selectedStore()});
		if (term) {
			query = window.Helper.String.contains(query, term, ["StorageAreaNo"]);
		}
		return query;
	};

	async onStoreSelect(store: Crm.Article.Rest.Model.CrmArticle_Store): Promise<void> {
		let locationCount: number = 0;
		let storageAreaCount: number = 0;
		if (store) {
			this.loading(true);
			locationCount = await window.database.CrmArticle_Location.filter("StoreId", "===", store.Id).count();
			storageAreaCount = await window.database.CrmArticle_StorageArea.filter("StoreId", "===", store.Id).count();
		}
		this.storeLocationQuantity(locationCount);
		if (store == null) {
			this.locationQuantity(0)
			this.storageAreaQuantity(0)
		} else if (store.StoreNo != this.serviceOrderMaterial().FromWarehouse() || this.serviceOrderMaterial().FromStorageArea() == null)
			this.locationQuantity(this.storeLocationQuantity())
		this.storageAreaQuantity(storageAreaCount);
		if (store != null && store.StoreNo != this.serviceOrderMaterial().FromWarehouse()) {
			this.serviceOrderMaterial().FromLocation(null);
			this.selectedLocation(null);
			if (store.StorageAreas.length > 1) {
				this.serviceOrderMaterial().FromStorageArea(null);
				this.selectedStorageArea(null);
			} else if (store.StorageAreas.length == 0) {
				const storeLocations = await window.database.CrmArticle_Location
					.filter("it.StoreId === this.storeId", { storeId: store.Id })
					.toArray();
				if (storeLocations.length == 1) {
					this.serviceOrderMaterial().FromLocation(storeLocations[0].LocationNo);
					this.selectedLocation(storeLocations[0].Id);
					this.serviceOrderMaterial().FromStorageArea(storeLocations[0].StorageArea?.StorageAreaNo);
					this.selectedStorageArea(storeLocations[0].StorageAreaId);
				}
			} else if (store.StorageAreas.length == 1) {
				this.serviceOrderMaterial().FromStorageArea(store.StorageAreas[0].StorageAreaNo);
				this.selectedStorageArea(store.StorageAreas[0].Id);
				if (store.StorageAreas[0].Locations.length == 1) {
					this.serviceOrderMaterial().FromLocation(store.StorageAreas[0].Locations[0].LocationNo);
					this.selectedLocation(store.StorageAreas[0].Locations[0].Id);
				}
			}
		}
		if (store == null) {
			this.serviceOrderMaterial().FromStorageArea(null);
			this.selectedStorageArea(null);
			this.serviceOrderMaterial().FromLocation(null);
			this.selectedLocation(null);

			this.showInStockArticles(false);
			if (this.serviceOrderMaterial().FromWarehouse() != null && this.serviceOrderMaterial().SerialId()) {
				this.serviceOrderMaterial().SerialId(null);
				this.serviceOrderMaterial().SerialNo(null);
			}
		} else {
			if (this.serviceOrderMaterial().FromWarehouse() != store.StoreNo && this.serviceOrderMaterial().SerialId()) {
				this.serviceOrderMaterial().SerialId(null);
				this.serviceOrderMaterial().SerialNo(null);
			}
		}
		this.serviceOrderMaterial().FromWarehouse(store != null ? store.StoreNo : null);
		this.loading(false);
	};

	async onStorageAreaSelect(storageArea: Crm.Article.Rest.Model.CrmArticle_StorageArea): Promise<void> {
		this.loading(true);
		let locationCount: number = 0;
		if (storageArea) {
			this.loading(true);
			locationCount = await window.database.CrmArticle_Location.filter("StorageAreaId", "===", storageArea.Id).count();
		}
		this.storageAreaLocationQuantity(locationCount);
		if (storageArea == null)
			this.locationQuantity(this.storeLocationQuantity())
		else
			this.locationQuantity(this.storageAreaLocationQuantity())
		if (storageArea != null && storageArea.StorageAreaNo != this.serviceOrderMaterial().FromStorageArea()) {
			if (storageArea.Locations.length == 1) {
				this.serviceOrderMaterial().FromLocation(storageArea.Locations[0].LocationNo);
				this.selectedLocation(storageArea.Locations[0].Id);
			} else {
				this.serviceOrderMaterial().FromLocation(null);
				this.selectedLocation(null);
			}
		}
		if (storageArea == null) {
			this.serviceOrderMaterial().FromLocation(null);
			this.selectedLocation(null);

			if (this.serviceOrderMaterial().FromStorageArea() != null && this.serviceOrderMaterial().SerialId()) {
				this.serviceOrderMaterial().SerialId(null);
				this.serviceOrderMaterial().SerialNo(null);
			}
		} else {
			if (this.serviceOrderMaterial().FromStorageArea() != storageArea.StorageAreaNo && this.serviceOrderMaterial().SerialId()) {
				this.serviceOrderMaterial().SerialId(null);
				this.serviceOrderMaterial().SerialNo(null);
			}
		}
		this.serviceOrderMaterial().FromStorageArea(storageArea != null ? storageArea.StorageAreaNo : null);
		this.loading(false);
	};

	onLocationSelect(location: Crm.Article.Rest.Model.CrmArticle_Location): void {
		if (location != null) {
			if (this.serviceOrderMaterial().FromLocation() != location.LocationNo && this.serviceOrderMaterial().SerialId()) {
				this.serviceOrderMaterial().SerialId(null);
				this.serviceOrderMaterial().SerialNo(null);
			}
			this.serviceOrderMaterial().FromLocation(location.LocationNo);
			this.serviceOrderMaterial().FromStorageArea(location.StorageArea ? location.StorageArea.StorageAreaNo : null);
			this.selectedStorageArea(location.StorageAreaId);
		} else {
			if (this.serviceOrderMaterial().FromLocation() != null && this.serviceOrderMaterial().SerialId()) {
				this.serviceOrderMaterial().SerialId(null);
				this.serviceOrderMaterial().SerialNo(null);
			}
		}
	};

	onSerialSelect(serial: Crm.Article.Rest.Model.CrmArticle_Serial): void {
		if (serial != null) {
			this.serviceOrderMaterial().SerialNo(serial.SerialNo);
			this.selectedStore(serial.Store != null ? serial.Store.Id : null);
			this.serviceOrderMaterial().FromWarehouse(serial.Store != null ? serial.Store.StoreNo : null);
			this.selectedStorageArea(serial.StorageArea != null ? serial.StorageArea.Id : null);
			this.serviceOrderMaterial().FromStorageArea(serial.StorageArea != null ? serial.StorageArea.StorageAreaNo : null);
			this.selectedLocation(serial.Location != null ? serial.Location.Id : null);
			this.serviceOrderMaterial().FromLocation(serial.Location != null ? serial.Location.LocationNo : null);
		} else
			this.serviceOrderMaterial().SerialNo(null);
	};

	toggleShowReasonForSerial(): void {
		this.showReason(!this.showReason());
	};

	toggleShowSerialDropdown(): void {
		this.showSerialDropdown(!this.showSerialDropdown());
	};

	isJobEditable(): boolean {
		return (this.serviceOrderMaterial().ServiceOrderMaterialType() !== window.Crm.Service.ServiceOrderMaterialType.Preplanned || window.AuthorizationManager.currentUserHasPermission("ServiceOrder::EditMaterialPrePlannedJob")) &&
			(this.dispatch() == null || !Crm.Service.Settings.Dispatch.RestrictEditingToActiveJob)
	}
}

namespace("Crm.Service.ViewModels").ServiceOrderMaterialEditModalViewModel = ServiceOrderMaterialEditModalViewModel;