import type { PmbbViewModel } from "@Main/PmbbViewModel";
export class UserDetailsViewModelExtension extends window.Main.ViewModels.UserDetailsViewModel {
	serviceTeamsIds: KnockoutObservableArray<Edm.Guid>;
	serviceTeamsObjects: KnockoutObservableArray<Main.Rest.Model.ObservableMain_Usergroup>;

	constructor() {
		super();
		this.serviceTeamsIds = ko.observableArray<string>();
		this.serviceTeamsObjects = ko.observableArray<Main.Rest.Model.ObservableMain_Usergroup>();
	}

	override async init(id?: string, params?: { [key: string]: string }): Promise<void> {
		await super.init(id, params);
		this.initServiceTeams();
	}

	override async onSave(context: PmbbViewModel): Promise<void> {
		const user = context.viewContext.user();
		this.serviceTeamsIds().forEach(x => user.UsergroupIds.push(x));
		this.serviceTeamsObjects().forEach(x => user.UsergroupObjects.push(x));
		await super.onSave(context);
	}

	override async onAfterSave(context: PmbbViewModel): Promise<void> {
		await super.onAfterSave(context);
		this.initServiceTeams();
	}

	initServiceTeams(): void {
		this.serviceTeamsObjects(this.user().UsergroupObjects().filter(x => x.ExtensionValues().IsServiceTeam()));
		this.serviceTeamsIds(this.serviceTeamsObjects().map(x => x.Id()));
		this.user().UsergroupObjects(this.user().UsergroupObjects().filter(x => !x.ExtensionValues().IsServiceTeam()));
		this.user().UsergroupIds(this.user().UsergroupObjects().map(x => x.Id()));
	}
}
window.Main.ViewModels.UserDetailsViewModel = UserDetailsViewModelExtension;