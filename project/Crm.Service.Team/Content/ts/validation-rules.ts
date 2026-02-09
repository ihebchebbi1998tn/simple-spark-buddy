window.ko.validationRules.add("CrmServiceTeam_ArticleUserGroupRelationship",
	function (entity: Crm.Service.Team.Rest.Model.ObservableCrmServiceTeam_ArticleUserGroupRelationship) {
		entity.To.extend({
			validation: {
				async: true,
				validator: window.Helper.Article.OverlappingArticleUserRelationshipValidator.getValidationFunction(entity)
			}
		});
		entity.To.extend({
			validation: {
				message: window.Helper.String.getTranslatedString("RuleViolation.DateCanNotBeBeforeDate")
					.replace("{0}", window.Helper.String.getTranslatedString("To"))
					.replace("{1}", window.Helper.String.getTranslatedString("From").toLowerCase()),
				validator: function (val) {
					if (!!entity.To() && !!entity.From())
						return window.moment(val).isSameOrAfter(entity.From());
					else
						return true;
				}
			}
		});
		entity.ArticleKey.extend({
			required: {
				params: true,
				message: window.Helper.String
					.getTranslatedString("RuleViolation.Required")
					.replace("{0}", window.Helper.String.getTranslatedString("Article"))
			}
		});
		entity.UserGroupKey.extend({
			required: {
				params: true,
				message: window.Helper.String
					.getTranslatedString("RuleViolation.Required")
					.replace("{0}", window.Helper.String.getTranslatedString("Team"))
			}
		});
	});
window.ko.validationRules.add("Main_Usergroup", (entity: Main.Rest.Model.ObservableMain_Usergroup) => {
	entity.ExtensionValues().ValidTo.extend({
		validation:
			{
				message: window.Helper.String.getTranslatedString("RuleViolation.DateCanNotBeAfterDate")
					.replace("{0}", window.Helper.String.getTranslatedString("ValidFrom"))
					.replace("{1}", window.Helper.String.getTranslatedString("ValidTo")),
				onlyIf: () => entity.ExtensionValues().IsServiceTeam() && !!entity.ExtensionValues().ValidFrom(),
				validator: val => !window.moment(entity.ExtensionValues().ValidFrom()).isAfter(val)
			}
	});
});