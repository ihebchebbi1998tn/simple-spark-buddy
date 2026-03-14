import {namespace} from "@Main/namespace";
import {Breadcrumb} from "@Main/breadcrumbs";
import type {PmbbViewModel} from "@Main/PmbbViewModel";
import type {AddressEditorViewModel} from "@Crm/AddressEditorViewModel";

export class InstallationDetailsViewModel extends window.Crm.ViewModels.ContactDetailsViewModel {
	tabs = ko.observable<{}>({});
	installation = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_Installation>(null);
	serviceContracts = ko.observableArray<Crm.Service.Rest.Model.ObservableCrmService_ServiceContract>([]);
	currentUser = ko.observable<Main.Rest.Model.Main_User>(null);
	showCustomArticle = ko.observable<Boolean>(false);
	lookups: LookupType = {
		addressTypes: {$tableName: "Crm_AddressType"},
		countries: {$tableName: "Main_Country"},
		emailTypes: {$tableName: "Crm_EmailType"},
		faxTypes: {$tableName: "Crm_FaxType"},
		installationHeadStatuses: {$tableName: "CrmService_InstallationHeadStatus"},
		installationTypes: {$tableName: "CrmService_InstallationType"},
		manufacturers: { $tableName: "CrmService_Manufacturer" },
		phoneTypes: {$tableName: "Crm_PhoneType"},
		regions: {$tableName: "Main_Region"},
		serviceContractTypes: {$tableName: "CrmService_ServiceContractType"},
		statisticsKeyProductTypes: { $tableName: "CrmService_StatisticsKeyProductType"},
		websiteTypes: {$tableName: "Crm_WebsiteType"}
	};
	dropboxName = ko.pureComputed<string>(() => this.installation().InstallationNo() + "-" + this.installation().Company().Name() + "-" + this.installation().Description().substring(0, 25));
	additionalTitle = ko.observable<string>(null);
	customAddress = window.ko.observable(false);
	addressEditor: AddressEditorViewModel;
	filterValue = ko.pureComputed<string>(() => this.installation().InstallationNo() + (this.installation().Description() ? ' - ' + this.installation().Description() : ''));

	constructor() {
		super();
		this.contactType("Installation");
	}

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		this.contactId(id);
		await super.init(id, params);
		let installation = await window.database.CrmService_Installation
			.include("Address")
			.include("Article")
			.include("Company")
			.include("Person")
			.include("PreferredUserUser")
			.include("ResponsibleUserUser")
			.include("ServiceObject")
			.include("Station")
			.include2("Tags.orderBy(function(t) { return t.Name; })")
			.find(id);
		this.installation(installation.asKoObservable());
		this.installation().InstallationNo.subscribe(function () {
			this.setTitle();
		}, this);
		this.installation().Description.subscribe(async () => { 
			this.setTitle();
		}, this);
		this.installation().LegacyInstallationId.subscribe(async () => {
			this.setTitle();
		}, this);

		this.installation().InstallationTypeKey.subscribe(function () {
			this.setAdditionalTitle();
		}, this);
		this.contact(this.installation());
		this.contactName(window.Helper.Installation.getDisplayName(this.installation()));
		this.isEditable(installation.LegacyId === null);
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		await window.Helper.Address.loadCommunicationData(this.installation().Address());
		await this.loadServiceContracts();
		await this.setVisibilityAlertText();
		this.currentUser(await window.Helper.User.getCurrentUser());
		this.showCustomArticle(this.installation().CustomItemNo() !== null)
		this.setTitle();
		this.setAdditionalTitle();
		await this.setBreadcrumbs(id, this.title());
	};

	contactPersonFilter =
		window.Crm.Service.ViewModels.InstallationCreateViewModel.prototype.contactPersonFilter;

	async loadServiceContracts(): Promise<void> {
		if (!window.AuthorizationManager.currentUserHasPermission("WebAPI::ServiceContractInstallationRelationship")) {
			return;
		}
		await window.database.CrmService_ServiceContractInstallationRelationship
			.include("Parent")
			.include("Parent.ResponsibleUserUser")
			.filter(function (it) {
				return it.ChildId === this.installationId && it.Parent.StatusKey === "Active";
			}, {installationId: this.installation().Id()})
			.map("it.Parent")
			.take(5)
			.toArray(this.serviceContracts);
	}

	locationAddressFilter =
		window.Crm.Service.ViewModels.InstallationCreateViewModel.prototype.locationAddressFilter;

	onBeforeSaveGeneral(viewModel: PmbbViewModel): void {
		if(viewModel.editContext().installation().CustomItemNo()) {
			viewModel.editContext().installation().ArticleKey(null);
			viewModel.editContext().installation().Article(null);
		}
		viewModel.editContext().installation().Name(viewModel.editContext().installation().Description());
	};

	onLocationContactSelect(installation: Crm.Service.Rest.Model.ObservableCrmService_Installation, contact: Crm.Rest.Model.Crm_Company): void {
		if (contact) {
			installation.Company(contact.asKoObservable());
			if (this.customAddress() && this.addressEditor && this.addressEditor.contactId !== contact.Id) {
				this.addressEditor.address().CompanyId(contact.Id);
				this.addressEditor.address().Name1(contact.Name);
			}
			if (installation.Address() && installation.Address().CompanyId() !== contact.Id) {
				installation.Address(null);
				installation.LocationAddressKey(null);
			}
			if (installation.Person() && installation.Person().ParentId() !== contact.Id) {
				installation.LocationPersonId(null);
				installation.Person(null);
			}
		} else {
			installation.Company(null);
			installation.Address(null);
			installation.LocationAddressKey(null);
			installation.LocationPersonId(null);
			installation.Person(null);
		}
	};

	onLocationAddressSelect(installation: Crm.Service.Rest.Model.ObservableCrmService_Installation, address: Crm.Rest.Model.Crm_Address): void {
		if (address) {
			installation.Address(address.asKoObservable());
		} else {
			installation.Address(null);
		}
	};

	onLocationPersonSelect(installation: Crm.Service.Rest.Model.ObservableCrmService_Installation, person: Crm.Rest.Model.Crm_Person): void {
		if (person) {
			installation.Person(person.asKoObservable());
		} else {
			installation.Person(null);
		}
	};

	async setBreadcrumbs(id: string, title: string): Promise<void> {
		await window.breadcrumbsViewModel.setBreadcrumbs([
			new Breadcrumb(window.Helper.String.getTranslatedString("Installation"), "Installation::Index", "#/Crm.Service/InstallationList/IndexTemplate"),
			new Breadcrumb(title, null, window.location.hash, null, id)
		]);
	};

	setTitle(): void {
		this.title(window.Helper.Installation.getDisplayName(this.installation()));
	};

	setAdditionalTitle(): void {
		const typeKey = this.installation().InstallationTypeKey();
		const type = this.lookups.installationTypes.$array.find(function (x) {
			return x.Key === typeKey;
		});
		this.additionalTitle(type ? type.Value : "");
	};

	onLoadAddressEditor(installation: Crm.Service.Rest.Model.ObservableCrmService_Installation, addressEditor: AddressEditorViewModel): void {
		this.addressEditor = addressEditor;
		addressEditor.name = installation.Company()?.Name();
	};

	toggleCustomAddress(): void {
		if (this.customAddress()) {
			this.addressEditor.dispose();
		}
		this.customAddress(!this.customAddress());
	};

	onBeforeSaveContactInfo(viewModel: PmbbViewModel): void {
		if (this.customAddress() && this.addressEditor) {
			if (this.addressEditor.addressErrors() && this.addressEditor.addressErrors().length > 0) {
				this.addressEditor.showValidationErrors();
				throw new Error('');
			} else {
				if (viewModel.editContext().installation().LocationAddressKey() === null) {
					viewModel.editContext().installation().LocationAddressKey(this.addressEditor.address().Id());
				}
			}
		}
	};

	onSaveContactInfo(): void {
		if (this.customAddress() && this.addressEditor) {
			this.installation().LocationAddressKey(this.addressEditor.address().Id())
			this.onLocationAddressSelect(this.installation(), this.addressEditor.address().innerInstance);
		}
	};

	onAfterSaveContactInfo(): void {
		if (this.customAddress()) {
			this.customAddress(false);
		}
	};

	onCancelContactInfo(): void {
		if (this.customAddress()) {
			this.customAddress(false);
			if (this.addressEditor) {
				this.addressEditor.dispose();
			}
		}
	};

	getArticleSelect2Filter(query: $data.Queryable<Crm.Article.Rest.Model.CrmArticle_Article>, term: string): $data.Queryable<Crm.Article.Rest.Model.CrmArticle_Article> {
		query = query.filter("(it.ArticleTypeKey === 'Material' || it.ArticleTypeKey === 'ConfigurationBase') && it.ExtensionValues.IsHidden === false");
		return window.Helper.Article.getArticleAutocompleteFilter(query, term, this.currentUser().DefaultLanguageKey);
	};

	onArticleSelect(article: Crm.Article.Rest.Model.CrmArticle_Article): void {
		if(article) {
			this.installation().Article(article.asKoObservable());
			this.installation().CustomItemNo(null);
			this.installation().CustomItemDesc(null);
		} else {
			this.installation().Article(null);
		}
	}
}

namespace("Crm.Service.ViewModels").InstallationDetailsViewModel = InstallationDetailsViewModel;