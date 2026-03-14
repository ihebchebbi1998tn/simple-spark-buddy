import {namespace} from "@Main/namespace";
import type {VisibilityViewModel} from "@Main/VisibilityViewModel";
import type {AddressEditorViewModel} from "@Crm/AddressEditorViewModel";
import { Breadcrumb } from "@Main/breadcrumbs";

export class InstallationCreateViewModel extends window.Main.ViewModels.ViewModelBase {
	installation = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_Installation>(null);
	currentUser = ko.observable<Main.Rest.Model.Main_User>(null);
	errors = ko.validation.group(this.installation, { deep: true });
	visibilityViewModel: VisibilityViewModel = new window.VisibilityViewModel(this.installation, "Installation");
	addAddress = ko.observable<boolean>(false);
	lookups: LookupType = {
		installationHeadStatuses: {$tableName: "CrmService_InstallationHeadStatus"},
		installationTypes: {$tableName: "CrmService_InstallationType"},
		phoneTypes: {$tableName: "Crm_PhoneType"},
		faxTypes: {$tableName: "Crm_FaxType"},
		emailTypes: {$tableName: "Crm_EmailType"},
		websiteTypes: {$tableName: "Crm_WebsiteType"}
	};
	addressEditor: AddressEditorViewModel;

	async init(id?: string, params?: {[key:string]:string}) {
		await super.init(id, params);
		const installation = window.database.CrmService_Installation.defaultType.create();
		const currentUserName = window.Helper.User.getCurrentUserName();
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		installation.ResponsibleUser = currentUserName;
		installation.LastActivity = new Date();
		installation.InstallationTypeKey =
			window.Helper.Lookup.getDefaultLookupValueSingleSelect(this.lookups.installationTypes,
				installation.InstallationTypeKey);
		installation.StatusKey =
			window.Helper.Lookup.getDefaultLookupValueSingleSelect(this.lookups.installationHeadStatuses,
				installation.StatusKey);
		this.installation(installation.asKoObservable());
		this.installation().Description.subscribe(description => {
			this.installation().Name(description);
		});
		this.installation().LocationContactId.subscribe(() => {
			this.installation().LocationAddressKey(null);
			this.installation().LocationPersonId(null);
		});
		if (params && params.locationContactId) {
			this.installation().LocationContactId(params.locationContactId);
		}
		if (params && params.folderId) {
			this.installation().FolderId(params.folderId);
		}
		this.currentUser(await window.Helper.User.getCurrentUser());
		this.installation().StationKey(this.currentUser().ExtensionValues.StationIds.length === 1 ? this.currentUser().ExtensionValues.StationIds[0] : null);
		await this.visibilityViewModel.init();
		await this.setBreadcrumbs();
		window.database.add(this.installation().innerInstance);
	};

	cancel(): void {
		window.database.detach(this.installation().innerInstance);
		window.history.back();
	};

	contactPersonFilter(query: $data.Queryable<Crm.Rest.Model.Crm_Person>, term: string, installation: Crm.Service.Rest.Model.ObservableCrmService_Installation): $data.Queryable<Crm.Rest.Model.Crm_Person> {
		if (term) {
			query = window.Helper.String.contains(query, term, ["Firstname", "Surname"]);
		}
		return query.filter("this.locationContactId !== null && it.ParentId === this.locationContactId && it.IsRetired === false", {locationContactId: installation.LocationContactId()});
	};

	locationAddressFilter(query: $data.Queryable<Crm.Rest.Model.Crm_Address>, term: string, installation: Crm.Service.Rest.Model.ObservableCrmService_Installation): $data.Queryable<Crm.Rest.Model.Crm_Address> {
		if (term) {
			query = window.Helper.String.contains(query, term, ["Name1", "Name2", "Name3", "ZipCode", "City", "Street"]);
		}
		return query.filter(function (it) {
				return this.locationContactId !== null && it.CompanyId === this.locationContactId;
			},
			{locationContactId: installation.LocationContactId()});
	};

	async submit(): Promise<void> {
		this.loading(true);

		if (this.errors().length == 1 && !this.installation().InstallationNo() && !window.Crm.Service.Settings.InstallationNoIsGenerated) {
			this.loading(false);
			this.errors.showAllMessages();
			this.errors.scrollToError();
			this.errors.expandCollapsiblesWithErrors();
			return;
		}

		try {
			if (window.ko.unwrap(this.addAddress)) {
				if (this.addressEditor.hasErrors()) {
					this.errors.expandCollapsiblesWithErrors();
				} else {
					this.addressEditor.removeEmptyEntries();
				}
				this.addressEditor.showValidationErrors();

				this.installation().LocationAddressKey(this.addressEditor.address().Id());
			}
			let installationNo = await window.NumberingService.createNewNumberBasedOnAppSettings(window.Crm.Service.Settings.InstallationNoIsGenerated, window.Crm.Service.Settings.InstallationNoIsCreateable, this.installation().InstallationNo(), "SMS.Installation", window.database.CrmService_Installation, "InstallationNo");
			if (installationNo !== null) {
				this.installation().InstallationNo(installationNo);
			}
			if (this.errors().length > 0) {
				this.loading(false);
				this.errors.showAllMessages();
				this.errors.scrollToError();
				this.errors.expandCollapsiblesWithErrors();
				return;
			}
			//necessary to change order of inserts, as we should guarantee that address is saved sooner than installation
			if (this.addAddress()) {
				window.database.detach(this.installation());
				window.database.add(this.installation());
			}
			await window.database.saveChanges();
			window.location.hash = "/Crm.Service/Installation/DetailsTemplate/" + this.installation().Id();
		} catch {
			this.loading(false);
		}
	};

	toggleAddAddress(): void {
		this.addAddress(!this.addAddress());
	};

	onLocationContactSelect(contact: Crm.Rest.Model.Crm_Company): void {
		const installation = this.installation();
		this.addAddress(false);
		installation.Company(contact?.asKoObservable() ?? null);
	};

	onLoadAddressEditor(addressEditor: AddressEditorViewModel): void {
		this.addressEditor = addressEditor;
		addressEditor.name = this.installation().Company().Name();
		this.installation().LocationAddressKey(null);
	};

	onDisposeAddressEditor(addressEditor: AddressEditorViewModel): void {
		this.installation().LocationAddressKey(addressEditor.addressId());
	};

	async setBreadcrumbs(): Promise<void> {
		await window.breadcrumbsViewModel.setBreadcrumbs([
			new Breadcrumb(window.Helper.getTranslatedString("Installation"), "Installation::Index", "#/Crm.Service/InstallationList/IndexTemplate"),
			new Breadcrumb(window.Helper.getTranslatedString("CreateInstallation"), null, window.location.hash, null, null)
		]);
	};
}

namespace("Crm.Service.ViewModels").InstallationCreateViewModel = InstallationCreateViewModel;