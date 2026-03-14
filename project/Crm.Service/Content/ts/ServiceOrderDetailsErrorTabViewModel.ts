import { namespace } from "@Main/namespace";
import type { ServiceOrderDetailsViewModel } from "@Crm.Service/ServiceOrderDetailsViewModel";
import type { ServiceOrderTemplateDetailsViewModel } from "@Crm.Service/ServiceOrderTemplateDetailsViewModel";
import type { DispatchDetailsViewModel } from "@Crm.Service/DispatchDetailsViewModel";
import type { ServiceCaseDetailsViewModel } from "@Crm.Service/ServiceCaseDetailsViewModel";
import type {PmbbViewModel} from "@Main/PmbbViewModel";

export class ServiceOrderDetailsErrorTabViewModelBase<T extends ServiceOrderDetailsViewModel | ServiceOrderTemplateDetailsViewModel | DispatchDetailsViewModel | ServiceCaseDetailsViewModel> extends window.Main.ViewModels.GenericListViewModel<Crm.Service.Rest.Model.CrmService_ServiceOrderErrorType, Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderErrorType> {
	serviceOrder: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead>;
	parentViewModel: T;
	errorTypesCanBeAdded = ko.pureComputed<boolean>(() => {
		if (this.parentViewModel instanceof window.Crm.Service.ViewModels.ServiceCaseDetailsViewModel) {
			return this.parentViewModel.isEditable();
		} else {
			return this.parentViewModel.serviceOrderIsEditable();
		}
	});

	errorCausesCanBeAdded = ko.pureComputed<boolean>(() => {
		if (this.parentViewModel instanceof window.Crm.Service.ViewModels.ServiceCaseDetailsViewModel) {
			return this.parentViewModel.isEditable();
		} else {
			return this.parentViewModel.serviceOrderIsEditable();
		}
	});

	constructor(parentViewModel: T) {
		super("CrmService_ServiceOrderErrorType",
			["IsMainErrorType"],
			["DESC"],
			["ServiceOrderErrorCauses", "ServiceOrderTime", "ServiceOrderTime.Installation"]);
		this.parentViewModel = parentViewModel;
		this.lookups = parentViewModel.lookups || {};

		this.lookups.statisticsKeyFaultImages = this.lookups.statisticsKeyFaultImages || { $tableName: "CrmService_StatisticsKeyFaultImage" };
		this.lookups.statisticsKeyCauses = this.lookups.statisticsKeyCauses || { $tableName: "CrmService_StatisticsKeyCause" };
		this.serviceOrder = parentViewModel.serviceOrder ? parentViewModel.serviceOrder : null;
		this.infiniteScroll(true);
	}


	async init(): Promise<void> {
		await super.init();
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
	};

	applyFilters(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderErrorType>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderErrorType> {
		query = super.applyFilters(query);
		if (this.serviceOrder()) {
			query = query.filter("it.OrderId === this.orderId", { orderId: this.serviceOrder().Id() });
		}
		return query;
	};

	async deleteServiceOrderErrorType(serviceOrderErrorType: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderErrorType): Promise<void> {
		let confirm = await window.Helper.Confirm.confirmDeleteAsync();
		if (!confirm) {
			return;
		}
		this.loading(true);
		// remove all service order error type related error causes
		for (const errorCause of Object.values(serviceOrderErrorType.ServiceOrderErrorCauses())) {
			window.database.remove(errorCause);
		}

		window.database.remove(serviceOrderErrorType);
		await window.database.saveChanges();
		this.filter();
	};


	async deleteServiceOrderErrorCause(serviceOrderErrorCause: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderErrorCause): Promise<void> {
		let confirm = await window.Helper.Confirm.confirmDeleteAsync();
		if (!confirm) {
			return;
		}
		this.loading(true);

		window.database.remove(serviceOrderErrorCause);
		await window.database.saveChanges();
		this.filter();
	};

	errorTypeCanBeEdited(serviceOrderErrorType: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderErrorType): boolean {
		if (this.parentViewModel instanceof window.Crm.Service.ViewModels.ServiceCaseDetailsViewModel) {
			return this.parentViewModel.isEditable();
		} else {
			const editable = this.parentViewModel.serviceOrderIsEditable();
			if (editable) {
				return window.AuthorizationManager.currentUserHasPermission("ServiceOrderErrorType::Edit");
			}
			return editable;
		}
	};

	errorTypeCanBeDeleted(serviceOrderErrorType: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderErrorType): boolean {

		if (this.parentViewModel instanceof window.Crm.Service.ViewModels.ServiceCaseDetailsViewModel) {
			return this.parentViewModel.isEditable();
		} else {
			const editable = this.parentViewModel.serviceOrderIsEditable();
			if (editable) {
				return window.AuthorizationManager.currentUserHasPermission("ServiceOrderErrorType::Delete");
			}
			return editable;
		}
	};

	errorCauseCanBeEdited(serviceOrderErrorCause: Crm.Service.Rest.Model.CrmService_ServiceOrderErrorCause): boolean {
		if (this.parentViewModel instanceof window.Crm.Service.ViewModels.ServiceCaseDetailsViewModel) {
			return this.parentViewModel.isEditable();
		} else {
			const editable = this.parentViewModel.serviceOrderIsEditable();
			if (editable) {
				return window.AuthorizationManager.currentUserHasPermission("ServiceOrderErrorCause::Edit");
			}
			return editable;
		}
	};

	errorCauseCanBeDeleted(serviceOrderErrorCause: Crm.Service.Rest.Model.CrmService_ServiceOrderErrorCause): boolean {

		if (this.parentViewModel instanceof window.Crm.Service.ViewModels.ServiceCaseDetailsViewModel) {
			return this.parentViewModel.isEditable();
		} else {
			const editable = this.parentViewModel.serviceOrderIsEditable();
			if (editable) {
				return window.AuthorizationManager.currentUserHasPermission("ServiceOrderErrorCause::Delete");
			}
			return editable;
		}
	};

	getDisplayedServiceOrderErrorCauses(serviceOrderErrorType: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderErrorType): Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderErrorCause[] {
		return serviceOrderErrorType.ServiceOrderErrorCauses();
	}
	
	getItemGroup(serviceOrderErrorType: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderErrorType): ItemGroup {
		return window.Helper.ServiceOrder.getServiceOrderPositionItemGroup(serviceOrderErrorType);
	};


	isVisible(serviceOrderErrorType: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderErrorType): boolean {
		return true;
	}

	async onSaveErrorCause(context: PmbbViewModel): Promise<void> {
	}

	async onSaveErrorType(context: PmbbViewModel): Promise<void> {
	}
}


export class ServiceOrderDetailsErrorTabViewModel extends ServiceOrderDetailsErrorTabViewModelBase<ServiceOrderDetailsViewModel>{

}

namespace("Crm.Service.ViewModels").ServiceOrderDetailsErrorTabViewModel = ServiceOrderDetailsErrorTabViewModel;