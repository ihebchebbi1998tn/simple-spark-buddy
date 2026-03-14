import {namespace} from "@Main/namespace";
import type {DispatchDetailsViewModel} from "./DispatchDetailsViewModel";
import {ServiceOrderDetailsTimePostingsTabViewModelBase} from "./ServiceOrderDetailsTimePostingsTabViewModel";
import {HelperDispatch} from "@Crm.Service/helper/Helper.Dispatch";

export class DispatchDetailsTimePostingsTabViewModel extends ServiceOrderDetailsTimePostingsTabViewModelBase<DispatchDetailsViewModel> {
	dispatch: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch>;
	validItemNosAfterCustomerSignature = ko.observableArray<string>([]);
	editableItemNosAfterCustomerSignature = ko.observableArray<string>([]);
	timePostingsCanBeAdded = ko.pureComputed<boolean>(() => {
		return HelperDispatch.dispatchIsEditable(this.dispatch()) ||
			(["SignedByCustomer", "ClosedNotComplete", "ClosedComplete"].indexOf(this.dispatch().StatusKey()) !== -1 &&
				this.validItemNosAfterCustomerSignature().length > 0);
	});
	currentJobItemGroup = ko.pureComputed<ItemGroup>(() => window.Helper.Dispatch.getCurrentJobItemGroup(this));
	parentViewModel: DispatchDetailsViewModel;

	constructor(parentViewModel: DispatchDetailsViewModel) {
		super(parentViewModel);
		this.parentViewModel = parentViewModel;
		this.dispatch = parentViewModel.dispatch;
		this.bookmarks([]);
		this.bookmark(null);
	}

	applyFilters(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTimePosting>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTimePosting> {
		query = super.applyFilters(query);
		query = query.filter(function (it) {
			return it.DispatchId === this.dispatchId ||
				(it.DispatchId == null && it.ServiceOrderTimePostingType === window.Crm.Service.ServiceOrderTimePostingType.Preplanned)
		}, {dispatchId: this.dispatch().Id()});
		return query;
	};

	applyOrderBy(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTimePosting>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTimePosting> {
		let id = null;
		if (this.dispatch() && this.dispatch().CurrentServiceOrderTimeId()) {
			id = this.dispatch().CurrentServiceOrderTimeId();
		}
		// @ts-ignore
		query = query.orderByDescending("orderByCurrentServiceOrderTime", {currentServiceOrderTimeId: id});
		return super.applyOrderBy(query);
	};

	getActualDuration(serviceOrderTimePosting: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting): string {
		if (serviceOrderTimePosting.ServiceOrderTimePostingType() === window.Crm.Service.ServiceOrderTimePostingType.Used) {
			return serviceOrderTimePosting.Duration();
		}

		return "P0M";
	}

	getEstimatedDuration(serviceOrderTimePosting: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting): string {
		if (serviceOrderTimePosting.ServiceOrderTimePostingType() === window.Crm.Service.ServiceOrderTimePostingType.Preplanned) {
			return serviceOrderTimePosting.Duration();
		}

		if (serviceOrderTimePosting.ServiceOrderTimePostingType() === window.Crm.Service.ServiceOrderTimePostingType.Used) {
			if (serviceOrderTimePosting.ParentServiceOrderTimePosting()?.ServiceOrderTimePostingType() === window.Crm.Service.ServiceOrderTimePostingType.Preplanned) {
				return serviceOrderTimePosting.ParentServiceOrderTimePosting().Duration();
			}
			if (serviceOrderTimePosting.ServiceOrderTime()) {
				return serviceOrderTimePosting.ServiceOrderTime().EstimatedDuration();
			}
			return "PT0M";
		}

		return "PT0M";
	}

	getInvoiceDuration(serviceOrderTimePosting: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting): string {
		return "PT0M";
	}

	getItemGroup = window.Crm.Service
		.ViewModels.DispatchDetailsViewModel.prototype.getServiceOrderPositionItemGroup;

	async init(): Promise<void> {
		let itemNos = await window.database.CrmArticle_Article.filter(function (it) {
			return it.ArticleTypeKey === "Service" && it.ExtensionValues.CanBeAddedAfterCustomerSignature && !it.ExtensionValues.IsHidden;
		}).map(function (it) {
			return it.ItemNo;
		}).toArray();
		this.validItemNosAfterCustomerSignature(itemNos);
		let editablesItemNos = await window.database.CrmArticle_Article.filter(function (it) {
			return it.ArticleTypeKey === "Service" && it.ExtensionValues.CanBeEditedAfterCustomerSignature && !it.ExtensionValues.IsHidden;
		}).map(function (it) {
			return it.ItemNo;
		}).toArray();
		this.editableItemNosAfterCustomerSignature(editablesItemNos);
		await super.init();
	};

	isVisible(serviceOrderTimePosting: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting): boolean {
		if (serviceOrderTimePosting.ServiceOrderTimePostingType() === window.Crm.Service.ServiceOrderTimePostingType.Used) {
			return true;
		}

		let hasUsedPosition = serviceOrderTimePosting.ChildServiceOrderTimePostings().find(x => x.DispatchId() === this.dispatch().Id() && x.ServiceOrderTimePostingType() === window.Crm.Service.ServiceOrderTimePostingType.Used);
		if (serviceOrderTimePosting.ServiceOrderTimePostingType() === window.Crm.Service.ServiceOrderTimePostingType.Preplanned && !hasUsedPosition) {
			return true;
		}

		return false;
	}

	timePostingCanBeEdited(serviceOrderTimePosting: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting): boolean {
		return ((HelperDispatch.dispatchIsEditable(this.dispatch()) ||
			(
				["SignedByCustomer", "ClosedNotComplete", "ClosedComplete"]
					.indexOf(this.dispatch().StatusKey()) !== -1 &&
				this.validItemNosAfterCustomerSignature()
					.indexOf(serviceOrderTimePosting.ItemNo()) !== -1
			) ||
			(
				["SignedByCustomer", "ClosedNotComplete", "ClosedComplete"]
					.indexOf(this.dispatch().StatusKey()) !== -1 &&
				this.editableItemNosAfterCustomerSignature()
					.indexOf(serviceOrderTimePosting.ItemNo()) !== -1
			)
		)
			&& !serviceOrderTimePosting.IsClosed()
			&& (
				Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode == "OrderPerInstallation" ||
				!Crm.Service.Settings.Dispatch.RestrictEditingToActiveJob ||
				(
					serviceOrderTimePosting.ServiceOrderTimeId() &&
					serviceOrderTimePosting.ServiceOrderTimeId() == this.dispatch().CurrentServiceOrderTimeId()
				)
			)
		);
	};

	timePostingCanBeDeleted(serviceOrderTimePosting: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting): boolean {
		const editable = this.timePostingCanBeEdited(serviceOrderTimePosting);
		if (editable && window.Helper.ServiceOrderTimePosting.isPrePlanned(serviceOrderTimePosting)) {
			return window.AuthorizationManager.currentUserHasPermission("ServiceOrder::TimePostingPrePlannedRemove");
		}
		return editable;
	};
}

namespace("Crm.Service.ViewModels").DispatchDetailsTimePostingsTabViewModel = DispatchDetailsTimePostingsTabViewModel;