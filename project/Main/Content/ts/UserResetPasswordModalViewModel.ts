import {namespace} from "./namespace";
import {HelperLookup} from "@Main/helper/Helper.Lookup";
import {HelperUser} from "@Main/helper/Helper.User";

class UserResetPasswordModalViewModel extends window.Main.ViewModels.ViewModelBase {

	user = ko.observable<Main.Rest.Model.ObservableMain_User>(null);
	password = ko.observable<string>(null);
	password2 = ko.observable<string>(null);
	resultErrors = ko.observableArray<string>(null);
	site = ko.observable<Main.Rest.Model.Main_Site>(null);
	minPasswordStrength = ko.observable<number>(null);
	lookups: LookupType = {
		passwordStrengths: {$tableName: "Main_PasswordStrength"},
	};

	async init(id?: string, params?: { [key: string]: string }): Promise<void> {
		await super.init(id, params);
		let user = await window.database.Main_User.find(id);
		this.user(user.asKoObservable());
		let site = await window.database.Main_Site.GetCurrentSite().first();
		this.site(site);
		this.password.extend({
			required: {
				message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("Password")),
				params: true
			}
		});
		this.password.extend({
			validation: {
				async: true,
				onlyIf: () => {
					return this.password() !== null;
				},
				validator: async (val, params, callback) => {
					let minPasswordStrength = this.site().ExtensionValues.MinPasswordStrength;
					await HelperUser.passwordValidation(val, minPasswordStrength, callback);
				}
			}
		});
		this.password2.extend({
			equal: {
				message: window.Helper.String.getTranslatedString("RuleViolation.PasswordsDoNotMatch"),
				onlyIf: () => {
					return this.password();
				},
				params: this.password
			}
		});
		await HelperLookup.getLocalizedArrayMaps(this.lookups);
	}

	async resetPassword(): Promise<void> {
		let errors = ko.validation.group(this);
		if (errors().length > 0) {
			errors.showAllMessages();
			return;
		}
		this.loading(true);
		try {
			const result = await (await fetch(window.Helper.Url.resolveUrl("~/Main/UserAdmin/ResetPassword"), {
				method: "POST",
				body: new URLSearchParams({
					"username": this.user().Id(),
					"Password": this.password(),
					"ConfirmPassword": this.password2(),
					"PasswordChangeRequired": this.user().PasswordChangeRequired().toString()
				})
			})).json();
			if (result.length > 0) {
				const resultErrors = result.filter(x => x.Key !== "MESSAGE").map(x => x.Value);
				this.resultErrors(resultErrors);
			} else {
				this.resultErrors(null);
			}
			if (this.resultErrors().length === 0) {
				$(".modal:visible").modal("hide");
				this.showSnackbar(window.Helper.String.getTranslatedString("ResetPasswordSuccess"));
			}
			this.loading(false);
		} catch (e) {
			this.loading(false);
			window.swal(window.Helper.String.getTranslatedString("Error"), (e as Error).message, "error");
			throw e;
		}
	}
}

namespace("Main.ViewModels").UserResetPasswordModalViewModel = UserResetPasswordModalViewModel;