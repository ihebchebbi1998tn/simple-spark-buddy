import {namespace} from "@Main/namespace";
import type {ArticleDetailsViewModel} from "@Crm.Article/ArticleDetailsViewModel";

export class ArticleDetailsInstallationsTabViewModel extends window.Crm.Service.ViewModels.InstallationListIndexViewModel {
	article = ko.observable<Crm.Article.Rest.Model.ObservableCrmArticle_Article>(null);

	constructor(parentViewModel: ArticleDetailsViewModel) {
		super();
		this.isTabViewModel(true);
		this.article(parentViewModel.article());
		this.infiniteScroll(true);
	}

	applyFilters(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_Installation>): $data.Queryable<Crm.Service.Rest.Model.CrmService_Installation> {
		query = super.applyFilters(query);
		if(window.Crm.Service.Settings.ArticleInstallations.FilterByItemNo) {
			query = query.filter("it.Article.ItemNo == this.itemNo || it.CustomItemNo == this.itemNo", { itemNo: this.article().ItemNo()});
		} else {
			query = query.filter("it.ArticleKey === this.articleKey", {articleKey: this.article().Id()});
		}
		return query;
	};
}

namespace("Crm.Article.ViewModels").ArticleDetailsInstallationsTabViewModel = ArticleDetailsInstallationsTabViewModel;