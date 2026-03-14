import { namespace } from "@Main/namespace";
import type { UserGroupDetailsViewModel } from "@Main/UserGroupDetailsViewModel";
import { HelperConfirm } from "@Main/helper/Helper.Confirm";
import { HelperDatabase } from "@Main/helper/Helper.Database";
import { HelperLookup } from "@Main/helper/Helper.Lookup";

export class UserGroupDetailsArticlesTabViewModel extends window.Main.ViewModels.GenericListViewModel<Crm.Service.Team.Rest.Model.CrmServiceTeam_ArticleUserGroupRelationship, Crm.Service.Team.Rest.Model.ObservableCrmServiceTeam_ArticleUserGroupRelationship> {
	userGroupKey = ko.observable<string>(null);
	lookups: LookupType = {
		articleTypes: { $tableName: "CrmArticle_ArticleType" }
	};

	constructor(parentViewModel: UserGroupDetailsViewModel) {

		super("CrmServiceTeam_ArticleUserGroupRelationship", "UserGroup.Name", "ASC", ["Article", "UserGroup"]);
		this.userGroupKey(parentViewModel.userGroup().Id());
		this.getFilter("UserGroupKey").extend({ filterOperator: "===" })(this.userGroupKey());

	}

	async init(): Promise<void> {
		await super.init();
		this.isTabViewModel(true);
		await HelperLookup.getLocalizedArrayMaps(this.lookups);

	}

	getItemGroup(item: any): ItemGroup {
		switch (item.Article().ArticleTypeKey()) {
			case "Tool":
				return { title: window.Helper.String.getTranslatedString("Tools") };
			case "Vehicle":
				return { title: window.Helper.String.getTranslatedString("Vehicles") };
			default: return null;
		};
	};

	applyOrderBy(query: $data.Queryable<Crm.Service.Team.Rest.Model.CrmServiceTeam_ArticleUserGroupRelationship>): $data.Queryable<Crm.Service.Team.Rest.Model.CrmServiceTeam_ArticleUserGroupRelationship> {
		query = query.orderBy("it.Article.ArticleTypeKey");
		return super.applyOrderBy(query);
	};

	async delete(ArticleUserGroupRelationship: any): Promise<void> {
		if (!await HelperConfirm.confirmDeleteAsync()) {
			return;
		}
		this.loading(true);
		let entity = await HelperDatabase.getDatabaseEntity(ArticleUserGroupRelationship);
		window.database.remove(entity);
		await window.database.saveChanges();
	}


}

namespace("Main.ViewModels").UserGroupDetailsArticlesTabViewModel = UserGroupDetailsArticlesTabViewModel;
