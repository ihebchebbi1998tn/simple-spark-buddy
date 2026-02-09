import {HelperBatch} from "@Main/helper/Helper.Batch";


export class ServiceOrderDetailsDispatchesTabViewModelExtensions extends window.Crm.Service.ViewModels.ServiceOrderDetailsDispatchesTabViewModel {
	async initItems(items: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch[]): Promise<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch[]> {
		items = await super.initItems(items);
		const queries = [];
		for (const dispatch of items) {
			if (dispatch.ExtensionValues().TeamId() !== null) {
				queries.push({
					queryable: window.database.Main_Usergroup.include("Members").include("Members.User").filter("it.Id === this.id", { id: dispatch.ExtensionValues().TeamId() }),
					method: "first",
					handler: (usergroup: Main.Rest.Model.Main_Usergroup) => {
						// @ts-ignore
						dispatch.Team = usergroup.asKoObservable();
						// @ts-ignore
						dispatch.Technicians = usergroup.Members.map(x => x.User).filter(x => x.Id !== dispatch.Username());
					}
				});
			}
		}
		await HelperBatch.Execute(queries);
		return items;
	}
}

window.Crm.Service.ViewModels.ServiceOrderDetailsDispatchesTabViewModel = ServiceOrderDetailsDispatchesTabViewModelExtensions;