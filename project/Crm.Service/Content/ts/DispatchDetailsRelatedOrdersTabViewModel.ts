import {namespace} from "@Main/namespace";
import type { DispatchDetailsViewModel } from "./DispatchDetailsViewModel";

export class DispatchDetailsRelatedOrdersTabViewModel extends window.Main.ViewModels.ViewModelBase {
	lists = ko.observableArray<{
		subtitle?: string,
		title: string,
		type: string,
		viewModel: any
	}>([]);
	dispatch: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch>;
	serviceOrder: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead>;
	currentUser = ko.observable<Main.Rest.Model.Main_User>(null);

	constructor(parentViewModel: DispatchDetailsViewModel) {
		super();
		this.dispatch = parentViewModel.dispatch;
		this.serviceOrder = parentViewModel.serviceOrder;
		this.currentUser = parentViewModel.currentUser;
	}

	async init(): Promise<void> {

		let RelatedDispatchesViewModel = new window.Crm.Service.ViewModels.ServiceOrderDispatchListIndexViewModel(this);
		let RelatedOrdersViewModel = new window.Crm.Service.ViewModels.ServiceOrderHeadListIndexViewModel();


		RelatedDispatchesViewModel.applyFilters = (query) => {
			query = window.Main.ViewModels.GenericListViewModel.prototype.applyFilters.call(RelatedDispatchesViewModel, query);
			return query.filter(function (it) {
				return it.Id !== this.DispatchId && ((it.ServiceOrder.InstallationId !== null && it.ServiceOrder.InstallationId === this.InstallationId) ||
					(it.ServiceOrder.CustomerContactId !== null && it.ServiceOrder.CustomerContactId === this.CustomerContactId) || (it.ServiceOrder.MaintenancePlanningRun !== null && it.ServiceOrder.MaintenancePlanningRun === this.MaintenancePlanningRun));
			}, {
				DispatchId: this.dispatch().Id(), InstallationId: this.serviceOrder().InstallationId(), CustomerContactId: this.serviceOrder().CustomerContactId(), MaintenancePlanningRun: this.serviceOrder().MaintenancePlanningRun()
			});
		}
		RelatedOrdersViewModel.applyFilters = (query) => {
			query = window.Main.ViewModels.GenericListViewModel.prototype.applyFilters.call(RelatedOrdersViewModel, query);
			return query.filter(function (it) {
				return it.Id !== this.ServiceOrderId && ((it.InstallationId !== null && it.InstallationId === this.InstallationId) ||
					(it.CustomerContactId !== null && it.CustomerContactId === this.CustomerContactId) || (it.MaintenancePlanningRun !== null && it.MaintenancePlanningRun === this.MaintenancePlanningRun));
			}, { ServiceOrderId: this.serviceOrder().Id(), InstallationId: this.serviceOrder().InstallationId(), CustomerContactId: this.serviceOrder().CustomerContactId(), MaintenancePlanningRun: this.serviceOrder().MaintenancePlanningRun() });
		}

		RelatedDispatchesViewModel.bookmarks([]);
		RelatedOrdersViewModel.bookmarks([]);
		const bookmarkAllRelatedDispatches = {
			Category: window.Helper.String.getTranslatedString("Filter"),
			Name: window.Helper.String.getTranslatedString("All"),
			Key: "All",
			Expression: query => query
		}
		const bookmarkMyDispatches = {
			Category: window.Helper.String.getTranslatedString("Filter"),
			Name: window.Helper.String.getTranslatedString("MyDispatches"),
			Key: "MyDispatches",
			Expression: query => {
				return query.filter(function (it) {
					return it.Username === this.username;
				}, { username: this.currentUser().Id });
			}
		}
		RelatedDispatchesViewModel.bookmarks.push(bookmarkAllRelatedDispatches);
		RelatedDispatchesViewModel.bookmarks.push(bookmarkMyDispatches);
		RelatedDispatchesViewModel.bookmark(bookmarkAllRelatedDispatches);
		const bookmarkAllRelatedOrders = {
			Category: window.Helper.String.getTranslatedString("Filter"),
			Name: window.Helper.String.getTranslatedString("All"),
			Key: "All",
			Expression: query => query
		}
		RelatedOrdersViewModel.bookmarks.push(bookmarkAllRelatedOrders);
		RelatedOrdersViewModel.bookmark(bookmarkAllRelatedOrders);
		if (window.AuthorizationManager.isAuthorizedForAction("Dispatch", "SeeAllUsersDispatches")) {

			const bookmarkRelatedDispatchesForOrder = {
				Category: window.Helper.String.getTranslatedString("Filter"),
				Name: window.Helper.String.getTranslatedString("RelatedDispatchesForOrder"),
				Key: "RelatedDispatchesForOrder",
				Expression: query => {
					return query.filter(function (it) {
						return it.Id !== this.DispatchId && it.OrderId === this.OrderId
					}, { DispatchId: this.dispatch().Id(), OrderId: this.serviceOrder().Id() });
				}
			}
			RelatedDispatchesViewModel.bookmarks.push(bookmarkRelatedDispatchesForOrder);
		}
		if (this.serviceOrder().MaintenancePlanningRun() !== null) {

			const bookmarkRelatedDispatchesForMaintenancePlanningRun = {
				Category: window.Helper.String.getTranslatedString("Filter"),
				Name: window.Helper.String.getTranslatedString("RelatedDispatchesForMaintenancePlanningRun"),
				Key: "RelatedDispatchesForMaintenancePlanningRun",
				Expression: query => {
					return query.filter(function (it) {
						return it.Id !== this.DispatchId && it.ServiceOrder.MaintenancePlanningRun === this.MaintenancePlanningRun;
					}, { DispatchId: this.dispatch().Id(), MaintenancePlanningRun: this.serviceOrder().MaintenancePlanningRun() });
				}
			}
			RelatedDispatchesViewModel.bookmarks.push(bookmarkRelatedDispatchesForMaintenancePlanningRun);
			const bookmarkRelatedOrdersForMaintenancePlanningRun = {
				Category: window.Helper.String.getTranslatedString("Filter"),
				Name: window.Helper.String.getTranslatedString("RelatedOrdersForMaintenancePlanningRun"),
				Key: "RelatedOrdersForMaintenancePlanningRun",
				Expression: query => {
					return query.filter(function (it) {
						return it.Id !== this.ServiceOrderId && it.MaintenancePlanningRun === this.MaintenancePlanningRun;
					}, { ServiceOrderId: this.serviceOrder().Id(), MaintenancePlanningRun: this.serviceOrder().MaintenancePlanningRun() });
				}
			}
			RelatedOrdersViewModel.bookmarks.push(bookmarkRelatedOrdersForMaintenancePlanningRun);
		}
		if (this.serviceOrder().InstallationId() !== null) {
			const bookmarkRelatedDispatchesForInstallation = {
				Category: window.Helper.String.getTranslatedString("Filter"),
				Name: window.Helper.String.getTranslatedString("RelatedDispatchesForInstallation"),
				Key: "RelatedDispatchesForInstallation",
				Expression: query => {
					return query.filter(function (it) {
						return it.Id !== this.DispatchId && it.ServiceOrder.InstallationId === this.InstallationId;
					}, { DispatchId: this.dispatch().Id(), InstallationId: this.serviceOrder().InstallationId() });
				}
			}
			RelatedDispatchesViewModel.bookmarks.push(bookmarkRelatedDispatchesForInstallation);
			const bookmarkRelatedServiceOrdersForInstallation = {
				Category: window.Helper.String.getTranslatedString("Filter"),
				Name: window.Helper.String.getTranslatedString("RelatedOrdersForInstallation"),
				Key: "RelatedOrdersForInstallation",
				Expression: query => {
					return query.filter(function (it) {
						return it.Id !== this.ServiceOrderId && it.InstallationId === this.InstallationId;
					}, { ServiceOrderId: this.serviceOrder().Id(), InstallationId: this.serviceOrder().InstallationId() });
				}
			}
			RelatedOrdersViewModel.bookmarks.push(bookmarkRelatedServiceOrdersForInstallation);
		}
		if (this.serviceOrder().ServiceOrderTimes().length > 0) {
			let ids = this.serviceOrder().ServiceOrderTimes().map(j => j.InstallationId())
			let bookmarkRelatedDispatchesForInstallation: Bookmark<Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch>;
			if (window.database.storageProvider.name === "oData") {
				bookmarkRelatedDispatchesForInstallation = {
					Category: window.Helper.String.getTranslatedString("Filter"),
					Name: window.Helper.String.getTranslatedString("RelatedDispatchesForInstallation"),
					Key: "RelatedDispatchesForInstallations",
					Expression: query => {
						return query.filter(function (it) {
							return it.Id !== this.DispatchId;
						}, {DispatchId: this.dispatch().Id()});
					}
				}
				const originalInitItems = RelatedDispatchesViewModel.initItems;
				RelatedDispatchesViewModel.initItems = async function (items) {
					if (this.bookmark().Key === "RelatedDispatchesForInstallations") {
						const parentVm = this.parentViewModel;
						const ids = parentVm.serviceOrder().ServiceOrderTimes().map(j => j.InstallationId());
						items = items.filter(i => {
							return i.ServiceOrder().ServiceOrderTimes().some(time => {
								return ids.includes(time.InstallationId())
							}) && i.Id() !== parentVm.dispatch().Id();
						})
					
					}
					let result = await originalInitItems.call(this, items);
					this.totalItemCount(result.length);
					return result;
				}
			} else {
				bookmarkRelatedDispatchesForInstallation = {
					Category: window.Helper.String.getTranslatedString("Filter"),
					Name: window.Helper.String.getTranslatedString("RelatedDispatchesForInstallation"),
					Key: "RelatedDispatchesForInstallations",
					Expression: query => {
						return query.filter(function (it) {
							// @ts-ignore
							return it.Id !== this.DispatchId && it.ServiceOrder.ServiceOrderTimes.InstallationId in this.ids;
						}, {DispatchId: this.dispatch().Id(), ids: ids});
					}
				}
			}
			RelatedDispatchesViewModel.bookmarks.push(bookmarkRelatedDispatchesForInstallation);
			const bookmarkRelatedServiceOrdersForInstallation = {
				Category: window.Helper.String.getTranslatedString("Filter"),
				Name: window.Helper.String.getTranslatedString("RelatedOrdersForInstallation"),
				Key: "RelatedOrdersForInstallations",
				Expression: query => {
					return window.database.storageProvider.name === "oData"
						? query.filter(function (it) {
							return it.Id !== this.ServiceOrderId && it.ServiceOrderTimes.some(function (it2) {
								return it2.InstallationId in this.ids;
							});
						}, {ServiceOrderId: this.serviceOrder().Id(), ids: ids})
						: query.filter(function (it) {
							return it.Id !== this.ServiceOrderId && it.ServiceOrderTimes.InstallationId in this.ids;
						}, {ServiceOrderId: this.serviceOrder().Id(), ids: ids});
				}
			}
			RelatedOrdersViewModel.bookmarks.push(bookmarkRelatedServiceOrdersForInstallation);
		}
		if (this.serviceOrder().CustomerContactId() !== null) {
			const bookmarkRelatedDispatchesForCustomer = {
				Category: window.Helper.String.getTranslatedString("Filter"),
				Name: window.Helper.String.getTranslatedString("RelatedDispatchesForCustomer"),
				Key: "RelatedDispatchesForCustomer",
				Expression: query => {
					return query.filter(function (it) {
						return it.Id !== this.DispatchId && it.ServiceOrder.CustomerContactId === this.CustomerContactId;
					}, { DispatchId: this.dispatch().Id(), CustomerContactId: this.serviceOrder().CustomerContactId() });
				}
			}
			RelatedDispatchesViewModel.bookmarks.push(bookmarkRelatedDispatchesForCustomer);
			const bookmarkRelatedServiceOrdersForCustomer = {
				Category: window.Helper.String.getTranslatedString("Filter"),
				Name: window.Helper.String.getTranslatedString("RelatedOrdersForCustomer"),
				Key: "RelatedOrdersForCustomer",
				Expression: query => {
					return query.filter(function (it) {
						return it.Id !== this.ServiceOrderId && it.CustomerContactId === this.CustomerContactId;
					}, { ServiceOrderId: this.serviceOrder().Id(), CustomerContactId: this.serviceOrder().CustomerContactId() });
				}
			}
			RelatedOrdersViewModel.bookmarks.push(bookmarkRelatedServiceOrdersForCustomer);
		}
		if (this.serviceOrder().ServiceObjectId() !== null) {

			const bookmarkRelatedDispatchesForServiceObject = {
				Category: window.Helper.String.getTranslatedString("Filter"),
				Name: window.Helper.String.getTranslatedString("RelatedDispatchesForServiceObject"),
				Key: "RelatedDispatchesForServiceObject",
				Expression: query => {
					return query.filter(function (it) {
						return it.Id !== this.DispatchId && it.ServiceOrder.ServiceObjectId === this.ServiceObjectId;
					}, { DispatchId: this.dispatch().Id(), ServiceObjectId: this.serviceOrder().ServiceObjectId() });
				}
			}
			RelatedDispatchesViewModel.bookmarks.push(bookmarkRelatedDispatchesForServiceObject);
			const bookmarkRelatedServiceOrdersForServiceObject = {
				Category: window.Helper.String.getTranslatedString("Filter"),
				Name: window.Helper.String.getTranslatedString("RelatedOrdersForServiceObject"),
				Key: "RelatedOrdersForServiceObject",
				Expression: query => {
					return query.filter(function (it) {
						return it.Id !== this.ServiceOrderId && it.ServiceObjectId === this.ServiceObjectId;
					}, { ServiceOrderId: this.serviceOrder().Id(), ServiceObjectId: this.serviceOrder().ServiceObjectId() });
				}
			}
			RelatedOrdersViewModel.bookmarks.push(bookmarkRelatedServiceOrdersForServiceObject);
		}
		this.lists.push({
			title: "Dispatches",
			type: "dispatch",
			viewModel: RelatedDispatchesViewModel
		});
		this.lists.push({
			title: "ServiceOrders",
			type: "serviceOrder",
			viewModel: RelatedOrdersViewModel
		});
		await Promise.all(this.lists().map(x => {
			x.viewModel.infiniteScroll(false);
			return x.viewModel.init();
		}));
		this.lists().forEach(x => {
			x.viewModel.bulkActions([]);
			x.viewModel.loading(false);
			x.viewModel.loading.subscribe(() => {
				this.loading(this.lists().some(list => list.viewModel.loading()));
			});
		});
	}
	toggleBookmark(bookmark: Bookmark<$data.Entity>): void {
		const viewModel = this;
		// @ts-ignore
		viewModel.bookmark(bookmark);
		if (bookmark.ApplyFilters) {
			bookmark.ApplyFilters();
			// @ts-ignore
			viewModel.bookmark(null);
		}
		// @ts-ignore
		viewModel.page(1);
		// @ts-ignore
		viewModel.currentSearch = viewModel.search(false, true);
	}

}

namespace("Crm.Service.ViewModels").DispatchDetailsRelatedOrdersTabViewModel = DispatchDetailsRelatedOrdersTabViewModel;