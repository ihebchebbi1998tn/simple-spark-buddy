import {namespace} from "./namespace";
import {Breadcrumb} from "./breadcrumbs";
import {ViewModelBase} from "./ViewModelBase";
import {HelperConfirm} from "./helper/Helper.Confirm";
import {HelperString} from "./helper/Helper.String";

export class RoleDetailsViewModel extends window.Main.ViewModels.ViewModelBase {

	tabs = ko.observable<{}>({});
	permissionGroups = ko.observableArray<Main.Rest.Model.RolePermissionGroup>([]);
	permissions = ko.observableArray<string>([]);
	sourcePermissions = ko.observableArray<string>([]);
	role = ko.observable<Main.Rest.Model.ObservableMain_PermissionSchemaRole>(null);
	allPermissionGroupPermissions: KnockoutComputed<Main.Rest.Model.PermissionGroupPermission[]>;
	permissionFilter = ko.observable<string>("");
	selectedPermissions = ko.observableArray<Main.Rest.Model.PermissionGroupPermission>([]);
	selectablePermissions = ko.observableArray<Main.Rest.Model.PermissionGroupPermission>([]);
	title = ko.observable<string>(null);

	constructor() {
		super();
		this.allPermissionGroupPermissions = ko.pureComputed<Main.Rest.Model.PermissionGroupPermission[]>(() =>
			this.permissionGroups().reduce((acc, x) => acc.concat(x.Permissions), []));
	}

	async applyPermissions(): Promise<void> {
		this.loading(true);
		let selectablePermissionNames = this.selectablePermissions().map(x => x.PermissionName);
		let selectedPermissionNames = this.selectedPermissions().map(x => x.PermissionName);
		let assignedPermissionNames = selectedPermissionNames.filter(x => this.permissions().indexOf(x) === -1);
		let unassignedPermissionNames = selectablePermissionNames.filter(x => selectedPermissionNames.indexOf(x) === -1 && this.permissions().indexOf(x) !== -1);
		if (assignedPermissionNames.length > 0 || unassignedPermissionNames.length > 0) {
			await window.database.Main_PermissionSchemaRole.AssignPermissions({
				RoleKey: this.role().UId(),
				AssignedPermissions: assignedPermissionNames,
				UnassignedPermissions: unassignedPermissionNames
			}).first();
			await this.loadPermissions();
		}
		$(".modal:visible").modal("hide");
		this.loading(false);
	}

	getImportedPermissions(permission: string): string[] {
		let result = [];
		this.allPermissionGroupPermissions().filter(x => x.ImportedBy.indexOf(permission) !== -1).forEach((x) => {
			if (result.indexOf(x.PermissionName) === -1 && x.PermissionName !== permission) {
				result.push(x.PermissionName);
			}
			this.getImportedPermissions(x.PermissionName).forEach((y) => {
				if (result.indexOf(y) === -1 && y !== permission) {
					result.push(y);
				}
			});
		});
		return result;
	}

	getPermissionColor(permissionGroup: string, permissionName: string): "c-red" | "c-green" | "c-gray" | "hidden" {
		let permission = permissionGroup + "::" + permissionName;
		if (!this.role().SourcePermissionSchemaRoleId()) {
			return this.permissions().indexOf(permission) !== -1 ? "c-gray" : "hidden";
		}
		if (this.sourcePermissions().indexOf(permission) !== -1 && this.permissions().indexOf(permission) === -1) {
			return "c-red";
		}
		if (this.sourcePermissions().indexOf(permission) === -1 && this.permissions().indexOf(permission) !== -1) {
			return "c-green";
		}
		if (this.sourcePermissions().indexOf(permission) !== -1 && this.permissions().indexOf(permission) !== -1) {
			return "c-gray";
		}
		return "hidden";
	}

	getPermissionColorForSelection(permissionGroup: string, permissionName: string): "c-red" | "c-black" {
		let permission = permissionGroup + "::" + permissionName;

		if (this.sourcePermissions().indexOf(permission) !== -1 && this.permissions().indexOf(permission) === -1) {
			return "c-red";
		}

		return "c-black";
	}

	getPermissionIcon(permissionGroup: string, permissionName: string): "zmdi-minus" | "zmdi-plus" | "zmdi-circle" | "hidden" {
		if (!this.role().SourcePermissionSchemaRoleId()) {
			return "hidden";
		}
		let permission = permissionGroup + "::" + permissionName;
		if (this.sourcePermissions().indexOf(permission) !== -1 && this.permissions().indexOf(permission) === -1) {
			return "zmdi-minus";
		}
		if (this.sourcePermissions().indexOf(permission) === -1 && this.permissions().indexOf(permission) !== -1) {
			return "zmdi-plus";
		}
		if (this.sourcePermissions().indexOf(permission) !== -1 && this.permissions().indexOf(permission) !== -1) {
			return "zmdi-circle";
		}
		return null;
	}

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		let role = await window.database.Main_PermissionSchemaRole
			.include("SourcePermissionSchemaRole")
			.find(id);
		this.role(role.asKoObservable());
		await this.loadPermissions();
		this.title = this.role().Name;
		await this.setBreadcrumbs(this.title());

		this.selectedPermissions.subscribe((changes) => {
			changes.forEach(change => {
				if (change.status === "added") {
					this.selectablePermissions().filter(x => change.value.ImportedPermissions.indexOf(x.PermissionName) !== -1 && this.selectedPermissions().indexOf(x) === -1).forEach((x) => {
						this.selectedPermissions.push(x);
					});
				}
				if (change.status === "deleted") {
					this.selectedPermissions().filter(x => change.value.ImportedBy.indexOf(x.PermissionName) !== -1).forEach((x) => {
						this.selectedPermissions.remove(x);
					});
				}
			})

		}, null, "arrayChange");
	}

	async loadPermissions(): Promise<void> {
		let rolePermissions = await window.database.Main_PermissionSchemaRole.GetRolePermissions(this.role().UId()).first();
		if (this.permissionGroups().length === 0) {
			rolePermissions.PermissionGroups.sort((a, b) => a.Name.localeCompare(b.Name))
			for (let i = 0; i < rolePermissions.PermissionGroups.length; i++) {
				(rolePermissions.PermissionGroups[i] as any).id = i;
				rolePermissions.PermissionGroups[i].Permissions.sort((a, b) => a.PermissionName.localeCompare(b.PermissionName))
			}
			this.permissionGroups(rolePermissions.PermissionGroups);
			let sourcePermissions = rolePermissions.SourcePermissions.slice();
			rolePermissions.SourcePermissions.forEach((permission) => {
				this.getImportedPermissions(permission).forEach((x) => {
					if (sourcePermissions.indexOf(x) === -1) {
						sourcePermissions.push(x);
					}
				});
			});
			this.sourcePermissions(sourcePermissions);
		}
		let permissions = rolePermissions.Permissions.slice();
		rolePermissions.Permissions.forEach((permission) => {
			this.getImportedPermissions(permission).forEach((x) => {
				if (permissions.indexOf(x) === -1) {
					permissions.push(x);
				}
			});
		});
		this.permissions(permissions);
	}

	async remove(): Promise<void> {
		const confirmed = await HelperConfirm.confirmDeleteAsync();
		if (confirmed) {
			this.loading(true);
			window.database.remove(this.role().innerInstance);
			await window.database.saveChanges();
			window.location.hash = "/Main/PermissionSchemaRoleList/IndexTemplate";
		}
	}

	async reset(): Promise<void> {
		let roleName = HelperString.getTranslatedString(this.role().Name(), this.role().Name());
		const confirmed = await HelperConfirm.genericConfirmAsync({
			text: HelperString.getTranslatedString("ConfirmResetRole").replace("{0}", roleName),
			type: "warning"
		});
		if (confirmed) {
			this.loading(true);
			await window.database.Main_PermissionSchemaRole.ResetRole({RoleKey: this.role().UId()}).first();
			await this.loadPermissions();
			this.loading(false);
		}
	}

	selectPermissions(permissionGroup: Main.Rest.Model.RolePermissionGroup, permissionGroupGroup: string): void {
		this.permissionFilter("");
		this.selectablePermissions(permissionGroup.Permissions.filter(x => x.PermissionName.startsWith(permissionGroupGroup + "::")));
		this.selectedPermissions(this.selectablePermissions().filter(x => this.permissions().indexOf(x.PermissionName) !== -1));
	}

	async setBreadcrumbs(title: string): Promise<void> {
		await window.breadcrumbsViewModel.setBreadcrumbs([
			new Breadcrumb(HelperString.getTranslatedString("Role"), "UserAdmin::ListRoles", "#/Main/PermissionSchemaRoleList/IndexTemplate"),
			new Breadcrumb(HelperString.getTranslatedString(title, this.role().Name()), null, window.location.hash)
		]);
	}
}

namespace("Main.ViewModels").RoleDetailsViewModel = RoleDetailsViewModel;
