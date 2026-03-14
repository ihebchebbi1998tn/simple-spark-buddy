import { namespace } from "@Main/namespace";
import type { DispatchDetailsViewModel } from "@Crm.Service/DispatchDetailsViewModel";
import { ServiceOrderDetailsErrorTabViewModelBase } from "@Crm.Service/ServiceOrderDetailsErrorTabViewModel";
import type {PmbbViewModel} from "@Main/PmbbViewModel";
import {HelperDispatch} from "@Crm.Service/helper/Helper.Dispatch";
export class DispatchDetailsErrorTabViewModel extends ServiceOrderDetailsErrorTabViewModelBase<DispatchDetailsViewModel> {

	dispatch: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch>;
	currentJobItemGroup = ko.pureComputed<ItemGroup>(() => window.Helper.Dispatch.getCurrentJobItemGroup(this));
	parentViewModel: DispatchDetailsViewModel;
	errorTypesCanBeAdded = ko.pureComputed<boolean>(() => {
		return HelperDispatch.dispatchIsEditable(this.dispatch());
	});
	errorCausesCanBeAdded = ko.pureComputed<boolean>(() => {
		return HelperDispatch.dispatchIsEditable(this.dispatch());
	});

	constructor(parentViewModel: DispatchDetailsViewModel) {
		super(parentViewModel);
		this.parentViewModel = parentViewModel;
		this.dispatch = parentViewModel.dispatch;
	}

	applyFilters(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderErrorType>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderErrorType> {
		query = super.applyFilters(query);
		query = query.filter("it.DispatchId === this.dispatchId || it.DispatchId == null", { dispatchId: this.dispatch().Id() });
		return query;
	};

	applyOrderBy(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderErrorType>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderErrorType> {
		let id = null;
		if (this.dispatch() && this.dispatch().CurrentServiceOrderTimeId()) {
			id = this.dispatch().CurrentServiceOrderTimeId();
		}
		// @ts-ignore
		query = query.orderByDescending("orderByCurrentServiceOrderTime", { currentServiceOrderTimeId: id });
		return super.applyOrderBy(query);
	};
	
	getItemGroup = window.Crm.Service
		.ViewModels.DispatchDetailsViewModel.prototype.getServiceOrderPositionItemGroup;

	async init(): Promise<void> {
		await super.init();
	};

	errorTypeCanBeEdited(serviceOrderErrorType: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderErrorType): boolean {
		return HelperDispatch.dispatchIsEditable(this.dispatch()) &&
			(Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode == "OrderPerInstallation" || !Crm.Service.Settings.Dispatch.RestrictEditingToActiveJob || (serviceOrderErrorType.ServiceOrderTimeId() && serviceOrderErrorType.ServiceOrderTimeId() == this.dispatch().CurrentServiceOrderTimeId()));
	};

	errorTypeCanBeDeleted(serviceOrderErrorType: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderErrorType): boolean {
		const editable = this.errorTypeCanBeEdited(serviceOrderErrorType);
		if (editable) {
			return window.AuthorizationManager.currentUserHasPermission("ServiceOrderErrorType::Edit");
		}
		return editable;
	};

	errorCauseCanBeEdited(serviceOrderErrorCause: Crm.Service.Rest.Model.CrmService_ServiceOrderErrorCause): boolean {
		return HelperDispatch.dispatchIsEditable(this.dispatch());
	};

	errorCauseCanBeDeleted(serviceOrderErrorCause: Crm.Service.Rest.Model.CrmService_ServiceOrderErrorCause): boolean {
		const editable = this.errorCauseCanBeEdited(serviceOrderErrorCause);
		if (editable) {
			return window.AuthorizationManager.currentUserHasPermission("ServiceOrderErrorCause::Edit");
		}
		return editable;
	};

	getDisplayedServiceOrderErrorCauses(serviceOrderErrorType: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderErrorType): Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderErrorCause[] {
		let serviceOrderErrorCauses: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderErrorCause[] = [];
		if (serviceOrderErrorType.ParentServiceOrderErrorTypeId()) {
			serviceOrderErrorCauses = this.items().find(x => x.Id() === serviceOrderErrorType.ParentServiceOrderErrorTypeId())?.ServiceOrderErrorCauses() ?? [];
		}
		let errorCauses: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderErrorCause[] = serviceOrderErrorType.ServiceOrderErrorCauses().concat(serviceOrderErrorCauses);
		errorCauses = errorCauses.filter(errorCause => errorCause.ParentServiceOrderErrorCauseId() !== null || !errorCauses.some(x => x.ParentServiceOrderErrorCauseId() === errorCause.Id()));
		return errorCauses;
	}

	isVisible(serviceOrderErrorType: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderErrorType): boolean {
		return serviceOrderErrorType.ParentServiceOrderErrorTypeId() !== null || !this.items().some(x => x.ParentServiceOrderErrorTypeId() === serviceOrderErrorType.Id());
	}
	
	async onSaveErrorCause(context: PmbbViewModel): Promise<void> {
		let serviceOrderErrorCause: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderErrorCause = context.editContext().errorCause()
		let serviceOrderErrorType: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderErrorType = context.editContext().errorType()
		if (serviceOrderErrorType.DispatchId() === null){
			window.database.detach(serviceOrderErrorType.innerInstance);
			let newServiceOrderErrorType = window.Helper.Database.createClone(serviceOrderErrorType.innerInstance);
			newServiceOrderErrorType.Id = window.$data.createGuid().toString().toLowerCase()
			newServiceOrderErrorType.DispatchId = this.dispatch().Id();
			newServiceOrderErrorType.ParentServiceOrderErrorTypeId = serviceOrderErrorType.Id();
			window.database.add(newServiceOrderErrorType);
			serviceOrderErrorType = newServiceOrderErrorType.asKoObservable();
			context.viewContext.errorType = serviceOrderErrorType;
		}
		if (serviceOrderErrorCause.DispatchId() === null){
			window.database.detach(serviceOrderErrorCause.innerInstance);
			let newServiceOrderErrorCause = window.Helper.Database.createClone(serviceOrderErrorCause.innerInstance);
			newServiceOrderErrorCause.Id = window.$data.createGuid().toString().toLowerCase()
			newServiceOrderErrorCause.DispatchId = this.dispatch().Id();
			newServiceOrderErrorCause.ParentServiceOrderErrorCauseId = serviceOrderErrorCause.Id();
			newServiceOrderErrorCause.ServiceOrderErrorTypeId = serviceOrderErrorType.Id();
			window.database.add(newServiceOrderErrorCause);
			context.viewContext.errorCause = newServiceOrderErrorCause.asKoObservable();
		}
	}
	
	async onSaveErrorType(context: PmbbViewModel): Promise<void> {
		let serviceOrderErrorType: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderErrorType = context.editContext().errorType()
		if (serviceOrderErrorType.DispatchId() === null){
			window.database.detach(serviceOrderErrorType.innerInstance);
			let newServiceOrderErrorType = window.Helper.Database.createClone(serviceOrderErrorType.innerInstance);
			newServiceOrderErrorType.Id = window.$data.createGuid().toString().toLowerCase()
			newServiceOrderErrorType.DispatchId = this.dispatch().Id();
			newServiceOrderErrorType.ParentServiceOrderErrorTypeId = serviceOrderErrorType.Id();
			window.database.add(newServiceOrderErrorType);
			context.viewContext.errorType = newServiceOrderErrorType.asKoObservable();
		}
	}
}

namespace("Crm.Service.ViewModels").DispatchDetailsErrorTabViewModel = DispatchDetailsErrorTabViewModel;