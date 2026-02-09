import { namespace } from "./namespace";
import { Breadcrumb } from "./breadcrumbs";
import { MultiFactorAuthenticationMode } from "@Main/helper/Helper.MultiFactorAuthentication";

export class UserGroupDetailsViewModel extends window.Main.ViewModels.ViewModelBase {

	id: string;
	Name: string;

	tabs = ko.observable<{}>({});
	Members = ko.observableArray<string>([]);
	selectedUsers = ko.observableArray<Main.Rest.Model.Main_User>([]);
	userGroup = ko.observable<Main.Rest.Model.ObservableMain_Usergroup>(null);
	errors: CustomKnockoutValidationErrors;
	selectedUsersToDisplay = ko.observableArray<{ id: string, text: string }>([]);
	site: Main.Rest.Model.Main_Site;
	relatedMultiFactorAuthenticationModeKey = MultiFactorAuthenticationMode.MandatoryForSpecificUserGroupsKey;
	userGroupMembers = ko.observableArray<Main.Rest.Model.Main_User>([]);

	async init(id?: string, params?: { [key: string]: string }): Promise<void> {
		await super.init(id, params);
		await this.setUserGroup(id);
		window.database.attachOrGet(this.userGroup());
		await this.getUserGroupMembers();
		this.selectedUsersToDisplay(ko.unwrap(this.userGroupMembers()).map(u => {
			return {
				id: ko.unwrap(u.Id),
				text: window.Helper.User.getDisplayName(u.asKoObservable().innerInstance)
			};
		}));
		this.errors = window.ko.validation.group(this.userGroup, { deep: true });
		await this.setBreadcrumbs();
		this.site = await window.database.Main_Site.GetCurrentSite().first();
	}

	async getUserGroup(id: string): Promise<Main.Rest.Model.ObservableMain_Usergroup> {
		const userGroup = await window.database.Main_Usergroup
			.filter(it => it.Id === this.id, { id })
			.first();
		return userGroup.asKoObservable();
	}

	async getUserGroupMembers(): Promise<void> {
		let results = await window.database.Main_User
			.filter(function (it) {
				return it.Id in this.Ids;
			}, {
				Ids: this.userGroup().UsersIds()
			})
			.toArray();

		this.userGroupMembers(results);
		const memberIds = results.map(member => member.Id);
		this.Members(memberIds);
	}

	async onAfterSave(): Promise<void> {
		await this.setBreadcrumbs();
		return this.setUserGroup(this.userGroup().Id()).then(() => this.getUserGroupMembers());
	}

	async onSave(): Promise<void> {
		await window.database.Main_Usergroup.SetUsers({
			UserIds: this.Members(),
			UserGroup: ko.unwrap(this.userGroup().Id)
		}).first();
	}

	membersSelect2autocompleter: () => Select2AutoCompleter = () => {
		return {
			data: this.selectedUsersToDisplay(),
			autocompleteOptions: {
				orderBy: ["LastName"],
				table: "Main_User",
				mapDisplayObject: window.Helper.User.mapForSelect2Display,
				customFilter: (query, term = "") => {
					return window.Helper.String.contains(query, term, ["LastName.toLowerCase()", "FirstName.toLowerCase()"]);
				}
			}
		};
	}

	async setBreadcrumbs(): Promise<void> {
		await window.breadcrumbsViewModel.setBreadcrumbs([
			new Breadcrumb(window.Helper.String.getTranslatedString("Usergroup"), "UserAdmin::ListUsergroups", "#/Main/UserGroupList/IndexTemplate"),
			new Breadcrumb(this.userGroup().Name(), null, window.location.hash)
		]);
	}

	async setUserGroup(id): Promise<void> {
		const userGroup = await this.getUserGroup(id);
		this.userGroup(userGroup);
	}
}

namespace("Main.ViewModels").UserGroupDetailsViewModel = UserGroupDetailsViewModel;