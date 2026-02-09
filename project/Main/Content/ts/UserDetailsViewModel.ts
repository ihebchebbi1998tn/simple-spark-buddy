import {namespace} from "./namespace";
import {Breadcrumb} from "./breadcrumbs";
import {HelperDatabase} from "./helper/Helper.Database";
import type {PmbbViewModel} from "./PmbbViewModel";

export class UserDetailsViewModel extends window.Main.ViewModels.ViewModelBase {
	tabs = ko.observable<{}>({});
	locales = ko.observableArray<string>([]);
	lookups: LookupType = {
		languages: {$tableName: "Main_Language", $filterExpression: "it.IsSystemLanguage === true"},
	};
	roleIds = ko.observableArray<string>([]);
	roles: Array<Main.Rest.Model.Main_PermissionSchemaRole> = [];
	user = ko.observable<Main.Rest.Model.ObservableMain_User>(null);
	userGroups: Array<Main.Rest.Model.Main_Usergroup> = [];
	selectableLocales = ko.observableArray<string>([]);
	
	usedUserLicenses = ko.observable<number>(0);
	totalUserLicenses = ko.observable<number>(JSON.parse(window.Helper.Database.getFromLocalStorage("license")).UserLicenses);
	availableUserLicenses = ko.pureComputed<number>(() => this.totalUserLicenses()-this.usedUserLicenses());

	getRolesFromIds(ids: string[]): Select2AutoCompleterResult[] {
		return this.roles
			.filter(x => ids.indexOf(x.UId) !== -1)
			.map(window.Helper.User.mapRoleForSelect2Display);
	}

	getUsergroupsFromIds(ids: string[]): Select2AutoCompleterResult[] {
		return this.userGroups
			.filter(x => ids.indexOf(x.Id) !== -1)
			.map(x => window.Helper.User.mapUsergroupForSelect2Display(x));
	}
	title = ko.observable<string>(null);

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		this.roles = await window.database.Main_PermissionSchemaRole.toArray();
		await this.loadUser(id);
		this.user().FirstName.subscribe(function () {
			this.setTitle();
		}, this);
		this.user().LastName.subscribe(function () {
			this.setTitle();
		}, this);
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		const locales = await fetch(window.Helper.Url.resolveUrl("~/Main/Resource/ListLocales?format=json")).then(x => x.json());
		this.locales(locales);
		this.userGroups = await window.database.Main_Usergroup.toArray();
		this.setTitle();
		await this.setBreadcrumbs(this.title());
		HelperDatabase.registerEventHandlers(this, {
			Main_User: {
				afterUpdate: this.updateUsedUserLicenses
			}
		});
		await this.updateUsedUserLicenses();
	}

	async updateUsedUserLicenses(): Promise<void> {
		this.usedUserLicenses(await (window.Helper.User.getActiveUsers(window.database.Main_User) as $data.Queryable<Main.Rest.Model.Main_User>).count());
	}

	async loadUser(id: string): Promise<void> {
		const user = await window.database.Main_User
			.include("expandAvatar")
			.include("Roles")
			.include("UsergroupObjects")
			.find(id);
		this.user(user.asKoObservable());
		this.roleIds(user.RoleIds);
	}

	async onAfterSave(context: PmbbViewModel): Promise<void> {
		await this.loadUser(this.user().Id());
	}

	onCancel(context: PmbbViewModel): void {
		const user = context.viewContext.user();
		this.roleIds(user.RoleIds());
	}

	async onSave(context: PmbbViewModel): Promise<void> {
		const user = context.viewContext.user();

		const roleIds = this.roleIds();
		const roles = user.RoleIds();
		const addedRoleIds = roleIds.filter(id => !roles.some(r => r === id));
		const removedRoleIds = roles.filter(r => roleIds.indexOf(r) === -1);
		if (addedRoleIds.length > 0 || removedRoleIds.length > 0) {
			try {
				await window.database.Main_User.SetRoles({
					User: user.innerInstance,
					RoleIds: roleIds
				}).first();
			} catch (e) {
				const errorMessage = window.Helper.String.tryExtractErrorMessageValue(JSON.parse((e as Error).message));
				window.swal(window.Helper.String.getTranslatedString("Error"),
					errorMessage,
					"error");
			}
		}

		const usergroupIds = user.UsergroupIds();
		const usergroups = user.UsergroupObjects();
		const addedUsergroupIds = usergroupIds.filter(id => !usergroups.some(ug => ug.Id() === id));
		const removedUsergroupIds = usergroups.filter(ug => usergroupIds.indexOf(ug.Id()) === -1).map(ug => ug.Id());
		if (addedUsergroupIds.length > 0 || removedUsergroupIds.length > 0) {
			await window.database.Main_User.SetUsergroups({
				User: user.innerInstance,
				UsergroupIds: usergroupIds
			}).first();
			user.Usergroups(this.getUsergroupsFromIds(usergroupIds).map((x: any) => x.text));
		}
	}

	async setBreadcrumbs(title: string): Promise<void> {
		await window.breadcrumbsViewModel.setBreadcrumbs([
			new Breadcrumb(window.Helper.String.getTranslatedString("User"), "UserAdmin::Index", "#/Main/UserList/IndexTemplate"),
			new Breadcrumb(title, null, window.location.hash)
		]);
	}

	setTitle(): void {
		this.title(window.Helper.User.getDisplayName(this.user()));
	}

	async toggleActive(user: Main.Rest.Model.ObservableMain_User): Promise<void> {
		this.loading(true);
		await window.Helper.User.toggleActive(user);
		this.loading(false);
	}

	onLanguageSelect(accountInfo: Main.Rest.Model.ObservableMain_User, language: Main.Rest.Model.Lookups.Main_Language): void {
		if (!!language) {
			if (!(accountInfo.DefaultLocale() && accountInfo.DefaultLocale().startsWith(language.Key))) {
				accountInfo.DefaultLocale(null);
			}
			this.selectableLocales(this.locales().filter(x => x.startsWith(language.Key)));
		} else {
			accountInfo.DefaultLocale(null);
		}
	}

	async resetTotp(user: Main.Rest.Model.ObservableMain_User): Promise<void> {
		if (this.loading()) {
			return;
		}

		this.loading(true);
		if (!await window.Helper.User.resetTotp(user)) {
			this.loading(false);
		} else {
			await this.onAfterSave(null);
			this.loading(false);
			this.showSnackbar([window.Helper.String.getTranslatedString("TotpAuthenticationSuccessfullyRemoved"), window.Helper.User.getDisplayName(user)].join(" - "))
		}
	}
}
namespace("Main.ViewModels").UserDetailsViewModel ||= UserDetailsViewModel;