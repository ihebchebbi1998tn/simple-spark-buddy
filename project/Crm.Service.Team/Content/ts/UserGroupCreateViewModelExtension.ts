import {Breadcrumb} from "@Main/breadcrumbs";

export class UserGroupCreateViewModelExtension extends window.Main.ViewModels.UserGroupCreateViewModel {

	serviceTeam = false;

	async init(id?: string, params?: { [key: string]: string }): Promise<void> {
		if (params.serviceTeam === "true") {
			this.serviceTeam = true;
		}

		await super.init(id, params);

		if (this.serviceTeam) {
			this.userGroup().ExtensionValues().IsServiceTeam(true);
		}
	}

	mainResourceSelect2autocompleter: () => Select2AutoCompleter = () => {
		return {
			data: this.userGroup().ExtensionValues().MainResourceId,
			autocompleteOptions: {
				mapDisplayObject: window.Helper.User.mapForSelect2Display,
				table: "Main_User",
				customFilter: (query, term = "") => {
					query = window.Helper.String.contains(query, term, ["LastName.toLowerCase()", "FirstName.toLowerCase()"]);
					return query.filter("it.Id in this.ids", {
						ids: this.Members()
					});
				}
			}
		};
	}

	async setBreadcrumbs(): Promise<void> {
		if (this.serviceTeam) {
			await window.breadcrumbsViewModel.setBreadcrumbs([
				new Breadcrumb(window.Helper.getTranslatedString("ServiceTeam"), "#/Crm.Service.Team/ServiceTeamList/IndexTemplate"),
				new Breadcrumb(window.Helper.getTranslatedString("CreateServiceTeam"), window.location.hash, null, null)
			]);
		} else {
			await super.setBreadcrumbs();
		}
	}
}

window.Main.ViewModels.UserGroupCreateViewModel = UserGroupCreateViewModelExtension;
