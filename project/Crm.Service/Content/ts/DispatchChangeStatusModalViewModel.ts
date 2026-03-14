import {namespace} from "@Main/namespace";
import type {DispatchDetailsViewModel} from "./DispatchDetailsViewModel";

export class DispatchValidatorError {
	links: {
		condition: () => boolean;
		routeValues: {};
		text: string;
		url: string;
	}[];
	message: string;
}

export class DispatchChangeStatusModalViewModel extends window.Main.ViewModels.ViewModelBase {
	dispatch = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch>(null);
	dispatchValidators = ko.observableArray<(viewModel: DispatchChangeStatusModalViewModel) => Promise<DispatchValidatorError>>([]);
	dispatchValidatorErrors = ko.observableArray<DispatchValidatorError>([]);
	previousDispatchStatusKey = ko.observable<string>(null);
	checkForEmptyTimesOrMaterials = ko.observable<boolean>(false);
	emptyTimesOrMaterialsMessage = ko.observable<string>(null);

	lookups: LookupType = {
		installationHeadStatuses: {},
		serviceOrderDispatchStatuses: {}
	};
	displayRequiredOperationsInput = ko.pureComputed<boolean>(() => {
		return this.dispatch() !== null && this.dispatch().StatusKey() === "ClosedNotComplete";
	});
	formDisabled = ko.pureComputed<boolean>(() => {
		return this.dispatchValidatorErrors().length > 0;
	});
	errors = ko.validation.group(this.dispatch, {deep: true});
	parentViewModel: DispatchDetailsViewModel;

	constructor(parentViewModel: DispatchDetailsViewModel) {
		super();
		this.parentViewModel = parentViewModel;
	}

	refreshParentViewModel(): void {
		this.parentViewModel.init(this.dispatch().Id());
	};

	emptyMaterials = ko.pureComputed<boolean>(() => {
		return this.dispatch().ServiceOrderMaterial().length === 0;
	});
	emptyTimes = ko.pureComputed<boolean>(() => {
		return this.dispatch().ServiceOrderTimePostings().length === 0;
	});
	emptyTimesOrMaterialsWarning = window.Crm.Service.Settings?.Service.Dispatch.Show.EmptyTimesOrMaterialsWarning;
	showError = ko.observable<boolean>(false);

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		let dispatch = await window.database.CrmService_ServiceOrderDispatch
			.include("ServiceOrder")
			.include("ServiceOrder.Installation")
			.include("ServiceOrderMaterial")
			.include("ServiceOrderTimePostings")
			.find(id);
		this.previousDispatchStatusKey(dispatch.StatusKey);
		dispatch.StatusKey = null;
		window.database.attachOrGet(dispatch);
		this.dispatch(dispatch.asKoObservable());
		this.dispatch().FollowUpServiceOrder.subscribe(() => {
			this.dispatch().FollowUpServiceOrderRemark(null);
		});
		this.dispatch().StatusKey.subscribe(statusKey => {
			const serviceOrderStatusKey = statusKey === "ClosedNotComplete" ? "PartiallyCompleted" : "Completed";
			this.dispatch().ServiceOrder().StatusKey(serviceOrderStatusKey);
			this.dispatch().RequiredOperations(null);
		});
		this.lookups.installationHeadStatuses = await window.Helper.Lookup.getLocalizedArrayMap("CrmService_InstallationHeadStatus");

		this.lookups.serviceOrderDispatchStatuses = await window.Helper.Lookup
			.getLocalizedArrayMap("CrmService_ServiceOrderDispatchStatus",
				null,
				"it.SortOrder > 5 && it.SortOrder < 8;");

		const dispatchValidatorErrors = [];
		for (const dispatchValidator of this.dispatchValidators()) {
			let error = await dispatchValidator(this);
			if (error) {
				dispatchValidatorErrors.push(error);
			}
		}
		this.dispatchValidatorErrors(dispatchValidatorErrors);
		this.dispatchValidatorErrors.valueHasMutated();
	};

	dispose(): void {
		window.database.detach(this.dispatch().innerInstance);
	};

	async save(): Promise<void> {
		this.loading(true);
		this.dispatch().CloseDate(new Date());
		const dispatchValidatorErrors: DispatchValidatorError[] = [];
		for (const dispatchValidator of this.dispatchValidators()) {
			let error = await dispatchValidator(this);
			if (error) {
				dispatchValidatorErrors.push(error);
			}
		}
		this.dispatchValidatorErrors(dispatchValidatorErrors);
		if (this.errors().length > 0 || this.dispatchValidatorErrors().length > 0) {
			this.errors.showAllMessages();
			this.loading(false);
			return;
		}

		let addMessages = () => {
			if (this.emptyMaterials() && this.emptyTimes()) {
				this.emptyTimesOrMaterialsMessage(window.Helper.String.getTranslatedString("EmptyTimesAndMaterials"));
			} else {
				if (this.emptyMaterials()) {
					this.emptyTimesOrMaterialsMessage(window.Helper.String.getTranslatedString("EmptyMaterials"));
				} else if (this.emptyTimes()) {
					this.emptyTimesOrMaterialsMessage(window.Helper.String.getTranslatedString("EmptyTimes"));
				}
			}
			this.checkForEmptyTimesOrMaterials(true);
		}

		if ((this.emptyTimesOrMaterialsWarning === "WARN" && !this.checkForEmptyTimesOrMaterials() && (this.emptyMaterials() || this.emptyTimes()))) {
			this.loading(false);
			this.showError(false);
			addMessages();
			return;
		}
		if (this.emptyTimesOrMaterialsWarning === "ERROR") {
			if ((this.emptyMaterials() && this.emptyTimes())) {
				this.loading(false);
				this.showError(true);
				addMessages(); 
				return;
			} else if (!(!this.emptyMaterials() && !this.emptyTimes()) && !this.checkForEmptyTimesOrMaterials()) {
				this.loading(false);
				this.showError(false);
				addMessages();
				return;
			}
		}

		try {
			await window.database.saveChanges();
			this.loading(false);
			this.refreshParentViewModel();
			$(".modal:visible").modal("hide");
		} catch (e) {
			this.loading(false);
			window.swal(window.Helper.String.getTranslatedString("UnknownError"),
				window.Helper.String.getTranslatedString("Error_InternalServerError"),
				"error");
		}
	};
}

namespace("Crm.Service.ViewModels").DispatchChangeStatusModalViewModel = DispatchChangeStatusModalViewModel;