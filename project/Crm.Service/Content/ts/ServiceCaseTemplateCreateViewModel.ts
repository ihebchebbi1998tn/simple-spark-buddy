import {namespace} from "@Main/namespace";
import { Breadcrumb } from "@Main/breadcrumbs";

export class ServiceCaseTemplateCreateViewModel extends window.Main.ViewModels.ViewModelBase {
	serviceCaseTemplate = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceCaseTemplate>(null);
	errors = ko.validation.group(this.serviceCaseTemplate, {deep: true});
	lookups: LookupType = {
		serviceCaseCategories: {$tableName: "CrmService_ServiceCaseCategory"},
		servicePriorities: {$tableName: "CrmService_ServicePriority"}
	}

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups)
		const serviceCaseTemplate = window.database.CrmService_ServiceCaseTemplate.defaultType.create();
		const currentUserName = window.Helper.User.getCurrentUserName();
		serviceCaseTemplate.CategoryKey = window.Helper.Lookup.getDefaultLookupValueSingleSelect(this.lookups.serviceCaseCategories, serviceCaseTemplate.CategoryKey);
		serviceCaseTemplate.PriorityKey = window.Helper.Lookup.getDefaultLookupValueSingleSelect(this.lookups.servicePriorities, serviceCaseTemplate.PriorityKey);
		serviceCaseTemplate.ResponsibleUser = currentUserName;
		this.serviceCaseTemplate(serviceCaseTemplate.asKoObservable());
		this.serviceCaseTemplate().UserGroupKey.subscribe(() => {
			if (this.serviceCaseTemplate().UserGroupKey() !== null) {
				this.serviceCaseTemplate().ResponsibleUser(null);
			}
		});
		await this.setBreadcrumbs();
		window.database.add(this.serviceCaseTemplate().innerInstance);
	};

	cancel(): void {
		window.database.detach(this.serviceCaseTemplate().innerInstance);
		window.history.back();
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
		await window.database.saveChanges();
		window.location.hash = "/Crm.Service/ServiceCaseTemplate/DetailsTemplate/" + this.serviceCaseTemplate().Id();
	};

	async setBreadcrumbs(): Promise<void> {
		await window.breadcrumbsViewModel.setBreadcrumbs([
			new Breadcrumb(window.Helper.getTranslatedString("ServiceCaseTemplate"), "ServiceCaseTemplate::Index", "#/Crm.Service/ServiceCaseList/IndexTemplate"),
			new Breadcrumb(window.Helper.getTranslatedString("CreateServiceCaseTemplate"), null, window.location.hash, null, null)
		]);
	};
}

namespace("Crm.Service.ViewModels").ServiceCaseTemplateCreateViewModel = ServiceCaseTemplateCreateViewModel;