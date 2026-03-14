import {namespace} from "./namespace";

export class RoleAssignModalViewModel extends window.Main.ViewModels.ViewModelBase {
	role = ko.observable<Main.Rest.Model.ObservableMain_PermissionSchemaRole>(null);
	roleUsers = ko.observableArray<Main.Rest.Model.Main_User>([]);
	usernames = ko.observableArray<string>([]);

	getUsersFromUsernames(usernames: string[]): Select2AutoCompleterResult[] {
		return this.roleUsers().filter(x => usernames.indexOf(x.Id) !== -1).map(window.Helper.User.mapForSelect2Display);
	}

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		let role = await window.database.Main_PermissionSchemaRole
			.find(id);
		this.role(role.asKoObservable());
		let roleUsers = await window.database.Main_User.filter("filterByRoleId", id).map(it => {
			return {Id: it.Id, FirstName: it.FirstName, LastName: it.LastName}
		}).toArray();
		this.roleUsers(roleUsers);
		this.usernames(roleUsers.map(x => x.Id));
	}

	async assign(): Promise<void> {
		let usernamesBefore = this.roleUsers().map(x => x.Id);
		let assignedUsernames = this.usernames().filter(x => usernamesBefore.indexOf(x) === -1);
		let unassignedUsernames = usernamesBefore.filter(x => this.usernames().indexOf(x) === -1);
		if (assignedUsernames.length > 0 || unassignedUsernames.length > 0) {
			this.loading(true);
			try {
				let role = await window.database.Main_PermissionSchemaRole.AssignUsers({
					RoleKey: this.role().UId(),
					AssignedUsernames: assignedUsernames,
					UnassignedUsernames: unassignedUsernames
				}).first();
				role.entityState = $data.EntityState.Modified;
				window.database.processEntityTypeAfterEventHandler({data: role});
				await window.Helper.User.refreshUserCache();
			} catch (e) {
				this.loading(false);
				const errorMessage = window.Helper.String.tryExtractErrorMessageValue(JSON.parse((e as Error).message));
				window.swal(window.Helper.String.getTranslatedString("Error"), errorMessage, "error");
			}
			this.loading(false);
		}
		$(".modal:visible").modal("hide");
	}

}

namespace("Main.ViewModels").RoleAssignModalViewModel = RoleAssignModalViewModel;
