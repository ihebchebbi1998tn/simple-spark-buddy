import moment from "moment";
import { namespace } from "@Main/namespace";
import type {VisibilityViewModel} from "@Main/VisibilityViewModel";
import { Breadcrumb } from "@Main/breadcrumbs";

export class ServiceOrderCreateViewModel extends window.Main.ViewModels.ViewModelBase {
	installationIds = ko.observableArray<string>([]);
	installations = ko.observableArray<Crm.Service.Rest.Model.CrmService_Installation>([]);
	currentUser = ko.observable<Main.Rest.Model.Main_User>(null);
	customAddress = ko.observable<boolean>(false);
	customContactPerson = ko.observable<boolean>(false);
	selectedAddress = ko.observable<string>(null);
	selectedContactPerson = ko.observable<string>(null);
	selectedInstallation = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_Installation>(null);
	selectedServiceOrderType = ko.observable<Crm.Service.Rest.Model.Lookups.CrmService_ServiceOrderType>(null);
	serviceOrder = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead>(null);
	visibilityViewModel: VisibilityViewModel = new window.VisibilityViewModel(this.serviceOrder, "ServiceOrderHead");
	errors = ko.validation.group([this.serviceOrder]);
	lookups: LookupType = {
		serviceOrderTypes: {$tableName: "CrmService_ServiceOrderType"},
		servicePriorities: {$tableName: "CrmService_ServicePriority"},
		installationTypes: {$tableName: "CrmService_InstallationType"},
		installationHeadStatuses: {$tableName: "CrmService_InstallationHeadStatus"}
	};
	latitude = ko.observable<string>();
	longitude = ko.observable<string>();
	showCoordinate = ko.observable<boolean>(false);
	isThereAnyActiveServiceOrderTemplate = ko.observable<boolean>(false);
	isThereAnyActiveServiceObject = ko.observable<boolean>(false);
	isThereAnyErrorCode = ko.observable<boolean>(false);
	initiatorBelongsToCustomer = ko.observable<boolean>(true);

	constructor() {
		super();
		this.installations.subscribe(list => {
			this.onInstallationSelect(list[list.length - 1]);
		});

		window.Helper.Address.addValidationRules(this.latitude, this.longitude);

		this.latitude.subscribe((val) => {
			this.serviceOrder().Latitude(window.Helper.Address.convertDegreesToDecimal(val));
		});
		this.longitude.subscribe((val) => {
			this.serviceOrder().Longitude(window.Helper.Address.convertDegreesToDecimal(val));
		});
	}

	cancel(): void {
		window.database.detach(this.serviceOrder().innerInstance);
		window.history.back();
	};

	customerContactFilter(query: $data.Queryable<Crm.Rest.Model.Crm_Company>, term: string): $data.Queryable<Crm.Rest.Model.Crm_Company> {
		if (term) {
			query = window.Helper.String.contains(query, term, ["LegacyId", "Name"]);
		}
		return query.filter("it.IsEnabled === true");
	};

	filterAddresses(query: $data.Queryable<Crm.Rest.Model.Crm_Address>, term: string): $data.Queryable<Crm.Rest.Model.Crm_Address> {
		if (term) {
			query = window.Helper.String.contains(query, term, ["Name1", "Name2", "Name3", "ZipCode", "City", "Street"]);
		}
		const addressIds = window._.uniq(window._.compact(this.installations().map(function (x) {
			return x.LocationAddressKey;
		})));
		const addressCompanyIds = window._.uniq(window._.compact([this.serviceOrder().CustomerContactId(), this.serviceOrder().ServiceObjectId()]));
		query = query.filter(function (it) {
				return it.Id in this.ids || it.CompanyId in this.companyIds;
			},
			{ids: addressIds, companyIds: addressCompanyIds});
		query = query.orderByDescending("it.IsCompanyStandardAddress");
		return query;
	};

	formatAddress(data: Select2AutoCompleterResult): string | JQuery {
		if (data.item) {
			let result;
			if (this.isServiceObjectAddress(data.item)) {
				result = window.Helper.String.getTranslatedString("ServiceObject");
			} else if (this.isInstallationAddress(data.item)) {
				result = window.Helper.String.getTranslatedString("Installation");
			} else {
				result = window.Helper.String.getTranslatedString("Company");
			}
			result += ": " + window.Helper.Address.getAddressLine(data.item);
			if (data.item.IsCompanyStandardAddress) {
				return $('<span style="font-weight: bold;"></span>').text(result);
			}
			return result;
		}
		return data.text;
	};

	getInstallationsByIds(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_Installation>, ids: string[]): $data.Queryable<Crm.Service.Rest.Model.CrmService_Installation> {
		return query.filter(function (it) {
				return it.Id in this.ids;
			},
			{ids: ids});
	};

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		let activeServiceOrderTemplateCount = await window.database.CrmService_ServiceOrderHead
			.filter(serviceOrder => serviceOrder.IsTemplate === true && serviceOrder.IsActive === true).count();
		this.isThereAnyActiveServiceOrderTemplate(activeServiceOrderTemplateCount > 0);

		let activeServiceObjectCount = await window.database.CrmService_ServiceObject
			.filter(serviceObject => serviceObject.IsActive === true).count();
		this.isThereAnyActiveServiceObject(activeServiceObjectCount > 0);

		let errorCodeCount = await window.database.CrmService_ErrorCode.count();
		this.isThereAnyErrorCode(errorCodeCount > 0);

		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		let currentUser = await window.Helper.User.getCurrentUser();
		this.currentUser(currentUser);
		const serviceOrder = window.database.CrmService_ServiceOrderHead.defaultType.create();
		serviceOrder.PriorityKey = window.Helper.Lookup.getDefaultLookupValueSingleSelect(this.lookups.servicePriorities, serviceOrder.PriorityKey);
		serviceOrder.Reported = new Date();
		serviceOrder.LastActivity = new Date();
		serviceOrder.StationKey = currentUser.ExtensionValues.StationIds.length === 1 ? currentUser.ExtensionValues.StationIds[0] : null;
		serviceOrder.StatusKey = "New";
		serviceOrder.ResponsibleUser = currentUser.Id;
		serviceOrder.TypeKey = window.Helper.Lookup.getDefaultLookupValueSingleSelect(this.lookups.serviceOrderTypes, serviceOrder.TypeKey);
		if (params && params.customerContactId) {
			serviceOrder.CustomerContactId = params.customerContactId;
		}
		if (params && params.serviceObjectId) {
			serviceOrder.ServiceObjectId = params.serviceObjectId;
		}
		if (params && params.installationId) {
			if (window.Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode === "JobPerInstallation") {
				this.installationIds.push(params.installationId);
			} else {
				serviceOrder.InstallationId = params.installationId;
			}
		}
		if (params && params.followUpServiceOrderRemark) {
			serviceOrder.ErrorMessage = params.followUpServiceOrderRemark;
		}
		this.serviceOrder(serviceOrder.asKoObservable());
		this.serviceOrder().UserGroupKey.subscribe(() => {
			if (this.serviceOrder().UserGroupKey() === null) {
				this.serviceOrder().ResponsibleUser(this.currentUser().Id)
			} else {
				this.serviceOrder().ResponsibleUser(null);
			}
		});
		this.serviceOrder().PreferredTechnicianUsergroupKey.subscribe(() => {
			this.serviceOrder().PreferredTechnician(null);
		});
		let currency = await window.database.Main_Currency.filter(x => x.Favorite === true).take(1).toArray();
		this.serviceOrder().CurrencyKey(currency[0] ? currency[0].Key : null);
		const favoriteTypes = this.lookups.serviceOrderTypes.$array.filter(function (type) {
			return type.Favorite;
		}).sort(function (a, b) {
			return a.SortOrder - b.SortOrder;
		});
		if (!!favoriteTypes[0]) {
			this.serviceOrder().TypeKey(favoriteTypes[0].Key);
		}
		this.serviceOrder().Reported(moment().toDate());
		await this.visibilityViewModel.init();
		await this.setBreadcrumbs();
		window.database.add(this.serviceOrder().innerInstance);
	};

	onServiceOrderTypeSelect(serviceOrderType) {
		this.selectedServiceOrderType(serviceOrderType)
		this.serviceOrder().InvoicingTypeKey(serviceOrderType.DefaultInvoicingTypeKey)
	}

	initiatorFilter(query: $data.Queryable<Crm.Rest.Model.Crm_Company>, term: string): $data.Queryable<Crm.Rest.Model.Crm_Company> {
		if (term) {
			query = window.Helper.String.contains(query, term, ["LegacyId", "Name"]);
		}
		return query.filter("it.IsEnabled === true");
	};

	initiatorPersonFilter(serviceOrder: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead, query: $data.Queryable<Crm.Rest.Model.Crm_Person>, term: string): $data.Queryable<Crm.Rest.Model.Crm_Person> {
		if (term) {
			query = window.Helper.String.contains(query, term, ["Firstname", "Surname"]);
		}
		if (serviceOrder.InitiatorId()) {
			query = query.filter(function (it) {
					return it.ParentId === this.initiatorId;
				},
				{initiatorId: serviceOrder.InitiatorId()});
		}
		return query.filter("it.IsRetired === false");
	};

	installationFilter(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_Installation>, term: string): $data.Queryable<Crm.Service.Rest.Model.CrmService_Installation> {
		query = query
			.include("Address")
			.include("Company")
			.include("ServiceObject");

		if (term) {
			query = window.Helper.String.contains(query, term, ["LegacyId", "InstallationNo", "Address.Street", "Address.City", "Address.ZipCode", "ExternalReference", "Room", "ExactPlace", "Description"]);
		}
		if (this.serviceOrder().ServiceObjectId()) {
			const serviceObjectCondition = "(this.serviceObjectId === null || it.FolderId === this.serviceObjectId)";
			query =  query.filter(serviceObjectCondition,
				{
					serviceObjectId: this.serviceOrder().ServiceObjectId()
				});
		}
		if (window.Crm.Service.Settings.ServiceOrder.OnlyInstallationsOfReferencedCustomer) {
			const companyCondition = "(this.customerContactId === null || it.LocationContactId === this.customerContactId)";
			query = query.filter(companyCondition,
				{
					customerContactId: this.serviceOrder().CustomerContactId() || null
				});
		}
		return query;
	};

	isInstallationAddress(address: Crm.Rest.Model.Crm_Address): boolean {
		if (this.serviceOrder().Installation()) {
			return this.serviceOrder().Installation().LocationAddressKey() === address.Id;
		}
		return this.installations().some(function (x) {
			return x.LocationAddressKey === address.Id;
		});
	};

	isServiceObjectAddress(address: Crm.Rest.Model.Crm_Address): boolean {
		return this.serviceOrder().ServiceObjectId() === address.CompanyId;
	};

	onCustomerContactSelect(customerContact: Crm.Rest.Model.Crm_Company): void {
		if (customerContact) {
			this.serviceOrder().Company(customerContact.asKoObservable());
			this.serviceOrder().CustomerContactId(customerContact.Id);
			if (window.Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode === "OrderPerInstallation" &&
				this.serviceOrder().Installation() &&
				this.serviceOrder().Installation().LocationContactId() !== customerContact.Id) {
				this.onInstallationSelect(null);
			} else {
				const removed = this.installations.remove(function (x) {
					return x.LocationContactId !== customerContact.Id;
				});
				this.installationIds.removeAll(removed.map(function (x) {
					return x.Id;
				}));
			}
			if (!this.serviceOrder().StationKey()) {
				if (customerContact.StationKey) {
					this.serviceOrder().StationKey(customerContact.StationKey);
				} else if (this.currentUser().ExtensionValues.StationIds?.length > 0) {
					this.serviceOrder().StationKey(this.currentUser().ExtensionValues.StationIds[0]);
				}
			}
		} else {
			this.serviceOrder().Company(null);
			this.serviceOrder().CustomerContactId(null);
		}

		if (!this.customAddress() && this.selectedInstallation()) {
			this.selectedAddress(this.selectedInstallation().Address().Id());
			this.selectedAddressOnSelect(this.selectedInstallation().innerInstance.Address);
		} else {
			this.selectedAddress(null);
		} 
	};

	onInitiatorSelect(initiator: Crm.Rest.Model.Crm_Company): void {
		if (initiator) {
			this.serviceOrder().Initiator(initiator.asKoObservable());
			this.serviceOrder().InitiatorId(initiator.Id);
			if (this.serviceOrder().InitiatorPerson() && this.serviceOrder().InitiatorPerson().ParentId() !== initiator.Id) {
				this.serviceOrder().InitiatorPerson(null);
				this.serviceOrder().InitiatorPersonId(null);
			}
		} else {
			this.serviceOrder().Initiator(null);
			this.serviceOrder().InitiatorId(null);
			this.serviceOrder().InitiatorPerson(null);
			this.serviceOrder().InitiatorPersonId(null);
		}
	};

	onInstallationSelect(installation: Crm.Service.Rest.Model.CrmService_Installation): void {
		if (this.selectedInstallation()) {
			window.database.detach(this.selectedInstallation().innerInstance);
			this.selectedInstallation(null);
		}
		if (installation) {
			window.database.attachOrGet(installation);
			this.selectedInstallation(installation.asKoObservable());
			if (window.Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode === "OrderPerInstallation") {
				this.serviceOrder().Installation(installation.asKoObservable());
				this.serviceOrder().InstallationId(installation.Id);
			}
			if (!!this.serviceOrder().InitiatorId() === false) {
				this.serviceOrder().InitiatorId(installation.LocationContactId);
			}
			if (!!this.serviceOrder().CustomerContactId() === false && installation.Company) {
				this.onCustomerContactSelect(installation.Company);
			} else if (!this.customAddress()) {
				if (installation.Address) {
					this.selectedAddress(installation.Address.Id);
					this.selectedAddressOnSelect(installation.Address);
				} else {
					this.selectedAddress(null);
					this.selectedAddressOnSelect(null);
				}
			}
			if (installation.ServiceObject) {
				this.onServiceObjectSelect(installation.ServiceObject);
			}
		} else {
			if (window.Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode === "OrderPerInstallation") {
				this.serviceOrder().Installation(null);
				this.serviceOrder().InstallationId(null);
			}
			if (!this.customAddress()) {
				this.selectedAddress(null);
				this.selectedAddressOnSelect(null);
			}
		}
	};

	onServiceObjectSelect(serviceObject: Crm.Service.Rest.Model.CrmService_ServiceObject): void {
		if (serviceObject) {
			this.serviceOrder().ServiceObject(serviceObject.asKoObservable());
			this.serviceOrder().ServiceObjectId(serviceObject.Id);
			if (this.serviceOrder().Installation() &&
				this.serviceOrder().Installation().FolderId() !== serviceObject.Id) {
				this.onInstallationSelect(null);
			}
			const removed = this.installations.remove(function (x) {
				return x.FolderId !== serviceObject.Id;
			});
			this.installationIds.removeAll(removed.map(function (x) {
				return x.Id;
			}));
		} else {
			this.serviceOrder().ServiceObject(null);
			this.serviceOrder().ServiceObjectId(null);
			if (this.serviceOrder().Installation() && this.serviceOrder().Installation().LocationContactId() !== this.serviceOrder().Company().Id()) {
				this.onInstallationSelect(null);
			}
			const removed = this.installations.remove(x => x.LocationContactId !== this.serviceOrder().Company().Id());
			this.installationIds.removeAll(removed.map(x => x.Id));
		}
	};

	async onServiceOrderTemplateSelect(serviceOrderTemplate: Crm.Service.Rest.Model.CrmService_ServiceOrderHead): Promise<void> {
		if (serviceOrderTemplate) {
			this.loading(true);
			this.serviceOrder().ServiceOrderTemplate(serviceOrderTemplate.asKoObservable());
			this.serviceOrder().ServiceOrderTemplateId(serviceOrderTemplate.Id);
			await window.Helper.ServiceOrder.transferTemplateData(serviceOrderTemplate, this.serviceOrder(), false);
			this.loading(false);
		} else {
			this.loading(true);
			// @ts-ignore
			const serviceOrderClone = JSON.parse(window.ko.wrap.toJSON(this.serviceOrder)).innerInstance;
			await window.Helper.ServiceOrder.transferTemplateData(serviceOrderClone, this.serviceOrder(), true);
			this.loading(false);
		}
	};

	preferredTechnicianFilter(query: $data.Queryable<Main.Rest.Model.Main_User>, term: string): $data.Queryable<Main.Rest.Model.Main_User> {
		const serviceOrder = this.serviceOrder();
		// @ts-ignore
		if (query.specialFunctions.filterByPermissions[window.database.storageProvider.name]) {
			query = query.filter("filterByPermissions", "Dispatch::Edit");
		}
		return window.Helper.User.filterUserQuery(query, term, serviceOrder.PreferredTechnicianUsergroupKey());
	};

	serviceObjectFilter(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceObject>, term: string): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceObject> {
		if (term) {
			query = window.Helper.String.contains(query, term, ["LegacyId", "ObjectNo", "Name"]);
		}
		return query;
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
		let serviceOrderNo = await window.NumberingService.createNewNumberBasedOnAppSettings(window.Crm.Service.Settings.ServiceOrder.OrderNoIsGenerated, window.Crm.Service.Settings.ServiceOrder.OrderNoIsCreateable, this.serviceOrder().OrderNo(), this.selectedServiceOrderType().NumberingSequence, window.database.CrmService_ServiceOrderHead, "OrderNo");
		if (serviceOrderNo !== null) {
			this.serviceOrder().OrderNo(serviceOrderNo);
		}
		await new window.Helper.ServiceOrder.CreateServiceOrderData(this.serviceOrder(),
			this.serviceOrder().ServiceOrderTemplate(),
			this.installationIds()).create();
		await window.database.saveChanges();
		window.location.hash = "/Crm.Service/ServiceOrder/DetailsTemplate/" + this.serviceOrder().Id();
	};

	toggleCustomAddress(): void {
		this.customAddress(!this.customAddress());
		if (this.customAddress()) {
			this.selectedAddress(null);
		} else {
			this.serviceOrder().Name1(null);
			this.serviceOrder().Name2(null);
			this.serviceOrder().Name3(null);
			this.serviceOrder().Street(null);
			this.serviceOrder().ZipCode(null);
			this.serviceOrder().City(null);
			this.serviceOrder().CountryKey(null);
			this.serviceOrder().RegionKey(null);
		}
	};

	selectedAddressOnSelect(selectedAddress: Crm.Rest.Model.Crm_Address): void {
		if (selectedAddress) {
			this.serviceOrder().Name1(selectedAddress.Name1);
			this.serviceOrder().Name2(selectedAddress.Name2);
			this.serviceOrder().Name3(selectedAddress.Name3);
			this.serviceOrder().Street(selectedAddress.Street);
			this.serviceOrder().ZipCode(selectedAddress.ZipCode);
			this.serviceOrder().City(selectedAddress.City);
			this.serviceOrder().CountryKey(selectedAddress.CountryKey);
			this.serviceOrder().RegionKey(selectedAddress.RegionKey);
		} else {
			this.serviceOrder().Name1(null);
			this.serviceOrder().Name2(null);
			this.serviceOrder().Name3(null);
			this.serviceOrder().Street(null);
			this.serviceOrder().ZipCode(null);
			this.serviceOrder().City(null);
			this.serviceOrder().CountryKey(null);
			this.serviceOrder().RegionKey(null);
		}
	};

	toggleCustomContactPerson(): void {
		this.customContactPerson(!this.customContactPerson());
		if (this.customContactPerson()) {
			this.selectedContactPerson(null);
		} else {
			this.serviceOrder().ServiceLocationResponsiblePerson(null);
			this.serviceOrder().ServiceLocationPhone(null);
			this.serviceOrder().ServiceLocationEmail(null);
		}
	};

	async selectedContactPersonOnSelect(selectedContact: Crm.Rest.Model.Crm_Person): Promise<void> {
		if (selectedContact) {
			this.serviceOrder().ServiceLocationResponsiblePerson(window.Helper.Person.getDisplayName(selectedContact));
			if (selectedContact.Phones.length > 0)
				this.serviceOrder().ServiceLocationPhone(await window.Helper.Address.getPhoneNumberAsStringAsync(window.Helper.Address.getPrimaryCommunication(selectedContact.Phones), true));
			if (selectedContact.Emails.length > 0)
				this.serviceOrder().ServiceLocationEmail(window.Helper.Address.getPrimaryCommunication(selectedContact.Emails).Data);
		} else {
			this.serviceOrder().ServiceLocationResponsiblePerson(null);
			this.serviceOrder().ServiceLocationPhone(null);
			this.serviceOrder().ServiceLocationEmail(null);
		}
	};

	contactPersonFilter(query: $data.Queryable<Crm.Rest.Model.Crm_Person>, term: string): $data.Queryable<Crm.Rest.Model.Crm_Person> {
		if (term) {
			query = window.Helper.String.contains(query, term, ["Firstname", "Surname"]);
		}
		let installationContactIds = [];
		if (window.Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode === "OrderPerInstallation") {
			if (this.serviceOrder().Installation())
				installationContactIds = [this.serviceOrder().Installation().LocationContactId()];
		} else {
			installationContactIds = window._.uniq(window._.compact(this.installations().map(function (it) {
				return it.LocationContactId;
			})));
		}
		query = query.filter(function (it) {
				return it.ParentId === this.customerId || it.ParentId === this.objectContactId || it.ParentId in this.installationContactIds;
			},
			{
				customerId: this.serviceOrder().CustomerContactId() || null,
				objectContactId: this.serviceOrder().ServiceObjectId() || null,
				installationContactIds: installationContactIds
			});
		return query.filter("it.IsRetired === false");
	};

	toggleCoordinate(): void {
		this.showCoordinate(!this.showCoordinate())
	}

	toggleInitiatorBelongsToCustomer(): void {
		this.initiatorBelongsToCustomer(!this.initiatorBelongsToCustomer());
		if (this.initiatorBelongsToCustomer()) {
			this.serviceOrder().InitiatorPerson(null);
			this.serviceOrder().InitiatorPersonId(null);
			this.onInitiatorSelect(this.serviceOrder().Company()?.innerInstance ?? null);
		}
	}

	async setBreadcrumbs(): Promise<void> {
		await window.breadcrumbsViewModel.setBreadcrumbs([
			new Breadcrumb(window.Helper.getTranslatedString("ServiceOrder"), "ServiceOrder::Index", "#/Crm.Service/ServiceOrderHeadList/IndexTemplate"),
			new Breadcrumb(window.Helper.getTranslatedString("CreateServiceOrder"), null, window.location.hash, null, null)
		]);
	};
}

namespace("Crm.Service.ViewModels").ServiceOrderCreateViewModel = ServiceOrderCreateViewModel;