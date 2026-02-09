window.ko.validationRules.add("Main_PermissionSchemaRole", function (entity: Main.Rest.Model.ObservableMain_PermissionSchemaRole) {
		entity.Name.extend({
			unique: {
				params: [window.database.Main_PermissionSchemaRole, 'Name', entity.UId],
				onlyIf: function () {
					return entity.innerInstance.entityState === $data.EntityState.Added;
				},
				message: window.Helper.String.getTranslatedString("RuleViolation.Unique").replace("{0}", window.Helper.String.getTranslatedString("Name"))
			},
		});
	});
window.ko.validationRules.add("Main_Usergroup", function (entity: Main.Rest.Model.ObservableMain_Usergroup) {
		ko.validation.addRule(entity.Name, {
			rule: "required",
			message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("Name")),
			params: true
		});
		ko.validation.addRule(entity.Name, {
			rule: "unique",
			params: [window.database.Main_Usergroup, "Name", entity.Id],
			message: window.Helper.String.getTranslatedString("RuleViolation.Unique").replace("{0}", window.Helper.String.getTranslatedString("Name"))
		});
	});
window.ko.validationRules.add("Main_User", function (entity: Main.Rest.Model.ObservableMain_User) {
	window.ko.validation.addRule(entity.AdName, {
			rule: "required",
			message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("AdName")),
			params: window.Main.Settings.UseActiveDirectoryAuthenticationService
		});
	window.ko.validation.addRule(entity.Email, {
			rule: "unique",
			params: [window.database.Main_User, "Email", entity.Id, "EMail"]
		});
	});
window.ko.validationRules.add("Main_UserSkill", function (entity: Main.Rest.Model.ObservableMain_UserSkill) {
	ko.validation.addRule(entity.SkillKey, {
		rule: "required",
		message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("SkillKey")),
		params: true
	});
});
window.ko.validationRules.add("Main_UserAsset", function (entity: Main.Rest.Model.ObservableMain_UserAsset) {
	ko.validation.addRule(entity.AssetKey, {
		rule: "required",
		message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("Asset")),
		params: true
	});
});