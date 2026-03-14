import {namespace} from "./namespace";
import type {RoleDetailsViewModel} from "./RoleDetailsViewModel";

export class RoleDetailsUsersTabViewModel extends window.Main.ViewModels.UserListIndexViewModel {
	role = ko.observable<Main.Rest.Model.ObservableMain_PermissionSchemaRole>();

	constructor(parentViewModel: RoleDetailsViewModel) {
		super();
		this.role = parentViewModel.role;
		window.Helper.Database.registerEventHandlers(this, {
			"Main_PermissionSchemaRole": {
				"afterUpdate": function () {
					this.currentSearch = this.search(false, true);
				}
			}
		});
		this.bulkActions.push({
			Name: "UnassignFromRole",
			Action: async (arrayOrQueryable) => {
				this.loading(true);
				let role = this.role().innerInstance;
				let unassignUsersFromRole = async (usernames) => {
					role = await window.database.Main_PermissionSchemaRole.AssignUsers({
						RoleKey: this.role().UId(),
						UnassignedUsernames: usernames
					}).first();
				}
				if (Array.isArray(arrayOrQueryable)) {
					await unassignUsersFromRole(arrayOrQueryable.map(x => x.Id()));
				} else if (arrayOrQueryable instanceof window.$data.Queryable) {
					let pageSize = 25;
					let processNextPage = async function () {
						let usernames = await arrayOrQueryable
							.map("it.Id")
							.orderBy("it.Id")
							.take(pageSize)
							.toArray();
						await unassignUsersFromRole(usernames);
						if (usernames.length === pageSize) {
							await processNextPage();
						}
					};
					await processNextPage();
				}
				role.entityState = $data.EntityState.Modified;
				window.database.processEntityTypeAfterEventHandler({data: role});
				this.loading(false);
			}
		});
	}

	applyFilters(query: $data.Queryable<Main.Rest.Model.Main_User>): $data.Queryable<Main.Rest.Model.Main_User> {
		return super.applyFilters(query)
			.filter("filterByRoleId", this.role().UId());
	}
}

namespace("Main.ViewModels").RoleDetailsUsersTabViewModel = RoleDetailsUsersTabViewModel;
