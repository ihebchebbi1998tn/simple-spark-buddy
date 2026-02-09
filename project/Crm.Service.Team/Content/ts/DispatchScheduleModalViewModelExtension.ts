export class DispatchScheduleModalViewModelExtension extends window.Crm.Service.ViewModels.DispatchScheduleModalViewModel {

	isTeamDispatch: { [key: string]: KnockoutObservable<boolean> } = {};

	constructor(parentViewModel: any) {
		super(parentViewModel);
	}

	pushDispatch(dispatch: Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch): void {
		this.isTeamDispatch[dispatch.Id] = ko.observable<boolean>(dispatch.ExtensionValues.TeamId !== null);
		this.isTeamDispatch[dispatch.Id].subscribe(value => {
			if (value === false){
				dispatch.ExtensionValues.TeamId = null;
			}
		});
		return super.pushDispatch(dispatch);
	};

	async createUserGroup(): Promise<void> {
		let confirm = await window.Helper.Confirm.confirmContinueAsync();
		if (!confirm){
			return;
		}
		this.dispose();
		location.hash = "/Main/UserGroup/Create?serviceTeam=true";
	};

	filterTechnicianQuery(query, term: string, dispatch: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch): $data.Queryable<Main.Rest.Model.Main_User> {
		return dispatch.ExtensionValues().TeamId() ? window.Helper.User.filterUserQuery(query, term, dispatch.ExtensionValues().TeamId()) : super.filterTechnicianQuery(query, term, dispatch);
	};

	async onUserGroupSelect(userGroup: Main.Rest.Model.Main_Usergroup,dispatch: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch): Promise<void> {
		const isNewDispatch = dispatch.innerInstance.entityState === $data.EntityState.Added;
		const mainResourceId = userGroup?.ExtensionValues.MainResourceId;
		const firstUserId = userGroup?.UsersIds?.[0];

		if (mainResourceId) {
			dispatch.Username(mainResourceId);
		} else if (firstUserId) {
			dispatch.Username(firstUserId);
		} else {
			dispatch.Username(isNewDispatch ? null : window.Helper.User.getCurrentUserName());
		}
	}
}

window.Crm.Service.ViewModels.DispatchScheduleModalViewModel = DispatchScheduleModalViewModelExtension;