import type {Model, Store} from "@bryntum/schedulerpro";

export class SchedulerExtension extends window.Sms.Scheduler.ViewModels.Scheduler {
	constructor(data) {
		super(data);
		const self = this;
		let cellMenu: any = self.scheduler.features.cellMenu;
		const baseOpenDetails = cellMenu.items.openDetails.onItem;
		const baseProcessItems = cellMenu.processItems;
		cellMenu.items.openDetails.onItem = ({record}) => {
			if (record["key"] instanceof Main.Rest.Model.Main_Usergroup) {
				window.open(`#/Main/UserGroup/DetailsTemplate/${record["key"].Id}`, window.Helper.Scheduler.URLTarget());
			} else {
				baseOpenDetails({record});
			}
		}
		cellMenu.processItems = (data) => {
			baseProcessItems(data);
			const {items, record} = data;
			if (record["key"] instanceof Main.Rest.Model.Main_Usergroup) {
				items.openDetails.hidden = false;
			}
		}
		self.scheduler.project.resourceStore.on("beforeAdd", async (event: { source: Store, records: Model[], parent: Model }) => {
			await self.parentViewModel.loadTeams(event.records);
			self.scheduler.refreshRows();
		});
		this.setupTeamAssignmentHandling();
	}

	private setupTeamAssignmentHandling() {
		const self = this;
		const eventMenu = self.scheduler.features.eventMenu;
		//@ts-ignore
		const originalUnassignItem = eventMenu.items.unassignEvent;
		if (originalUnassignItem) {
			if (originalUnassignItem.onItem){
			const baseUnassignOnItem = originalUnassignItem.onItem;
			originalUnassignItem.onItem = async function({assignmentRecord, eventRecord}) {
				if (window.Helper.String.isGuid(assignmentRecord.resourceId as string)) {
					if (eventRecord.OriginalData.ExtensionValues.TeamId != null) {
						const articleUserGroupRelationship = await window.database.CrmServiceTeam_ArticleUserGroupRelationship.filter(function(it){
							return it.ArticleKey == this.id && it.UserGroupKey == this.userGroupKey
						},
						{	
							id: assignmentRecord.resourceId,
							userGroupKey: eventRecord.OriginalData.ExtensionValues.TeamId
						})
						.toArray();
						
						if (articleUserGroupRelationship) {
							window.database.remove(articleUserGroupRelationship[0]);
						}
					} else {
						const articleUserRelationship = await window.database.CrmArticle_ArticleUserRelationship.filter(function(it){
							return it.ArticleKey == this.id && it.UserKey == this.userKey
						},
						{	
							id: assignmentRecord.resourceId,
							userKey: eventRecord.OriginalData.Username
						})
						.toArray();
						
						if (articleUserRelationship) {
							window.database.remove(articleUserRelationship[0]);
						}
					}
				}
				return baseUnassignOnItem.call(this, {assignmentRecord, eventRecord});
				}
			};
		}
	}
}
window.Sms.Scheduler.ViewModels.Scheduler = SchedulerExtension;