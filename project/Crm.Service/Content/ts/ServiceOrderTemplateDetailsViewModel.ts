import {namespace} from "@Main/namespace";
import type {PmbbViewModel} from "@Main/PmbbViewModel";

export class ServiceOrderTemplateDetailsViewModel extends window.Crm.ViewModels.ContactDetailsViewModel {
	currentTabId = ko.observable<string>(null);
	previousTabId = ko.observable<string>(null);
	currentUser = ko.observable<Main.Rest.Model.Main_User>(null);
	serviceOrderTemplate = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead>(null);
	serviceOrder = this.serviceOrderTemplate;
	serviceOrderTemplateIsEditable = ko.pureComputed<boolean>(() => {
		return window.AuthorizationManager.isAuthorizedForAction("ServiceOrderTemplate", "Edit");
	});
	serviceOrderIsEditable = ko.pureComputed<boolean>(() => this.serviceOrderTemplateIsEditable());
	showInvoiceData = ko.pureComputed<boolean>(() => {
		return false;
	});
	lookups: LookupType = {
		invoicingTypes: {$tableName: "Main_InvoicingType"},
		serviceOrderStatuses: {$tableName: "CrmService_ServiceOrderStatus"},
		serviceOrderTypes: {$tableName: "CrmService_ServiceOrderType"},
		servicePriorities: {$tableName: "CrmService_ServicePriority"},
		skills: {$tableName: "Main_Skill"},
		assets: {$tableName: "Main_Asset"}
	};
	tabs = ko.observable<{}>({});
	contactName = ko.pureComputed<string>(() => {
		return this.serviceOrderTemplate() ? this.serviceOrderTemplate().OrderNo() : null;
	});
	title = ko.observable<string>(null);
	additionalTitle = ko.observable<string>(null);
	showTotalPrices = ko.pureComputed<boolean>(() => true);

	constructor() {
		super();
		this.contactType("ServiceOrder");
	}

	getSkillsFromKeys = window.Crm.Service.ViewModels.ServiceOrderDetailsViewModel.prototype.getSkillsFromKeys;
	getAssetsFromKeys = window.Crm.Service.ViewModels.ServiceOrderDetailsViewModel.prototype.getAssetsFromKeys;

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		let serviceOrderTemplate = await window.database.CrmService_ServiceOrderHead
			.include("PreferredTechnicianUser")
			.include("PreferredTechnicianUsergroupObject")
			.include("ResponsibleUserUser")
			.include("UserGroup")
			.find(id);
		this.serviceOrderTemplate(serviceOrderTemplate.asKoObservable());
		this.serviceOrderTemplate().TypeKey.subscribe(function () {
			this.setAdditionalTitle();
		}, this);
		this.contact(this.serviceOrderTemplate());
		let user = await window.Helper.User.getCurrentUser();
		this.currentUser(user);
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		await this.setVisibilityAlertText();
		this.title = this.serviceOrderTemplate().OrderNo;
		this.setAdditionalTitle();
		await this.setBreadcrumbs(id, this.title());
	};

	onSaveScheduling(viewModel: PmbbViewModel): void {
		viewModel.viewContext.serviceOrderTemplate()
			.PreferredTechnicianUser(viewModel.editContext().serviceOrderTemplate().PreferredTechnicianUser());
		viewModel.viewContext.serviceOrderTemplate()
			.PreferredTechnicianUsergroupObject(viewModel.editContext().serviceOrderTemplate().PreferredTechnicianUsergroupObject());
		viewModel.viewContext.serviceOrderTemplate()
			.ResponsibleUserUser(viewModel.editContext().serviceOrderTemplate().ResponsibleUserUser());
		viewModel.viewContext.serviceOrderTemplate()
			.UserGroup(viewModel.editContext().serviceOrderTemplate().UserGroup());
	};

	preferredTechnicianFilter(query: $data.Queryable<Main.Rest.Model.Main_User>, term: string): $data.Queryable<Main.Rest.Model.Main_User> {
		const serviceOrderTemplate = this.serviceOrderTemplate();
		// @ts-ignore
		if (query.specialFunctions.filterByPermissions[window.database.storageProvider.name]) {
			query = query.filter("filterByPermissions", "Dispatch::Edit");
		}
		let usergroupKey = serviceOrderTemplate?.PreferredTechnicianUsergroupKey?.();
		if (usergroupKey && typeof usergroupKey === 'string' && usergroupKey.trim() !== '') {
			return window.Helper.User.filterUserQuery(query, term, usergroupKey);
		}
		return window.Helper.User.filterUserQuery(query, term, null);
	};

	async setBreadcrumbs(id: string, title: string): Promise<void> {
		await window.breadcrumbsViewModel.setBreadcrumbs([
			new window.Breadcrumb(window.Helper.String.getTranslatedString("ServiceOrderTemplate"),
				"ServiceOrderTemplate::Index",
				"#/Crm.Service/ServiceOrderTemplateList/IndexTemplate"),
			new window.Breadcrumb(title, null, window.location.hash, null, id)
		]);
	};

	setAdditionalTitle(): void {
		const typeKey = this.serviceOrderTemplate().TypeKey();
		const type = this.lookups.serviceOrderTypes.$array.find(function (x) {
			return x.Key === typeKey;
		});
		this.additionalTitle(type ? type.Value : "");
	};

	getServiceOrderStatusAutocompleterOptions(tableName: string): Select2AutoCompleterOptions {
		const excluded = ["Released", "PartiallyReleased", "Scheduled"];
		return {
			customFilter: (query, term) => {
				if (term) {
					query = window.Helper.String.contains(query, term, ["Value.toLowerCase()"]);
				}
				return query.filter("(it.Groups == 'Preparation' || it.Groups == 'Scheduling') && !(it.Key in this.excluded) && it.Language === language", {
					language: this.currentUser().DefaultLanguageKey,
					excluded: excluded
				});
			},
			table: tableName,
			mapDisplayObject: window.Helper.Lookup.mapLookupForSelect2Display,
			getElementByIdQuery: window.Helper.Lookup.getLookupByKeyQuery
		}
	};
}

namespace("Crm.Service.ViewModels").ServiceOrderTemplateDetailsViewModel = ServiceOrderTemplateDetailsViewModel;