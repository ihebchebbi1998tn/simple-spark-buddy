import type {TechnicianExtension} from "@Sms.Scheduler.Team/Model/TechnicianExtension";
import type {ArticleExtension} from "@Sms.Scheduler.Team/Model/ArticleExtension";

export class SchedulerDetailsViewModelExtension extends window.Sms.Scheduler.ViewModels.SchedulerDetailsViewModel {
	override async initSchedulerData(profileId: number = null): Promise<void> {
		await super.initSchedulerData(profileId);
		await this.loadTeams();
	}
	async loadTeams(resources: ResourceTypes[] = null): Promise<void> {
		const startDate = window.moment(this.startDate).subtract(this.profile().ClientConfig.LowerBound, "d").toDate();
		const endDate = window.moment(this.startDate).add(this.profile().ClientConfig.UpperBound, "d").toDate();
		const technicians = resources?.filter((r: any): r is TechnicianExtension => r.ResourceType === 'Technician') ?? this.inlineData.resourcesData.filter((r: any): r is TechnicianExtension => r.ResourceType === 'Technician');
		const articles = resources?.filter((r: any): r is ArticleExtension => r.OriginalData instanceof Crm.Article.Rest.Model.CrmArticle_Article) ?? this.inlineData.resourcesData.filter((r: any): r is ArticleExtension => r.OriginalData instanceof Crm.Article.Rest.Model.CrmArticle_Article);
		const userIds: string[] = technicians?.map(r => r.id as string);
		const articleIds: string[] = articles?.map(r => r.id as string);

		const articleTeamRelationships = await window.database.CrmServiceTeam_ArticleUserGroupRelationship
			.include("UserGroup")
			.filter("it.ArticleKey in this.articleKeys", {articleKeys: articleIds})
			.filter(function (it) {
				return (it.UserGroup.ExtensionValues.ValidFrom === null || it.UserGroup.ExtensionValues.ValidFrom <= this.endDate)
					&& (it.UserGroup.ExtensionValues.ValidTo === null || it.UserGroup.ExtensionValues.ValidTo >= this.startDate);
			}, {startDate, endDate})
			.toArray();
		const articleTeams = articleTeamRelationships.filter(team => articleIds.some(articleId => team.ArticleKey.indexOf(articleId) !== -1));
		articles.forEach(article => {
			article.Teams = articleTeams.filter(t => t.ArticleKey === article.OriginalData.Id).map(t => t.UserGroup);
		});
		const userUserGroupRelationships = await window.database.Main_UserUserGroup
			.include("UserGroup")
			.filter("it.Username in this.userIds", {userIds: userIds})
			.filter(function (it) {
				return (it.UserGroup.ExtensionValues.ValidFrom === null || it.UserGroup.ExtensionValues.ValidFrom <= this.endDate)
					&& (it.UserGroup.ExtensionValues.ValidTo === null || it.UserGroup.ExtensionValues.ValidTo >= this.startDate);
			}, {startDate, endDate})
			.toArray();
		const userTeams = userUserGroupRelationships.filter(team => team.UserGroup.ExtensionValues.IsServiceTeam && userIds.some(userId => team.Username.indexOf(userId) !== -1));
		technicians.forEach(technician => {
			technician.Teams = userTeams.filter(t => t.Username === technician.OriginalData.Id).map(t => t.UserGroup);
		});
	}

}

window.Sms.Scheduler.ViewModels.SchedulerDetailsViewModel = SchedulerDetailsViewModelExtension;