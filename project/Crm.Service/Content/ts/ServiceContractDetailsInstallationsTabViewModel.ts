import {namespace} from "@Main/namespace";
import type {ServiceContractDetailsViewModel} from "./ServiceContractDetailsViewModel";

export class ServiceContractDetailsInstallationsTabViewModel extends window.Main.ViewModels.GenericListViewModel<Crm.Service.Rest.Model.CrmService_ServiceContractInstallationRelationship, Crm.Service.Rest.Model.ObservableCrmService_ServiceContractInstallationRelationship> {
	lookups: LookupType = {
		installationTypes: {$tableName: "CrmService_InstallationType"}
	};

	constructor(parentViewModel: ServiceContractDetailsViewModel) {
		super("CrmService_ServiceContractInstallationRelationship", ["Child.InstallationNo", "Child.Description"], ["ASC", "ASC"], ["Child"]);
		this.getFilter("ParentId").extend({filterOperator: "==="})(parentViewModel.serviceContract().Id());
	}

	async init(): Promise<void> {
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups)
		await super.init();
	};

	deleteRelationship = window.Crm.ViewModels.BaseRelationshipsTabViewModel.prototype.deleteRelationship;

	async getInverseRelationship(): Promise<void> {
		return;
	};
}

namespace("Crm.Service.ViewModels").ServiceContractDetailsInstallationsTabViewModel = ServiceContractDetailsInstallationsTabViewModel;