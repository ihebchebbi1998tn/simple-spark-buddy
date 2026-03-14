import {namespace} from "@Main/namespace";
import type {ServiceObjectDetailsViewModel} from "./ServiceObjectDetailsViewModel";

export class ServiceObjectDetailsInstallationsTabViewModel extends window.Crm.Service.ViewModels.InstallationListIndexViewModel {
	serviceObjectId = ko.observable<string>(null);

	constructor(parentViewModel: ServiceObjectDetailsViewModel) {
		super();
		this.isTabViewModel(true);
		const serviceObjectId = parentViewModel.serviceObject().Id();
		this.serviceObjectId(serviceObjectId);
		this.getFilter("FolderId").extend({filterOperator: "==="})(serviceObjectId);
		this.joins().push("Company");
		this.joins.remove("ServiceObject");
		let orderBy = this.orderBy();
		if (orderBy instanceof Array) {
			orderBy.unshift("Company.Name");
		}
		let orderByDirection = this.orderByDirection();
		if (orderByDirection instanceof Array) {
			orderByDirection.unshift("ASC");
		}
	}

	getItemGroup(x: Crm.Service.Rest.Model.ObservableCrmService_Installation): ItemGroup {
		if (!x.Company()) {
			return null;
		}
		return {title: window.Helper.Company.getDisplayName(x.Company())};
	};

	async removeInstallation(installation: Crm.Service.Rest.Model.ObservableCrmService_Installation): Promise<void> {
		let confirm = await window.Helper.Confirm.confirmDeleteAsync();
		if (!confirm) {
			return;
		}
		this.loading(true);
		window.database.attachOrGet(window.Helper.Database.getDatabaseEntity(installation));
		installation.FolderId(null);
		await window.database.saveChanges();
	};
}

namespace("Crm.Service.ViewModels").ServiceObjectDetailsInstallationsTabViewModel = ServiceObjectDetailsInstallationsTabViewModel;