export class ServiceOrderTimePostingEditModalViewModelExtension extends window.Crm.Service.ViewModels.ServiceOrderTimePostingEditModalViewModel {

	async init(id?: string, params?: { [key: string]: string }): Promise<void> {
		await super.init(id, params);
		if (this.dispatch()?.ExtensionValues().TeamId()) {
			this.enableUserGroupSelection(false);
			this.showTechnicianSelection(true);
			this.userGroupId(this.dispatch().ExtensionValues().TeamId());
			if (!this.prePlanned() && (!id || window.Helper.ServiceOrderTimePosting.isPrePlanned(this.serviceOrderTimePosting()))) {
				this.serviceOrderTimePosting().Username(this.dispatch().Username());
				this.selectedUsernames([]);
				this.selectedUsernames.push(this.serviceOrderTimePosting().Username());
			}
		}
	};
}

window.Crm.Service.ViewModels.ServiceOrderTimePostingEditModalViewModel = ServiceOrderTimePostingEditModalViewModelExtension;