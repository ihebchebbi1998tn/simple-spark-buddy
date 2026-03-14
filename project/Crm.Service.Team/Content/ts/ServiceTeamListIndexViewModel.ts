import {namespace} from "@Main/namespace";

export class ServiceTeamListIndexViewModel extends window.Main.ViewModels.UserGroupListIndexViewModel {
	constructor() {
		super();
		this.pageTitle = window.Helper.String.getTranslatedString("ServiceTeams");
		const bookmarkAll = {
			Category: window.Helper.String.getTranslatedString("Filter"),
			Name: window.Helper.String.getTranslatedString("All"),
			Key: "All",
			Expression: (query: $data.Queryable<Main.Rest.Model.Main_Usergroup>) => query
		}
		const bookmarkActive = {
			Category: window.Helper.String.getTranslatedString("Filter"),
			Name: window.Helper.String.getTranslatedString("Active"),
			Key: "Active",
			Expression: (query: $data.Queryable<Main.Rest.Model.Main_Usergroup>) => {
				return query.filter(function (it) {
					return (it.ExtensionValues.ValidFrom === null || it.ExtensionValues.ValidFrom <= this.now)
						&& (it.ExtensionValues.ValidTo === null || it.ExtensionValues.ValidTo >= this.now);
				}, {now: new Date()});
			}
		}
		const bookmarkInactive = {
			Category: window.Helper.String.getTranslatedString("Filter"),
			Name: window.Helper.String.getTranslatedString("Inactive"),
			Key: "Inactive",
			Expression: (query: $data.Queryable<Main.Rest.Model.Main_Usergroup>) => {
				return query.filter(function (it) {
					return it.ExtensionValues.ValidFrom > this.now || it.ExtensionValues.ValidTo < this.now;
				}, {now: new Date()});
			}
		}
		this.bookmarks.push(bookmarkAll);
		this.bookmarks.push(bookmarkActive);
		this.bookmarks.push(bookmarkInactive);
		this.bookmark(bookmarkActive);
	}

	applyFilters(query: $data.Queryable<Main.Rest.Model.Main_Usergroup>): $data.Queryable<Main.Rest.Model.Main_Usergroup> {
		query = query.filter(it => it.ExtensionValues.IsServiceTeam === true);
		return super.applyFilters(query);
	}
}

namespace("Crm.Service.Team.ViewModels").ServiceTeamListIndexViewModel = ServiceTeamListIndexViewModel;