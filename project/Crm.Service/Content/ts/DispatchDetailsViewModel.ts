import {namespace} from "@Main/namespace";
import {Breadcrumb} from "@Main/breadcrumbs";
import {HelperPerson} from "@Crm/helper/Helper.Person";
import {HelperAddress} from "@Crm/helper/Helper.Address";
import {HelperDispatch} from "@Crm.Service/helper/Helper.Dispatch";

export class DispatchDetailsViewModel extends window.Main.ViewModels.ViewModelBase {
	geolocationViewModel = new window.Crm.ViewModels.GenericListGeolocationViewModel();
	currentTabId = ko.observable<string>(null);
	previousTabId = ko.observable<string>(null);
	currentUser = ko.observable<Main.Rest.Model.Main_User>(null);
	vehicles = ko.observableArray<Crm.Article.Rest.Model.ObservableCrmArticle_Article>([]);
	tools = ko.observableArray<Crm.Article.Rest.Model.ObservableCrmArticle_Article>([]);
	currentUserDropboxAddress = ko.pureComputed<string>(() => {
		return this.currentUser() && this.currentUser().ExtensionValues.DropboxToken && window.Crm.Settings.DropboxDomain
			? "?BCC=" + this.currentUser().ExtensionValues.DropboxToken + "@" + window.Crm.Settings.DropboxDomain
			: "";
	});
	dispatch = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch>(null);
	serviceOrder = ko.pureComputed<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead>(() => {
		return this.dispatch() ? this.dispatch().ServiceOrder() : null;
	});
	serviceCase = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceCase>(null);
	contact = this.serviceOrder;
	contactId = ko.pureComputed<string>(() => {
		return this.serviceOrder() ? this.serviceOrder().Id() : null;
	});
	contactName = ko.pureComputed<string>(() => {
		return this.serviceOrder() ? this.serviceOrder().OrderNo() : null;
	});
	contactType = ko.pureComputed<string>(() => {
		return "ServiceOrder";
	});
	serviceObject = ko.pureComputed<Crm.Service.Rest.Model.ObservableCrmService_ServiceObject>(() => {
		return this.serviceOrder() ? this.serviceOrder().ServiceObject() : null;
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
	statusAlert = ko.pureComputed<string>(() => {
		if (!this.dispatch()) {
			return null;
		}
		if (["Scheduled", "Released", "Read"].indexOf(this.dispatch().StatusKey()) !== -1) {
			return "DispatchNotInProgressYetAlert";
		}
		if ("SignedByCustomer" === this.dispatch().StatusKey()) {
			return "DispatchIsAlreadySignedByCustomerAlert";
		}
		if (["ClosedNotComplete", "ClosedComplete"].indexOf(this.dispatch().StatusKey()) !== -1) {
			return "DispatchIsClosedAlert";
		}
		if ("Rejected" === this.dispatch().StatusKey()) {
			return "DispatchIsRejectedAlert";
		}
		if (["CancelledNotComplete", "Cancelled"].includes(this.dispatch().StatusKey())) {
			return "DispatchIsCancelledAlert";
		}
		return null;
	});
	canEditCustomerContact = ko.observable<boolean>(false);
	canEditInitiator = ko.observable<boolean>(false);
	serviceOrderIsEditable = ko.pureComputed<boolean>(() => {
		const hasPermission = window.AuthorizationManager.isAuthorizedForAction("ServiceOrder", "Edit");
		return hasPermission || this.serviceOrder().CreateUser() === this.currentUser().Id;
	});
	dispatchIsCompletable = ko.pureComputed<boolean>(() => {
		if (!this.dispatch()) {
			return false;
		}
		return HelperDispatch.dispatchIsEditable(this.dispatch()) || HelperDispatch.dispatchIsNotStarted(this.dispatch()) || this.dispatch().StatusKey() === "SignedByCustomer";
	});
	lookups: LookupType = {
		addressTypes: {$tableName: "Crm_AddressType"},
		components: {$tableName: "CrmService_Component"},
		countries: { $tableName: "Main_Country" },
		currencies: { $tableName: "Main_Currency" },
		emailTypes: {$tableName: "Crm_EmailType"},
		errorCodes: { $tableName: "CrmService_ErrorCode" },
		expenseTypes: { $tableName: "CrmPerDiem_ExpenseType" },
		faxTypes: {$tableName: "Crm_FaxType"},
		invoicingTypes: {$tableName: "Main_InvoicingType"},
		phoneTypes: {$tableName: "Crm_PhoneType"},
		regions: {$tableName: "Main_Region"},
		serviceObjectCategories: {$tableName: "CrmService_ServiceObjectCategory"},
		serviceOrderStatuses: {$tableName: "CrmService_ServiceOrderStatus"},
		serviceOrderTypes: {$tableName: "CrmService_ServiceOrderType"},
		servicePriorities: {$tableName: "CrmService_ServicePriority"},
		websiteTypes: {$tableName: "Crm_WebsiteType"},
		noPreviousSerialNoReasons: { $tableName: "CrmService_NoPreviousSerialNoReason" },
		distanceUnits: { $tableName: "CrmPerDiem_DistanceUnit" },
		serviceOrderDispatchStatuses: {$tableName: "CrmService_ServiceOrderDispatchStatus"},
		languages: { $tableName: "Main_Language" },
	};
	maintenanceOrderGenerationMode = ko.observable<string>(window.Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode);
	tabs = ko.observable<{}>({});
	isEditable = ko.observable<boolean>(true);
	showInvoiceData = ko.pureComputed<boolean>(() => false);
	showTotalPrices = ko.pureComputed<boolean>(() => false);
	selectedLanguage = ko.observable<string>(null);
	userGroupId: string;
	title = ko.observable<string>(null);
	customAddress = ko.observable<boolean>(false);
	customContactPerson = ko.observable<boolean>(false);
	selectedAddress = ko.observable<string>(null);
	selectedAddressObject = ko.observable<Crm.Rest.Model.Crm_Address>(null);
	selectedContactPerson = ko.observable<string>(null);
	selectedContactPersonObject = ko.observable(null);
	serviceOrderStatisticsKey = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderStatisticsKey>(null);
	contactWarning = ko.observable<boolean>(false);
	addressWarning = ko.observable<boolean>(false);
	installations = ko.computed<Crm.Service.Rest.Model.CrmService_Installation[]>(() => {
		if (!this.dispatch() || !this.dispatch().ServiceOrder()) {
			return [];
		}
		if (window.Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode === "JobPerInstallation") {
			return this.dispatch().ServiceOrder().ServiceOrderTimes();
		} else {
			let installationArray = [];
			if (this.dispatch().ServiceOrder().Installation()) {
				installationArray.push(this.dispatch().ServiceOrder().Installation().innerInstance);
			}
			return installationArray;
		}
	});
	constructor() {
		super();
	}

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		await this.geolocationViewModel.init();
		if (params && params.tab === "tab-report") {
			params.tab = "tab-details";
		}
		let dispatch = await window.database.CrmService_ServiceOrderDispatch
			.include("CurrentServiceOrderTime")
			.include("CurrentServiceOrderTime.Installation")
			.include("ServiceOrder")
			.include("ServiceOrder.Installation")
			.include("ServiceOrder.InitiatorPerson")
			.include("ServiceOrder.InitiatorPerson.Emails")
			.include("ServiceOrder.InitiatorPerson.Phones")
			.include("ServiceOrder.InvoiceRecipient")
			.include("ServiceOrder.Payer")
			.include("ServiceOrder.Station")
			.include("ServiceOrder.ServiceOrderTimes")
			.include("ServiceOrder.ServiceOrderTimes.Installation")
			.include("DispatchedUser")
			.include("ReportRecipients")
			.find(id);
		this.dispatch(dispatch.asKoObservable());
		const queries = [];
		queries.push({
			queryable: window.database.Crm_Company
				.include2("Addresses.filter(function(x) { x.IsCompanyStandardAddress === true; })")
				.filter("it.Id === this.id", {id: this.dispatch().ServiceOrder().CustomerContactId()}),
			method: "first",
			handler: this.dispatch().ServiceOrder().Company
		});
		if (this.dispatch().ServiceOrder().InitiatorId()) {
			queries.push({
				queryable: window.database.Crm_Company
					.include2("Addresses.filter(function(x) { x.IsCompanyStandardAddress === true; })")
					.filter("it.Id === this.id", {id: this.dispatch().ServiceOrder().InitiatorId()}),
				method: "first",
				handler: this.dispatch().ServiceOrder().Initiator
			});
		}
		if (this.dispatch().ServiceOrder().ServiceObjectId()) {
			queries.push({
				queryable: window.database.CrmService_ServiceObject
					.include2("Addresses.filter(function(x) { x.IsCompanyStandardAddress === true; })")
					.include("Addresses.Emails")
					.include("Addresses.Faxes")
					.include("Addresses.Phones")
					.include("Addresses.Websites")
					.filter("it.Id === this.id", {id: this.dispatch().ServiceOrder().ServiceObjectId()}),
				method: "first",
				handler: this.dispatch().ServiceOrder().ServiceObject
			});
		}
		queries.push({
			queryable: window.database.CrmService_ServiceOrderStatisticsKey
				.filter(function(it){ return it.ServiceOrderId === this.serviceOrderId && (it.DispatchId === this.dispatchId || it.DispatchId === null); }, {dispatchId: this.dispatch().Id(), serviceOrderId: this.dispatch().OrderId()}),
			method: "toArray",
			handler: (results: Crm.Service.Rest.Model.CrmService_ServiceOrderStatisticsKey[]) => this.serviceOrderStatisticsKey((results.find(x => x.DispatchId !== null) ?? results[0] ?? window.database.CrmService_ServiceOrderStatisticsKey.defaultType.create()).asKoObservable()),
		});
		await window.Helper.Batch.Execute(queries);
		let user = await window.Helper.User.getCurrentUser();
		this.currentUser(user);
		await window.Helper.StatisticsKey.getAvailableLookups(this.lookups, this.serviceOrderStatisticsKey());
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		let serviceOrderStatusIsInSchedulingOrInProgress = await window.Helper.ServiceOrder.isInStatusGroup(this.dispatch().ServiceOrder().Id(), ["Scheduling", "InProgress"], this.dispatch().ServiceOrder().StatusKey());
		if (this.dispatch().StatusKey() === "Released" && serviceOrderStatusIsInSchedulingOrInProgress && this.dispatch().DispatchedUser().Id() === this.currentUser().Id) {
			window.database.attachOrGet(this.dispatch().innerInstance);
			this.dispatch().StatusKey("Read");
			await window.database.saveChanges();
		}
		this.title = ko.observable<string>(this.dispatch().DispatchNo() !== null ? this.dispatch().DispatchNo() : this.serviceOrder().OrderNo());
		if (this.dispatch().ExtensionValues().TeamId() !== null) {
			//@ts-ignore
			this.userGroup(await this.getUserGroup(this.dispatch().ExtensionValues().TeamId()));
			await this.getUserGroupMembers();
			await this.loadTeamVehiclesAndTools();
		}
		else await this.loadTechnicianVehiclesAndTools(); 
		await this.setBreadcrumbs(this.title());
		const addresses = await window.database.Crm_Address.filter("it.City == city && it.Street == street && it.CountryKey == countryKey && it.RegionKey == regionKey && it.ZipCode == zipCode && it.Name1 == name1 && it.Name2 == name2 && it.Name3 == name3",
		{
			city : this.serviceOrder().City(),
			street : this.serviceOrder().Street(),
			countryKey : this.serviceOrder().CountryKey(),
			regionKey : this.serviceOrder().RegionKey(),
			zipCode : this.serviceOrder().ZipCode(),
			name1 : this.serviceOrder().Name1(),
			name2 : this.serviceOrder().Name2(),
			name3 : this.serviceOrder().Name3()
		}).toArray();
		if (addresses.length!=0){
			this.selectedAddress(addresses[0].Id);
		}
		await window.Helper.Database.registerEventHandlers(this, {
			"CrmService_ServiceOrderStatisticsKey": {
				"afterCreate": async (sender, serviceOrderStatisticsKey) => {
					this.loading(true);
					this.serviceOrderStatisticsKey(serviceOrderStatisticsKey.asKoObservable());
					this.loading(false);
				},
				"afterUpdate": async (sender, serviceOrderStatisticsKey) => {
					this.loading(true);
					this.serviceOrderStatisticsKey(serviceOrderStatisticsKey.asKoObservable());
					this.loading(false);
				}
			}
		});
	};

	getServiceOrderPositionItemGroup(serviceOrderPosition: { ServiceOrderTime: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime>}): ItemGroup {
		const itemGroup = window.Helper.ServiceOrder.getServiceOrderPositionItemGroup(serviceOrderPosition);
		if (itemGroup && this.dispatch && this.dispatch() && this.dispatch().CurrentServiceOrderTimeId() && serviceOrderPosition.ServiceOrderTime() && serviceOrderPosition.ServiceOrderTime().Id() === this.dispatch().CurrentServiceOrderTimeId()) {
			itemGroup.css = "c-green";
			itemGroup.title += " (" + window.Helper.String.getTranslatedString("CurrentServiceOrderTime") + ")";
		}
		return itemGroup;
	};

	async confirm(): Promise<void> {
		window.database.attachOrGet(this.dispatch().innerInstance);
		this.loading(true);
		this.dispatch().StatusKey("Released");
		await window.database.saveChanges();
		await this.init(this.dispatch().Id());
		this.loading(false);
	};

	async workOnDispatch(): Promise<void> {
		this.loading(true);
		window.database.attachOrGet(this.dispatch().innerInstance);
		window.database.attachOrGet(this.serviceOrder().innerInstance);
		if (window.Crm.Service.Settings.ServiceOrderDispatch.ReadGeolocationOnDispatchStart) {
			// prefer gps locations to cell tower locations and don't allow cached locations
			const positionOptions = {enableHighAccuracy: true, maximumAge: 0, timeout: 10000};
			await new Promise<void>((resolve, reject) => {
				navigator.geolocation.getCurrentPosition(position => {
					const latitude = position.coords.latitude;
					const longitude = position.coords.longitude;
					window.database.Main_User.find(this.currentUser().Id).then(user => {
						window.database.attachOrGet(user);
						user.Latitude = latitude;
						user.Longitude = longitude;
						user.LastStatusUpdate = new Date();
						this.dispatch().LatitudeOnDispatchStart(latitude);
						this.dispatch().LongitudeOnDispatchStart(longitude);
						resolve();
					}).catch(reject);
				}, error => {
					window.Log.warn("getting current position via geolocation api failed, error: " + error.message);
					resolve();
				}, positionOptions);
			});
		}

		let isServiceOrderInScheduling = await window.Helper.ServiceOrder.isInStatusGroup(this.serviceOrder().Id(), "Scheduling");
		if (isServiceOrderInScheduling) {
			window.database.attachOrGet(this.serviceOrder());
			this.serviceOrder().StatusKey("InProgress");
		}
		this.dispatch().StatusKey("InProgress");
		if (this.serviceOrder().StatusKey() === "PartiallyReleased" || this.serviceOrder().StatusKey() === "Released") {
			this.serviceOrder().StatusKey("InProgress");
		}
		await window.database.saveChanges();
		if (window.Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode !== "JobPerInstallation" && !window.Crm.Service.Settings.ServiceOrderDispatch.ToggleSingleJob) {
			this.loading(false);
			return;
		}
		if (this.dispatch().CurrentServiceOrderTimeId()) {
			this.loading(false);
			return;
		}
		let jobs = await window.database.CrmService_ServiceOrderTime
			.filter("it.OrderId === this.orderId", {orderId: this.serviceOrder().Id()})
			.take(2)
			.toArray();
		if (jobs && jobs.length === 1) {
			await window.Helper.Dispatch.toggleCurrentJob(this.dispatch, jobs[0].Id)
		}
		this.loading(false);
	};

	async setBreadcrumbs(title: string): Promise<void> {
		if (!window.breadcrumbsViewModel) {
			return;
		}
		await window.breadcrumbsViewModel.setBreadcrumbs([
			new Breadcrumb(window.Helper.String.getTranslatedString("Dispatch"), "Dispatch::Index", "#/Crm.Service/ServiceOrderDispatchList/IndexTemplate?status=upcoming"),
			new Breadcrumb(title, null, window.location.hash)
		]);

	};

	onStationSelect(station: Crm.Rest.Model.Crm_Station): void {
		if (station) {
			this.serviceOrder().Station(station.asKoObservable());
			this.serviceOrder().StationKey(station.Id);
		} else {
			this.serviceOrder().Station(null);
			this.serviceOrder().StationKey(null);
		}
	};

	toggleCustomContactPerson(serviceOrder: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead): void {
		let viewModel = this;
		if (!viewModel.customContactPerson() && !viewModel.selectedContactPersonObject()) {
			let obj: { name: string, email: string, phone: string; } = {
				name: serviceOrder.ServiceLocationResponsiblePerson(),
				email: serviceOrder.ServiceLocationEmail(),
				phone: serviceOrder.ServiceLocationPhone()
			};
			viewModel.selectedContactPersonObject(obj);
		}
		if (viewModel.customContactPerson()) {
			let changed = false;
			if (!(viewModel.selectedContactPersonObject() instanceof window.database.Crm_Person.defaultType)) {
				if (viewModel.selectedContactPersonObject().name !== serviceOrder.ServiceLocationResponsiblePerson() ||
					viewModel.selectedContactPersonObject().email !== serviceOrder.ServiceLocationEmail() ||
					viewModel.selectedContactPersonObject().phone !== serviceOrder.ServiceLocationPhone()) {
					changed = true;
				}
			} else {
				if (HelperPerson.getDisplayName(viewModel.selectedContactPersonObject()) !== serviceOrder.ServiceLocationResponsiblePerson() ||
					window.Helper.Address.getPhoneNumberAsString(HelperAddress.getPrimaryCommunication(viewModel.selectedContactPersonObject().Phones), true, viewModel.lookups.countries) !== serviceOrder.ServiceLocationPhone() ||
					HelperAddress.getPrimaryCommunication(viewModel.selectedContactPersonObject().Emails).Data !== serviceOrder.ServiceLocationEmail()) {
					changed = true;
				}
			}
			if (changed) {
				viewModel.contactWarning(!viewModel.contactWarning());
			}
		}
		if (viewModel.contactWarning()) {
			return;
		}
		viewModel.customContactPerson(!viewModel.customContactPerson());
		if (!viewModel.customContactPerson() && viewModel.selectedContactPersonObject() instanceof window.database.Crm_Person.defaultType) {
			viewModel.selectedContactPersonOnSelect(serviceOrder, viewModel.selectedContactPersonObject());
		}
		if (!viewModel.customContactPerson() && !(viewModel.selectedContactPersonObject() instanceof window.database.Crm_Person.defaultType)) {
			serviceOrder.ServiceLocationResponsiblePerson(viewModel.selectedContactPersonObject().name);
			serviceOrder.ServiceLocationEmail(viewModel.selectedContactPersonObject().email);
			serviceOrder.ServiceLocationPhone(viewModel.selectedContactPersonObject().phone);
		}
	};

	toggleCustomAddress(serviceOrder: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead): void {
		let viewModel = this;
		let properties = ["Name1", "Name2", "Name3", "Street", "ZipCode", "City", "CountryKey", "RegionKey"];
		if (!viewModel.customAddress() && !viewModel.selectedAddressObject()) {
			let obj = new Crm.Rest.Model.Crm_Address();
			for (const prop of properties) {
				obj[prop] = serviceOrder[prop]();
			}
			viewModel.selectedAddressObject(obj);
		}
		if (viewModel.customAddress()) {
			let changed = false;
			for (const prop of properties) {
				if (viewModel.selectedAddressObject()[prop] !== serviceOrder[prop]()) {
					changed = true;
					break;
				}
			}
			if (changed) {
				viewModel.addressWarning(!viewModel.addressWarning());
			}
		}
		if (viewModel.addressWarning()) {
			return;
		}
		viewModel.customAddress(!viewModel.customAddress());
		if (!viewModel.customAddress() && viewModel.selectedAddressObject()) {
			viewModel.selectedAddressOnSelect(serviceOrder, viewModel.selectedAddressObject());
		}
	};

	selectedContactPersonOnSelect(serviceOrder: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead, selectedContact: Crm.Rest.Model.Crm_Person): void {
		let viewModel = this;
		viewModel.selectedContactPersonObject(selectedContact);
		let properties = ["ServiceLocationResponsiblePerson", "ServiceLocationPhone", "ServiceLocationEmail"];
		if (selectedContact) {
			serviceOrder.ServiceLocationResponsiblePerson(HelperPerson.getDisplayName(selectedContact));
			if (selectedContact.Phones.length > 0)
				serviceOrder.ServiceLocationPhone(HelperAddress.getPhoneNumberAsString(HelperAddress.getPrimaryCommunication(selectedContact.Phones), true, viewModel.lookups.countries));
			if (selectedContact.Emails.length > 0)
				serviceOrder.ServiceLocationEmail(HelperAddress.getPrimaryCommunication(selectedContact.Emails).Data);
		} else {
			serviceOrder.ServiceLocationResponsiblePerson(null);
			serviceOrder.ServiceLocationPhone(null);
			serviceOrder.ServiceLocationEmail(null);
		}
	};

	selectedAddressOnSelect(serviceOrder: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead, selectedAddress: Crm.Rest.Model.Crm_Address): void {
		let viewModel = this;
		viewModel.selectedAddressObject(selectedAddress);
		if (selectedAddress) {
			serviceOrder.Name1(selectedAddress.Name1);
			serviceOrder.Name2(selectedAddress.Name2);
			serviceOrder.Name3(selectedAddress.Name3);
			serviceOrder.Street(selectedAddress.Street);
			serviceOrder.ZipCode(selectedAddress.ZipCode);
			serviceOrder.City(selectedAddress.City);
			serviceOrder.CountryKey(selectedAddress.CountryKey);
			serviceOrder.RegionKey(selectedAddress.RegionKey);
		} else {
			serviceOrder.Name1(null);
			serviceOrder.Name2(null);
			serviceOrder.Name3(null);
			serviceOrder.Street(null);
			serviceOrder.ZipCode(null);
			serviceOrder.City(null);
			serviceOrder.CountryKey(null);
			serviceOrder.RegionKey(null);
		}
	};

	onLocationPmbCancel(): void {
		let viewModel = this;
		viewModel.customContactPerson(false);
		viewModel.customAddress(false);
		viewModel.selectedContactPerson(null);
		viewModel.selectedContactPersonObject(null);
		viewModel.selectedAddress(null);
		viewModel.selectedAddressObject(null);
	}

	async loadTeamVehiclesAndTools(): Promise<void> {
		const articles = await window.database.CrmServiceTeam_ArticleUserGroupRelationship
			.include("Article")
			.filter(function (it) {
				return it.UserGroupKey === this.teamId && it.From <= this.dispatchDate && it.To >= this.dispatchDate;
			}, {
				teamId: this.dispatch().ExtensionValues().TeamId(),
				dispatchDate: this.dispatch().Date()
			})
			.map("it.Article")
			.toArray();
		this.tools(articles.filter(x => x.ArticleTypeKey === 'Tool'));
		this.vehicles(articles.filter(x => x.ArticleTypeKey === 'Vehicle'));
	}

	async loadTechnicianVehiclesAndTools(): Promise<void> {
		const articles = await window.database.CrmArticle_ArticleUserRelationship
			.include("Article")
			.filter(function (it) {
				return it.UserKey === this.username && it.From <= this.dispatchDate && it.To >= this.dispatchDate;
			}, {
				username: this.dispatch().Username(),
				dispatchDate: this.dispatch().Date()
			})
			.map("it.Article")
			.toArray();
		this.tools(articles.filter(x => x.ArticleTypeKey === 'Tool'));
		this.vehicles(articles.filter(x => x.ArticleTypeKey === 'Vehicle'));
	}

	async getUserGroup(userGroupId: string): Promise<Main.Rest.Model.ObservableMain_Usergroup> {
		const userGroup = await window.database.Main_Usergroup
			.filter(it => it.Id === this.userGroupId, {userGroupId})
			.first();
		return userGroup.asKoObservable();
	}

	async getUserGroupMembers(): Promise<void> {
		await window.database.Main_UserUserGroup
			.include("User")
			.filter(function (it) {
				return it.UserGroupKey === this.teamId;
			}, {
				teamId: this.dispatch().ExtensionValues().TeamId(),
			})
			.map("it.User")
			//@ts-ignore
			.toArray(result => this.teamMembers(result.filter(x => x.Id !== this.dispatch().Username())));
	}
	
	getLocalTimeZoneText = window.Crm.Service.ViewModels.ServiceOrderDispatchListIndexViewModel.prototype.getLocalTimeZoneText;
	contactPersonFilter = window.Crm.Service.ViewModels.ServiceOrderCreateViewModel.prototype.contactPersonFilter;
	filterAddresses = window.Crm.Service.ViewModels.ServiceOrderCreateViewModel.prototype.filterAddresses;
	formatAddress = window.Crm.Service.ViewModels.ServiceOrderCreateViewModel.prototype.formatAddress;
	getInstallationAutocompleteFilter = window.Crm.Service.ViewModels.ServiceOrderCreateViewModel.prototype.installationFilter;
	isInstallationAddress = window.Crm.Service.ViewModels.ServiceOrderCreateViewModel.prototype.isInstallationAddress;
	isServiceObjectAddress = window.Crm.Service.ViewModels.ServiceOrderCreateViewModel.prototype.isServiceObjectAddress;
	getDirection = window.Crm.Service.ViewModels.ServiceOrderHeadListIndexViewModel.prototype.getDirection;
	getDistance = window.Crm.Service.ViewModels.ServiceOrderHeadListIndexViewModel.prototype.getDistance;
	onPayerSelect = window.Crm.Service.ViewModels.ServiceOrderDetailsViewModel.prototype.onPayerSelect;
	payerAddressFilter = window.Crm.Service.ViewModels.ServiceOrderDetailsViewModel.prototype.payerAddressFilter;
	onInvoiceRecipientSelect = window.Crm.Service.ViewModels.ServiceOrderDetailsViewModel.prototype.onInvoiceRecipientSelect;
	invoiceRecipientAddressFilter = window.Crm.Service.ViewModels.ServiceOrderDetailsViewModel.prototype.invoiceRecipientAddressFilter;
}

namespace("Crm.Service.ViewModels").DispatchDetailsViewModel = DispatchDetailsViewModel;