export class SchedulerAddResourceModalViewModelExtension extends window.Sms.Scheduler.ViewModels.SchedulerAddResourceModalViewModel {
	selectedTeam: KnockoutObservable<Main.Rest.Model.Main_Usergroup> = ko.observable<Main.Rest.Model.Main_Usergroup>(null);
	userGroupSelector: KnockoutObservable<boolean> = ko.observable<boolean>(false);
	
	toggleUserGroupSelector(): void {
		this.userGroupSelector(!this.userGroupSelector());
		if (this.userGroupSelector()) {
			this.addedResources([]);
			this.addedTools([]);
		} else {
			this.selectedTeam(null);
		}
	};

	async OnTeamSelected(team: Main.Rest.Model.Main_Usergroup): Promise<void> {
		this.addedResources(team?.Members.map(x => x.User).filter((m: Main.Rest.Model.Main_User) => !this.existingTechnicianResources().map(t => t.Id).includes(m.Id)) ?? []);
		this.userIds(team?.Members.map(x => x.User).filter((m: Main.Rest.Model.Main_User) => !this.existingTechnicianResources().map(t => t.Id).includes(m.Id)).map(m => m.Id) ?? []);
	}
	userGroupFilter(query: $data.Queryable<Main.Rest.Model.Main_Usergroup>, term: string): $data.Queryable<Main.Rest.Model.Main_Usergroup> {
		query = query.filter("it => !it.Members.every(m => m.Username in this.users)", { users: this.existingTechnicianResources().map(t => t.Id)});
		//This ts-ignore is to mitigate a sonarqube error; The way our 'extensions' work doesn't give TSC enough information on current and correct types
		//But; Sms.Scheduler.Team plugin requires Crm.Service.Team plugin this function can be 'safely' assumed to be defined correctly
		//@ts-ignore
		return window.Helper.User.filterUsergroupQueryForServiceTeams(query, term, null);
	}pl
}

window.Sms.Scheduler.ViewModels.SchedulerAddResourceModalViewModel = SchedulerAddResourceModalViewModelExtension;