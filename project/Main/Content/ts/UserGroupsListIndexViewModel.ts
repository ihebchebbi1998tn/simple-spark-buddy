import {namespace} from "./namespace";
import {HelperString} from "./helper/Helper.String";
import {MultiFactorAuthenticationMode} from "@Main/helper/Helper.MultiFactorAuthentication";

export class UserGroupListIndexViewModel extends window.Main.ViewModels.GenericListViewModel<Main.Rest.Model.Main_Usergroup, Main.Rest.Model.ObservableMain_Usergroup> {
	id: string;
	pageTitle = HelperString.getTranslatedString("UserGroups");
	site: Main.Rest.Model.Main_Site;
	relatedMultiFactorAuthenticationModeKey = MultiFactorAuthenticationMode.MandatoryForSpecificUserGroupsKey;

	constructor() {
		super("Main_Usergroup", "Name", "ASC");
	}

	async init(id?: string, params?: { [key: string]: string }): Promise<void> {
		await super.init(id, params);
		this.site = await window.database.Main_Site.GetCurrentSite().first();
	}


	async deleteUserGroup(group: Main.Rest.Model.Main_Usergroup): Promise<void> {
		const dbGroup = await window.database.Main_Usergroup
			.include("Members")
			.filter(it => it.Id === this.id, {id: ko.unwrap(group.Id)})
			.first();
		const text = (dbGroup.Members.length > 0 ? HelperString.getTranslatedString("GroupIsNotEmpty") + " " : "")
			+ HelperString.getTranslatedString("ConfirmDelete");
		let confirm = await window.Helper.Confirm.genericConfirmAsync({
			text,
			type: "error",
			confirmButtonText: HelperString.getTranslatedString("Delete")
		});
		if (confirm) {
			const userGroup = window.database.Main_Usergroup.attachOrGet(group);
			window.database.remove(userGroup);
			await window.database.saveChanges();
		}
	}

	async toggleMultiFactorAuthenticationMandatory(group: Main.Rest.Model.Main_Usergroup): Promise<void> {
		const userGroup = window.database.Main_Usergroup.attachOrGet(group);
		userGroup.MultiFactorAuthenticationMandatory = !userGroup.MultiFactorAuthenticationMandatory;
		await window.database.saveChanges();

		if (userGroup.MultiFactorAuthenticationMandatory) {
			this.showSnackbar(window.Helper.String.getTranslatedString("MultiFactorAuthenticationSetAsMandatory"));
		} else {
			this.showSnackbar(window.Helper.String.getTranslatedString("MultiFactorAuthenticationNoLongerMandatory"));
		}
	}
}

namespace("Main.ViewModels").UserGroupListIndexViewModel = UserGroupListIndexViewModel;