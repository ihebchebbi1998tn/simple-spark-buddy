import {namespace} from "@Main/namespace";
import type {ServiceOrderDetailsViewModel} from "./ServiceOrderDetailsViewModel";
import type {ServiceOrderTemplateDetailsViewModel} from "./ServiceOrderTemplateDetailsViewModel";
import type {DispatchDetailsViewModel} from "./DispatchDetailsViewModel";
import {HelperServiceOrder} from "@Crm.Service/helper/Helper.ServiceOrder";

export class ServiceOrderDetailsMaterialsTabViewModelBase<T extends ServiceOrderDetailsViewModel | ServiceOrderTemplateDetailsViewModel | DispatchDetailsViewModel> extends window.Main.ViewModels.GenericListViewModel<Crm.Service.Rest.Model.CrmService_ServiceOrderMaterial, Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial> {
	accumulatedTotalPricePreplanned = ko.pureComputed<number>(() => {
		let positions = window._.uniqBy(this.items().map(x => window.Helper.ServiceOrderMaterial.getEstimatedPosition(x)).filter(Boolean), x => x.Id());
		return positions.reduce((partialSum, item: any) => partialSum + window.Helper.ServiceOrderMaterial.getTotalPrice(item), 0);
	});
	accumulatedTotalPriceUsed = ko.pureComputed<number>(() => {
		let positions = window._.uniqBy(this.items().map(x => window.Helper.ServiceOrderMaterial.getActualPositions(x)).flat(), x => x.Id());
		return positions.reduce((partialSum, item: any) => partialSum + window.Helper.ServiceOrderMaterial.getTotalPrice(item), 0);
	});
	accumulatedTotalPriceInvoice = ko.pureComputed<number>(() => {
		let positions = window._.uniqBy(this.items().map(x => window.Helper.ServiceOrderMaterial.getInvoicePositions(x)).flat(), x => x.Id());
		return positions.reduce((partialSum, item: any) => partialSum + window.Helper.ServiceOrderMaterial.getTotalPrice(item), 0);
	});
	expandedPositions = ko.observableArray<string>();
	getAllPositions = (material: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial[] => {
		return this.items()
			.filter(x => this.getRootPosition(x).Id() === this.getRootPosition(material).Id())
			.sort((x, y) => x.CreateDate().getTime() - y.CreateDate().getTime());
	};
	getRootPosition = (material: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial => {
		if (material.ParentServiceOrderMaterial()) {
			return this.getRootPosition(material.ParentServiceOrderMaterial());
		}
		return material;
	}
	serviceOrder: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead>;
	hideActions: boolean = false;
	isEditable = ko.pureComputed<boolean>(() => {
		return !!ko.unwrap(this.serviceOrder) ? (this.serviceOrder().IsTemplate() ? (this.parentViewModel as ServiceOrderTemplateDetailsViewModel).serviceOrderTemplateIsEditable() : this.parentViewModel.serviceOrderIsEditable()) : true;
	});
	parentViewModel: T;
	showServiceOrderMaterialType: KnockoutReadonlyObservable<boolean>;
	isOrderInPostProcessing = window.ko.pureComputed(() => {
		return Helper.ServiceOrder.isInStatusGroupSync(this.serviceOrder().StatusKey(), this.lookups.serviceOrderStatuses as { [key: string]: Crm.Service.Rest.Model.Lookups.CrmService_ServiceOrderStatus }, ["PostProcessing"]);
	});
	canEditEstimatedQuantities = ko.pureComputed<boolean>(() => {
		return HelperServiceOrder.canEditEstimatedQuantitiesSync(this.serviceOrder().StatusKey(), this.lookups.serviceOrderStatuses as {
			[key: string]: Crm.Service.Rest.Model.Lookups.CrmService_ServiceOrderStatus
		});
	});
	materialsCanBeAdded = ko.pureComputed<boolean>(() => {
		return this.parentViewModel.serviceOrderIsEditable() && this.isEditable() && (this.canEditEstimatedQuantities() || (this.isOrderInPostProcessing && window.AuthorizationManager.currentUserHasPermission("ServiceOrder::EditTimePostingWhenDispatchClosed")));
	});
	invoicePositionsCanBeAdded = ko.pureComputed<boolean>(() => {
		return this.isOrderInPostProcessing() && this.materialsCanBeAdded();
	});

	constructor(parentViewModel: T) {
		super("CrmService_ServiceOrderMaterial",
			["ServiceOrderTime.PosNo", "PosNo", "ItemNo"],
			["ASC", "ASC", "ASC"],
			["ServiceOrderTime", "ServiceOrderTime.Installation", "DocumentAttributes", "DocumentAttributes.FileResource", "Article", "ServiceOrderDispatch", "ServiceOrderDispatch.DispatchedUser"]);
		this.parentViewModel = parentViewModel;
		this.lookups = parentViewModel.lookups || {};
		this.lookups.commissioningStatuses = this.lookups.commissioningStatuses || {$tableName: "CrmService_CommissioningStatus"};
		this.lookups.currencies = this.lookups.currencies || {$tableName: "Main_Currency"};
		this.lookups.noPreviousSerialNoReasons = this.lookups.noPreviousSerialNoReason || {$tableName: "CrmService_NoPreviousSerialNoReason"};
		this.lookups.quantityUnits = this.lookups.quantityUnits || {$tableName: "CrmArticle_QuantityUnit"};
		this.serviceOrder = parentViewModel.serviceOrder;
		this.infiniteScroll(true);
		this.bookmarks.push({
			Category: window.Helper.String.getTranslatedString("Show"),
			Name: window.Helper.String.getTranslatedString("All"),
			Key: "All",
			Expression: (query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderMaterial>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderMaterial> => {
				return query;
			}
		});
		this.bookmarks.push({
			Category: window.Helper.String.getTranslatedString("Show"),
			Name: window.Helper.String.getTranslatedString("ServiceOrderMaterialTypePreplanned"),
			Key: "Preplanned",
			Expression: (query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderMaterial>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderMaterial> => {
				return query.filter(it => it.ServiceOrderMaterialType === window.Crm.Service.ServiceOrderMaterialType.Preplanned);
			}
		});
		this.bookmarks.push({
			Category: window.Helper.String.getTranslatedString("Show"),
			Name: window.Helper.String.getTranslatedString("ServiceOrderMaterialTypeUsed"),
			Key: "Used",
			Expression: (query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderMaterial>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderMaterial> => {
				return query.filter(it => it.ServiceOrderMaterialType === window.Crm.Service.ServiceOrderMaterialType.Used);
			}
		});
		this.bookmarks.push({
			Category: window.Helper.String.getTranslatedString("Show"),
			Name: window.Helper.String.getTranslatedString("ServiceOrderMaterialTypeInvoice"),
			Key: "Invoice",
			Expression: (query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderMaterial>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderMaterial> => {
				return query.filter(it => it.ServiceOrderMaterialType === window.Crm.Service.ServiceOrderMaterialType.Invoice);
			}
		});
		this.bookmark(this.bookmarks()[0]);
		this.pageSize(20);
		this.showServiceOrderMaterialType = ko.pureComputed(() => {
			return this.bookmark() !== null && this.bookmark().Key === "All";
		});
		window.Helper.Database.registerEventHandlers(this, {
			"CrmService_ServiceOrderHead": {
				"afterUpdate": async (sender, serviceOrderHead) => {
					this.loading(true);
					this.items([]);
					this.page(1);
					await this.init();
					this.loading(false);
				}
			}
		});
	}

	async init(): Promise<void> {
		await super.init();
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
	};

	async deleteServiceOrderMaterial(serviceOrderMaterial) {
		let confirm = await window.Helper.Confirm.confirmDeleteAsync();
		if (!confirm) {
			return;
		}
		this.loading(true);
		if (!!serviceOrderMaterial.ReplenishmentOrderItemId()) {
			let replenishmentOrderItem = await window.database.CrmService_ReplenishmentOrderItem
				.find(serviceOrderMaterial.ReplenishmentOrderItemId());
			window.database.remove(replenishmentOrderItem);
		}
		var entity = window.Helper.Database.getDatabaseEntity(serviceOrderMaterial);
		entity.DocumentAttributes.forEach(function (documentAttribute) {
			window.database.remove(documentAttribute);
			window.database.remove(documentAttribute.FileResource);
			documentAttribute.FileResource = null;
		});
		window.database.remove(entity);
		await window.database.saveChanges();
	}

	applyFilter(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderMaterial>, filterValue: {
		Value: string
	}, filterName: string): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderMaterial> {
		if (filterName === "ItemDescription") {
			return query.filter("filterByArticleDescription",
				{filter: filterValue.Value, language: this.currentUser().DefaultLanguageKey});
		}
		return super.applyFilter(query, filterValue, filterName);
	};

	applyFilters(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderMaterial>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderMaterial> {
		query = super.applyFilters(query);
		query = query
			.filter(function (it) {
					return it.OrderId === this.orderId;
				},
				{orderId: this.serviceOrder().Id()});
		return query;
	};

	getActualQuantity(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): number {
		return window.Helper.ServiceOrderMaterial.getActualQuantity(serviceOrderMaterial);
	}

	getEstimatedQuantity(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): number {
		return window.Helper.ServiceOrderMaterial.getEstimatedQuantity(serviceOrderMaterial);
	}

	getInvoiceQuantity(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): number {
		return window.Helper.ServiceOrderMaterial.getInvoiceQuantity(serviceOrderMaterial);
	}

	getItemGroup(serviceOrderPosition: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): ItemGroup {
		return window.Helper.ServiceOrder.getServiceOrderPositionItemGroup(serviceOrderPosition);
	};

	async initItems(items: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial[]): Promise<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial[]> {
		await window.Helper.Article.loadArticleDescriptionsMapFromItemNo(items, this.currentUser().DefaultLanguageKey);
		items = await super.initItems(items);
		await window.Helper.ServiceOrderMaterial.loadParentAndChildPositions(items, () => this.applyJoins(window.database.CrmService_ServiceOrderMaterial));
		return items;
	};

	isExpanded(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): KnockoutComputed<boolean> {
		return ko.pureComputed(() => {
			return this.expandedPositions().indexOf(this.getRootPosition(serviceOrderMaterial).Id()) !== -1;
		});
	}

	isVisible(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): boolean {
		return this.getAllPositions(this.getRootPosition(serviceOrderMaterial))[0]?.Id() === serviceOrderMaterial.Id();
	}

	canEditMaterial(material: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): boolean {
		return this.parentViewModel.serviceOrderIsEditable() && this.isEditable() &&
			(!material.ServiceOrderDispatch() || !window.Helper.Dispatch.dispatchIsClosed(material.ServiceOrderDispatch())
				|| (this.isOrderInPostProcessing() && window.AuthorizationManager.currentUserHasPermission("ServiceOrder::EditMaterialWhenDispatchClosed")));
	};

	showActualQuantityOfOtherDispatches(): boolean {
		return false;
	}

	togglePosition(material: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): void {
		let id = this.getRootPosition(material).Id();
		if (this.expandedPositions().indexOf(id) === -1) {
			this.expandedPositions.push(id);
		} else {
			this.expandedPositions.remove(id);
		}
	}

	async createInvoicePosition(material: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): Promise<void> {
		const invoiceServiceOrderMaterial = window.Helper.ServiceOrderMaterial.createInvoicePosition(material);
		window.database.add(invoiceServiceOrderMaterial);
		await window.database.saveChanges();
	}
}

export class ServiceOrderDetailsMaterialsTabViewModel extends ServiceOrderDetailsMaterialsTabViewModelBase<ServiceOrderDetailsViewModel> {
}

namespace("Crm.Service.ViewModels").ServiceOrderDetailsMaterialsTabViewModel = ServiceOrderDetailsMaterialsTabViewModel;