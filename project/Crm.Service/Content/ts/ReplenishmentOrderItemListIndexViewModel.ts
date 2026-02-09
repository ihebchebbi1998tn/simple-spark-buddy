import {namespace} from "@Main/namespace";
import {HelperDownload} from "@Main/helper/Helper.Download";

export class ReplenishmentOrderListViewModel extends window.Main.ViewModels.GenericListViewModel<Crm.Service.Rest.Model.CrmService_ReplenishmentOrder, Crm.Service.Rest.Model.ObservableCrmService_ReplenishmentOrder> {
	selectedUser = ko.observable<string>();
}

export class ReplenishmentOrderItemListIndexViewModel extends window.Main.ViewModels.GenericListViewModel<Crm.Service.Rest.Model.CrmService_ReplenishmentOrderItem, Crm.Service.Rest.Model.ObservableCrmService_ReplenishmentOrderItem> {
	currentUserName: string;
	replenishmentOrder = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ReplenishmentOrder>(null);
	responsibleUser = ko.observable<string>(null);
	showSidebar = ko.pureComputed<boolean>(() => window.AuthorizationManager.isAuthorizedForAction("ReplenishmentOrder", "ReplenishmentsFromOtherUsersSelectable") || window.AuthorizationManager.isAuthorizedForAction("ReplenishmentOrder", "SeeClosedReplenishmentOrders"));
	lookups: LookupType;
	showSummary = ko.observable<boolean>(true);
	replenishmentOrderListViewModel: ReplenishmentOrderListViewModel;

	constructor() {
		super("CrmService_ReplenishmentOrderItem",
			"MaterialNo",
			"ASC",
			["ServiceOrderMaterials", "ServiceOrderMaterials.ServiceOrderHead", "QuantityUnitEntry", "Article.QuantityUnitEntry"]);

		this.currentUserName = window.Helper.User.getCurrentUserName();
		this.responsibleUser(this.currentUserName);
		this.lookups = {
			quantityUnits: {$tableName: "CrmArticle_QuantityUnit"}
		};
		this.infiniteScroll(true);
		this.replenishmentOrderListViewModel = new ReplenishmentOrderListViewModel("CrmService_ReplenishmentOrder", "CreateDate", "DESC", ["ResponsibleUserObject"]);
		this.replenishmentOrderListViewModel.enableUrlUpdate(false);
		this.replenishmentOrderListViewModel.pageSize(10);
		this.replenishmentOrderListViewModel.getFilter("CloseDate").subscribe(this.replenishmentOrderListViewModel.filter.bind(this.replenishmentOrderListViewModel));
		this.replenishmentOrderListViewModel.selectedUser = this.responsibleUser;
		this.replenishmentOrderListViewModel.selectedUser.subscribe(this.replenishmentOrderListViewModel.filter.bind(this.replenishmentOrderListViewModel));
		this.replenishmentOrderListViewModel.applyFilters = (query) => {
			query = window.Main.ViewModels.GenericListViewModel.prototype.applyFilters.call(this.replenishmentOrderListViewModel, query);
			if (!!this.replenishmentOrderListViewModel.selectedUser()) {
				query = query.filter("it.ResponsibleUser == this.selectedUser",
					{selectedUser: this.replenishmentOrderListViewModel.selectedUser()});
			}
			return query.filter("it.IsClosed === true");
		};
	}

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		await this.initReplenishmentOrder();
		await super.init(id, params);
		if (window.AuthorizationManager.isAuthorizedForAction("ReplenishmentOrder", "SeeClosedReplenishmentOrders")) {
			await this.replenishmentOrderListViewModel.init();
		}
		this.replenishmentOrderListViewModel.loading(false);
		this.pageTitle = window.Helper.String.getTranslatedString("ReplenishmentOrder");
		if (this.replenishmentOrder() && this.replenishmentOrder().IsClosed()) {
			this.pageTitle = window.Helper.String.getTranslatedString("ReplenishmentOrderDatedOf").replace("{0}", window.Globalize.formatDate(this.replenishmentOrder().CloseDate(), {date: "medium"}));
		}
		if (window.AuthorizationManager.isAuthorizedForAction("ReplenishmentOrder", "ReplenishmentsFromOtherUsersSelectable")) {
			this.pageTitle += " " + window.Helper.String.getTranslatedString("For");
		}
	};

	async initReplenishmentOrder(): Promise<void> {
		let replenishmentOrder = await window.Helper.ReplenishmentOrder.getOrCreateCurrentReplenishmentOrder(this.responsibleUser());
		this.replenishmentOrder(replenishmentOrder);
		this.getFilter("ReplenishmentOrderId").extend({filterOperator: "==="})(replenishmentOrder.Id());
		await window.database.saveChanges();
	};

	async deleteReplenishmentOrderItem(replenishmentOrderItem: Crm.Service.Rest.Model.ObservableCrmService_ReplenishmentOrderItem): Promise<void> {
		let confirm = await window.Helper.Confirm.confirmDeleteAsync();
		if (!confirm) {
			return;
		}
		this.loading(true);
		if (replenishmentOrderItem.ServiceOrderMaterials()[0]) {
			replenishmentOrderItem.ServiceOrderMaterials()[0].ReplenishmentOrderItemId(null);
		}
		window.database.remove(replenishmentOrderItem);
		await window.database.saveChanges();
		await this.filter();
	};

	async closeReplenishmentOrder(): Promise<void> {
		let confirm = await window.Helper.Confirm.genericConfirmAsync({
			confirmButtonText: window.Helper.String.getTranslatedString("Complete"),
			text: window.Helper.String.getTranslatedString("ConfirmCloseReplenishmentOrder"),
			title: window.Helper.String.getTranslatedString("Complete")
		});
		if (!confirm) {
			return;
		}

		this.loading(true);
		window.database.attachOrGet(this.replenishmentOrder().innerInstance);
		this.replenishmentOrder().CloseDate(new Date());
		this.replenishmentOrder().ClosedBy(this.currentUserName);
		this.replenishmentOrder().IsClosed(true);
		await window.database.saveChanges();
		let replenishmentOrder = await window.Helper.ReplenishmentOrder.getOrCreateCurrentReplenishmentOrder(this.responsibleUser(), this.replenishmentOrder().Id());
		this.replenishmentOrder(replenishmentOrder);
		this.getFilter("ReplenishmentOrderId").extend({filterOperator: "==="})(replenishmentOrder.Id());
		this.defaultFilters["ReplenishmentOrderId"].Value = replenishmentOrder.Id();
		await window.database.saveChanges();
		await this.filter();
	};

	async onResponsibleUserSelect(responsibleUser: Main.Rest.Model.Main_User): Promise<void> {
		if (!responsibleUser) {
			this.loading(true);
			this.replenishmentOrder(window.database.CrmService_ReplenishmentOrder.defaultType.create().asKoObservable());
			this.getFilter("ReplenishmentOrderId").extend({filterOperator: "==="})(window.Helper.String.emptyGuid());
			this.defaultFilters["ReplenishmentOrderId"].Value = window.Helper.String.emptyGuid();
			await this.filter();
			return;
		}
		if (this.replenishmentOrder().ResponsibleUser() === responsibleUser.Id) {
			return;
		}
		this.loading(true);
		await this.initReplenishmentOrder()
		this.defaultFilters["ReplenishmentOrderId"].Value = this.replenishmentOrder().Id();
		await this.filter();
	}

	async showReplenishmentOrder(replenishmentOrder: Crm.Service.Rest.Model.ObservableCrmService_ReplenishmentOrder): Promise<void> {
		this.loading(true);
		this.replenishmentOrder(replenishmentOrder);
		this.getFilter("ReplenishmentOrderId").extend({filterOperator: "==="})(replenishmentOrder.Id());
		this.defaultFilters["ReplenishmentOrderId"].Value = replenishmentOrder.Id();
		await this.filter();
	}

	getItemGroup(item: Crm.Service.Rest.Model.ObservableCrmService_ReplenishmentOrderItem): ItemGroup {
		if (!this.showSummary()) {
			return null;
		}
		const itemGroup = {title: "", css: "hidden"};
		switch (this.orderBy()) {
			case "Description":
				itemGroup.title = item.Description();
				break;
			case "Quantity":
				return null;
			default:
				itemGroup.title = item.MaterialNo();
		}
		return itemGroup;
	}

	getQuantitySummary(itemGroup: { title: string }): number {
		let sum = 0;
		this.items().forEach(item => {
			if (this.getItemGroup(item).title == itemGroup.title) {
				if (item.QuantityUnitEntryKey() && item.Article()) {
					sum += Helper.QuantityUnit.getConversionRate(item.QuantityUnitEntry, item.Article().QuantityUnitEntry) * item.Quantity();
				}
				else {
					sum += item.Quantity();
				}
			}

		});
		return sum;
	}

	async downloadReplenishmentOrderReport(replenishmentOrder: Crm.Service.Rest.Model.ObservableCrmService_ReplenishmentOrder): Promise<void> {
		const viewModel = this;
		viewModel.loading(true);
		const reportName = `${Helper.String.getTranslatedString('ReplenishmentOrder')}_${new Date().toISOString().substring(0, 10)}.pdf`;
		await HelperDownload.downloadPdf(`~/Crm.Service/ReplenishmentOrder/CreatePdf/${replenishmentOrder.Id()}`, reportName);
		viewModel.loading(false);
	};
}

namespace("Crm.Service.ViewModels").ReplenishmentOrderItemListIndexViewModel = ReplenishmentOrderItemListIndexViewModel;