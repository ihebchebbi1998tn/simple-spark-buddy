import {namespace} from "@Main/namespace";
import {Breadcrumb} from "@Main/breadcrumbs";
import type {PmbbViewModel} from "@Main/PmbbViewModel";

export class ServiceOrderDetailsViewModel extends window.Crm.ViewModels.ContactDetailsViewModel {
	geolocationViewModel = new window.Crm.ViewModels.GenericListGeolocationViewModel();
	currentTabId = ko.observable<string>(null);
	previousTabId = ko.observable<string>(null);
	currentUser = ko.observable<Main.Rest.Model.Main_User>(null);
	dispatch = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch>(null);
	serviceOrder = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead>(null);
	serviceCase = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceCase>(null);
	serviceOrderIsEditable = ko.pureComputed<boolean>(() => {
		let hasPermission = window.AuthorizationManager.isAuthorizedForAction("ServiceOrder", "Edit");
		let autonomousPlanningPermission = window.AuthorizationManager.isAuthorizedForAction("Dispatch", "AutonomousDispatchPlanning")
		hasPermission = hasPermission || autonomousPlanningPermission || this.serviceOrder().CreateUser() === this.currentUser().Id;
		if (hasPermission && this.serviceOrder() && this.lookups.serviceOrderStatuses) {
			const status = this.lookups.serviceOrderStatuses[this.serviceOrder().StatusKey()];
			if (status && status.Groups.split(",").indexOf("Closed") === -1) {
				return true;
			}
		}
		return false;
	});
	serviceObject = ko.pureComputed<Crm.Service.Rest.Model.ObservableCrmService_ServiceObject>(() => {
		return this.serviceOrder() ? this.serviceOrder().ServiceObject() : null;
	});
	serviceContract = ko.pureComputed<Crm.Service.Rest.Model.ObservableCrmService_ServiceContract>(() => {
		return this.serviceOrder() ? this.serviceOrder().ServiceContract() : null;
	});
	serviceObjectAddress = ko.pureComputed<Crm.Rest.Model.ObservableCrm_Address>(() => {
		return this.serviceObject() ? this.serviceObject().Addresses()[0] : null;
	});
	customerContact = ko.pureComputed<Crm.Rest.Model.ObservableCrm_Company>(() => {
		return this.serviceOrder() ? this.serviceOrder().Company() : null;
	});
	customerContactAddress = ko.pureComputed<Crm.Rest.Model.ObservableCrm_Address>(() => {
		return this.customerContact() ? this.customerContact().Addresses()[0] : null;
	});
	initiator = ko.pureComputed<Crm.Rest.Model.ObservableCrm_Company>(() => {
		return this.serviceOrder() ? this.serviceOrder().Initiator() : null;
	});
	initiatorAddress = ko.pureComputed<Crm.Rest.Model.ObservableCrm_Address>(() => {
		return this.initiator() ? this.initiator().Addresses()[0] : null;
	});
	initiatorPerson = ko.pureComputed<Crm.Rest.Model.ObservableCrm_Person>(() => {
		return this.serviceOrder() ? this.serviceOrder().InitiatorPerson() : null;
	});

	lookups: LookupType = {
		addressTypes: {$tableName: "Crm_AddressType"},
		countries: {$tableName: "Main_Country"},
		emailTypes: {$tableName: "Crm_EmailType"},
		expenseTypes: { $tableName: "CrmPerDiem_ExpenseType" },
		errorCodes: { $tableName: "CrmService_ErrorCode" },
		faxTypes: {$tableName: "Crm_FaxType"},
		invoicingTypes: {$tableName: "Main_InvoicingType"},
		phoneTypes: {$tableName: "Crm_PhoneType"},
		regions: {$tableName: "Main_Region"},
		serviceContractTypes: {$tableName: "CrmService_ServiceContractType"},
		serviceObjectCategories: {$tableName: "CrmService_ServiceObjectCategory"},
		serviceOrderNoInvoiceReasons: {$tableName: "CrmService_ServiceOrderNoInvoiceReason"},
		serviceOrderStatuses: {$tableName: "CrmService_ServiceOrderStatus"},
		serviceOrderTypes: {$tableName: "CrmService_ServiceOrderType"},
		servicePriorities: {$tableName: "CrmService_ServicePriority"},
		skills: {$tableName: "Main_Skill"},
		websiteTypes: {$tableName: "Crm_WebsiteType"},
		currencies: {$tableName: "Main_Currency"},
		assets: { $tableName: "Main_Asset" },
		distanceUnits: { $tableName: "CrmPerDiem_DistanceUnit" }
	};
	settableStatuses = ko.pureComputed<Crm.Service.Rest.Model.Lookups.CrmService_ServiceOrderStatus[]>(() => {
		const currentStatus = this.lookups.serviceOrderStatuses.$array.find(x => x.Key === this.serviceOrder().StatusKey());
		let settableStatusKeys = currentStatus
			? (currentStatus.SettableStatuses || "").split(",")
			: [];
		if (window.AuthorizationManager.isAuthorizedForAction("ServiceOrder", "SetAdditionalHeadStatuses")) {
			settableStatusKeys = window._.uniq(settableStatusKeys.concat(["ReadyForScheduling", "Closed"]));
		}
		return this.lookups.serviceOrderStatuses.$array.filter(x => x === currentStatus || settableStatusKeys.indexOf(x.Key) !== -1);
	});
	canEditCustomerContact = ko.pureComputed<boolean>(() => {
		return this.serviceOrderIsEditable();
	});
	canEditInitiator = ko.pureComputed<boolean>(() => {
		return this.serviceOrderIsEditable();
	});
	canSetStatus = ko.pureComputed<boolean>(() => {
		return this.settableStatuses().length > 1 &&
			window.AuthorizationManager.isAuthorizedForAction("ServiceOrder", "SetHeadStatus");
	});
	showInvoiceData = ko.pureComputed<boolean>(() => {
		return window.Helper.ServiceOrder.isInStatusGroupSync(this.serviceOrder().StatusKey(), this.lookups.serviceOrderStatuses as {[key:string]: Crm.Service.Rest.Model.Lookups.CrmService_ServiceOrderStatus}, ["PostProcessing", "Closed"]);
	});
	showJobsTab = ko.pureComputed<boolean>(() => {
		if (window.Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode === "JobPerInstallation" || window.Crm.Service.Settings.ServiceOrder.GenerateAndAttachJobsToUnattachedTimePostings) {
			return true;
		}
		const status: Crm.Service.Rest.Model.Lookups.CrmService_ServiceOrderStatus = this.serviceOrder() && this.lookups.serviceOrderStatuses ? this.lookups.serviceOrderStatuses[this.serviceOrder().StatusKey()] : null;
		if (!status) {
			return false;
		}
		return status.Groups.split(",").indexOf("PostProcessing") !== -1 || status.Groups.split(",").indexOf("Closed") !== -1;
	});
	showTotalPrices = ko.pureComputed<boolean>(() => true);
	tabs = ko.observable<{}>({});
	contactType = ko.observable<string>("ServiceOrder");
	contactName = ko.pureComputed<string>(() => {
		return this.serviceOrder() ? this.serviceOrder().OrderNo() : null;
	});
	dropboxName = ko.pureComputed<string>(() => {
		return this.serviceOrder() && this.serviceOrder().Company() ? this.serviceOrder().OrderNo() + "-" + this.serviceOrder().Company().Name().substring(0, 25) : "";
	})
	additionalTitle = ko.observable<string>(null);
	customAddress = ko.observable<boolean>(false);
	customContactPerson = ko.observable<boolean>(false);
	selectedAddress = ko.observable<string>(null);
	selectedAddressObject = ko.observable<Crm.Rest.Model.Crm_Address>(null);
	selectedContactPerson = ko.observable<string>(null);
	selectedContactPersonObject = ko.observable(null);
	contactWarning = ko.observable<boolean>(false);
	addressWarning = ko.observable<boolean>(false);
	installations = ko.computed<Crm.Service.Rest.Model.CrmService_Installation[]>(() => {
		if (!this.dispatch() || !this.serviceOrder()) {
			return [];
		}
		if (window.Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode === "JobPerInstallation") {
			return this.serviceOrder().ServiceOrderTimes();
		} else {
			let installationArray = [];
			if (this.serviceOrder().Installation()) {
				installationArray.push(this.serviceOrder().Installation().innerInstance);
			}
			return installationArray;
		}
	});

	constructor() {
		super();
	}

	customerContactFilter = window.Crm.Service.ViewModels.ServiceOrderCreateViewModel.prototype.customerContactFilter

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

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		await this.geolocationViewModel.init();
		await this.loadServiceOrder(id);
		this.serviceOrder().TypeKey.subscribe(function () {
			this.setAdditionalTitle();
		}, this);
		this.serviceOrder().OrderNo.subscribe(function () {
			this.setTitle();
		}, this);

		window.Helper.Database.registerEventHandlers(this, {
			"CrmService_ServiceOrderStatisticsKey": {
				"afterCreate": async (sender, serviceOrderStatisticsKey) => {
					this.loading(true);
					await this.loadServiceOrder(this.serviceOrder().Id());
					this.loading(false);
				},
				"afterUpdate": async (sender, serviceOrderStatisticsKey) => {
					this.loading(true);
					await this.loadServiceOrder(this.serviceOrder().Id());
					this.loading(false);
				}
			}
		});
		const queries = [];
		queries.push({
			queryable: window.database.Crm_Company
				.include2("Addresses.filter(function(x) { x.IsCompanyStandardAddress === true; })")
				.filter("it.Id === this.id", {id: this.serviceOrder().CustomerContactId()}),
			method: "first",
			handler: this.serviceOrder().Company
		});
		if (this.serviceOrder().InitiatorId()) {
			queries.push({
				queryable: window.database.Crm_Company
					.include2("Addresses.filter(function(x) { x.IsCompanyStandardAddress === true; })")
					.filter("it.Id === this.id", {id: this.serviceOrder().InitiatorId()}),
				method: "first",
				handler: this.serviceOrder().Initiator
			});
		}
		if (this.serviceOrder().ServiceObjectId()) {
			queries.push({
				queryable: window.database.CrmService_ServiceObject
					.include2("Addresses.filter(function(x) { x.IsCompanyStandardAddress === true; })")
					.include("Addresses.Emails")
					.include("Addresses.Faxes")
					.include("Addresses.Phones")
					.include("Addresses.Websites")
					.filter("it.Id === this.id", {id: this.serviceOrder().ServiceObjectId()}),
				method: "first",
				handler: this.serviceOrder().ServiceObject
			});
		}
		await window.Helper.Batch.Execute(queries);
		let user = await window.Helper.User.getCurrentUser();

		this.currentUser(user);
		await window.Helper.StatisticsKey.getAvailableLookups(this.lookups, ...this.serviceOrder().ServiceOrderStatisticsKeys());
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		this.serviceOrder().ServiceCaseNo.subscribe(function () {
			this.setTitle();
		}, this);
		this.serviceOrder().ErrorMessage.subscribe(async () => {
			this.setTitle();
		}, this);
		this.setTitle();
		this.setAdditionalTitle();
		await this.setVisibilityAlertText();
		await this.setBreadcrumbs(id, this.title());
	};

	initiatorFilter = window.Crm.Service.ViewModels.ServiceOrderCreateViewModel.prototype.initiatorFilter;

	initiatorPersonFilter = window.Crm.Service.ViewModels.ServiceOrderCreateViewModel.prototype.initiatorPersonFilter;

	invoiceRecipientAddressFilter(query: $data.Queryable<Crm.Rest.Model.Crm_Address>, term: string): $data.Queryable<Crm.Rest.Model.Crm_Address> {
		const serviceOrder = this.serviceOrder();
		query = window.Helper.Address.getAutocompleteFilter(query, term);
		return query.filter(function (it) {
				return it.CompanyId === this.companyId;
			},
			{companyId: serviceOrder.InvoiceRecipientId()});
	};

	async loadServiceOrder(id: string): Promise<void> {
		let serviceOrder = await window.database.CrmService_ServiceOrderHead
			.include("InitiatorPerson")
			.include("InitiatorPerson.Phones")
			.include("InitiatorPerson.Emails")
			.include("Installation")
			.include("InvoiceRecipient")
			.include("InvoiceRecipientAddress")
			.include("Payer")
			.include("PayerAddress")
			.include("PreferredTechnicianUser")
		.include("PreferredTechnicianUsergroupObject")
			.include("ResponsibleUserUser")
			.include2("ServiceOrderStatisticsKeys.filter(function(it2) { it2.DispatchId === null; })")
			.include("ServiceOrderTemplate")
			.include("UserGroup")
			.include("Station")
			.include("ServiceContract")
			.include("ServiceContract.ParentCompany")
			.include("ServiceOrderTimes")
			.include("ServiceOrderTimes.Installation")
			.include2("Tags.orderBy(function(t) { return t.Name; })")
			.find(id);
		this.serviceOrder(serviceOrder.asKoObservable());
		this.contact(this.serviceOrder());
	}

	onInitiatorSelect = window.Crm.Service.ViewModels.ServiceOrderCreateViewModel.prototype.onInitiatorSelect;

	onInvoiceRecipientSelect(invoiceRecipient: Crm.Rest.Model.Crm_Company): void {
		if (invoiceRecipient) {
			this.serviceOrder().InvoiceRecipient(invoiceRecipient.asKoObservable());
			this.serviceOrder().InvoiceRecipientId(invoiceRecipient.Id);
			if (this.serviceOrder().InvoiceRecipientAddress() &&
				this.serviceOrder().InvoiceRecipientAddress().CompanyId() !== invoiceRecipient.Id) {
				this.serviceOrder().InvoiceRecipientAddress(null);
				this.serviceOrder().InvoiceRecipientAddressId(null);
			}
		} else {
			this.serviceOrder().InvoiceRecipient(null);
			this.serviceOrder().InvoiceRecipientId(null);
			this.serviceOrder().InvoiceRecipientAddress(null);
			this.serviceOrder().InvoiceRecipientAddressId(null);
		}
	};

	onPayerSelect(payer: Crm.Rest.Model.Crm_Company): void {
		if (payer) {
			this.serviceOrder().Payer(payer.asKoObservable());
			this.serviceOrder().PayerId(payer.Id);
			if (this.serviceOrder().PayerAddress() &&
				this.serviceOrder().PayerAddress().CompanyId() !== payer.Id) {
				this.serviceOrder().PayerAddress(null);
				this.serviceOrder().PayerAddressId(null);
			}
		} else {
			this.serviceOrder().Payer(null);
			this.serviceOrder().PayerId(null);
			this.serviceOrder().PayerAddress(null);
			this.serviceOrder().PayerAddressId(null);
		}
	};
	onPreferredTechnicianUsergroupSelect(technicianUserGroup: Main.Rest.Model.Main_Usergroup) {
		window.Log.debug("Selected 'PreferredTechnicianUsergroup' is '" + (technicianUserGroup ? technicianUserGroup.Name : "null") + "'");

		if (technicianUserGroup) {
			this.serviceOrder().PreferredTechnicianUsergroupKey(technicianUserGroup.Id);
			this.serviceOrder().PreferredTechnicianUsergroupObject(technicianUserGroup.asKoObservable());

			if (this.serviceOrder().PreferredTechnician() && !this.serviceOrder().PreferredTechnicianUser().UsergroupIds().includes(technicianUserGroup.Id)) {
				window.Log.debug("Selected 'PreferredTechnician' (" + this.serviceOrder().PreferredTechnician() + ") is not member of the selected 'PreferredTechnicianUsergroup' (" + technicianUserGroup.Name + ") and will be removed");
				this.serviceOrder().PreferredTechnician(null);
				this.serviceOrder().PreferredTechnicianUser(null);
			}
		}
	};

	onUsergroupSelect(userGroup: Main.Rest.Model.Main_Usergroup) {
		window.Log.debug("Selected 'Usergroup' is '" + (userGroup ? userGroup.Name : "null") + "'");

		if (userGroup) {
			this.serviceOrder().UserGroup(userGroup.asKoObservable());
			this.serviceOrder().UserGroupKey(userGroup.Id);

			if (this.serviceOrder().ResponsibleUserUser() && !this.serviceOrder().ResponsibleUserUser().UsergroupIds().includes(userGroup.Id)) {
				window.Log.debug("Selected 'ResponsibleUser' (" + this.serviceOrder().ResponsibleUser() + ") is not member of the selected 'Usergroup' (" + userGroup.Name + ") and will be removed");
				this.serviceOrder().ResponsibleUser(null);
				this.serviceOrder().ResponsibleUserUser(null);
			}
		} else {
			this.serviceOrder().UserGroup(null);
			this.serviceOrder().UserGroupKey(null);
		}
	}

	onSaveCustomerContact(viewModel: PmbbViewModel): void {
		viewModel.viewContext.serviceOrder().Company(viewModel.editContext().serviceOrder().Company());
	};

	onSaveInitiator(viewModel: PmbbViewModel): void {
		viewModel.viewContext.serviceOrder().Initiator(viewModel.editContext().serviceOrder().Initiator());
		viewModel.viewContext.serviceOrder().InitiatorPerson(viewModel.editContext().serviceOrder().InitiatorPerson());
	};

	async onSaveInvoicing(viewModel: PmbbViewModel): Promise<void> {
		viewModel.viewContext.serviceOrder().InvoiceRecipient(viewModel.editContext().serviceOrder().InvoiceRecipient());
		viewModel.viewContext.serviceOrder().InvoiceRecipientAddress(viewModel.editContext().serviceOrder().InvoiceRecipientAddress());
		viewModel.viewContext.serviceOrder().Payer(viewModel.editContext().serviceOrder().Payer());
		viewModel.viewContext.serviceOrder().PayerAddress(viewModel.editContext().serviceOrder().PayerAddress());
		await Helper.Service.resetInvoicingIfLumpSumSettingsChanged(viewModel.viewContext.serviceOrder());
	};

	onInitScheduling(viewModel: PmbbViewModel): void {
		const toggle = viewModel.toggle;
		viewModel.toggle = function (element, e) {
			toggle(element, e);
			viewModel.editContext().serviceOrder().UserGroupKey.subscribe(function () {
				viewModel.editContext().serviceOrder().ResponsibleUserUser(null);
				viewModel.editContext().serviceOrder().ResponsibleUser(null);
			});
			viewModel.editContext().serviceOrder().PreferredTechnicianUsergroupKey.subscribe(function() {
				viewModel.editContext().serviceOrder().PreferredTechnicianUser(null);
				viewModel.editContext().serviceOrder().PreferredTechnician(null);
			});
		};
	};

	onSaveScheduling(viewModel: PmbbViewModel): void {
		viewModel.viewContext.serviceOrder()
			.ResponsibleUserUser(viewModel.editContext().serviceOrder().ResponsibleUserUser());
		viewModel.viewContext.serviceOrder()
			.PreferredTechnicianUsergroupObject(viewModel.editContext().serviceOrder().PreferredTechnicianUsergroupObject());
		viewModel.viewContext.serviceOrder().UserGroup(viewModel.editContext().serviceOrder().UserGroup());
	};

	payerAddressFilter(query: $data.Queryable<Crm.Rest.Model.Crm_Address>, term: string): $data.Queryable<Crm.Rest.Model.Crm_Address> {
		const serviceOrder = this.serviceOrder();
		query = window.Helper.Address.getAutocompleteFilter(query, term);
		return query.filter(function (it) {
				return it.CompanyId === this.companyId;
			},
			{companyId: serviceOrder.PayerId()});
	};

	preferredTechnicianFilter = window.Crm.Service.ViewModels.ServiceOrderCreateViewModel.prototype.preferredTechnicianFilter;

	async setStatus(status: Crm.Service.Rest.Model.Lookups.CrmService_ServiceOrderStatus): Promise<void> {
		if (status.Key === "PostProcessing") {
			$("#lgModal").modal("show", {
				route: "Crm.Service/ServiceOrder/CreateInvoice/" + this.serviceOrder().Id(),
				viewModel: this
			});
			return;
		}
		this.loading(true);
		if (await this.promptStatusChange(status)) {
			window.database.attachOrGet(this.serviceOrder().innerInstance);
			window.Helper.ServiceOrder.setStatus(this.serviceOrder(), status);
			await window.database.saveChanges();
		}
		this.loading(false);
	}

	async promptStatusChange(status: Crm.Service.Rest.Model.Lookups.CrmService_ServiceOrderStatus): Promise<boolean> {
		const group = "Scheduling";
		const serviceOrderStatus = this.lookups.serviceOrderStatuses[this.serviceOrder().StatusKey()];
		const groupsOfStatus = (serviceOrderStatus?.Groups ?? "").split(",");
		let confirm = true;
		if (groupsOfStatus.indexOf(group) !== -1 && (status?.Groups ?? "").split(",").indexOf("Closed") !== -1) {
			const dispatchCount = await window.database.CrmService_ServiceOrderDispatch
				.filter(function (it) {
						return it.OrderId === this.orderId && (it.StatusKey === "Released" || it.StatusKey === "Read" || it.StatusKey === "Scheduled");
					},
					{orderId: this.serviceOrder().Id()})
				.count();
			if (dispatchCount > 0) {
				confirm = await window.Helper.Confirm.genericConfirmAsync({
					text: window.Helper.String.getTranslatedString("ChangeServiceOrderStatusPrompt"),
					type: "warning"
				});
			}
		}
		if (this.serviceOrder().StatusKey() === 'PartiallyCompleted' && status?.Key === 'Completed') {
			confirm = await window.Helper.Confirm.genericConfirmAsync({
				text: window.Helper.String.getTranslatedString("CompleteServiceOrderPrompt"),
				type: "warning"
			});
		}
		return confirm;
	}

	async setBreadcrumbs(id: string, title: string): Promise<void> {
		await window.breadcrumbsViewModel.setBreadcrumbs([
			new Breadcrumb(window.Helper.String.getTranslatedString("ServiceOrder"), "ServiceOrder::Index", "#/Crm.Service/ServiceOrderHeadList/IndexTemplate"),
			new Breadcrumb(title, null, window.location.hash, null, id)
		]);
	};

	setTitle(): void {
		this.title(window.Helper.ServiceOrder.getDisplayName(this.serviceOrder()));
	};

	setAdditionalTitle(): void {
		const typeKey = this.serviceOrder().TypeKey();
		const type = this.lookups.serviceOrderTypes.$array.find(function (x) {
			return x.Key === typeKey;
		});
		this.additionalTitle(type ? type.Value : "");
	};

	getShortName(): string {
		return ko.unwrap(this.title);
	}

	onStationSelect = window.Crm.Service.ViewModels.DispatchDetailsViewModel.prototype.onStationSelect;

	getInstallationAutocompleteFilter = window.Crm.Service.ViewModels.ServiceOrderCreateViewModel.prototype.installationFilter;
	toggleCustomContactPerson = window.Crm.Service.ViewModels.DispatchDetailsViewModel.prototype.toggleCustomContactPerson;
	toggleCustomAddress = window.Crm.Service.ViewModels.DispatchDetailsViewModel.prototype.toggleCustomAddress;
	selectedContactPersonOnSelect = window.Crm.Service.ViewModels.DispatchDetailsViewModel.prototype.selectedContactPersonOnSelect;
	selectedAddressOnSelect = window.Crm.Service.ViewModels.DispatchDetailsViewModel.prototype.selectedAddressOnSelect;
	contactPersonFilter = window.Crm.Service.ViewModels.ServiceOrderCreateViewModel.prototype.contactPersonFilter;
	filterAddresses = window.Crm.Service.ViewModels.ServiceOrderCreateViewModel.prototype.filterAddresses;
	formatAddress = window.Crm.Service.ViewModels.ServiceOrderCreateViewModel.prototype.formatAddress;
	isInstallationAddress = window.Crm.Service.ViewModels.ServiceOrderCreateViewModel.prototype.isInstallationAddress;
	isServiceObjectAddress = window.Crm.Service.ViewModels.ServiceOrderCreateViewModel.prototype.isServiceObjectAddress;
	onLocationPmbCancel = window.Crm.Service.ViewModels.DispatchDetailsViewModel.prototype.onLocationPmbCancel;
	getDirection = window.Crm.Service.ViewModels.ServiceOrderHeadListIndexViewModel.prototype.getDirection;
	getDistance = window.Crm.Service.ViewModels.ServiceOrderHeadListIndexViewModel.prototype.getDistance;
}

namespace("Crm.Service.ViewModels").ServiceOrderDetailsViewModel = ServiceOrderDetailsViewModel;
