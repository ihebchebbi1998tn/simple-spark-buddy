import {namespace} from "./namespace";
import type {UserListIndexViewModel} from "./UserListIndexViewModel";
import type {UserDetailsViewModel} from "./UserDetailsViewModel";
import moment from "moment";

export class UserAssignLicenseModalViewModel extends window.Main.ViewModels.ViewModelBase {
	parentViewModel: UserDetailsViewModel | UserListIndexViewModel;
	bulk = ko.observable<boolean>(false);
	revoke = ko.observable<boolean>(false);
	user = ko.observable<Main.Rest.Model.ObservableMain_User>(null);
	usersWithoutLicenseCount = ko.validatedObservable<number>(0);
	displayUsernames = ko.observableArray<string>([]);
	availableUserLicenses = ko.observable<number>(0);
	canAssignLicense = ko.pureComputed<boolean>(() => this.availableUserLicenses() >= this.usersWithoutLicenseCount())

	constructor(parentViewModel: UserDetailsViewModel | UserListIndexViewModel) {
		super();
		this.parentViewModel = parentViewModel;
		if (parentViewModel instanceof window.Main.ViewModels.UserDetailsViewModel) {
			this.user(ko.unwrap(parentViewModel.user));
		} else if (!(parentViewModel instanceof window.Main.ViewModels.UserListIndexViewModel)) {
			//@ts-ignore
			this.parentViewModel = ko.isObservable(parentViewModel.tabs) ? parentViewModel.tabs()["tab-users"]() : parentViewModel;
		}
		this.availableUserLicenses = this.parentViewModel.availableUserLicenses;
	}

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		this.revoke(params && params.revoke === "true");
		this.bulk(params && params.bulk === "true");
				
		if(!this.bulk()) {
			if(!this.user() && id != null) {
				this.user((await window.database.Main_User.find(id)).asKoObservable());
			}
			this.usersWithoutLicenseCount(window.Helper.User.hasValidLicense(this.user()) ? 0 : 1);
			this.displayUsernames([window.Helper.User.getDisplayName(this.user())]);
		} else if (this.parentViewModel instanceof window.Main.ViewModels.UserListIndexViewModel) {
			let arrayOrQueryable : Array<Main.Rest.Model.ObservableMain_User> | $data.Queryable<Main.Rest.Model.Main_User> = this.parentViewModel.allItemsSelected() === true ? this.parentViewModel.getFilterQuery(true, true) : this.parentViewModel.selectedItems();
			if(Array.isArray(arrayOrQueryable)) {
				this.usersWithoutLicenseCount((window.Helper.User.getNotLicensedUsers(arrayOrQueryable) as Main.Rest.Model.ObservableMain_User[]).length);
				this.displayUsernames(arrayOrQueryable.map(user => window.Helper.User.getDisplayName(user)));
			} else {
				this.usersWithoutLicenseCount(await (window.Helper.User.getNotLicensedUsers(arrayOrQueryable) as $data.Queryable<Main.Rest.Model.Main_User>).count());
			}
		}
	}
	
	cancel(): void {
	}

	async bulkSave(): Promise<void> {
		if (!(this.parentViewModel instanceof window.Main.ViewModels.UserListIndexViewModel)) {
			return;
		}
		let errors = ko.validation.group(this);
		if (errors().length > 0) {
			errors.showAllMessages();
			return;
		}
		this.parentViewModel.loading(true);
		let arrayOrQueryable : Array<Main.Rest.Model.ObservableMain_User> | $data.Queryable<Main.Rest.Model.Main_User> = this.parentViewModel.allItemsSelected() === true ? this.parentViewModel.getFilterQuery(true, true) : this.parentViewModel.selectedItems();
		let licenseUsers = async (users: Main.Rest.Model.Main_User[] | Main.Rest.Model.ObservableMain_User[]): Promise<void> => {
			users.forEach(user => {
				window.database.attachOrGet(user);
				if(ko.isObservable(user.LicensedAt)) {
					user.LicensedAt(this.revoke() ? null : moment().utc().toDate());
				} else {
					user.LicensedAt = this.revoke() ? null : moment().utc().toDate();
				}
			});
			try {
				await window.database.saveChanges();
			} catch (e: any) {
				const errorMessage = window.Helper.String.tryExtractErrorMessageValue(JSON.parse(e.data.response.body));
				window.swal(window.Helper.String.getTranslatedString("Error"),
					errorMessage,
					"error");
			}
		}
		if (Array.isArray(arrayOrQueryable)) {
			await licenseUsers(arrayOrQueryable);
		} else if (arrayOrQueryable instanceof window.$data.Queryable) {
			let pageSize = 25;
			let page = 0;
			let processNextPage = async (): Promise<void> => {
				let users = await (arrayOrQueryable as $data.Queryable<Main.Rest.Model.Main_User>).orderBy("it.Id")
					.skip(page * pageSize)
					.take(pageSize)
					.toArray();
				await licenseUsers(users);
				if (users.length === pageSize) {
					page++;
					await processNextPage();
				}
			};
			await processNextPage();
		}
		this.parentViewModel.loading(false);
		$(".modal:visible").modal("hide");
	};

	async save(): Promise<void> {
		let errors = ko.validation.group(this);
		if (errors().length > 0) {
			errors.showAllMessages();
			return;
		}
		this.loading(true);
		window.database.attachOrGet(this.user());
		this.user().LicensedAt(this.revoke() ? null : moment().utc().toDate());
		try {
			await window.database.saveChanges();
		} catch (e: any) {
			const errorMessage = window.Helper.String.tryExtractErrorMessageValue(JSON.parse(e.data.response.body));
			window.swal(window.Helper.String.getTranslatedString("Error"),
				errorMessage,
				"error");
		}
		this.loading(false);
		$(".modal:visible").modal("hide");
	};
}

namespace("Main.ViewModels").UserAssignLicenseModalViewModel = UserAssignLicenseModalViewModel;