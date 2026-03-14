import type {DispatchDetailsViewModel} from "@Crm.Service/DispatchDetailsViewModel";
import {namespace} from "@Main/namespace";

class DispatchCancelModalViewModel extends window.Main.ViewModels.ViewModelBase {

	dispatch = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch>(null);
	errors = ko.validation.group(this.dispatch, {deep: true});
	parentViewModel: DispatchDetailsViewModel;
	displayRequiredOperationsInput = ko.pureComputed<boolean>(() => {
		return this.dispatch() !== null && this.dispatch().StatusKey() === "CancelledNotComplete";
	});


	lookups: LookupType = {
		serviceOrderDispatchStatuses: {},
		serviceOrderDispatchCancellationReasons: {}
	};

	constructor(parentViewModel: DispatchDetailsViewModel) {
		super();
		this.parentViewModel = parentViewModel
	}

	async init(id?: string) {
		const dispatch = await window.database.CrmService_ServiceOrderDispatch
			.include("ServiceOrder")
			.include("ServiceOrder.Installation")
			.include("ServiceOrderMaterial")
			.include("ServiceOrderTimePostings")
			.find(id);
		dispatch.StatusKey = null;
		window.database.attachOrGet(dispatch);
		this.dispatch(dispatch.asKoObservable());
		this.lookups.serviceOrderDispatchStatuses = await window.Helper.Lookup
			.getLocalizedArrayMap("CrmService_ServiceOrderDispatchStatus",
				null,
				"it.SortOrder > 8 && it.SortOrder < 11;");

		this.lookups.serviceOrderDispatchCancellationReasons = await window.Helper.Lookup.getLocalizedArrayMap("CrmService_ServiceOrderDispatchCancellationReason");
		this.lookups.serviceOrderDispatchCancellationReasons.$array = this.lookups.serviceOrderDispatchCancellationReasons.$array.filter(lookup => lookup.Key != null)
		this.initValidationRules(this.dispatch());
	}

	initValidationRules(dispatch: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch) {
		dispatch.CancellationReasonKey.extend({
			required: {
				message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("CancellationReason")),
				params: true
			}
		})
	}

	dispose(): void {
		window.database.detach(this.dispatch().innerInstance);
	};

	refreshParentViewModel(): void {
		if (this.parentViewModel instanceof window.Crm.Service.ViewModels.DispatchDetailsViewModel) {
			this.parentViewModel.init(this.dispatch().Id());
		}
	};

	async save() {
		this.loading(true);
		if (this.errors().length > 0) {
			this.errors.showAllMessages();
			this.loading(false);
			return;
		}
		try {
			await window.database.saveChanges(); // this also triggers `ServiceOrderDispatchChangedEventHandler` which also triggers sending of cancel notification e-mail
			const dispatchStatus = this.lookups.serviceOrderDispatchStatuses[this.dispatch().StatusKey()].Value;
			const cancellationReason = this.lookups.serviceOrderDispatchCancellationReasons[this.dispatch().CancellationReasonKey()].Value
			const attentionTaskType = (await window.Helper.Lookup.getLookupByKeyQuery("Crm_TaskType", window.Main.Settings.Task.AttentionTaskTypeKey).toArray())[0];

			if (attentionTaskType) {
				const newTask = window.database.Crm_Task.defaultType.create();
				newTask.TaskCreateUser = window.Helper.User.getCurrentUserName();
				newTask.Text = `${window.Helper.getTranslatedString("DispatchStatusSetTo").replace("{0}", dispatchStatus)} ${window.Helper.getTranslatedString("Reason")}: ${cancellationReason}.`;
				newTask.ContactId = this.dispatch().OrderId();
				newTask.TypeKey = attentionTaskType.Key;
				newTask.DueDate = new Date();
				newTask.ResponsibleUser = this.dispatch().CreateUser();
				window.database.add(newTask);
				await window.database.saveChanges();
			}
			
			this.loading(false);
			this.refreshParentViewModel();
			const parent = this.parentViewModel as any;
			if (parent?.loadServiceOrder && parent?.serviceOrder) {
				this.loading(true);
				await parent.loadServiceOrder(parent.serviceOrder().Id());
				this.loading(false);
			}
			$(".modal:visible").modal("hide");
		} catch (e) {
			this.loading(false);
			window.swal(window.Helper.String.getTranslatedString("UnknownError"),
				window.Helper.String.getTranslatedString("Error_InternalServerError"),
				"error");
		}
	}

}

namespace("Crm.Service.ViewModels").DispatchCancelModalViewModel = DispatchCancelModalViewModel;
