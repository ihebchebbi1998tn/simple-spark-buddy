import {namespace} from "@Main/namespace";
import type {VisibilityViewModel} from "@Main/VisibilityViewModel";
import type {AddressEditorViewModel} from "@Crm/AddressEditorViewModel";
import { Breadcrumb } from "@Main/breadcrumbs";

export class ServiceObjectCreateViewModel extends window.Main.ViewModels.ViewModelBase {
	serviceObject = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceObject>(null);
	visibilityViewModel: VisibilityViewModel = new window.VisibilityViewModel(this.serviceObject, "ServiceObject");
	errors = ko.validation.group(this.serviceObject, {deep: true});
	numberingSequenceName = "SMS.ServiceObject";
	lookups: LookupType = {
		phoneTypes: {$tableName: "Crm_PhoneType"},
		faxTypes: {$tableName: "Crm_FaxType"},
		emailTypes: {$tableName: "Crm_EmailType"},
		websiteTypes: {$tableName: "Crm_WebsiteType"},
		serviceObjectCategories: {$tableName: "CrmService_ServiceObjectCategory"}
	};
	addressEditor: AddressEditorViewModel;

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		const serviceObject = window.database.CrmService_ServiceObject.defaultType.create();
		serviceObject.CategoryKey = window.Helper.Lookup.getDefaultLookupValueSingleSelect(this.lookups.serviceObjectCategories, serviceObject.CategoryKey);
		serviceObject.ResponsibleUser = window.Helper.User.getCurrentUserName();
		serviceObject.LastActivity = new Date();
		this.serviceObject(serviceObject.asKoObservable());
		await this.visibilityViewModel.init();
		await this.setBreadcrumbs();
		window.database.add(this.serviceObject().innerInstance);
	};

	cancel(): void {
		window.database.detach(this.serviceObject().innerInstance);
		window.history.back();
	};

	onLoadAddressEditor(addressEditor: AddressEditorViewModel): void {
		this.addressEditor = addressEditor;
		const setAddressDefaultValues = this.addressEditor.setAddressDefaultValues;
		this.addressEditor.setAddressDefaultValues = function(addressEntity) {
			addressEntity.IsCompanyStandardAddress = true;
			setAddressDefaultValues.call(this, addressEntity);
		};
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
		try {
			if (this.addressEditor.hasErrors()) {
				this.errors.expandCollapsiblesWithErrors();
			} else {
				this.addressEditor.removeEmptyEntries();
			}
			this.addressEditor.showValidationErrors();

			let objectNo = await window.NumberingService.createNewNumberBasedOnAppSettings(window.Crm.Service.Settings.ServiceObject.ObjectNoIsGenerated, window.Crm.Service.Settings.ServiceObject.ObjectNoIsCreateable, this.serviceObject().ObjectNo(), this.numberingSequenceName, window.database.CrmService_ServiceObject, "ObjectNo");
			if (objectNo !== null) {
				this.serviceObject().ObjectNo(objectNo);
			}
			await window.database.saveChanges();
			window.location.hash = "/Crm.Service/ServiceObject/DetailsTemplate/" + this.serviceObject().Id();
		} catch {
			this.loading(false);
		}
	};

	async setBreadcrumbs(): Promise<void> {
		await window.breadcrumbsViewModel.setBreadcrumbs([
			new Breadcrumb(window.Helper.getTranslatedString("ServiceObject"), "ServiceObject::Index", "#/Crm.Service/ServiceObjectList/IndexTemplate"),
			new Breadcrumb(window.Helper.getTranslatedString("CreateServiceObject"), null, window.location.hash, null, null)
		]);
	};
}

namespace("Crm.Service.ViewModels").ServiceObjectCreateViewModel = ServiceObjectCreateViewModel;