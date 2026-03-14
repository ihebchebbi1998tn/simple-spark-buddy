import {HelperUrl} from "./Helper.Url";
import { HelperString } from "./Helper.String";
import {HelperConfirm} from "./Helper.Confirm";
import {HelperLookup} from "./Helper.Lookup";

function appendToken(url: string): string {
	const tokentest = /([?&])token=([^&#]+)/.exec(window.location.search);
	const token = !!tokentest ? tokentest[2] : $("#meta\\.CurrentToken").attr("content") || null;

	if (token && url && url.search(/([?&])+token=/) === -1) {
		const urlParts = url.split("?");
		const urlParams = new URLSearchParams(urlParts[1]);
		if (urlParams.get("token") === null) {
			urlParams.append("token", token);
		}
		url = urlParts[0] + "?" + urlParams.toString();
	}
	return url;
}

export class HelperUser {
	static currentUser: Main.Rest.Model.Main_User;
	static getCurrentUserName(): string {
		const userElement = document.getElementById("meta.CurrentUser") as HTMLMetaElement;
		return userElement ? userElement.content : null;
	}
	static async getCurrentUser(): Promise<Main.Rest.Model.Main_User> {
		if (window.Helper.User.currentUser){
			return window.Helper.User.currentUser
		}
		const url = HelperUrl.resolveUrl("~/Main/Users/CurrentUser.json");
		let user = await fetch(appendToken(url)).then(r => r.json());
		window.Helper.User.currentUser = window.database && window.database.Main_User ? window.database.Main_User.defaultType.create(user) : user;
		return user;
	}

	static async fetchUserWithAvatar(username: string): Promise<Main.Rest.Model.Main_User> {
		return new Promise(resolve => {
			window.database.Main_User.include("expandAvatar").find(username).then(resolve);
		});
	}

	static getDisplayName(user: Main.Rest.Model.Main_User | Main.Rest.Model.ObservableMain_User): string {
		user = ko.unwrap(user);
		if (!user) {
			return HelperString.getTranslatedString("Unspecified");
		}
		const tokens = [];
		if (ko.unwrap(user.LastName)) {
			tokens.push(ko.unwrap(user.LastName));
		}
		if (ko.unwrap(user.FirstName)) {
			tokens.push(ko.unwrap(user.FirstName));
		}
		if (tokens.length < 2 && ko.unwrap(user.Email)) {
			tokens.push(ko.unwrap(user.Email));
		}
		return tokens.join(", ");
	}
	static filterUserQuery(query: $data.Queryable<Main.Rest.Model.Main_User>, term: string, userGroupId?: string | {Operator:string,Value:string}, roleIds?: string | string[]): $data.Queryable<Main.Rest.Model.Main_User> {
		if (term) {
			query = window.Helper.String.contains(query, term, ["FirstName.toLowerCase()", "LastName.toLowerCase()"]);
		}
		if (userGroupId) {
			let userGroupIdValue = typeof userGroupId === "string" ? userGroupId : userGroupId.Value;
			if (window.database.storageProvider.name === "webSql") {
				query = query.filter("it.UsergroupIds.contains(this.userGroupId)", { userGroupId: `"${userGroupIdValue}"` });
			} else {
				query = query.filter("it.UsergroupObjects.some(function(it2) { return it2.Id === this.userGroupId; })", { userGroupId: userGroupIdValue });
			}
		}
		if (typeof roleIds === "string"){
			roleIds = [roleIds];
		}
		if (roleIds && roleIds.length > 0) {
			if (window.database.storageProvider.name === "webSql") {
				let roleIdFilter = roleIds.map(roleId => "it.RoleIds.contains('" + roleId + "')").join(" || ");
				query = query.filter(roleIdFilter);
			} else {
				let roleIdFilter = roleIds.map(roleId => "it2.UId === '" + roleId + "'").join(" || ");
				query = query.filter("it.Roles.some(function(it2) { return " + roleIdFilter + "; })");
			}
		}
		query = query.filter(function(it) {
			return it.Discharged === false;
		});
		return query;
	}

	static filterUsergroupQuery(query: $data.Queryable<Main.Rest.Model.Main_Usergroup>, term: string): $data.Queryable<Main.Rest.Model.Main_Usergroup> {
		if (term) {
			query = window.Helper.String.contains(query, term, ["Name"]);
		}
		return query;
	}
	static filterUsergroupQueryById(query: $data.Queryable<Main.Rest.Model.Main_Usergroup>, term: string): $data.Queryable<Main.Rest.Model.Main_Usergroup> {
		if (term) {
			query = query.filter(function(it) {
					return it.Id === this.term;
				},
				{ term: term });
		}
		return query;
	}
	static languageFilter(query: $data.Queryable<Main.Rest.Model.Lookups.Main_Language>, term: string): $data.Queryable<Main.Rest.Model.Lookups.Main_Language> {
		query = query.filter("it.IsSystemLanguage === true");
		return HelperLookup.queryLookup(query, term, undefined, undefined);
	}
	static mapForSelect2Display(user: Main.Rest.Model.Main_User): Select2AutoCompleterResult {
		return {
			id: user.Id,
			item: user,
			text: window.Helper.User.getDisplayName(user)
		};
	}
	static getUsersByIds(query: $data.Queryable<Main.Rest.Model.Main_User>, ids: string[]): $data.Queryable<Main.Rest.Model.Main_User> {
		return query.filter(function (it) {
			return it.Id in this.ids;
		},
			{ ids: ids });
	};
	static mapRoleForSelect2Display(role: Main.Rest.Model.Main_PermissionSchemaRole): Select2AutoCompleterResult {
		role = window.Helper.Database.getDatabaseEntity(role);
		return {
			id: role.UId,
			item: role,
			text: HelperString.getTranslatedString(role.Name, role.Name)
		};
	}
	static mapUsergroupForSelect2Display(usergroup: Main.Rest.Model.Main_Usergroup): Select2AutoCompleterResult {
		usergroup = window.Helper.Database.getDatabaseEntity(usergroup);
		return {
			id: usergroup.Id,
			item: usergroup,
			text: usergroup.Name
		};
	}
	static getUsergroupsDisplay(usergroups: Main.Rest.Model.ObservableMain_Usergroup[]): string {
		// @ts-ignore
		return usergroups().map(x => x.Name()).sort((a, b) => a.localeCompare(b)).join(', ');
	}
	static areUsergroupsVisible(usergroups: Main.Rest.Model.ObservableMain_Usergroup[]): boolean {
		// @ts-ignore
		return usergroups().length === 0;
	}
	static async toggleActive(user: Main.Rest.Model.ObservableMain_User) : Promise<boolean> {
		try {
			await HelperConfirm.genericConfirm({
				text: HelperString
					.getTranslatedString(user.Discharged() ? "ConfirmActivateUser" : "ConfirmDeactivateUser")
					.replace("{0}", ko.unwrap(user.Id)),
				type: "info"
			});
		} catch {
			return false;
		}
		window.database.attachOrGet(user.innerInstance);
		user.Discharged(!user.Discharged());
		user.DischargeDate(user.Discharged() ? new Date() : null);
		if(user.Discharged()) {
			const devices = await window.database.Main_Device.filter("it.Username == this.username", {username: user.Id()}).toArray();
			for(const device of devices) {
				window.database.remove(device);
			}
		}
		try {
			await window.database.saveChanges();
		} catch (e: any) {
			user.Discharged(!user.Discharged());
			user.DischargeDate(user.Discharged() ? new Date() : null);
			const errorMessage = window.Helper.String.tryExtractErrorMessageValue(JSON.parse(e.data.response.body));
			window.swal(window.Helper.String.getTranslatedString("Error"),
				errorMessage,
				"error");
		}
		return true;
	}
	
	static async resetGeneralToken(user) {
		try {
			await HelperConfirm.genericConfirm({
				text: HelperString
					.getTranslatedString("ConfirmResetToken")
					.replace("{0}", ko.unwrap(this.getDisplayName(user))),
				type: "info"
			});
		} catch {
			return false;
		}

		try {
			const result = await $.get(window.Helper.Url.resolveUrl("~/Account/ResetGeneralToken"), {email: user.Email()});
			if(result.errorMessage)
				throw new Error(result.errorMessage);
			user.GeneralToken(result);
			return true;
		} catch (e) {
			window.Log.error(e);
			window.swal(window.Helper.String.getTranslatedString("UnknownError"),
				(e as Error)?.message ?? HelperString.getTranslatedString("Error_InternalServerError"),
				"error");
			return false;
		}
	}

	static async resetTotp(user: Main.Rest.Model.ObservableMain_User) : Promise<boolean> {
		try {
			await HelperConfirm.genericConfirm({
				text: HelperString
					.getTranslatedString("ConfirmTotpAuthenticationRemove")
					.replace("{0}", ko.unwrap(this.getDisplayName(user))),
				type: "info"
			});
		} catch {
			return false;
		}

		try {
			const result = await (await fetch(window.Helper.Url.resolveUrl("~/Account/RemoveTotpAuthenticator"), {
				method: "POST",
				body: new URLSearchParams({
					"userName": user.Id()
				})
			})).json();

			if (result.errorMessage) {
				throw new Error(result.errorMessage);
			}

			return true;
		} catch (e) {
			window.Log.error(e);
			window.swal(window.Helper.String.getTranslatedString("UnknownError"),
				(e as Error)?.message ?? HelperString.getTranslatedString("Error_InternalServerError"),
				"error");
			return false;
		}
	}

	static getShortDisplayName(user) {
		const u = ko.toJS(user);
		if (u === null) {
			return HelperString.getTranslatedString("Unspecified");
		}
		return (u.FirstName.substr(0, 1) + u.LastName.substr(0, 2)).toUpperCase();
	}
	static hasValidLicense(user: Main.Rest.Model.ObservableMain_User): boolean {
		return ko.unwrap(user.LicensedAt) !== null;
	}

	static getActiveUsers(users: (Main.Rest.Model.ObservableMain_User[] | $data.Queryable<Main.Rest.Model.Main_User>)) : (Main.Rest.Model.ObservableMain_User[] | $data.Queryable<Main.Rest.Model.Main_User>) {
		if(Array.isArray(users)) {
			return users.filter(user => !user.Discharged() && window.Helper.User.hasValidLicense(user));
		} else {
			return users.filter("!it.Discharged && it.LicensedAt != null");
		}
	}

	static getInactiveUsers(users: (Main.Rest.Model.ObservableMain_User[] | $data.Queryable<Main.Rest.Model.Main_User>)) : (Main.Rest.Model.ObservableMain_User[] | $data.Queryable<Main.Rest.Model.Main_User>) {
		if(Array.isArray(users)) {
			return users.filter(user => user.Discharged() || !window.Helper.User.hasValidLicense(user));
		} else {
			return users.filter("it.Discharged || it.LicensedAt == null");
		}
	}

	static getLicensedUsers(users: (Main.Rest.Model.ObservableMain_User[] | $data.Queryable<Main.Rest.Model.Main_User>)) : (Main.Rest.Model.ObservableMain_User[] | $data.Queryable<Main.Rest.Model.Main_User>) {
		if(Array.isArray(users)) {
			return users.filter(user => window.Helper.User.hasValidLicense(user));
		} else {
			return users.filter("it.LicensedAt != null");
		}
	}

	static getNotLicensedUsers(users: (Main.Rest.Model.ObservableMain_User[] | $data.Queryable<Main.Rest.Model.Main_User>)) : (Main.Rest.Model.ObservableMain_User[] | $data.Queryable<Main.Rest.Model.Main_User>) {
		if(Array.isArray(users)) {
			return users.filter(user => !window.Helper.User.hasValidLicense(user));
		} else {
			return users.filter("it.LicensedAt == null");
		}
	}

	static async refreshUserCache(): Promise<void> {
		await fetch(window.Helper.Url.resolveUrl("~/Main/UserAdmin/RefreshUserCache"), {redirect: "manual"});
	}

	static async passwordValidation(value, minPasswordStrength, callback): Promise<void> {
		try {
			let passwordStrength = await (await fetch(window.Helper.Url.resolveUrl("~/Account/EvaluatePassword"), {
				method: "POST",
				body: new URLSearchParams({
					"password": value
				})
			})).json();
			if (passwordStrength.Score < minPasswordStrength){
				let message = window.Helper.String.getTranslatedString("RuleViolation.PasswordTooWeak");
				message += " (" + passwordStrength.Score / 0.04 + "%)";
				if (passwordStrength.Feedback.Warning){
					message += "\n" + passwordStrength.Feedback.Warning;
				}
				callback({
					isValid: false,
					message: message
				});
			} else {
				callback(true);
			}
		} catch {
			callback(true);
		}
	}

}

// @ts-ignore
(window.Helper = window.Helper || {}).User = HelperUser;
