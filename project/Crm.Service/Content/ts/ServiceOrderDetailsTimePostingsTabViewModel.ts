import {namespace} from "@Main/namespace";
import type {ServiceOrderDetailsViewModel} from "./ServiceOrderDetailsViewModel";
import type {ServiceOrderTemplateDetailsViewModel} from "./ServiceOrderTemplateDetailsViewModel";
import type {DispatchDetailsViewModel} from "./DispatchDetailsViewModel";
import {HelperString} from "@Main/helper/Helper.String";

export class ServiceOrderDetailsTimePostingsTabViewModelBase<T extends ServiceOrderDetailsViewModel | ServiceOrderTemplateDetailsViewModel | DispatchDetailsViewModel> extends window.Main.ViewModels.GenericListViewModel<Crm.Service.Rest.Model.CrmService_ServiceOrderTimePosting, Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting> {
	expandedPositions = ko.observableArray<string>();
	hideActions: boolean = false;
	serviceOrder: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead>;
	canEditEstimatedQuantities = ko.pureComputed<boolean>(() => {
		return window.Helper.ServiceOrder.canEditEstimatedQuantitiesSync(this.serviceOrder().StatusKey(), this.lookups.serviceOrderStatuses as {[key:string]: Crm.Service.Rest.Model.Lookups.CrmService_ServiceOrderStatus});
	});
	showServiceOrderTimePostingType: KnockoutComputed<boolean>;
	isOrderInPostProcessing = window.ko.pureComputed(() => {
		return Helper.ServiceOrder.isInStatusGroupSync(this.serviceOrder().StatusKey(), this.lookups.serviceOrderStatuses as { [key: string]: Crm.Service.Rest.Model.Lookups.CrmService_ServiceOrderStatus }, ["PostProcessing"]);
	});
	timePostingsCanBeAdded = ko.pureComputed<boolean>(() => {
		return this.parentViewModel.serviceOrderIsEditable() && (this.canEditEstimatedQuantities() || (this.isOrderInPostProcessing && window.AuthorizationManager.currentUserHasPermission("ServiceOrder::EditTimePostingWhenDispatchClosed")));
	});
	invoicePositionsCanBeAdded = ko.pureComputed<boolean>(() => {
		return this.isOrderInPostProcessing() && this.timePostingsCanBeAdded();
	});
	parentViewModel: T;

	constructor(parentViewModel: T) {
		super("CrmService_ServiceOrderTimePosting",
			["ServiceOrderTime.PosNo", "From"],
			["ASC", "ASC"],
			["ServiceOrderDispatch", "ServiceOrderTime", "ServiceOrderTime.Installation", "User", "ServiceOrderDispatch", "ServiceOrderDispatch.DispatchedUser"]);
		this.parentViewModel = parentViewModel;
		this.lookups = parentViewModel.lookups;
		this.serviceOrder = parentViewModel.serviceOrder;
		this.infiniteScroll(true);
		this.bookmarks.push({
			Category: window.Helper.String.getTranslatedString("Show"),
			Name: window.Helper.String.getTranslatedString("All"),
			Key: "All",
			Expression: (query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTimePosting>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTimePosting> => {
				return query;
			}
		});
		this.bookmarks.push({
			Category: window.Helper.String.getTranslatedString("Show"),
			Name: window.Helper.String.getTranslatedString("ServiceOrderTimePostingTypePreplanned"),
			Key: "Preplanned",
			Expression: (query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTimePosting>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTimePosting> => {
				return query.filter(it => it.ServiceOrderTimePostingType === window.Crm.Service.ServiceOrderTimePostingType.Preplanned);
			}
		});
		this.bookmarks.push({
			Category: window.Helper.String.getTranslatedString("Show"),
			Name: window.Helper.String.getTranslatedString("ServiceOrderTimePostingTypeUsed"),
			Key: "Used",
			Expression: (query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTimePosting>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTimePosting> => {
				return query.filter(it => it.ServiceOrderTimePostingType === window.Crm.Service.ServiceOrderTimePostingType.Used);
			}
		});
		this.bookmarks.push({
			Category: window.Helper.String.getTranslatedString("Show"),
			Name: window.Helper.String.getTranslatedString("ServiceOrderTimePostingTypeInvoice"),
			Key: "Invoice",
			Expression: (query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTimePosting>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTimePosting> => {
				return query.filter(it => it.ServiceOrderTimePostingType === window.Crm.Service.ServiceOrderTimePostingType.Invoice);
			}
		});
		this.bookmark(this.bookmarks()[0]);
		this.pageSize(20);
		this.showServiceOrderTimePostingType = ko.pureComputed(() => {
			return this.bookmark() !== null && this.bookmark().Key === "All";
		})
	}

	async init() {
		await super.init();
		window.Helper.Database.registerEventHandlers(this,
			{
				"CrmService_ServiceOrderTimePosting": {
					"afterDelete": async (sender, item) => {
						if (item.PerDiemReportId) {
							await window.Helper.PerDiem.refreshPerDiemReport(item.PerDiemReportId);
						}
					}
				}
			});

		window.Helper.Database.registerEventHandlers(this,
			{
				"CrmService_ServiceOrderExpensePosting": {
					"afterDelete": async (sender, item) => {
						if (item.PerDiemReportId) {
							window.Helper.PerDiem.refreshPerDiemReport(item.PerDiemReportId);
						}
					}
				}
			});
	};

	applyFilter(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTimePosting>, filterValue: { Value: string }, filterName: string): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTimePosting> {
		if (filterName === "ItemDescription") {
			return query.filter("filterByArticleDescription",
				{filter: filterValue.Value, language: this.currentUser().DefaultLanguageKey});
		}
		return super.applyFilter(query, filterValue, filterName);
	};

	applyFilters(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTimePosting>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTimePosting> {
		query = super.applyFilters(query);
		return query.filter("it.OrderId === this.orderId", {orderId: this.serviceOrder().Id()});
	};

	async deleteServiceOrderTimePosting(serviceOrderTimePosting: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting, reset = false): Promise<void> {
		let confirm: boolean = false;
		let confirmReset: boolean = false;
		if(reset) {
			confirmReset = await window.Helper.Confirm.genericConfirmAsync({
				text: HelperString.getTranslatedString("ConfirmResetTimeEntry"),
				type: "error",
				confirmButtonText: HelperString.getTranslatedString("Reset")
			});
		} else {
			confirm = await window.Helper.Confirm.confirmDeleteAsync();
		}
		if (reset && !confirmReset) {
			return;
		}
		if (!reset && !confirm) {
			return;
		}
		this.loading(true);

		window.database.remove(serviceOrderTimePosting);
		await window.database.saveChanges();
		this.filter();
	};

	getActualDuration(serviceOrderTimePosting: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting): string {
		return window.Helper.ServiceOrderTimePosting.getActualDuration(serviceOrderTimePosting);
	}

	getAllPositions = (serviceOrderTimePosting: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting): Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting[] => {
		return this.items()
			.filter(x => this.getRootPosition(x).Id() === this.getRootPosition(serviceOrderTimePosting).Id())
			.sort((x, y) => x.CreateDate().getTime() - y.CreateDate().getTime());
	}
	
	getEstimatedDuration(serviceOrderTimePosting: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting): string {
		return window.Helper.ServiceOrderTimePosting.getEstimatedDuration(serviceOrderTimePosting);
	}
	
	getInvoiceDuration(serviceOrderTimePosting: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting): string {
		return window.Helper.ServiceOrderTimePosting.getInvoiceDuration(serviceOrderTimePosting);
	}
	
	getItemGroup(serviceOrderPosition: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting): ItemGroup {
		return window.Helper.ServiceOrder.getServiceOrderPositionItemGroup(serviceOrderPosition);
	};

	getRootPosition = (serviceOrderTimePosting: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting): Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting => {
		if (serviceOrderTimePosting.ParentServiceOrderTimePosting()) {
			return this.getRootPosition(serviceOrderTimePosting.ParentServiceOrderTimePosting());
		}
		return serviceOrderTimePosting;
	}

	isExpanded(serviceOrderTimePosting: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting): KnockoutComputed<boolean> {
		return ko.pureComputed(() => {
			return this.expandedPositions().indexOf(this.getRootPosition(serviceOrderTimePosting).Id()) !== -1;
		});
	}
	
	timePostingCanBeEdited(serviceOrderTimePosting: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting): boolean {
		const editable = this.parentViewModel.serviceOrderIsEditable();
		if (editable && window.Helper.ServiceOrderTimePosting.isPrePlanned(serviceOrderTimePosting)) {
			return window.AuthorizationManager.currentUserHasPermission("ServiceOrder::TimePostingPrePlannedEdit");
		}
		return editable && !serviceOrderTimePosting.IsClosed() &&
			(!serviceOrderTimePosting.ServiceOrderDispatch() || !window.Helper.Dispatch.dispatchIsClosed(serviceOrderTimePosting.ServiceOrderDispatch())
				|| (this.isOrderInPostProcessing() && window.AuthorizationManager.currentUserHasPermission("ServiceOrder::EditTimePostingWhenDispatchClosed")));
	};

	timePostingCanBeDeleted(serviceOrderTimePosting: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting): boolean {
		const editable = this.parentViewModel.serviceOrderIsEditable();
		if (editable && (window.Helper.ServiceOrderTimePosting.isPrePlanned(serviceOrderTimePosting) || window.Helper.ServiceOrderTimePosting.wasPrePlanned(serviceOrderTimePosting))) {
			return window.AuthorizationManager.currentUserHasPermission("ServiceOrder::TimePostingPrePlannedRemove");
		}
		return editable && !serviceOrderTimePosting.IsClosed() &&
			(!serviceOrderTimePosting.ServiceOrderDispatch() || !window.Helper.Dispatch.dispatchIsClosed(serviceOrderTimePosting.ServiceOrderDispatch())
				|| (this.isOrderInPostProcessing() && window.AuthorizationManager.currentUserHasPermission("ServiceOrder::EditTimePostingWhenDispatchClosed")));
	};

	async initItems(items: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting[]): Promise<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting[]> {
		await window.Helper.Article.loadArticleDescriptionsMapFromItemNo(items, this.currentUser().DefaultLanguageKey);
		const loadParentPositions = async (allItems: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting[]): Promise<void> => {
			let itemsWithParent = allItems.filter(x => x.ParentServiceOrderTimePostingId() !== null);
			if (itemsWithParent.length === 0) {
				return;
			}
			let parentIds = itemsWithParent.map(x => x.ParentServiceOrderTimePostingId());
			let parents = await this.applyJoins(window.database.CrmService_ServiceOrderTimePosting).filter(function(it) { return it.Id in this.ids; }, {ids: parentIds}).toArray();
			let parentsAsKoObservable = parents.map(x => this.initItem(x));
			for (const itemWithParent of itemsWithParent) {
				let parent = parentsAsKoObservable.find(x => x.Id() === itemWithParent.ParentServiceOrderTimePostingId()) || null;
				itemWithParent.ParentServiceOrderTimePosting(parent);
			}
			await loadParentPositions(parentsAsKoObservable);
		}

		const loadChildPositions = async (allItems: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting[]): Promise<void> => {
			if (allItems.length === 0) {
				return;
			}
			let parentIds = allItems.map(x => x.Id());
			let children = await this.applyJoins(window.database.CrmService_ServiceOrderTimePosting).filter(function(it) { return it.ParentServiceOrderTimePostingId in this.ids; }, {ids: parentIds}).toArray();
			let childrenAsKoObservable = children.map(x => this.initItem(x));
			for (const item of allItems) {
				let children = childrenAsKoObservable.filter(x => x.ParentServiceOrderTimePostingId() === item.Id()) || [];
				item.ChildServiceOrderTimePostings(children);
			}
			await loadChildPositions(childrenAsKoObservable);
		}

		await loadParentPositions(items);
		await loadChildPositions(items);
		return await super.initItems(items);
	}

	getLocalTimeZoneText(entry: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting): string {
		if(!ko.unwrap(entry.From) || !ko.unwrap(entry.To)) {
			return "";
		}
		return `${window.Helper.String.getTranslatedString("CurrentTimeZoneIs")} ${window.Helper.Date.getTimeZoneWithUtcText(window.moment.tz.guess())}<br>
		${window.Helper.String.getTranslatedString("From")}: ${window.Globalize.formatDate(ko.unwrap(entry.From), {"datetime": "short"})}<br>
		${window.Helper.String.getTranslatedString("To")}: ${window.Globalize.formatDate(ko.unwrap(entry.To), {"datetime": "short"})}`;
	}
	
	isVisible(serviceOrderTimePosting: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting): boolean {
		const allPositions = this.getAllPositions(this.getRootPosition(serviceOrderTimePosting));
		return allPositions.length > 0 && allPositions[0].Id() === serviceOrderTimePosting.Id();
	}

	togglePosition(serviceOrderTimePosting: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting): void {
		let id = this.getRootPosition(serviceOrderTimePosting).Id();
		if (this.expandedPositions().indexOf(id) === -1) {
			this.expandedPositions.push(id);
		} else {
			this.expandedPositions.remove(id);
		}
	}

	async createInvoicePosition(serviceOrderTimePosting: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting): Promise<void> {
		const invoiceServiceOrderTimePosting = window.Helper.ServiceOrderTimePosting.createInvoicePosition(serviceOrderTimePosting);
		window.database.add(invoiceServiceOrderTimePosting);
		await window.database.saveChanges();
	}
}

export class ServiceOrderDetailsTimePostingsTabViewModel extends ServiceOrderDetailsTimePostingsTabViewModelBase<ServiceOrderDetailsViewModel>{
	
}

namespace("Crm.Service.ViewModels").ServiceOrderDetailsTimePostingsTabViewModel = ServiceOrderDetailsTimePostingsTabViewModel;