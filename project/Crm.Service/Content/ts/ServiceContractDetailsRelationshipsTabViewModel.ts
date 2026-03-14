import type {ServiceContractDetailsViewModel} from "./ServiceContractDetailsViewModel";
import {namespace} from "@Main/namespace";

export class ServiceContractDetailsRelationshipsTabViewModel extends window.Main.ViewModels.GenericListViewModel<Crm.Service.Rest.Model.CrmService_ServiceContractAddressRelationship, Crm.Service.Rest.Model.ObservableCrmService_ServiceContractAddressRelationship> {
	lookups: LookupType = {
		serviceContractAddressRelationshipTypes: {$tableName: "CrmService_ServiceContractAddressRelationshipType"}
	};

	constructor(parentViewModel: ServiceContractDetailsViewModel) {
		super("CrmService_ServiceContractAddressRelationship",
			[
				"RelationshipTypeKey", "Child.Name1", "Child.Name2", "Child.Name3", "Child.ZipCode", "Child.City",
				"Child.Street"
			],
			["ASC", "ASC", "ASC", "ASC", "ASC", "ASC", "ASC"],
			["Child"]);
		this.getFilter("ParentId").extend({filterOperator: "==="})(parentViewModel.serviceContract().Id());
		window.Helper.Distinct.createIndex(this.items, "RelationshipTypeKey");
	}

	async init(): Promise<void> {
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		await super.init();
	};

	async getInverseRelationship(): Promise<void> {
		return;
	};

	deleteRelationship =
		window.Crm.ViewModels.BaseRelationshipsTabViewModel.prototype.deleteRelationship;

	getItemGroup(serviceContractAddressRelationship: Crm.Service.Rest.Model.ObservableCrmService_ServiceContractAddressRelationship): ItemGroup {
		const relationshipTypeKey = serviceContractAddressRelationship.RelationshipTypeKey();
		return {
			title: window.Helper.Lookup.getLookupValue(this.lookups.serviceContractAddressRelationshipTypes,
				relationshipTypeKey)
		};
	};
}

namespace("Crm.Service.ViewModels").ServiceContractDetailsRelationshipsTabViewModel = ServiceContractDetailsRelationshipsTabViewModel;