import { namespace } from "@Main/namespace";
import type { ArticleDetailsViewModel } from "@Crm.Article/ArticleDetailsViewModel";

export class ArticleDetailsTeamsTabViewModel extends window.Main.ViewModels.GenericListViewModel<Crm.Service.Team.Rest.Model.CrmServiceTeam_ArticleUserGroupRelationship, Crm.Service.Team.Rest.Model.ObservableCrmServiceTeam_ArticleUserGroupRelationship> {
	articleId = ko.observable<string>(null);
	lookups: LookupType = {
		articleTypes: { $tableName: "CrmArticle_ArticleType" }
	};

	constructor(parentViewModel: ArticleDetailsViewModel) {

		super("CrmServiceTeam_ArticleUserGroupRelationship", "UserGroup.Name", "ASC", ["Article", "UserGroup"]);
		this.articleId(parentViewModel.contactId());
		this.getFilter("ArticleKey").extend({ filterOperator: "===" })(this.articleId());

	}

	async init(): Promise<void> {
		await super.init();
		this.isTabViewModel(true);
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);

	}

	async delete(articleUserGroupRelationship: any): Promise<void> {
		if (!await window.Helper.Confirm.confirmDeleteAsync()) {
			return;
		}
		this.loading(true);
		let entity = await window.Helper.Database.getDatabaseEntity(articleUserGroupRelationship);
		window.database.remove(entity);
		await window.database.saveChanges();
	}


}

namespace("Crm.Article.ViewModels").ArticleDetailsTeamsTabViewModel = ArticleDetailsTeamsTabViewModel;
