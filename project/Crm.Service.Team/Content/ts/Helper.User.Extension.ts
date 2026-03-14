export class HelperUserExtension extends window.Helper.User {
	static filterUsergroupQuery(query: $data.Queryable<Main.Rest.Model.Main_Usergroup>, term: string): $data.Queryable<Main.Rest.Model.Main_Usergroup> {
		query = query.filter(it => it.ExtensionValues.IsServiceTeam === false);
		return super.filterUsergroupQuery(query, term);
	};
	static override getUsergroupsDisplay(usergroups: Main.Rest.Model.ObservableMain_Usergroup[]): string {
		// @ts-ignore
		return usergroups().map(x => x.Name()).sort((a, b) => a.localeCompare(b)).join(', ');
	};
	static override areUsergroupsVisible(usergroups: Main.Rest.Model.ObservableMain_Usergroup[]): boolean {
		// @ts-ignore
		return usergroups().length === 0;
	};
	//Don't forget to update the reference in Sms.Scheduler.Team plugin if this function changes (either in name or parameters)
	static filterUsergroupQueryForServiceTeams(query: $data.Queryable<Main.Rest.Model.Main_Usergroup>, term: string, date?: Date, toDate?: Date): $data.Queryable<Main.Rest.Model.Main_Usergroup> {
		toDate = toDate ?? date;
		query = query.filter(it => it.ExtensionValues.IsServiceTeam === true);
		date = ko.unwrap(date);
		if (date){
			query = query.filter(function (it) {
				return (it.ExtensionValues.ValidFrom === null || it.ExtensionValues.ValidFrom <= this.toDate)
					&& (it.ExtensionValues.ValidTo === null || it.ExtensionValues.ValidTo >= this.date);
			}, {date: date, toDate: toDate});
		}
		return super.filterUsergroupQuery(query, term);
	};

}

window.Helper.User = HelperUserExtension;