import {namespace} from "@Main/namespace";
import {Breadcrumb} from "@Main/breadcrumbs";
import type {PmbbViewModel} from "@Main/PmbbViewModel";

export class ServiceObjectDetailsViewModel extends window.Crm.ViewModels.ContactDetailsViewModel {
	tabs = ko.observable<{}>({});
	serviceObject = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceObject>(null);
	showAllAddresses = ko.observable<boolean>(false);
	lookups: LookupType = {
		addressTypes: {$tableName: "Crm_AddressType"},
		countries: {$tableName: "Main_Country"},
		emailTypes: {$tableName: "Crm_EmailType"},
		faxTypes: {$tableName: "Crm_FaxType"},
		phoneTypes: {$tableName: "Crm_PhoneType"},
		regions: {$tableName: "Main_Region"},
		serviceObjectCategories: {$tableName: "CrmService_ServiceObjectCategory"},
		websiteTypes: {$tableName: "Crm_WebsiteType"}
	};
	addresses = ko.observableArray<Crm.Rest.Model.ObservableCrm_Address>([]);
	standardAddress = ko.pureComputed<Crm.Rest.Model.ObservableCrm_Address>(() => {
		return ko.utils.arrayFirst(this.addresses(), x => x.IsCompanyStandardAddress()) || null;
	});
	primaryPhone = ko.pureComputed<Crm.Rest.Model.ObservableCrm_Phone>(() => {
		const standardAddress = this.standardAddress();
		if (standardAddress) {
			return window.Helper.Address.getPrimaryCommunication(standardAddress.Phones);
		}
		return null;
	});
	primaryFax = ko.pureComputed<Crm.Rest.Model.ObservableCrm_Fax>(() => {
		const standardAddress = this.standardAddress();
		if (standardAddress) {
			return window.Helper.Address.getPrimaryCommunication(standardAddress.Faxes);
		}
		return null;
	});
	primaryEmail = ko.pureComputed<Crm.Rest.Model.ObservableCrm_Email>(() => {
		const standardAddress = this.standardAddress();
		if (standardAddress) {
			return window.Helper.Address.getPrimaryCommunication(standardAddress.Emails);
		}
		return null;
	});
	primaryWebsite = ko.pureComputed<Crm.Rest.Model.ObservableCrm_Website>(() => {
		const standardAddress = this.standardAddress();
		if (standardAddress) {
			return window.Helper.Address.getPrimaryCommunication(standardAddress.Websites);
		}
		return null;
	});
	dropboxName = ko.pureComputed<string>(() => {
		return (this.serviceObject().ObjectNo() !== null && this.serviceObject().ObjectNo() !== '' ? this.serviceObject().ObjectNo() + "-" : "") + this.serviceObject().Name().substring(0, 25);
	});
	additionalTitle = ko.observable<string>(null);

	constructor() {
		super();
		this.contactType("ServiceObject");
	}

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		this.contactId(id);
		window.Helper.Address.registerEventHandlers(this);
		await super.init(id, params);
		let serviceObject = await window.database.CrmService_ServiceObject
			.include2("Addresses.orderBy(function(x) { return x.CreateDate; })")
			.include("ResponsibleUserUser")
			.include2("Tags.orderBy(function(t) { return t.Name; })")
			.find(id);
		this.serviceObject(serviceObject.asKoObservable());
		this.serviceObject().ObjectNo.subscribe(function () {
			this.setTitle();
		}, this);
		this.serviceObject().Name.subscribe(async () => {
			this.setTitle();
		}, this);
		this.serviceObject().CategoryKey.subscribe(function () {
			this.setAdditionalTitle();
		}, this);
		this.contact(this.serviceObject());
		this.contactName(window.Helper.ServiceObject.getDisplayName(this.serviceObject()));
		this.addresses(serviceObject.Addresses.map(function (x) {
			return x.asKoObservable();
		}));
		this.showAllAddresses.subscribe(async value => {
			if (value) {
				this.loading(true);
				await window.Helper.Address.loadCommunicationData(this.addresses(), this.contactId());
				this.loading(false);
			}
		});
		await window.Helper.Address.loadCommunicationData(this.standardAddress(), this.contactId());
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		await this.setVisibilityAlertText();
		this.setTitle();
		this.setAdditionalTitle();
		await this.setBreadcrumbs(id, this.title());
	};

	onSaveGeneral(context: PmbbViewModel): void {
		context.viewContext.serviceObject().ResponsibleUserUser(context.editContext().serviceObject().ResponsibleUserUser());
	};

	onAddressInsert = window.Crm.ViewModels.CompanyDetailsViewModel.prototype.onAddressInsert;
	onAddressUpdate = window.Crm.ViewModels.CompanyDetailsViewModel.prototype.onAddressUpdate;
	onAddressDelete = window.Crm.ViewModels.CompanyDetailsViewModel.prototype.onAddressDelete;
	checkRelevantAddress = window.Crm.ViewModels.CompanyDetailsViewModel.prototype.checkRelevantAddress;
	getCommunicationCollectionFromAddress = window.Crm.ViewModels.CompanyDetailsViewModel.prototype.getCommunicationCollectionFromAddress;
	onCommunicationInsert = window.Crm.ViewModels.CompanyDetailsViewModel.prototype.onCommunicationInsert;
	onCommunicationUpdate = window.Crm.ViewModels.CompanyDetailsViewModel.prototype.onCommunicationUpdate;
	onCommunicationDelete = window.Crm.ViewModels.CompanyDetailsViewModel.prototype.onCommunicationDelete;
	getUniqueAddress = window.Crm.ViewModels.CompanyDetailsViewModel.prototype.getUniqueAddress;
	checkRelevantCommunication = window.Crm.ViewModels.CompanyDetailsViewModel.prototype.checkRelevantCommunication;
	makeStandardAddress = window.Crm.ViewModels.CompanyDetailsViewModel.prototype.makeStandardAddress;

	async setBreadcrumbs(id: string, title: string): Promise<void> {
		await window.breadcrumbsViewModel.setBreadcrumbs([
			new Breadcrumb(window.Helper.String.getTranslatedString("ServiceObject"), "ServiceObject::Index", "#/Crm.Service/ServiceObjectList/IndexTemplate"),
			new Breadcrumb(title, null, window.location.hash, null, id)
		]);
	};

	setTitle(): void {
		this.title(window.Helper.ServiceObject.getDisplayName(this.serviceObject()));
	};

	setAdditionalTitle(): void {
		const categoryKey = this.serviceObject().CategoryKey();
		const category = this.lookups.serviceObjectCategories.$array.find(function (x) {
			return x.Key === categoryKey;
		});
		this.additionalTitle(category ? category.Value : "");
	};
}

namespace("Crm.Service.ViewModels").ServiceObjectDetailsViewModel = ServiceObjectDetailsViewModel;