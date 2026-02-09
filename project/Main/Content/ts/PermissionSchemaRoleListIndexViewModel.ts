import {namespace} from "./namespace";

class PermissionSchemaRoleListIndexViewModel extends window.Main.ViewModels.GenericListViewModel<Main.Rest.Model.Main_PermissionSchemaRole, Main.Rest.Model.ObservableMain_PermissionSchemaRole> {
	constructor() {
		super("Main_PermissionSchemaRole", "Name", "ASC", ["SourcePermissionSchemaRole"]);
	}

	canEdit(role: Main.Rest.Model.ObservableMain_PermissionSchemaRole): boolean {
		let isAuthorized = window.AuthorizationManager.isAuthorizedForAction("UserAdmin", "EditRole");
		let isAdminRole = role.Name() === "Administrator" && role.CompilationName() === "DefaultCompilation";
		return isAuthorized && !isAdminRole;
	}

	getColor(role: Main.Rest.Model.ObservableMain_PermissionSchemaRole): string {
		return "#9E9E9E";
	}

	async remove(role: Main.Rest.Model.ObservableMain_PermissionSchemaRole): Promise<void> {
		try {
			await window.Helper.Confirm.confirmDelete();
		} catch (e) {
			return;
		}
		this.loading(true);
		let assignedUsers = await window.database.Main_User.filter("filterByRoleId", role.UId()).count();
		if (assignedUsers > 0) {
			this.loading(false);
			let roleName = window.Helper.String.getTranslatedString(role.Name(), role.Name());
			setTimeout(() => window.swal(window.Helper.String.getTranslatedString("Error"), window.Helper.String.getTranslatedString("UnassignUsers").replace("{0}", roleName), "error"), 200);
			return;
		}
		window.database.remove(role.innerInstance);
		await window.database.saveChanges();
	}

	async reset(role: Main.Rest.Model.ObservableMain_PermissionSchemaRole): Promise<void> {
		let roleName = window.Helper.String.getTranslatedString(role.Name(), role.Name());
		try {
			await window.Helper.Confirm.genericConfirm({
				text: window.Helper.String.getTranslatedString("ConfirmResetRole").replace("{0}", roleName),
				type: "warning"
			});
		} catch (e) {
			return;
		}
		this.loading(true);
		await window.database.Main_PermissionSchemaRole.ResetRole({ RoleKey: role.UId() }).first();
		this.loading(false);
		this.showSnackbar(window.Helper.String.getTranslatedString("RoleReset").replace("{0}", roleName));
	}

}

namespace("Main.ViewModels").PermissionSchemaRoleListIndexViewModel = PermissionSchemaRoleListIndexViewModel;
namespace("Main.ViewModels").PermissionSchemaRoleListIndexViewModel.prototype.refreshUserCache = namespace("Main.ViewModels").UserListIndexViewModel.prototype.refreshUserCache;