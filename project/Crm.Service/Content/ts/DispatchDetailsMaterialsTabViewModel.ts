import type {DispatchDetailsViewModel} from "./DispatchDetailsViewModel";
import {namespace} from "@Main/namespace";
import {ServiceOrderDetailsMaterialsTabViewModelBase} from "./ServiceOrderDetailsMaterialsTabViewModel";
import {HelperDispatch} from "@Crm.Service/helper/Helper.Dispatch";

export class DispatchDetailsMaterialsTabViewModel extends ServiceOrderDetailsMaterialsTabViewModelBase<DispatchDetailsViewModel> {
	dispatch: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch>;
	validCostItemNosAfterCustomerSignature = ko.observableArray<string>([]);
	validMaterialItemNosAfterCustomerSignature = ko.observableArray<string>([]);
	editableCostItemNosAfterCustomerSignature = ko.observableArray<string>([]);
	editableMaterialItemNosAfterCustomerSignature = ko.observableArray<string>([]);
	canAddMaterial = ko.pureComputed<boolean>(() => {
		return HelperDispatch.dispatchIsEditable(this.dispatch()) ||
			(["SignedByCustomer", "ClosedNotComplete", "ClosedComplete"].indexOf(this.dispatch().StatusKey()) !== -1 &&
				this.validMaterialItemNosAfterCustomerSignature().length > 0);
	});
	canAddCost = ko.pureComputed<boolean>(() => {
		return HelperDispatch.dispatchIsEditable(this.dispatch()) ||
			(["SignedByCustomer", "ClosedNotComplete", "ClosedComplete"].indexOf(this.dispatch().StatusKey()) !== -1 &&
				this.validCostItemNosAfterCustomerSignature().length > 0);
	});
	currentJobItemGroup = ko.pureComputed<ItemGroup>(() => window.Helper.Dispatch.getCurrentJobItemGroup(this));
	parentViewModel: DispatchDetailsViewModel;
	canEditActualQuantities = ko.observable<boolean>(false);

	constructor(parentViewModel: DispatchDetailsViewModel) {
		super(parentViewModel);
		this.parentViewModel = parentViewModel;
		this.dispatch = parentViewModel.dispatch;
		this.bookmarks([]);
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
			Name: window.Helper.String.getTranslatedString("ThisDispatch"),
			Key: "ThisDispatch",
			Expression: (query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderMaterial>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderMaterial> => {
				return query.filter(function(it) { return (it.DispatchId === null && it.ServiceOrderMaterialType === window.Crm.Service.ServiceOrderMaterialType.Preplanned) || it.DispatchId === this.dispatchId; }, { dispatchId: this.dispatch().Id() });
			}
		});
		this.bookmark(this.bookmarks()[1]);
		this.showServiceOrderMaterialType = ko.observable(false);
	}

	applyOrderBy(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderMaterial>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderMaterial> {
		let id = null;
		if (this.dispatch().CurrentServiceOrderTimeId()) {
			id = this.dispatch().CurrentServiceOrderTimeId();
		}
		// @ts-ignore
		query = query.orderByDescending("orderByCurrentServiceOrderTime", {currentServiceOrderTimeId: id});
		return super.applyOrderBy(query);
	};

	getActualQuantity(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): number {
		if (serviceOrderMaterial.ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Used && serviceOrderMaterial.DispatchId() === this.dispatch().Id()) {
			return serviceOrderMaterial.Quantity();
		}

		return 0;
	}

	getActualQuantityOfOtherDispatches(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): number {
		if (serviceOrderMaterial.ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Preplanned) {
			let usedPositions = serviceOrderMaterial.ChildServiceOrderMaterials().filter(x => x.ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Used && x.DispatchId() !== this.dispatch().Id());
			return usedPositions.reduce((result, position) => {
				return result + position.Quantity();
			}, 0);
		}

		if (serviceOrderMaterial.ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Used && serviceOrderMaterial.DispatchId() !== this.dispatch().Id()) {
			return serviceOrderMaterial.Quantity();
		}

		if (serviceOrderMaterial.ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Used && serviceOrderMaterial.DispatchId() === this.dispatch().Id() && serviceOrderMaterial.ParentServiceOrderMaterialId() !== null) {
			return this.getActualQuantityOfOtherDispatches(serviceOrderMaterial.ParentServiceOrderMaterial());
		}

		return 0;
	}
	
	getEstimatedQuantity(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): number {
		if (serviceOrderMaterial.ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Preplanned) {
			return serviceOrderMaterial.Quantity();
		}

		if (serviceOrderMaterial.ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Used && serviceOrderMaterial.ParentServiceOrderMaterial()?.ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Preplanned) {
			return serviceOrderMaterial.ParentServiceOrderMaterial().Quantity();
		}

		return 0;
	}
	
	getInvoiceQuantity(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): number {
		return 0;
	}
	
	getItemGroup = window.Crm.Service
		.ViewModels.DispatchDetailsViewModel.prototype.getServiceOrderPositionItemGroup;

	async init(): Promise<void> {
		let canEditActualQuantities = await window.Helper.ServiceOrder.canEditActualQuantities(this.serviceOrder().Id());
		this.canEditActualQuantities(canEditActualQuantities);
		await super.init()

		let result = await window.Helper.ServiceOrderMaterial.getValidItemNosAfterSignature();
		this.validMaterialItemNosAfterCustomerSignature(result.validMaterialItemNosAfterCustomerSignature);
		this.validCostItemNosAfterCustomerSignature(result.validCostItemNosAfterCustomerSignature);

		let editableItemNos = await window.Helper.ServiceOrderMaterial.getEditableItemNosAfterSignature();
		this.editableMaterialItemNosAfterCustomerSignature(editableItemNos.editableMaterialItemNosAfterCustomerSignature);
		this.editableCostItemNosAfterCustomerSignature(editableItemNos.editableCostItemNosAfterCustomerSignature);
	};

	isVisible(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): boolean {
		if (serviceOrderMaterial.ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Used && (serviceOrderMaterial.DispatchId() === this.dispatch().Id() || serviceOrderMaterial.ParentServiceOrderMaterialId() === null)) {
			return true;
		}

		let hasUsedPosition = serviceOrderMaterial.ChildServiceOrderMaterials().find(x => x.DispatchId() === this.dispatch().Id() && x.ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Used);
		if (serviceOrderMaterial.ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Preplanned && !hasUsedPosition) {
			return true;
		}

		return false;
	}

	canEditMaterial(material: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): boolean {
		return (HelperDispatch.dispatchIsEditable(this.dispatch()) ||
			(["SignedByCustomer", "ClosedNotComplete", "ClosedComplete"].indexOf(this.dispatch().StatusKey()) !== -1 &&
				(this.editableMaterialItemNosAfterCustomerSignature().indexOf(material.ItemNo()) !== -1 || this.editableCostItemNosAfterCustomerSignature().indexOf(material.ItemNo()) !== -1)) ||
				(["SignedByCustomer", "ClosedNotComplete", "ClosedComplete"].indexOf(this.dispatch().StatusKey()) !== -1 &&
					(this.validMaterialItemNosAfterCustomerSignature().indexOf(material.ItemNo()) !== -1 || this.validCostItemNosAfterCustomerSignature().indexOf(material.ItemNo()) !== -1)))
			&& (Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode == "OrderPerInstallation" || !Crm.Service.Settings.Dispatch.RestrictEditingToActiveJob || (material.ServiceOrderTimeId() && material.ServiceOrderTimeId() == this.dispatch().CurrentServiceOrderTimeId()));
	};

	canReportScheduledMaterial(): boolean {
		const hasPermission = window.AuthorizationManager.isAuthorizedForAction("ServiceOrder", "EditMaterial");
		let predefinedMaterialNos = this.items().filter(i => {
			return !!i.ServiceOrderTimeId() && i.ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Preplanned;
		}).map(i => i.ItemNo());
		predefinedMaterialNos = predefinedMaterialNos.filter((value, index, array) => array.indexOf(value) === index);
		let editableAfterSignature = false;
		if (this.dispatch().StatusKey() === "SignedByCustomer") {
			for (const itemNo of predefinedMaterialNos) {
				if (this.validMaterialItemNosAfterCustomerSignature().indexOf(itemNo) !== -1 || this.validCostItemNosAfterCustomerSignature().indexOf(itemNo) !== -1) {
					editableAfterSignature = true;
					break;
				}
			}
		}
		return hasPermission && this.canEditActualQuantities() && (HelperDispatch.dispatchIsEditable(this.dispatch()) || editableAfterSignature);
	};

	showActualQuantityOfOtherDispatches(): boolean {
		return this.bookmark().Key === "All";
	}
}

namespace("Crm.Service.ViewModels").DispatchDetailsMaterialsTabViewModel = DispatchDetailsMaterialsTabViewModel;