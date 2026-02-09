import { Mixin } from "ts-mixer";
import type { ArticleDetailsViewModel } from "@Crm.Article/ArticleDetailsViewModel";
import type { InstallationListIndexViewModel } from "./InstallationListIndexViewModel";

export class ArticleDetailsRelationshipsTabViewModelExtension extends window.Crm.Article.ViewModels.ArticleDetailsRelationshipsTabViewModel {
	genericLinkedInstallations: InstallationListIndexViewModel = null;
	installationIds = ko.observableArray<string[]>([]);

	constructor(parentViewModel: ArticleDetailsViewModel) {
		super(parentViewModel);
		if (!window.database.CrmService_InstallationPos) {
			return;
		}
		this.lookups.installationCompanyRelationshipTypes = { $tableName: "CrmService_InstallationCompanyRelationshipType" };
		this.lookups.countries = { $tableName: "Main_Country" };
		this.lookups.regions = { $tableName: "Main_Region" };
		this.lookups.installationHeadStatuses = { $tableName: "CrmService_InstallationHeadStatus" };
		this.lookups.installationTypes = { $tableName: "CrmService_InstallationType" };
	}

	async init(): Promise<void> {
		if (!window.database.CrmService_InstallationPos) {
			return;
		}
		await window.database.CrmService_InstallationPos
			.filter("it.ArticleId === this.articleId", { articleId: this.articleId })
			.map(function (x) {
				return x.InstallationId
			})
			.toArray(this.installationIds);

		this.genericLinkedInstallations = new window.Crm.Service.ViewModels.InstallationListIndexViewModel();
		this.genericLinkedInstallations.applyFilters = (query) => {
			query = window.Main.ViewModels.GenericListViewModel.prototype.applyFilters.call(this.genericLinkedInstallations, query);
			return query.filter("it.Id in this.ids", { ids: this.installationIds })
		}
		await this.genericLinkedInstallations.init();
		this.genericLinkedInstallations.bulkActions([]);
		this.subViewModels.push(this.genericLinkedInstallations);
		await super.init();
	}
}

window.Crm.Article.ViewModels.ArticleDetailsRelationshipsTabViewModel = Mixin(ArticleDetailsRelationshipsTabViewModelExtension);