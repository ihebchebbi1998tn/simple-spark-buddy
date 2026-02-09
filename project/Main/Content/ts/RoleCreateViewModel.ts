import {namespace} from "./namespace";
import { Breadcrumb } from "./breadcrumbs";

class RoleCreateViewModel extends window.Main.ViewModels.ViewModelBase {
	role = ko.observable<Main.Rest.Model.ObservableMain_PermissionSchemaRole>(null);

	cancel(): void {
		window.database.detach(this.role().innerInstance);
		window.history.back();
	}

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		await this.setBreadcrumbs();
		let newRole = window.database.Main_PermissionSchemaRole.defaultType.create();
		newRole.ModifiedAt = new Date();
		window.database.add(newRole);
		this.role(newRole.asKoObservable());
	}

	getNameDisplay(name: string): string {
		return name.replace(/([a-z])([A-Z])/g, '$1 $2');
	}

	async submit(): Promise<void> {
		this.loading(true);
		let errors = window.ko.validation.group(this);
		await errors.awaitValidation()
		if (errors().length > 0) {
			errors.showAllMessages();
			this.loading(false);
			return;
		}
		try {
			let result = await window.database.Main_PermissionSchemaRole.AddRole({
				Role: this.role().innerInstance
			})
				.first();
			window.location.hash = "/Main/Role/Details/" + result.UId;
		} catch (e) {
			this.loading(false);
			window.swal(window.Helper.String.getTranslatedString("Error"), (e as Error).message, "error");
			throw e;
		}
	}

	async setBreadcrumbs(): Promise<void> {
		await window.breadcrumbsViewModel.setBreadcrumbs([
			new Breadcrumb(window.Helper.getTranslatedString("Role"), "UserAdmin::ListRoles", "#/Main/PermissionSchemaRoleList/IndexTemplate"),
			new Breadcrumb(window.Helper.getTranslatedString("CreateRole"), null, window.location.hash, null, null)
		]);
	}
}

namespace("Main.ViewModels").RoleCreateViewModel = RoleCreateViewModel;