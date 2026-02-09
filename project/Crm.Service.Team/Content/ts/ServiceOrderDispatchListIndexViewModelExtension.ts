import {HelperBatch} from "@Main/helper/Helper.Batch";
import type {ServiceOrderDispatchListIndexItemViewModel} from "@Crm.Service/ServiceOrderDispatchListIndexViewModel";
import {HelperDownload} from "@Main/helper/Helper.Download";

export class ServiceOrderDispatchListIndexViewModelExtension extends window.Crm.Service.ViewModels.ServiceOrderDispatchListIndexViewModel {
	constructor(parentViewModel: any) {
		super(parentViewModel);

		const bookmarkMyteamDispatches: Bookmark<Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch> = {
			Category: window.Helper.String.getTranslatedString("Filter"),
			Name: window.Helper.String.getTranslatedString("MyTeamDispatches"),
			Key: "MyTeamDispatches",
			Expression: query => {
				return query.filter(function (it) {
					return it.ExtensionValues.TeamId !== null && it.Username === this.username;
				}, { username: this.currentUser().Id });
			}
		}
		this.bookmarks.push(bookmarkMyteamDispatches);
	}

	async initItems(items: (Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch & ServiceOrderDispatchListIndexItemViewModel)[]): Promise<(Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch & ServiceOrderDispatchListIndexItemViewModel)[]> {
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

	async downloadDispatchReport(dispatch: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch): Promise<void> {
		const viewModel = this;
		viewModel.loading(true);
		const reportName = `${dispatch.OrderNo()} - ${dispatch.Date().toISOString().substring(0, 10)} - ${Helper.User.getDisplayName(dispatch.DispatchedUser())}.pdf`;
		await HelperDownload.downloadPdf(`~/Crm.Service/Dispatch/GetReportPdf?dispatchId=${dispatch.Id()}`, reportName);
		viewModel.loading(false);
	}
}

window.Crm.Service.ViewModels.ServiceOrderDispatchListIndexViewModel = ServiceOrderDispatchListIndexViewModelExtension;