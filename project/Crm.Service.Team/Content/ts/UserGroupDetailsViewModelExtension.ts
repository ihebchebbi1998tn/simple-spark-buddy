import {Breadcrumb} from "@Main/breadcrumbs";

export class UserGroupDetailsViewModelExtension extends window.Main.ViewModels.UserGroupDetailsViewModel {

	MainResourceId = ko.observable<string>(null)

	async init(id?: string, params?: { [key: string]: string }): Promise<void> {
		await super.init(id, params);
		this.Members.subscribe(() => {
			if (!this.Members().includes(this.MainResourceId())) {
				this.MainResourceId(null);
			}
		});
		const resourceId = ko.unwrap(this.userGroup().ExtensionValues as any).MainResourceId();
		if (this.Members().includes(resourceId)) {
			this.MainResourceId(resourceId);
		}
	}

	async onSave(): Promise<void> {
		this.userGroup().ExtensionValues().MainResourceId(this.MainResourceId());
		await super.onSave();
	}

	mainResourceSelect2autocompleter: () => Select2AutoCompleter = () => {
		return {
			data: this.MainResourceId,
			autocompleteOptions: {
				mapDisplayObject: window.Helper.User.mapForSelect2Display,
				table: "Main_User",
				customFilter: (query, term = "") => {
					query = window.Helper.String.contains(query, term, ["LastName.toLowerCase()", "FirstName.toLowerCase()"]);
					return query.filter("it.Id in this.ids", {
						ids: this.Members(),
						term
					});
				}
			}
		};
	}

	async setBreadcrumbs(): Promise<void> {
		if (this.userGroup().ExtensionValues().IsServiceTeam()){
			await window.breadcrumbsViewModel.setBreadcrumbs([
				new Breadcrumb(window.Helper.String.getTranslatedString("ServiceTeam"), "UserAdmin::ListServiceTeams", "#/Crm.Service.Team/ServiceTeamList/IndexTemplate"),
				new Breadcrumb(this.userGroup().Name(), null, window.location.hash)
			]);
		} else {
			await super.setBreadcrumbs();
		}
	}
}

window.Main.ViewModels.UserGroupDetailsViewModel = UserGroupDetailsViewModelExtension;