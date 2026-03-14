export class DispatchDetailsViewModelExtension extends window.Crm.Service.ViewModels.DispatchDetailsViewModel {
	userGroup = ko.observable<Main.Rest.Model.ObservableMain_Usergroup>(null);
	teamMembers = ko.observableArray<Main.Rest.Model.Main_User>([]);

	async init(id?: string, params?: { [key: string]: string }): Promise<void> {
		await super.init(id, params);

		if (this.dispatch().ExtensionValues().TeamId() !== null) {
			this.userGroup(await this.getUserGroup(this.dispatch().ExtensionValues().TeamId()));
			await this.getUserGroupMembers();
		}
	};

	async getUserGroup(userGroupId: string): Promise<Main.Rest.Model.ObservableMain_Usergroup> {
		const userGroup = await window.database.Main_Usergroup
			.filter(it => it.Id === this.userGroupId, {userGroupId})
			.first();
		return userGroup.asKoObservable();
	}

	async getUserGroupMembers(): Promise<void> {
		await window.database.Main_UserUserGroup
			.include("User")
			.filter(function (it) {
				return it.UserGroupKey === this.teamId;
			}, {
				teamId: this.dispatch().ExtensionValues().TeamId(),
			})
			.map("it.User")
			.toArray(result => this.teamMembers(result.filter(x => x.Id !== this.dispatch().Username())));
	}
}

window.Crm.Service.ViewModels.DispatchDetailsViewModel = DispatchDetailsViewModelExtension;