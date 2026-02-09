import {namespace} from "@Main/namespace";
import {Breadcrumb} from "@Main/breadcrumbs";

export class ServiceCaseDetailsViewModel extends window.Crm.ViewModels.ContactDetailsViewModel {
	tabs = ko.observable<{}>({});
	createUser = ko.observable<Main.Rest.Model.Main_User>(null);
	serviceCase = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceCase>(null);
	dispatch = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch>(null);
	serviceOrder = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead>(null);
	serviceOrderTimeOrder = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead>(null);
	dropboxName = ko.pureComputed<string>(() => {
		return this.serviceCase()
			? this.serviceCase().ServiceCaseNo() +
			(this.serviceCase().AffectedCompany()
				? "-" + this.serviceCase().AffectedCompany().Name().substring(0, 25)
				: "")
			: "";
	});
	lookups: LookupType = {
		errorCodes: { $tableName: "CrmService_ErrorCode" },
		installationHeadStatuses: {$tableName: "CrmService_InstallationHeadStatus"},
		serviceCaseCategories: {$tableName: "CrmService_ServiceCaseCategory"},
		serviceCaseStatuses: {$tableName: "CrmService_ServiceCaseStatus"},
		servicePriorities: {$tableName: "CrmService_ServicePriority"},
		skills: {$tableName: "Main_Skill"},
		assets: {$tableName: "Main_Asset"},
	};
	settableStatuses = ko.pureComputed<Crm.Service.Rest.Model.Lookups.CrmService_ServiceCaseStatus[]>(() => {
		const currentStatus = this.lookups.serviceCaseStatuses.$array.find(x => x.Key === this.serviceCase().StatusKey());
		const settableStatusKeys = currentStatus
			? (currentStatus.SettableStatuses || "").split(",").map(function (x) {
				return parseInt(x);
			})
			: [];
		return this.lookups.serviceCaseStatuses.$array.filter(x => x === currentStatus || settableStatusKeys.indexOf(x.Key) !== -1);
	});
	canSetStatus = ko.pureComputed<boolean>(() => {
		return this.settableStatuses().length > 1 &&
			window.AuthorizationManager.isAuthorizedForAction("ServiceCase", "SetStatus")
			&& this.isEditable();
	});
	isEditable = ko.pureComputed<boolean>(() => {
		const hasEditPermission = window.AuthorizationManager.isAuthorizedForAction("ServiceCase", "Edit");
		const hasEditClosedPermission = window.AuthorizationManager.isAuthorizedForAction("ServiceCase", "EditClosed");
		const hasEditNotAssignedPermission = window.AuthorizationManager.isAuthorizedForAction("ServiceCase", "EditNotAssigned");
		const isClosed = window.Helper.ServiceCase.belongsToClosed(this.lookups.serviceCaseStatuses[this.serviceCase().StatusKey()]);
		const isUserAssigned = this.serviceCase().ResponsibleUser() === window.Helper.User.getCurrentUserName();
		const isUserCreator = this.serviceCase().ServiceCaseCreateUser() === window.Helper.User.getCurrentUserName();

		if (isClosed) {
			if (!hasEditClosedPermission)
				return false;
		}

		if (isUserCreator && hasEditPermission)
			return true;

		if (isUserAssigned && hasEditPermission)
			return true;

		if (!isUserAssigned && hasEditNotAssignedPermission)
			return true;

		return false;
	});

	constructor() {
		super();
		this.contactType("ServiceCase");
	}

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		this.contactId(id);
		await super.init(id, params);
		let serviceCase = await window.database.CrmService_ServiceCase
			.include("AffectedCompany")
			.include("AffectedInstallation")
			.include("AffectedInstallation.Address")
			.include("CompletionServiceOrder")
			.include("CompletionUserUser")
			.include("ContactPerson")
			.include("OriginatingServiceOrder")
			.include("OriginatingServiceOrderTime")
			.include("ResponsibleUserUser")
			.include("ServiceObject")
			.include("Station")
			.include("ServiceOrderTime")
		.include("UserGroup")
			.include2("Tags.orderBy(function(t) { return t.Name; })")
			.find(id);
		this.serviceCase(serviceCase.asKoObservable());
		if (serviceCase.ServiceOrderTime?.OrderId) {
			let serviceOrderTimeOrder = await window.database.CrmService_ServiceOrderHead.find(serviceCase.ServiceOrderTime.OrderId);
			if (serviceOrderTimeOrder) {
				this.serviceOrderTimeOrder(serviceOrderTimeOrder.asKoObservable());
			}
		}

		this.serviceCase().ServiceCaseNo.subscribe(function () {
			this.setTitle();
		}, this);
		this.serviceCase().ErrorMessage.subscribe(async () => {
			this.setTitle();
		}, this);
		this.contact(this.serviceCase());
		this.contactName(window.Helper.ServiceCase.getDisplayName(this.serviceCase()));
		this.createUser(await window.database.Main_User.find(this.serviceCase().CreateUser()));
		await window.Helper.StatisticsKey.getAvailableLookups(this.lookups, this.serviceCase());
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		this.setTitle();
		await this.setVisibilityAlertText();
		await this.setBreadcrumbs(id, this.title());
	};

	contactPersonFilter =
		window.Crm.Service.ViewModels.ServiceCaseCreateViewModel.prototype.contactPersonFilter;
	installationFilter =
		window.Crm.Service.ViewModels.ServiceCaseCreateViewModel.prototype.installationFilter;

	async assignToMe(serviceCase: Crm.Service.Rest.Model.ObservableCrmService_ServiceCase) : Promise<void> {
		if(serviceCase.ResponsibleUser() === window.Helper.User.getCurrentUserName()) {
			return;
		}
		if(serviceCase.UserGroup()) {
			const currentUser = await window.Helper.User.getCurrentUser();
			if(!currentUser.Usergroups.includes(serviceCase.UserGroup().Name())) {
				serviceCase.UserGroupKey(null);
				serviceCase.UserGroup(null);
			}
		}
		serviceCase.ResponsibleUser(window.Helper.User.getCurrentUserName());
	};

	async setStatus(status: Crm.Service.Rest.Model.Lookups.CrmService_ServiceCaseStatus): Promise<void> {
		this.loading(true);
		window.database.attachOrGet(this.serviceCase().innerInstance);
		window.Helper.ServiceCase.setStatus(this.serviceCase(), status);
		await window.database.saveChanges();
		this.loading(false);
	};

	onResponsibleUserSelect(serviceCase: Crm.Service.Rest.Model.ObservableCrmService_ServiceCase, user: Main.Rest.Model.Main_User): void {
		if (user) {
			serviceCase.ResponsibleUserUser(user.asKoObservable());
		} else {
			serviceCase.ResponsibleUserUser(null);
		}
	};

	getSkillsFromKeys(keys: string[]): Select2AutoCompleterResult[] {
		return this.lookups.skills.$array.filter(function (x) {
			return keys.indexOf(x.Key) !== -1;
		}).map(window.Helper.Lookup.mapLookupForSelect2Display);
	};
	getAssetsFromKeys(keys: string[]): Select2AutoCompleterResult[] {
		return this.lookups.assets.$array.filter(function (x) {
			return keys.indexOf(x.Key) !== -1;
		}).map(window.Helper.Lookup.mapLookupForSelect2Display);
	};

	onAffectedCompanySelect(serviceCase: Crm.Service.Rest.Model.ObservableCrmService_ServiceCase, company: Crm.Rest.Model.Crm_Company): void {
		if (company) {
			serviceCase.AffectedCompany(company.asKoObservable());
		} else {
			serviceCase.AffectedCompany(null);
		}
	};

	onContactPersonSelect(serviceCase: Crm.Service.Rest.Model.ObservableCrmService_ServiceCase, person: Crm.Rest.Model.Crm_Person): void {
		if (person) {
			serviceCase.ContactPerson(person.asKoObservable());
		} else {
			serviceCase.ContactPerson(null);
		}
	};

	onAffectedInstallationSelect(serviceCase: Crm.Service.Rest.Model.ObservableCrmService_ServiceCase, installation: Crm.Service.Rest.Model.CrmService_Installation): void {
		if (installation) {
			serviceCase.AffectedInstallation(installation.asKoObservable());
		} else {
			serviceCase.AffectedInstallation(null);
		}
	};

	async setBreadcrumbs(id: string, title: string): Promise<void> {
		return window.breadcrumbsViewModel.setBreadcrumbs([
			new Breadcrumb(window.Helper.String.getTranslatedString("ServiceCase"), "ServiceCase::Index", "#/Crm.Service/ServiceCaseList/IndexTemplate"),
			new Breadcrumb(title, null, window.location.hash, null, id)
		]);
	};

	setTitle(): void {
		this.title(window.Helper.ServiceCase.getDisplayName(this.serviceCase()));
	};
	getShortName() {
		return `${window.Helper.String.getTranslatedString(this.contactType())} / ${window.Helper.ServiceCase.getDisplayName(this.serviceCase())}`;
	}

	onUserGroupSelect(serviceCase: Crm.Service.Rest.Model.ObservableCrmService_ServiceCase, userGroup: Main.Rest.Model.Main_Usergroup): void {
		if(!userGroup || serviceCase.UserGroup()?.Id() !== userGroup?.Id) {
			serviceCase.ResponsibleUserUser(null);
			serviceCase.ResponsibleUser(null);
			serviceCase.ResponsibleUser.valueHasMutated();
		}
		serviceCase.UserGroup(userGroup ? userGroup.asKoObservable() : null);
	};

	onSelectOriginatingServiceOrder(serviceOrder: Crm.Service.Rest.Model.CrmService_ServiceOrderHead): void {
		if (serviceOrder) {
			this.serviceCase().OriginatingServiceOrder(serviceOrder.asKoObservable());
			this.serviceCase().OriginatingServiceOrderId(serviceOrder.Id);
			if (this.serviceCase().OriginatingServiceOrderTime() && this.serviceCase().OriginatingServiceOrderTime().OrderId() !== serviceOrder.Id) {
				this.serviceCase().OriginatingServiceOrderTime(null);
				this.serviceCase().OriginatingServiceOrderTimeId(null);
			}
		} else {
			this.serviceCase().OriginatingServiceOrder(null);
			this.serviceCase().OriginatingServiceOrderId(null);
			this.serviceCase().OriginatingServiceOrderTime(null);
			this.serviceCase().OriginatingServiceOrderTimeId(null);
		}
	};

	serviceOrderFilter(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderHead>, term: string): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderHead> {
		if (!!this.serviceCase().AffectedCompanyKey()) {
			query = query.filter(function (it) {
					return it.CustomerContactId === this.contactId
				},
				{contactId: this.serviceCase().AffectedCompanyKey()});
		}
		if (!term) {
			return query;
		}
		return window.Helper.String.contains(query, term, ["OrderNo", "ErrorMessage"]);
	};

	serviceOrderTimeFilter(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTime>, term: string): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTime> {
		if (this.serviceCase().OriginatingServiceOrderId()) {
			query = query.filter(function (it) {
					return it.OrderId === this.serviceOrderId;
				},
				{serviceOrderId: this.serviceCase().OriginatingServiceOrderId()});
		}
		if (!term) {
			return query;
		}
		return window.Helper.String.contains(query, term, ["PosNo", "Description"]);

	};
}

namespace("Crm.Service.ViewModels").ServiceCaseDetailsViewModel = ServiceCaseDetailsViewModel;
