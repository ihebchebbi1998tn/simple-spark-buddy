import { namespace } from "./namespace";
import { Breadcrumb } from "@Main/breadcrumbs";
import {HelperUser} from "@Main/helper/Helper.User";

export class UserCreateViewModel extends window.Main.ViewModels.ViewModelBase {
	user = ko.observable<Main.Rest.Model.ObservableMain_User>(null);
	password = ko.observable<string>(null);
	password2 = ko.observable<string>(null);
	usernameIsEditable = ko.observable<boolean>(false);
	roleIds = ko.observableArray<string>([]);
	usergroupIds = ko.observableArray<string>([]);
	locales = ko.observableArray<string>([]);
	site = ko.observable<Main.Rest.Model.Main_Site>(null);
	licensed = ko.observable<boolean>(false);

	usedUserLicenses = ko.observable<number>(0);
	totalUserLicenses = ko.observable<number>(JSON.parse(window.Helper.Database.getFromLocalStorage("license")).UserLicenses);
	availableUserLicenses = ko.pureComputed<number>(() => this.totalUserLicenses()-this.usedUserLicenses());

	async addUser(): Promise<Main.Rest.Model.Main_User> {
		return await window.database.Main_User.AddUser({
			User: this.user().innerInstance,
			Password: this.password(),
			RoleIds: [],
			UsergroupIds: this.usergroupIds()
		}).first();
	}

	cancel(): void {
		window.database.detach(this.user().innerInstance);
		window.history.back();
	}

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		await this.setBreadcrumbs();
		let newUser = window.database.Main_User.defaultType.create();
		newUser.CreateDate = new Date();
		newUser.CreateUser = window.Helper.User.getCurrentUserName();
		newUser.ModifyDate = new Date();
		newUser.ModifyUser = window.Helper.User.getCurrentUserName();
		newUser.DefaultLanguageKey = await window.Helper.Culture.languageCulture();
		newUser.TimeZoneName = window.Helper.Date.getTimeZoneForUser(await window.Helper.User.getCurrentUser());
		window.database.add(newUser);
		this.user(newUser.asKoObservable());
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
		this.usernameIsEditable = ko.pureComputed(() => {
				return !!this.user() && !!this.user().FirstName() && !!this.user().LastName();
			}
		);
		this.usernameIsEditable.subscribe(() => {
			if (this.usernameIsEditable()) {
				this.user().Id(this.replaceIllegalCharacters(this.user().FirstName().toLowerCase()) + "." + this.replaceIllegalCharacters(this.user().LastName().toLowerCase()));
			}
		});
		this.user().Id.extend({
			validation: {
				validator: (val) => {
					return !/[^a-zA-Z0-9\.]/.test('' + val + '');
				},
				message: window.Helper.String.getTranslatedString("InvalidUsernameCharacters")
			}
		});
		ko.validation.addRule(this.user().Id, {
			rule: "unique",
			params: [window.database.Main_User, "Id", this.user().Id, "UserName", true],
		});
		this.user().Id.subscribe(() => this.user().Email.valueHasMutated());
		this.licensed.subscribe((val) => {
			this.user().LicensedAt(val ? new Date() : null);
		})
		this.locales(await $.get(window.Helper.Url.resolveUrl("~/Main/Resource/ListLocales?format=json")));
		await this.updateUsedUserLicenses();
	}

	async updateUsedUserLicenses(): Promise<void> {
		this.usedUserLicenses(await (window.Helper.User.getActiveUsers(window.database.Main_User) as $data.Queryable<Main.Rest.Model.Main_User>).count());
	}

	replaceIllegalCharacters(str: string): string {
		const regEx = new RegExp("[^a-zA-Z0-9]");
		return str.replace(regEx, "");
	}

	async submit(): Promise<void> {
		let errors = ko.validation.group(this);
		if (errors().length > 0) {
			errors.showAllMessages();
			errors.expandCollapsiblesWithErrors();
			return;
		}
		this.loading(true);
		let result = null;
		try {
			result = await this.addUser();
			await window.database.Main_User.SetRoles({
				User: result,
				RoleIds: this.roleIds()
			}).first();
			window.location.hash = "/Main/User/DetailsTemplate/" + result.Id;
		} catch (e) {
			this.loading(false);
			const errorMessage = window.Helper.String.tryExtractErrorMessageValue(JSON.parse((e as Error).message));
			window.swal(window.Helper.String.getTranslatedString("Error"), errorMessage, "error");
			if(result !== null) {
				window.location.hash = "/Main/User/DetailsTemplate/" + result.Id;
			}
			throw e;
		}
	}

	async setBreadcrumbs(): Promise<void> {
		await window.breadcrumbsViewModel.setBreadcrumbs([
			new Breadcrumb(window.Helper.getTranslatedString("User"), "UserAdmin::Index", "#/Main/UserList/IndexTemplate"),
			new Breadcrumb(window.Helper.getTranslatedString("CreateUser"), null, window.location.hash, null, null)
		]);
	};
}

namespace("Main.ViewModels").UserCreateViewModel = UserCreateViewModel;