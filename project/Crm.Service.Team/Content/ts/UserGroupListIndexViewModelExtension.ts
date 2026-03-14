export class UserGroupListIndexViewModelExtension extends window.Main.ViewModels.UserGroupListIndexViewModel {
	applyFilters(query: $data.Queryable<Main.Rest.Model.Main_Usergroup>): $data.Queryable<Main.Rest.Model.Main_Usergroup> {
		query = query.filter(it => it.ExtensionValues.IsServiceTeam === false);
		return super.applyFilters(query);
	}
}

window.Main.ViewModels.UserGroupListIndexViewModel = UserGroupListIndexViewModelExtension;