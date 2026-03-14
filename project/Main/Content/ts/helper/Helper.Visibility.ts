import {HelperString} from "./Helper.String";

export class HelperVisibility {
	static getVisibilityInformationText(entityWithVisibility: any, entityTypeName: string, users: (Main.Rest.Model.Main_User | Main.Rest.Model.ObservableMain_User)[], usergroups: (Main.Rest.Model.Main_Usergroup | Main.Rest.Model.ObservableMain_Usergroup)[]) {
		let translationKey;
		if (entityWithVisibility.Visibility() === 1) {
			translationKey = "VisibleForOnlyMe";
		} else if (entityWithVisibility.Visibility() === 2) {
			translationKey = "VisibleForEverybody";
		} else if (entityWithVisibility.VisibleToUserIds().length === 1) {
			translationKey = "VisibleForUser";
		} else if (entityWithVisibility.VisibleToUserIds().length > 1) {
			translationKey = "VisibleForUsers";
		} else if (entityWithVisibility.VisibleToUsergroupIds().length === 1) {
			translationKey = "VisibleForUsergroup";
		} else if (entityWithVisibility.VisibleToUsergroupIds().length > 1) {
			translationKey = "VisibleForUsergroups";
		} else {
			return null;
		}
		let visibilityInformationText = HelperString.getTranslatedString(translationKey);
		visibilityInformationText = visibilityInformationText.replace("{0}", window.Helper.String.getTranslatedString("This" + entityTypeName));
		if (entityWithVisibility.Visibility() === 4) {
			const usergroupNames = usergroups.filter(function (x) {
				return entityWithVisibility.VisibleToUsergroupIds().indexOf(ko.unwrap(x.Id)) !== -1;
			}).map(function (x) {
				return ko.unwrap(x.Name);
			});
			visibilityInformationText = visibilityInformationText.replace("{1}", usergroupNames.join(", "));
		} else if (entityWithVisibility.Visibility() === 8) {
			const userDisplayNames = users.filter(function (x) {
				return entityWithVisibility.VisibleToUserIds().indexOf(ko.unwrap(x.Id)) !== -1;
			}).map(function (x) {
				return window.Helper.User.getDisplayName(x);
			});
			visibilityInformationText = visibilityInformationText.replace("{1}", userDisplayNames.join(", "));
		}
		return visibilityInformationText;
	}
}
