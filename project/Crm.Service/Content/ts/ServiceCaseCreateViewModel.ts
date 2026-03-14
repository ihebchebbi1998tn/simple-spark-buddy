import {namespace} from "@Main/namespace";
import type {VisibilityViewModel} from "@Main/VisibilityViewModel";
import { Breadcrumb } from "@Main/breadcrumbs";
export class ServiceCaseCreateViewModel extends window.Main.ViewModels.ViewModelBase {
	affectedInstallation = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_Installation>(null);
	affectedInstallationStatusKey = ko.pureComputed<KnockoutObservable<string>>(() => {
		return this.affectedInstallation() ? this.affectedInstallation().StatusKey : null;
	});
	contactPerson = ko.observable<Crm.Rest.Model.Crm_Person>(null);
	numberingSequenceName = "SMS.ServiceCase";
	serviceCase = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceCase>(null);
	serviceCaseTemplate = ko.observable<Crm.Service.Rest.Model.CrmService_ServiceCaseTemplate>(null);
	currentUser = ko.observable<Main.Rest.Model.Main_User>(null);
	visibilityViewModel: VisibilityViewModel = new window.VisibilityViewModel(this.serviceCase, "ServiceCase");
	errors = ko.validation.group([this.affectedInstallation, this.serviceCase], {deep: true});
	installationServiceCasesEnabled: boolean = window.Crm.Service.Settings.ServiceCase.ShowsRelatedServiceCasesOnInstallationSelection;
	items = ko.observableArray<Crm.Service.Rest.Model.ObservableCrmService_ServiceCase>([]);
	loadingRelatedServiceCases = ko.observable<boolean>(false);
	canShowMoreRelatedServiceCases = ko.observable<boolean>(true);
	infiniteScroll = ko.observable<boolean>(false);
	batchIndex = ko.observable<number>(0);
	lookups: LookupType = {
		errorCodes: { $tableName: "CrmService_ErrorCode" },
		serviceCaseCategories: {$tableName: "CrmService_ServiceCaseCategory"},
		serviceCaseStatuses: {$tableName: "CrmService_ServiceCaseStatus"},
		servicePriorities: {$tableName: "CrmService_ServicePriority"},
		skills: { $tableName: "Main_Skill" },
		serviceErrorMessages: {$tableName: "CrmService_ErrorCode"}
	}
	customErrorMessage = ko.observable<boolean>(false);

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		const serviceCase = window.database.CrmService_ServiceCase.defaultType.create();
		const currentUserName = window.Helper.User.getCurrentUserName();
		this.currentUser(await window.Helper.User.getCurrentUser());
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		serviceCase.CategoryKey = window.Helper.Lookup.getDefaultLookupValueSingleSelect(this.lookups.serviceCaseCategories, serviceCase.CategoryKey);
		serviceCase.PriorityKey = window.Helper.Lookup.getDefaultLookupValueSingleSelect(this.lookups.servicePriorities, serviceCase.PriorityKey);
		serviceCase.ServiceCaseCreateUser = currentUserName;
		serviceCase.ServiceCaseCreateDate = new Date();
		serviceCase.Reported = new Date();
		serviceCase.ResponsibleUser = currentUserName;
		serviceCase.LastActivity = new Date();
		serviceCase.StationKey = this.currentUser().ExtensionValues.StationIds.length === 1 ? this.currentUser().ExtensionValues.StationIds[0] : null;
		serviceCase.StatusKey = window.Helper.Lookup.getDefaultLookupValueSingleSelect(this.lookups.serviceCaseStatuses, serviceCase.StatusKey);
		this.serviceCase(serviceCase.asKoObservable());
		this.serviceCase().AffectedCompanyKey.subscribe(affectedCompanyKey => {
			this.serviceCase().ContactPersonId(null);
			if (affectedCompanyKey &&
				this.affectedInstallation() &&
				this.affectedInstallation().LocationContactId() !== affectedCompanyKey) {
				this.serviceCase().AffectedInstallationKey(null);
				this.onSelectAffectedInstallation(null);
			}
		});
		this.serviceCase().ErrorMessage.subscribe(errorMessage => {
			this.serviceCase().Name(errorMessage);
		});
		this.serviceCase().ErrorCodeKey.subscribe(errorCodeKey => {
			const selectedErrorMessageValue = window.Helper.Lookup.getLookupValue(this.lookups.serviceErrorMessages, errorCodeKey);
			this.serviceCase().ErrorMessage(selectedErrorMessageValue);
		});
		if (this.lookups.serviceErrorMessages.$array.length === 0) {
			this.customErrorMessage(true);
		}
		this.affectedInstallation.subscribe(installation => {
			if (!!this.serviceCase().AffectedCompanyKey() === false &&
				installation &&
				installation.LocationContactId() &&
				installation.LocationContactId() !== this.serviceCase().AffectedCompanyKey()) {
				this.serviceCase().AffectedCompanyKey(installation.LocationContactId());
			}
			if (installation &&
				installation.FolderId() &&
				installation.FolderId() !== this.serviceCase().ServiceObjectId()) {
				this.serviceCase().ServiceObjectId(installation.FolderId());
			}
		});
		this.serviceCaseTemplate.subscribe(serviceCaseTemplate => {
			if (serviceCaseTemplate) {
				this.serviceCase().CategoryKey(serviceCaseTemplate.CategoryKey);
				this.serviceCase().PriorityKey(serviceCaseTemplate.PriorityKey);
				this.serviceCase().UserGroupKey(serviceCaseTemplate.UserGroupKey);
				this.serviceCase().ResponsibleUser(serviceCaseTemplate.ResponsibleUser || currentUserName);
				this.serviceCase().RequiredSkillKeys(serviceCaseTemplate.RequiredSkillKeys);
				this.serviceCase().RequiredAssetKeys(serviceCaseTemplate.RequiredAssetKeys);
			} else {
				this.serviceCase().CategoryKey(null);
				this.serviceCase().PriorityKey(null);
				this.serviceCase().UserGroupKey(null);
				this.serviceCase().ResponsibleUser(currentUserName);
				this.serviceCase().RequiredSkillKeys([]);
				this.serviceCase().RequiredAssetKeys([]);
			}
		});
		if (params && params.affectedCompanyKey) {
			this.serviceCase().AffectedCompanyKey(params.affectedCompanyKey);
		}
		if (params && params.affectedInstallationKey) {
			this.serviceCase().AffectedInstallationKey(params.affectedInstallationKey);
		}
		if (params && params.serviceObjectId) {
			this.serviceCase().ServiceObjectId(params.serviceObjectId);
		}
		this.serviceCase().ErrorCodeKey.extend({
			required: {
				message: window.Helper.String.getTranslatedString("RuleViolation.Required")
					.replace("{0}", window.Helper.String.getTranslatedString("ErrorMessage")),
				onlyIf: () => {
					return !this.customErrorMessage() && !this.serviceCase().ErrorMessage();
				}
			},
			params: true
		});
		this.serviceCase().UserGroupKey.subscribe(() => {
			if (this.serviceCase().UserGroupKey() !== null) {
				this.serviceCase().ResponsibleUser(null);
			}
		});
		await this.visibilityViewModel.init();
		await this.setBreadcrumbs();
		window.database.add(this.serviceCase().innerInstance);
	};

	cancel(): void {
		window.database.detach(this.serviceCase().innerInstance);
		window.history.back();
	};
	
	async getRelatedServiceCases(): Promise<void> {
		if (this.installationServiceCasesEnabled) {
			this.loadingRelatedServiceCases(true);
			const batchSize = 4;
			const closedKeys = this.lookups.serviceCaseStatuses.$array.filter(x => x.Groups === "Closed").map(x => x.Key);
			let results = await window.database.CrmService_ServiceCase
				.filter("it.AffectedInstallationKey === this.installationId && !(it.StatusKey in this.closedKeys)", { installationId: this.affectedInstallation().Id(), closedKeys })
				.withInlineCount()
				.include("AffectedInstallation")
				.include("AffectedCompany")
				.include("ResponsibleUserUser")
				.include("ServiceObject")
				.orderByDescending("it.ModifyDate")
				.skip(this.batchIndex() * batchSize)
				.take(batchSize)
				.toArray();
			this.items().push(...results.map(it => {
				let result = it.asKoObservable();
				// @ts-ignore
				result.CreateUserUser = ko.observable(null);
				result.ServiceCaseCreateUser(null);
				return result;
			}));
			// @ts-ignore
			this.canShowMoreRelatedServiceCases(results.totalCount > this.items().length);
			this.items.valueHasMutated();
			this.loadingRelatedServiceCases(false);
		}
	}

	async showMoreRelatedServiceCases(): Promise<void> {
		this.batchIndex(this.batchIndex() + 1);
		await this.getRelatedServiceCases();
	};

	contactPersonFilter(query: $data.Queryable<Crm.Rest.Model.Crm_Person>, term: string): $data.Queryable<Crm.Rest.Model.Crm_Person> {
		if (term) {
			query = window.Helper.String.contains(query, term, ["Firstname", "Surname"]);
		}
		if (this.serviceCase().AffectedCompanyKey()) {
			query = query.filter(function (it) {
					return it.ParentId === this.affectedCompanyKey;
				},
				{affectedCompanyKey: this.serviceCase().AffectedCompanyKey()});
		}
		return query.filter("it.IsRetired === false");
	};

	getServiceCaseTemplateAutocompleteFilter(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceCaseTemplate>, term: string): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceCaseTemplate> {
		if (!term) {
			return query;
		}
		return window.Helper.String.contains(query, term, ["Name"]);
	};

	installationFilter(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_Installation>, term: string): $data.Queryable<Crm.Service.Rest.Model.CrmService_Installation> {
		if (term) {
			query = window.Helper.String.contains(query, term, ["InstallationNo", "Description"]);
		}
		if (this.serviceCase().ServiceObjectId()) {
			query = query.filter(function (it) {
					return it.FolderId === this.serviceObjectId;
				},
				{ serviceObjectId: this.serviceCase().ServiceObjectId() });
		} else {
			if (window.Crm.Service.Settings.ServiceCase.OnlyInstallationsOfReferencedCustomer && this.serviceCase().AffectedCompanyKey()) {
				query = query.filter(function (it) {
						return it.LocationContactId === this.affectedCompanyKey;
					},
					{ affectedCompanyKey: this.serviceCase().AffectedCompanyKey() });
			}
		}
		return query;
	};

	async onSelectAffectedInstallation(installation: Crm.Service.Rest.Model.CrmService_Installation): Promise<void> {
		if (this.affectedInstallation()) {
			window.database.detach(this.affectedInstallation().innerInstance);
			this.affectedInstallation(null);
			this.items([]);
			this.canShowMoreRelatedServiceCases(true);
		}
		if (installation) {
			window.database.attachOrGet(installation);
			this.affectedInstallation(installation.asKoObservable());
			if (this.installationServiceCasesEnabled) {
				this.batchIndex(0);
				await this.getRelatedServiceCases();
			}
		}
	};

	async setServiceCaseNo() {
		if (!this.serviceCase().ServiceCaseNo()) {
			let serviceCaseNo = await window.NumberingService.createNewNumberBasedOnAppSettings(window.Crm.Service.Settings.ServiceCase.ServiceCaseNoIsGenerated, window.Crm.Service.Settings.ServiceCase.ServiceCaseNoIsCreateable, this.serviceCase().ServiceCaseNo(), this.numberingSequenceName, window.database.CrmService_ServiceCase, "ServiceCaseNo");
			this.serviceCase().ServiceCaseNo(serviceCaseNo);
		}
		if (this.errors().length > 0) {
			this.loading(false);
			this.errors.showAllMessages();
			this.errors.scrollToError();
		}
	};

	async submit(): Promise<void> {
		this.loading(true);
		if (this.errors().length > 0) {
			this.loading(false);
			this.errors.showAllMessages();
			this.errors.scrollToError();
			this.errors.expandCollapsiblesWithErrors();
			return;
		}
		await this.setServiceCaseNo();
		await window.database.saveChanges()
		window.location.hash = "/Crm.Service/ServiceCase/DetailsTemplate/" + this.serviceCase().Id();
	};

	toggleCustomErrorMessage(): void {
		this.customErrorMessage(!this.customErrorMessage());
		if (this.customErrorMessage()) {
			this.serviceCase().ErrorCodeKey(null);
		} else {
			this.serviceCase().ErrorMessage(null);
		}
	};

	async setBreadcrumbs(): Promise<void> {
		await window.breadcrumbsViewModel.setBreadcrumbs([
			new Breadcrumb(window.Helper.getTranslatedString("ServiceCase"), "ServiceCase::Index", "#/Crm.Service/ServiceCaseList/IndexTemplate"),
			new Breadcrumb(window.Helper.getTranslatedString("CreateServiceCase"), null, window.location.hash, null, null)
		]);
	};
}

namespace("Crm.Service.ViewModels").ServiceCaseCreateViewModel = ServiceCaseCreateViewModel;