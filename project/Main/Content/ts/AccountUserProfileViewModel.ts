import {HelperLookup} from "./helper/Helper.Lookup";
import {HelperCulture} from "./helper/Helper.Culture";
import {namespace} from "./namespace";
import {HelperString} from "./helper/Helper.String";
import {HelperUrl} from "./helper/Helper.Url";
import {HelperDatabase} from "./helper/Helper.Database";
import {HelperUser} from "./helper/Helper.User";
import Log from "./Logger";
import type {PmbbViewModel} from "./PmbbViewModel";

export class AccountUserProfileViewModel extends window.Main.ViewModels.ViewModelBase {

	lookups: LookupType = {
		languages: {$tableName: "Main_Language", $filterExpression: "it.IsSystemLanguage === true"},
		passwordStrengths: {$tableName: "Main_PasswordStrength"},
	};
	tabs = ko.observable<{}>({});
	accountInfo = ko.observable<Main.Rest.Model.ObservableMain_User>(null);
	avatarBlob = ko.observable<Blob>(null);
	avatarContent = ko.observable<string>(null);
	avatarUrl = ko.observable<string>(null);
	errors = ko.validation.group(this, {deep: true});
	fileInput = ko.observable<any>(null);
	languages = ko.observableArray<any>([]);
	locales = ko.observableArray<any>([]);
	confirmPassword = ko.observable<string>(null);
	newPassword = ko.observable<string>(null);
	oldPassword = ko.observable<string>(null);
	user = ko.observable<Main.Rest.Model.ObservableMain_User>(null);
	newPassword2 = ko.observable<string>(null);
	title = ko.observable<string>(null);
	additionalTitle = ko.observable<string>(null);
	site = ko.observable<Main.Rest.Model.Main_Site>(null);
	totpIsSetUp = ko.observable<boolean>(null);

	constructor() {
		super();
		this.newPassword.extend({
			validation: {
				async: true,
				onlyIf: () => {
					return this.newPassword() !== null;
				},
				validator: async (val, params, callback) => {
					var minPasswordStrength = this.site().ExtensionValues.MinPasswordStrength;
					await HelperUser.passwordValidation(val, minPasswordStrength, callback);
				}
			}
		});
	}

	async init(id?: string, params?: { [key: string]: string }): Promise<void> {
		await super.init(id, params);
		await this.loadUser();
		this.user().FirstName.subscribe(function () {
			this.setTitle();
		}, this);
		this.user().LastName.subscribe(function () {
			this.setTitle();
		}, this);
		this.oldPassword.extend({
			required: {
				message: HelperString.getTranslatedString("RuleViolation.OldPasswordIsIncorrect"),
				onlyIf: () => {
					ko.unwrap(this.user().Email);
					return this.newPassword() || this.newPassword2();
				},
				params: true
			},
			validation: {
				onlyIf: () => {
					return this.newPassword() !== null;
				},
				validator: (val) => {
					return val !== this.newPassword();
				},
				message: HelperString.getTranslatedString("RuleViolation.NewPasswordIsTheSameAsOldPassword"),
			}
		});
		this.newPassword2.extend({
			equal: {
				message: HelperString.getTranslatedString("RuleViolation.PasswordsDoNotMatch"),
				onlyIf: () => {
					return this.newPassword();
				},
				params: this.newPassword
			}
		});
		this.newPassword.extend({
			required: {
				message: HelperString.getTranslatedString("NewPasswordIsInvalid"),
				onlyIf: () => {
					return this.oldPassword();
				},
				params: true
			}
		});
		await HelperCulture.initialize();
		await HelperLookup.getLocalizedArrayMaps(this.lookups);
		const locales = await $.get(HelperUrl.resolveUrl("~/Main/Resource/ListLocales?format=json"));
		this.locales(locales);
		HelperDatabase.registerEventHandlers({}, {
			Main_User: {
				"afterUpdate": this.loadUser.bind(this)
			}
		});
		this.setTitle();
		this.additionalTitle(this.user().Id());
		let site = await window.database.Main_Site.GetCurrentSite().first();
		this.site(site);
	}

	async removeTotpAuthenticator(): Promise<void> {
		const confirm = await window.Helper.Confirm.genericConfirmAsync({
			text: window.Helper.String.getTranslatedString("ConfirmTotpAuthenticationRemove"),
			type: "warning"
		});
		if (!confirm) {
			return;
		}

		try {
			const result = await (await fetch(window.Helper.Url.resolveUrl("~/Main/Account/RemoveTotpAuthenticatorOfCurrentUser"), {
				method: "POST"
			})).json();
			if (result.length > 0) {
				const resultErrors = result.filter(x => x.Key !== "MESSAGE").map(x => x.Value);

				if (resultErrors.length === 0) {
					this.totpIsSetUp(false);
					this.showSnackbar(window.Helper.String.getTranslatedString("TotpAuthenticationSuccessfullyRemoved"));
				} else {
					window.swal(resultErrors.join(" "), "error");
				}
			}

			this.loading(false);
		} catch (e) {
			this.loading(false);
			window.swal(window.Helper.String.getTranslatedString("Error"), (e as Error).message, "error");
			throw e;
		}
	}

	async loadUser(): Promise<void> {
		const currentUserName = window.Helper.User.getCurrentUserName();
		const user = await window.database.Main_User.include("expandAvatar").find(currentUserName);
		const blob = user.Avatar !== null ? HelperString.base64toBlob(user.Avatar) : null;
		if (blob !== null) {
			this.avatarBlob(blob);
			this.avatarContent(user.Avatar);
			this.avatarUrl(window.URL.createObjectURL(blob));
		} else {
			this.avatarBlob(null);
			this.avatarContent(null);
			this.avatarUrl(null);
		}
		this.user(user.asKoObservable());
		this.accountInfo(HelperDatabase.createClone(user).asKoObservable());
		this.accountInfo().FirstName.extend({
			required: {
				message: HelperString.getTranslatedString("RuleViolation.Required")
					.replace("{0}", HelperString.getTranslatedString("FirstName")),
				params: true
			},
			validation: {
				validator: (val) => val.length <= 120,
				message: HelperString.getTranslatedString("RuleViolation.MaxLength")
					.replace("{0}", HelperString.getTranslatedString("FirstName")),
			}
		});
		this.accountInfo().LastName.extend({
			required: {
				message: HelperString.getTranslatedString("RuleViolation.Required")
					.replace("{0}", HelperString.getTranslatedString("LastName")),
				params: true
			},
			validation: {
				validator: (val) => val.length <= 120,
				message: HelperString.getTranslatedString("RuleViolation.MaxLength")
					.replace("{0}", HelperString.getTranslatedString("LastName")),
			}
		});
		this.accountInfo().Email.extend({
			required: {
				message: HelperString.getTranslatedString("RuleViolation.Required")
					.replace("{0}", HelperString.getTranslatedString("EMail")),
				params: true
			},
			validation: {
				validator: (val) => {
					const re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
					return re.test(String(val).toLowerCase());
				},
				message: HelperString.getTranslatedString("RuleViolation.InvalidEmail"),
			}
		});
		this.accountInfo().DefaultLanguageKey.extend({
			required: {
				message: HelperString.getTranslatedString("RuleViolation.Required")
					.replace("{0}", HelperString.getTranslatedString("Language")),
				params: true
			}
		});
		this.totpIsSetUp(this.user().TotpIsSetUp());
	}
	async onAfterSave(context: PmbbViewModel): Promise<void> {
		await this.loadUser();
	}
	async beforeSaveAccountInfo(context: PmbbViewModel): Promise<void> {
		if (this.errors().length > 0) {
			this.errors.showAllMessages();
			this.loading(false);
			context.loading(false);
			throw this.errors();
		}
	}

	async saveAccountInfo(): Promise<void> {
		if (this.errors().length > 0) {
			this.errors.showAllMessages();
			return;
		}
		this.loading(true);
		window.database.attachOrGet(this.user().innerInstance);
		this.user().FirstName(this.accountInfo().FirstName());
		this.user().LastName(this.accountInfo().LastName());
		this.user().Email(this.accountInfo().Email());
		this.user().DefaultLanguageKey(this.accountInfo().DefaultLanguageKey());
		this.user().DefaultLocale(this.accountInfo().DefaultLocale());
		this.user().TimeZoneName(this.accountInfo().TimeZoneName());

		const passwordChangeResultErrorName = "PasswordChangeResultError";
		try {
			let resetPassword = this.newPassword();
			if (resetPassword) {
				const passwordChangeResult = await (await fetch(HelperUrl.resolveUrl("~/Main/Account/ChangePassword"),
					{
						method: "POST",
						body: new URLSearchParams({
							"oldPassword": this.oldPassword(),
							"newPassword": this.newPassword(),
							"newPassword2": this.newPassword2()
						})
					})).json();

				if (passwordChangeResult?.length > 0) {
					const error = new Error();
					error.message = passwordChangeResult[0];
					error.name = passwordChangeResultErrorName;
					throw error;
				}
			}

			await window.database.saveChanges();
			this.loading(false);
		} catch (e) {
			Log.error(e);
			this.loading(false);
			const error = e as Error;
			if (error.name === passwordChangeResultErrorName) {
				swal(HelperString.getTranslatedString("Error"), error.message, "error");
			} else {
				swal(HelperString.getTranslatedString("UnknownError"),
					HelperString.getTranslatedString("Error_InternalServerError"),
					"error");
			}
			throw e;
		}
	}

	setTitle(): void {
		this.title(window.Helper.User.getDisplayName(this.user()));
	}
}

namespace("Main.ViewModels").AccountUserProfileViewModel = AccountUserProfileViewModel;