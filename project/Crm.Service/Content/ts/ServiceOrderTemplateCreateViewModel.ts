import {namespace} from "@Main/namespace";
import { Breadcrumb } from "@Main/breadcrumbs";
export class ServiceOrderTemplateCreateViewModel extends window.Main.ViewModels.ViewModelBase {
	currentUser = ko.observable<Main.Rest.Model.Main_User>(null);
	numberingSequenceName = "SMS.ServiceOrderTemplate";
	serviceOrderTemplate = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead>(null);
	errors = ko.validation.group([this.serviceOrderTemplate]);
	lookups: LookupType = {
		serviceOrderTypes: {$tableName: "CrmService_ServiceOrderType"}
	}

	cancel(): void {
		window.history.back();
	};

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		let currentUser = await window.Helper.User.getCurrentUser();
		this.currentUser(currentUser);
		const serviceOrderTemplate = window.database.CrmService_ServiceOrderHead.defaultType.create();
		serviceOrderTemplate.IsTemplate = true;
		serviceOrderTemplate.StationKey = currentUser.ExtensionValues.StationIds.length === 1 ? currentUser.ExtensionValues.StationIds[0] : null;
		serviceOrderTemplate.ResponsibleUser = currentUser.Id;
		serviceOrderTemplate.LastActivity = new Date();
		serviceOrderTemplate.StatusKey = "New";
		serviceOrderTemplate.TypeKey = window.Helper.Lookup.getDefaultLookupValueSingleSelect(this.lookups.serviceOrderTypes, serviceOrderTemplate.TypeKey);
		await this.setBreadcrumbs();
		window.database.add(serviceOrderTemplate);
		this.serviceOrderTemplate(serviceOrderTemplate.asKoObservable());
		this.serviceOrderTemplate().UserGroupKey.subscribe(() => {
			if (this.serviceOrderTemplate().UserGroupKey() === null) {
				this.serviceOrderTemplate().ResponsibleUser(this.currentUser().Id)
			} else {
				this.serviceOrderTemplate().ResponsibleUser(null);
			}
		});
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
		if (!this.serviceOrderTemplate().OrderNo()) {
			let orderNo = await window.NumberingService.getNextFormattedNumber(this.numberingSequenceName);
			this.serviceOrderTemplate().OrderNo(orderNo);
		}
		await window.database.saveChanges();
		window.location.hash = "/Crm.Service/ServiceOrderTemplate/Details/" + this.serviceOrderTemplate().Id();
	};

	preferredTechnicianFilter(preferredTechnicianUsergroupKey: string, query: $data.Queryable<Main.Rest.Model.Main_User>, term: string): $data.Queryable<Main.Rest.Model.Main_User> {
		// @ts-ignore
		if (query.specialFunctions.filterByPermissions[window.database.storageProvider.name]) {
			query = query.filter("filterByPermissions", "Dispatch::Edit");
		}
		return window.Helper.User.filterUserQuery(query, term, preferredTechnicianUsergroupKey);
	};

	async setBreadcrumbs(): Promise<void> {
		await window.breadcrumbsViewModel.setBreadcrumbs([
			new Breadcrumb(window.Helper.getTranslatedString("ServiceOrderTemplate"), "ServiceOrderTemplate::Index", "#/Crm.Service/ServiceOrderTemplateList/IndexTemplate"),
			new Breadcrumb(window.Helper.getTranslatedString("NewServiceOrderTemplate"), null, window.location.hash, null, null)
		]);
	};
}

namespace("Crm.Service.ViewModels").ServiceOrderTemplateCreateViewModel = ServiceOrderTemplateCreateViewModel;