import { namespace } from "@Main/namespace";
import type { ArticleDetailsViewModel } from "@Crm.Article/ArticleDetailsViewModel";
import type { UserGroupDetailsViewModel } from "@Main/UserGroupDetailsViewModel";


export class ArticleUserGroupRelationshipEditModalViewModel extends window.Main.ViewModels.ViewModelBase {

	articleUserGroupRelationship = ko.observable<Crm.Service.Team.Rest.Model.ObservableCrmServiceTeam_ArticleUserGroupRelationship>(null);
	loading = ko.observable<boolean>(true);
	mode: string;
	parentViewModel: any;
	errors = ko.validation.group(this.articleUserGroupRelationship, { deep: true });
	articleDowntime = ko.observable<Crm.Article.Rest.Model.CrmArticle_ArticleDowntime>(null);

	constructor(parentViewModel: UserGroupDetailsViewModel | ArticleDetailsViewModel) {
		super();
		this.parentViewModel = parentViewModel;
		this.setMode(parentViewModel);

	}
	setMode(parentViewModel: UserGroupDetailsViewModel | ArticleDetailsViewModel): void {
		if (parentViewModel instanceof window.Main.ViewModels.UserGroupDetailsViewModel) {
			this.mode = "usergroup";
		} else if (parentViewModel instanceof window.Crm.Article.ViewModels.ArticleDetailsViewModel) {
			this.mode = "article";
		}
	};

	async init(id?: string): Promise<void> {
		if (id) {
			const articleUserGroupRelationship = await window.database.CrmServiceTeam_ArticleUserGroupRelationship.find(id);
			window.database.attachOrGet(articleUserGroupRelationship)
			this.articleUserGroupRelationship(articleUserGroupRelationship.asKoObservable());
		} else {
			const articleUserGroupRelationship = window.database.CrmServiceTeam_ArticleUserGroupRelationship.defaultType.create();
			if (this.mode === "usergroup") {
				articleUserGroupRelationship.UserGroupKey = this.parentViewModel.userGroup().Id();
			}
			else if (this.mode === "article") {
				articleUserGroupRelationship.ArticleKey = this.parentViewModel.contactId();
			}
			window.database.add(articleUserGroupRelationship);
			this.articleUserGroupRelationship(articleUserGroupRelationship.asKoObservable());
		}
		this.articleUserGroupRelationship().From.extend({
			required: {
				params: true,
				message: window.Helper.String.getTranslatedString("RuleViolation.Required")
					.replace("{0}", window.Helper.String.getTranslatedString("AssignedFrom"))
			}
		});
		this.articleUserGroupRelationship().To.extend({
			required: {
				params: true,
				message: window.Helper.String.getTranslatedString("RuleViolation.Required")
					.replace("{0}", window.Helper.String.getTranslatedString("AssignedTo"))
			}
		});
	};

	getArticleSelect2Filter(query: $data.Queryable<Crm.Article.Rest.Model.CrmArticle_Article>, filter: string): $data.Queryable<Crm.Article.Rest.Model.CrmArticle_Article> {
		const language = (document.getElementById("meta.CurrentLanguage") as HTMLMetaElement).content;
		query = query.filter(function (it) {
			return it.ArticleTypeKey in ["Vehicle", "Tool"];
		});
		return window.Helper.Article.getArticleAutocompleteFilter(query, filter, language);
	};

	async articleAvailabilityCheck(articleKey: string, from, to): Promise<void> {
		if (!articleKey || !from || !to || from > to || !window.database.CrmArticle_ArticleDowntime) {
			return;
		}
		try {
			const downtimes = await window.database.CrmArticle_ArticleDowntime
				.include("Article")
				.filter(function (articleDowntime) {
					return articleDowntime.ArticleKey === this.articleKey &&
						articleDowntime.From <= this.to &&
						articleDowntime.To >= this.from
				},
					{ articleKey: articleKey, from: from, to: to })
				.take(1)
				.toArray();
			this.articleDowntime(downtimes.length === 1 ? downtimes[0] : null);
		}
		catch (e) {
			window.swal(window.Helper.String.getTranslatedString("Error"), (e as Error).message, "error");
		}
	}

	async save(): Promise<void> {
		this.loading(true);
		await this.articleAvailabilityCheck(this.articleUserGroupRelationship().ArticleKey(), this.articleUserGroupRelationship().From(), this.articleUserGroupRelationship().To());
		if (this.errors().length > 0 || this.articleDowntime()) {
			this.loading(false);
			this.errors.showAllMessages();
			return;
		}
		try {
			await window.database.saveChanges()
			this.loading(false);
			$(".modal:visible").modal("hide");
		} catch {
			this.loading(false);
			swal(
				window.Helper.String.getTranslatedString("UnknownError"),
				window.Helper.String.getTranslatedString("Error_InternalServerError"),
				"error"
			);
		}
	};
}
namespace("Crm.Service.Team.ViewModels").ArticleUserGroupRelationshipEditModalViewModel = ArticleUserGroupRelationshipEditModalViewModel;
