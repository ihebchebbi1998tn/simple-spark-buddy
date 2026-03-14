window.Helper.Database.setTransactionId("CrmServiceTeam_ArticleUserGroupRelationship",
	async function (articleUserGroupRelationship: Crm.Service.Team.Rest.Model.CrmServiceTeam_ArticleUserGroupRelationship) {
		return [articleUserGroupRelationship.ArticleKey, articleUserGroupRelationship.UserGroupKey];
	});