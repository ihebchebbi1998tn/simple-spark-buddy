import {Mixin} from "ts-mixer";
import type {PmbbViewModel} from "@Main/PmbbViewModel";

export class ArticleDetailsViewModelExtension extends window.Crm.Article.ViewModels.ArticleDetailsViewModel {

	constructor() {
		super();
	}

	async onSaveArticleServiceExtensions(data: PmbbViewModel): Promise<void> {
		if (data.editContext().article().ExtensionValues().IsDefaultForServiceOrderTimes()) {
			await window.database.CrmArticle_Article
				.filter("it.ExtensionValues.IsDefaultForServiceOrderTimes === true && it.Id !== this.id", {id: data.editContext().article().Id()})
				.forEach(article => {
					window.database.attachOrGet(article);
					article.ExtensionValues.IsDefaultForServiceOrderTimes = false;
				});
		}
		if (data.editContext().article().ExtensionValues().CanBeAddedAfterCustomerSignature() && !data.editContext().article().ExtensionValues().CanBeEditedAfterCustomerSignature()) {
			this.article().ExtensionValues().CanBeEditedAfterCustomerSignature(true);
		}
	};
}
window.Crm.Article.ViewModels.ArticleDetailsViewModel = Mixin(ArticleDetailsViewModelExtension);
