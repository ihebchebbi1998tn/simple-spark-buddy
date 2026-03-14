import {namespace} from "@Main/namespace";
import type {InstallationDetailsViewModel} from "./InstallationDetailsViewModel";

export class InstallationDetailsServiceOrdersTabViewModel extends window.Crm.Service.ViewModels.ServiceOrderHeadListIndexViewModel {
	installationId = ko.observable<string>(null);

	constructor(parentViewModel: InstallationDetailsViewModel) {
		super();
		this.isTabViewModel(true);
		this.bookmark(this.bookmarks().find(x => x.Key === "All"));
		this.bulkActions([]);
		const installationId = parentViewModel.installation().Id();
		this.installationId(installationId);
		this.infiniteScroll(true);
		this.joins.removeAll([
			"Installation", "Installation.Address", "Installation.Company", "Company", "ServiceObject"
		]);
	}

	applyFilters(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderHead>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderHead> {
		query = super.applyFilters(query);
		if (window.Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode === "OrderPerInstallation") {
			return query.filter("it.InstallationId === this.installationId",
				{ installationId: this.installationId() });
		}

		if (window.database.storageProvider.supportedSetOperations.some) {
			query = query.filter("it.ServiceOrderTimes.some(function(it2) { return it2.InstallationId === this.installationId; }) || it.InstallationId === this.installationId",
				{installationId: this.installationId()});
		} else {
			query = query.filter("it.ServiceOrderTimes.InstallationId === this.installationId || it.InstallationId === this.installationId",
				{installationId: this.installationId()});
		}
		return query;
	};
}

namespace("Crm.Service.ViewModels").InstallationDetailsServiceOrdersTabViewModel = InstallationDetailsServiceOrdersTabViewModel;