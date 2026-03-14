import {namespace} from "@Main/namespace";
import type {DispatchDetailsViewModel} from "./DispatchDetailsViewModel";

export class DispatchReportPreviewModalViewModel extends window.Crm.Service.ViewModels.DispatchReportViewModel {
	parentViewModel: DispatchDetailsViewModel;
	selectedLanguage = ko.observable<string>(null);
	user: Main.Rest.Model.Main_User;
	attributeFormItems: Array<any> = [];

	constructor(parentViewModel: DispatchDetailsViewModel) {
		super();
		this.parentViewModel = parentViewModel;
	}

	filterLanguage(query: $data.Queryable<Main.Rest.Model.Lookups.Main_Language>, term: string): $data.Queryable<Main.Rest.Model.Lookups.Main_Language> {
		return window.Helper.Lookup.queryLookup(query.filter("it.IsSystemLanguage === true"), term);
	};

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await window.Helper.Database.initialize();
		let user = await window.Helper.User.getCurrentUser();
		this.user = user;
		this.selectedLanguage(user.DefaultLanguageKey);
		this.selectedLanguage.subscribe(async () => {
			this.loading(true);
			await this.updateLanguage()
			this.loading(false);
		});
		await super.init(id, params);
		for (const item of this.attributeFormItems) {
			const origGetLocalizationText = item.getLocalizationText;
			const viewModel = this;
			item.getLocalizationText = function(dynamicFormElement, choiceIndex, hint, language) {
				return origGetLocalizationText(dynamicFormElement, choiceIndex, hint, language ?? ko.unwrap(viewModel.selectedLanguage))
			}
		}
		await this.updateLanguage();
	};
	async initAttributeFormItems(language?: string): Promise<void> {
		if (language) {
			await super.initAttributeFormItems(language);
		}
	}
	async updateLanguage(): Promise<void> {
		const languageKey = this.selectedLanguage() || this.user.DefaultLanguageKey;
		if (this.parentViewModel) {
			this.parentViewModel.selectedLanguage(languageKey);
		}
		this.lookups.quantityUnits.$array = null;
		this.lookups.installationStatus.$array = null;
		this.lookups.installationTypes.$array = null;
		this.lookups.serviceOrderTypes.$array = null;
		this.lookups.invoicingTypes.$array = null;
		this.lookups.manufacturers.$array = null;
		this.lookups.expenseTypes.$array = null;
		this.lookups.statisticsKeyFaultImages.$array = null;
		this.lookups.statisticsKeyCauses.$array = null;
		this.lookups.currencies.$array = null;


		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups, languageKey);
		this.lookups.articleDescriptions = await Helper.Article.loadArticleDescriptionsMap(this.itemNos, languageKey);
		await this.initAttributeFormItems(languageKey);
	};
}

namespace("Crm.Service.ViewModels").DispatchReportPreviewModalViewModel = DispatchReportPreviewModalViewModel;
