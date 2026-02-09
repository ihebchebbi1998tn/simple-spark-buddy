import {namespace} from "./namespace";
import { Breadcrumb } from "./breadcrumbs";

export class UserGroupCreateViewModel extends window.Main.ViewModels.ViewModelBase {

	Members = ko.observableArray<string>([]);
	selectedUsers = ko.observableArray<Main.Rest.Model.Main_User>([]);
	userGroup = ko.observable<Main.Rest.Model.ObservableMain_Usergroup>(null);
	errors: CustomKnockoutValidationErrors;
	selectedUsersToDisplay = ko.observableArray<{ id: string, text: string }>([]);

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

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		await this.setBreadcrumbs();
		this.userGroup(window.database.Main_Usergroup.defaultType.create().asKoObservable());
		window.database.add(this.userGroup());
		this.errors = window.ko.validation.group(this.userGroup, {deep: true});
	}

	async submit(): Promise<void> {
		this.loading(true);
		await this.errors.awaitValidation();
		if (this.errors().length > 0) {
			this.errors.showAllMessages();
			this.loading(false);
			return;
		}
		try {
			await window.database.saveChanges();
			await window.database.Main_Usergroup.SetUsers({
				UserIds: this.Members(),
				UserGroup: ko.unwrap(this.userGroup().Id)
			}).first();
			window.location.hash = "/Main/UserGroup/DetailsTemplate/" + window.ko.unwrap(this.userGroup().Id);
		} catch (e) {
			this.loading(false);
			window.Log.error(e);
			window.swal(window.Helper.String.getTranslatedString("Error"), (e as Error).message, "error");
			throw e;
		}
	}

	dispose(): void {
		window.database.detach(this.userGroup());
	}

	cancel(): void {
		window.history.back();
	}

	async setBreadcrumbs(): Promise<void> {
		await window.breadcrumbsViewModel.setBreadcrumbs([
			new Breadcrumb(window.Helper.getTranslatedString("Usergroup"), "UserAdmin::ListUsergroups", "#/Main/UserGroupList/IndexTemplate"),
			new Breadcrumb(window.Helper.getTranslatedString("CreateUserGroup"), null, window.location.hash, null, null)
		]);
	};
}

namespace("Main.ViewModels").UserGroupCreateViewModel = UserGroupCreateViewModel;
