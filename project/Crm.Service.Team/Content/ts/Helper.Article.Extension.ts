import {HelperUserExtension} from "./Helper.User.Extension";

export class HelperArticleExtension extends window.Helper.Article {
	static async getAssignedVehicleIds(username: string, from: Date, to: Date): Promise<string[]> {
		let result = await super.getAssignedVehicleIds(username, from, to);
		let teams = await HelperUserExtension.filterUsergroupQueryForServiceTeams(window.database.Main_Usergroup, null, from, to).toArray();
		if (username) {
			teams = teams.filter(it => it.UsersIds.indexOf(username) !== -1);
		}
		let teamIds = teams.map(x => x.Id);
		let query = window.database.CrmServiceTeam_ArticleUserGroupRelationship
			.filter(it => it.Article.ArticleTypeKey === "Vehicle");
		query = query.filter(function (it) {
			return it.UserGroupKey in this.teamIds
		}, {teamIds: teamIds});
		if (from) {
			query = query.filter(function (it) {
				return it.To === null || it.To >= this.from
			}, {from: from});
		}
		if (to) {
			query = query.filter(function (it) {
				return it.From === null || it.From <= this.to
			}, {to: to});
		}
		return result.concat(await query
			.map(it => it.ArticleKey)
			.toArray());
	};
}

window.Helper.Article = HelperArticleExtension;