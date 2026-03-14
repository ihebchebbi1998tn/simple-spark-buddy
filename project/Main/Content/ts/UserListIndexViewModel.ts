import {namespace} from "./namespace";
import { HelperString } from "./helper/Helper.String";
import { HelperDatabase } from "./helper/Helper.Database";

export class UserListIndexViewModel extends window.Main.ViewModels.GenericListViewModel<Main.Rest.Model.Main_User, Main.Rest.Model.ObservableMain_User> {
	usedUserLicenses = ko.observable<number>(0);
	usersWithoutAdName = ko.observable<number>(0);
	totalUserLicenses = ko.observable<number>(JSON.parse(window.Helper.Database.getFromLocalStorage("license")).UserLicenses);
	availableUserLicenses = ko.pureComputed<number>(() => this.totalUserLicenses()-this.usedUserLicenses());
	lookups = {
		languages: {$tableName: "Main_Language"}
	};

	constructor() {
		super("Main_User", ["LastName", "FirstName"], ["ASC", "ASC"], ["Roles", "UsergroupObjects"]);
		this.addBookmarks();
		this.addBulkActions();
	}

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		HelperDatabase.registerEventHandlers(this, {
			Main_User: {
				afterUpdate: this.updateUsedUserLicenses
			}
		});
		await this.updateUsedUserLicenses();
	}
	async updateUsedUserLicenses(): Promise<void> {
		const activeUsers = window.Helper.User.getActiveUsers(window.database.Main_User) as $data.Queryable<Main.Rest.Model.Main_User>; 
		this.usedUserLicenses(await activeUsers.count());
		this.usersWithoutAdName(await activeUsers.filter(user => !user.Discharged && user.AdName === null).count());
	}

	addBookmarks(): void {
		let activeBookmark = {
			Category: window.Helper.String.getTranslatedString("Show"),
			Name: window.Helper.String.getTranslatedString("Active"),
			Key: "Active",
			Expression: query => window.Helper.User.getActiveUsers(query) as $data.Queryable<Main.Rest.Model.Main_User>
		}
		this.bookmarks.push(
			{
				Category: window.Helper.String.getTranslatedString("Show"),
				Name: window.Helper.String.getTranslatedString("All"),
				Key: "All",
				Expression: query => query
			},
			activeBookmark,
			{
				Category: window.Helper.String.getTranslatedString("Show"),
				Name: window.Helper.String.getTranslatedString("NotLicensed"),
				Key: "NotLicensed",
				Expression: query => {
					query = window.Helper.User.getNotLicensedUsers(query) as $data.Queryable<Main.Rest.Model.Main_User>;
					return query.filter("it.Discharged === false");
				}
			},
			{
				Category: window.Helper.String.getTranslatedString("Show"),
				Name: window.Helper.String.getTranslatedString("Online"),
				Key: "Online",
				Expression: query => query.filter("filterByOnlineStatus")
			},
			{
				Category: window.Helper.String.getTranslatedString("Show"),
				Name: window.Helper.String.getTranslatedString("Inactive"),
				Key: "Inactive",
				Expression: query => window.Helper.User.getInactiveUsers(query) as $data.Queryable<Main.Rest.Model.Main_User>
			}
		);
		this.bookmark(activeBookmark);
	}

	addBulkActions(): void {
		if(window.AuthorizationManager.isAuthorizedForAction("UserAdmin", "AssignLicense"))
		{
			this.bulkActions.push({
				Name: "AssignLicense",
				Action: (arrayOrQueryable) => {},
				Modal: {
					Target: "#modal",
					Route: "Main/User/AssignLicense/?bulk=true"
				}
			});
		}
		if(window.AuthorizationManager.isAuthorizedForAction("UserAdmin", "RevokeLicense"))
		{
			this.bulkActions.push({
				Name: "RevokeLicense",
				Action: (arrayOrQueryable) => {},
				Modal: {
					Target: "#modal",
					Route: "Main/User/AssignLicense/?bulk=true&revoke=true"
				}
			});
		}
	}

	applyFilters(query: $data.Queryable<Main.Rest.Model.Main_User>): $data.Queryable<Main.Rest.Model.Main_User> {
		if (this.filters["Usergroups"]) {
			if (this.filters["Usergroups"]() && this.filters["Usergroups"]().Operator) {
				this.filters["Usergroups"]().Operator.collectionName = "UsergroupObjects";
			}
			this.filters["UsergroupObjects"] = this.filters["Usergroups"];
			delete this.filters["Usergroups"];
		}
		query = super.applyFilters(query);
		if (this.filters["UsergroupObjects"]) {
			this.filters["Usergroups"] = this.filters["UsergroupObjects"];
			delete this.filters["UsergroupObjects"];
		}
		return query;
	}

	applyJoins(query: $data.Queryable<Main.Rest.Model.Main_User>): $data.Queryable<Main.Rest.Model.Main_User> {
		return super.applyJoins(query.include("expandAvatar"));
	}

	getColor(user: Main.Rest.Model.ObservableMain_User): string {
		return "#9E9E9E";
	}

	async initItems(items: Main.Rest.Model.ObservableMain_User[]): Promise<Main.Rest.Model.ObservableMain_User[]> {
		if (!window.AuthorizationManager.isAuthorizedForAction("UserAdmin", "SignalR")) {
			return items;
		}
		return window.database.Main_User.GetSignalRInformation()
			.filter("it.Id in this.ids", {ids: items.map(x => x.Id())})
			.toArray()
			.then(signalRInformations => {
				items.forEach(item => {
					// @ts-ignore
					item.SignalRInformation = signalRInformations.find(x => x.Id === item.Id());
				});
				return items;
			});
	}

	async refreshUserCache(): Promise<void> {
		this.loading(true);
		await window.Helper.User.refreshUserCache();
		this.showSnackbar(window.Helper.String.getTranslatedString("Success"));
		this.loading(false);
	}

	async requestLocalData(user: Main.Rest.Model.ObservableMain_User, type: string): Promise<void> {
		try {
			await window.Helper.Confirm.genericConfirm({
				text: window.Helper.String.getTranslatedString("ConfirmRequest" + type).replace("{0}", window.Helper.User.getDisplayName(user)),
				type: "warning"
			});
		} catch {
			return;
		}
		this.loading(true);
		await fetch(window.Helper.Url.resolveUrl("~/Main/UserAdmin/Request" + type), {
			method: "POST",
			body: new URLSearchParams({"username": user.Id()})
		});
		this.loading(false);
	}

	async requestLocalDatabase(user: Main.Rest.Model.ObservableMain_User): Promise<void> {
		await this.requestLocalData(user, "LocalDatabase");
	}

	async requestLocalStorage(user: Main.Rest.Model.ObservableMain_User): Promise<void> {
		await this.requestLocalData(user, "LocalStorage");
	}

	async toggleActive(user: Main.Rest.Model.ObservableMain_User): Promise<void> {
		this.loading(true);
		if (!await window.Helper.User.toggleActive(user)) {
			this.loading(false);
		}
	}

	async resetGeneralToken(user) {
		if (this.loading()) {
			return;
		}
		this.loading(true);
		if (!await window.Helper.User.resetGeneralToken(user)) {
			this.loading(false);
		} else {
			this.showSnackbar([HelperString.getTranslatedString("TokenSuccessfullyReset"), window.Helper.User.getDisplayName(user)].join(" - "))
			this.loading(false);

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
			await this.refreshUser(user.Id());
			this.loading(false);
			this.showSnackbar([HelperString.getTranslatedString("TotpAuthenticationSuccessfullyRemoved"), window.Helper.User.getDisplayName(user)].join(" - "))
		}
	}

	async refreshUser(id: string): Promise<void> {
		const user = await window.database.Main_User
			.filter("it.Id === this.id", { id: id })
			.include("Roles")
			.include("UsergroupObjects")
			.first();

		let index = this.items().findIndex(function (item: Main.Rest.Model.ObservableMain_User) {
			return item.Id() === user.Id;
		});

		if (index !== -1) {
			const currentItem = this.items()[index];
			const newItem = user.asKoObservable();
			// @ts-ignore
			newItem.SignalRInformation = currentItem.SignalRInformation;

			this.items.replace(currentItem, newItem);
		}
	}
}

namespace("Main.ViewModels").UserListIndexViewModel = UserListIndexViewModel;