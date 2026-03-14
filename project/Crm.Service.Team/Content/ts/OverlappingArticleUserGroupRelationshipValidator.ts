(function () {

	function getOverlappingArticleUserGroupRelationship(id: string, articleKey: string, from, to, callback) {
		if (!articleKey || !from || !to || from > to || !window.database.CrmServiceTeam_ArticleUserGroupRelationship) {
			callback(null);
			return;
		}
		window.database.CrmServiceTeam_ArticleUserGroupRelationship
			.include("Article")
			.include("UserGroup")
			.first(function (articleUserGroupRelationship) {
				return articleUserGroupRelationship.ArticleKey === this.articleKey &&
					articleUserGroupRelationship.From < this.to &&
					articleUserGroupRelationship.To > this.from &&
					articleUserGroupRelationship.Id !== this.id;
			},
				{ id: id, articleKey: articleKey, from: from, to: to })
			.then(function (result) {
				callback(result);
			}).catch(function () {
				callback(null);
			});
	}

	var getValidationFunction = window.Helper.Article.OverlappingArticleUserRelationshipValidator.getValidationFunction;
	window.Helper.Article.OverlappingArticleUserRelationshipValidator.getValidationFunction = function (articleUserGroupRelationship) {
		articleUserGroupRelationship = window.ko.unwrap(articleUserGroupRelationship);
		return function (val, params, callback) {
			getOverlappingArticleUserGroupRelationship(articleUserGroupRelationship.Id(),
				articleUserGroupRelationship.ArticleKey(),
				articleUserGroupRelationship.From(),
				articleUserGroupRelationship.To(),
				function (overlappingArticleUserGroupRelationship) {
					if (!overlappingArticleUserGroupRelationship) {
						getValidationFunction(articleUserGroupRelationship)(val, params, callback);
					} else {
						var message = window.Helper.String.getTranslatedString("OverlappingByArticleUserGroupRelationship")
							.replace("{0}", overlappingArticleUserGroupRelationship.Article.ArticleTypeKey)
							.replace("{1}", window.Globalize.formatDate(overlappingArticleUserGroupRelationship.From, { date: "short" }))
							.replace("{2}", window.Globalize.formatDate(overlappingArticleUserGroupRelationship.To, { date: "short" }))
							.replace("{3}", overlappingArticleUserGroupRelationship.UserGroup.Name);
						callback({ isValid: false, message: message });
					}
				});

		}
	}
})();