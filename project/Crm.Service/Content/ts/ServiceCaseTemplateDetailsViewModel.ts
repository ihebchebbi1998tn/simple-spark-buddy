import {namespace} from "@Main/namespace";
import {Breadcrumb} from "@Main/breadcrumbs";

export class ServiceCaseTemplateDetailsViewModel extends window.Main.ViewModels.ViewModelBase {
	tabs = ko.observable<{}>({});
	serviceCaseTemplate = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceCaseTemplate>(null);
	lookups: LookupType = {
		serviceCaseCategories: {$tableName: "CrmService_ServiceCaseCategory"},
		servicePriorities: {$tableName: "CrmService_ServicePriority"},
		skills: {$tableName: "Main_Skill"},
		assets: {$tableName: "Main_Asset"},
	};
	title = ko.observable<string>(null);
	additionalTitle = ko.observable<string>(null);

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		let serviceCaseTemplate = await window.database.CrmService_ServiceCaseTemplate
			.include("ResponsibleUserUser")
			.include("UserGroup")
			.find(id);
		this.serviceCaseTemplate(serviceCaseTemplate.asKoObservable());
		this.serviceCaseTemplate().CategoryKey.subscribe(function () {
			this.setAdditionalTitle();
		}, this);
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		this.title(this.serviceCaseTemplate().Name());
		this.serviceCaseTemplate().Name.subscribe(async (newTitle: string) => {
			this.title(newTitle);
		}, this);
		this.setAdditionalTitle();
		await this.setBreadcrumbs(this.title());
	};

	async setBreadcrumbs(title: string): Promise<void> {
		await window.breadcrumbsViewModel.setBreadcrumbs([
			new Breadcrumb(window.Helper.String.getTranslatedString("ServiceCaseTemplate"), "ServiceCaseTemplate::Index", "#/Crm.Service/ServiceCaseTemplateList/IndexTemplate"),
			new Breadcrumb(title, null, window.location.hash)
		]);
	};

	setAdditionalTitle(): void {
		const categoryKey = this.serviceCaseTemplate().CategoryKey();
		const category = this.lookups.serviceCaseCategories.$array.find(function (x) {
			return x.Key === categoryKey;
		});
		this.additionalTitle(category ? category.Value : "");
	};

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
	async onAfterSaveGeneralInfo(): Promise<void> {
		await this.setBreadcrumbs(this.title());
	};

	onUserGroupSelect =
		window.Crm.Service.ViewModels.ServiceCaseDetailsViewModel.prototype.onUserGroupSelect;
	onResponsibleUserSelect =
		window.Crm.Service.ViewModels.ServiceCaseDetailsViewModel.prototype.onResponsibleUserSelect;
}

namespace("Crm.Service.ViewModels").ServiceCaseTemplateDetailsViewModel = ServiceCaseTemplateDetailsViewModel;