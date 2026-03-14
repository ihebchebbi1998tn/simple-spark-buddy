export class SchedulerEditModalViewModelExtension extends window.Sms.Scheduler.ViewModels.SchedulerEditModalViewModel {
	
	selectedTeam = ko.observable<Main.Rest.Model.Main_Usergroup>(null);

	constructor(params) {
		super(params);
		this.canSelectMultipleTechnicians(true);
	}
	
	async onTeamSelected(team: Main.Rest.Model.Main_Usergroup): Promise<void> {
		this.selectedTeam(team);
		let dispatch = this.dispatch() as Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch;
		if (dispatch?.ExtensionValues()) {
			const teamId = team ? team.Id : null;
			dispatch.ExtensionValues().TeamId(teamId);
			if ((this.eventRecord as any)?.OriginalData?.ExtensionValues) {
				(this.eventRecord as any).OriginalData.ExtensionValues.TeamId = teamId;
			}
			if (this.eventRecord) {
				await this.eventRecord.setAsync("TeamId", teamId);
			}
		}
		if(team) {
			const teamMemberIds = team.Members.map(x => x.User.Id);
			const membersNotInTechniciansMap = teamMemberIds.filter(id => !this.techniciansMap.has(id));
			if (membersNotInTechniciansMap.length > 0) {
				(await this.parentViewModel.loadTechnicians(membersNotInTechniciansMap))
					.forEach(t => this.techniciansMap.set(t.OriginalData.Id, t));
			}
			
			const assignedResourcesNotInSelectedTeam = this.assignedResources().filter(r => !teamMemberIds.includes(r.Id));
			this.assignedResources.removeAll(assignedResourcesNotInSelectedTeam);
			if (this.assignedResources().length === 0 && team.Members.length > 0) {
				const firstTeamMember = team.Members[0].User;
				if (this.techniciansMap.has(firstTeamMember.Id)) {
					this.assignedResources.push(firstTeamMember);
				}
			}
		}
	}
	async addResourcesToProfile() {
		if (this.selectedTeam() !== null) {
			const teamMembersNotInAssignedResources = this.selectedTeam().Members.map(x => x.User).filter(m => !(this.assignedResources().map(r => r.Id).includes(m.Id)));
			const membersNotInTechniciansMap = teamMembersNotInAssignedResources.filter(t => !this.techniciansMap.has(t.Id));
			if (membersNotInTechniciansMap.length > 0) {
				(await this.parentViewModel.loadTechnicians(membersNotInTechniciansMap.map(t => t.Id)))
					.forEach(t => this.techniciansMap.set(t.OriginalData.Id as string, t));
			}
			let dispatch = this.dispatch() as Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch;
			this.assignedResources.push(...teamMembersNotInAssignedResources.filter(it => this.techniciansMap.has(it.Id)));
			const toolsAndVehicles = (await window.database.CrmServiceTeam_ArticleUserGroupRelationship.include("Article").filter("it => it.UserGroupKey == this.teamId && it.From < this.end && this.start < it.To", { teamId: this.selectedTeam().Id, start: dispatch.Date(), end: window.moment(dispatch.EndDate()) } ).toArray()).map(a => a.Article);
			this.assignedTools.push(...toolsAndVehicles.filter(t => !(this.assignedTools().map(x => x.Id).includes(t.Id))));
		}
	}
	
	resourceFilter(query: $data.Queryable<Main.Rest.Model.Main_User>, term: string): $data.Queryable<Main.Rest.Model.Main_User> {
		query = super.resourceFilter(query, term);
		if (this.selectedTeam() !== null){
			let teamMemberIds = this.selectedTeam().Members.map(x => x.User).filter(m => !(this.assignedResources().map(r => r.Id).includes(m.Id))).map(m => m.Id);
			query = query.filter(function(it) { return it.Id in this.teamMemberIds; }, { teamMemberIds: teamMemberIds });
		}
		return query;
	}

	protected handleResourceAssignments(added: string[], newTeamLeader: string, dispatch: Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch): void {
		const observableDispatch = this.dispatch() as Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch;
		const hasTeamAssigned = this.selectedTeam() !== null || 
			(observableDispatch?.ExtensionValues()?.TeamId() !== null);
		if (hasTeamAssigned) {
			const teamId = this.selectedTeam()?.Id ?? observableDispatch.ExtensionValues()?.TeamId();
			for (const resourceId of added) {
				if (window.Helper.String.isGuid(resourceId)) {
					const articleUserGroupRelationship = window.database.CrmServiceTeam_ArticleUserGroupRelationship.defaultType.create();
					articleUserGroupRelationship.UserGroupKey = teamId;
					articleUserGroupRelationship.ArticleKey = resourceId;
					articleUserGroupRelationship.From = dispatch.Date;
					articleUserGroupRelationship.To = dispatch.EndDate;
					window.database.add(articleUserGroupRelationship);
				}
			}
		} else {
			super.handleResourceAssignments(added, newTeamLeader, dispatch);
		}
	}
}

window.Sms.Scheduler.ViewModels.SchedulerEditModalViewModel = SchedulerEditModalViewModelExtension;