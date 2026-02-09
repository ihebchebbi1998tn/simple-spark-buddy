import {namespace} from "@Main/namespace";
import type {DispatchDetailsViewModel} from "./DispatchDetailsViewModel";
import type {DispatchValidatorError} from "@Crm.Service/DispatchChangeStatusModalViewModel";

export class DispatchSignatureEditModalViewModel extends window.Main.ViewModels.ViewModelBase {
	dispatch = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch>(null);
	dispatchValidators = ko.observableArray<(DispatchSignatureEditModalViewModel) => Promise<DispatchValidatorError>>([]);
	dispatchValidatorErrors = ko.observableArray<DispatchValidatorError>([]);
	site = ko.observable<Main.Rest.Model.Main_Site>(null);
	lookups: LookupType = {salutations: {$tableName: "Crm_Salutation"}};

	checkForEmptyTimesOrMaterials = ko.observable<boolean>(false);
	emptyTimesOrMaterialsMessage = ko.observable<string>(null);
	customerPersons = ko.observableArray<Crm.Rest.Model.Crm_Person>([]);
	originatorPersons = ko.observableArray<Crm.Rest.Model.Crm_Person>([]);
	selectableCustomerPersons = ko.pureComputed<{ DisplayName: string, Value: string }[]>(() => {
		const selectablePersons = this.customerPersons().map(person => {
			const displayName = window.Helper.Person.getDisplayNameWithSalutation(person, this.lookups.salutations as {[key:string]:Crm.Rest.Model.Lookups.Crm_Salutation}, window.Crm.Service.Settings.ServiceOrderDispatch.CustomerSignatureIncludesLegacyId);
			return {DisplayName: displayName, Value: displayName};
		});
		if (selectablePersons.length > 0) {
			selectablePersons.push({
				DisplayName: window.Helper.String.getTranslatedString("EnterCustomName"),
				Value: ""
			});
			selectablePersons.unshift({
				DisplayName: window.Helper.String.getTranslatedString("PleaseSelectContact"),
				Value: null
			});
		}
		return selectablePersons;
	});
	selectableOriginatorPersons = ko.pureComputed<{ DisplayName: string, Value: string }[]>(() => {
		const selectablePersons = this.originatorPersons().map(person => {
			const displayName = window.Helper.Person.getDisplayNameWithSalutation(person, this.lookups.salutations as {[key:string]:Crm.Rest.Model.Lookups.Crm_Salutation});
			return {DisplayName: displayName, Value: displayName};
		});
		if (selectablePersons.length > 0) {
			selectablePersons.push({
				DisplayName: window.Helper.String.getTranslatedString("EnterCustomName"),
				Value: ""
			});
			selectablePersons.unshift({
				DisplayName: window.Helper.String.getTranslatedString("PleaseSelectContact"),
				Value: null
			});
		}
		return selectablePersons;
	});
	displayCustomCustomerNameInput = ko.pureComputed<boolean>(() => {
		if (!this.dispatch()) {
			return false;
		}
		return this.dispatch().SignatureContactName() === "" || this.selectableCustomerPersons().length === 0 || (this.dispatch().SignatureContactName() && !this.selectableCustomerPersons().find(x => x.Value === this.dispatch().SignatureContactName()));
	});
	displayCustomOriginatorNameInput = ko.pureComputed<boolean>(() => {
		if (!this.dispatch()) {
			return false;
		}
		return this.dispatch().SignatureOriginatorName() === "" || this.selectableOriginatorPersons().length === 0 || (this.dispatch().SignatureOriginatorName() && !this.selectableOriginatorPersons().find(x => x.Value === this.dispatch().SignatureOriginatorName()));
	});

	errors = window.ko.validation.group(this.dispatch, {deep: true});
	parentViewModel: DispatchDetailsViewModel;

	async refreshParentViewModel(): Promise<void> {
		await this.parentViewModel.init(this.dispatch().Id());
	};

	emptyMaterials = ko.pureComputed<boolean>(() => this.dispatch().ServiceOrderMaterial().length === 0);
	emptyTimes = ko.pureComputed<boolean>(() => this.dispatch().ServiceOrderTimePostings().length === 0);
	emptyTimesOrMaterialsWarning = window.Crm.Service.Settings?.Service.Dispatch.Show.EmptyTimesOrMaterialsWarning;
	showError = ko.observable<boolean>(false);

	constructor(parentViewModel: DispatchDetailsViewModel) {
		super();
		this.parentViewModel = parentViewModel;
	}

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		let dispatch = await window.database.CrmService_ServiceOrderDispatch
			.include("ServiceOrder")
			.include("ServiceOrderTimePostings")
			.include("ServiceOrderMaterial")
			.find(id);
		window.database.attachOrGet(dispatch);
		this.dispatch(dispatch.asKoObservable());
		this.dispatch().SignatureJson.subscribe(value => {
			if (!value) {
				this.dispatch().SignatureContactName(null);
				this.dispatch().SignPrivacyPolicyAccepted(false);
				this.dispatch().StatusKey("InProgress");
			}
		});
		this.dispatch().SignatureOriginatorJson.subscribe(value => {
			if (!value) {
				this.dispatch().SignatureOriginatorName(null);
			}
		});
		if (this.dispatch().ServiceOrder().CustomerContactId()) {
			let company = await window.database.Crm_Company
				.include2("Staff.filter(function(x) { return x.IsRetired === false })")
				.find(this.dispatch().ServiceOrder().CustomerContactId());
			this.customerPersons(company.Staff);
		}
		if (this.dispatch().ServiceOrder().InitiatorId()) {
			let company = await window.database.Crm_Company
				.include2("Staff.filter(function(x) { return x.IsRetired === false })")
				.find(this.dispatch().ServiceOrder().InitiatorId());
			this.originatorPersons(company.Staff);
		}
		let site = await window.database.Main_Site.GetCurrentSite().first();
		this.site(site);
		const dispatchValidatorErrors = [];
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
			this.errors.switchToError();
		}
	};

	dispose(): void {
		window.database.detach(this.dispatch().innerInstance);
	};

	async save(): Promise<void> {
		this.loading(true);

		const dispatchValidatorErrors = [];
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
			this.errors.switchToError();
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

		if (this.dispatch().StatusKey() === "InProgress" && (this.dispatch().SignatureJson() || '').length > 0) {
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
		}

		if (this.dispatch().SignatureJson()) {
			this.dispatch().StatusKey("SignedByCustomer");
		}

		try {
			await window.database.saveChanges();
			await this.refreshParentViewModel();
			this.loading(false);
			$(".modal:visible").modal("hide");
		} catch {
			this.loading(false);
			window.swal(window.Helper.String.getTranslatedString("UnknownError"),
				window.Helper.String.getTranslatedString("Error_InternalServerError"),
				"error");
		}
	};

	toggleCustomerContactSelector(): void {
		this.dispatch().SignatureContactName(null);
	}

	toggleOriginatorContactSelector() {
		this.dispatch().SignatureOriginatorName(null);
	}
}

namespace("Crm.Service.ViewModels").DispatchSignatureEditModalViewModel = DispatchSignatureEditModalViewModel;