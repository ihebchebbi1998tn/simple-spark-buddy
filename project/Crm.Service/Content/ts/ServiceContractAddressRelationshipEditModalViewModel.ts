import {namespace} from "@Main/namespace";

export class ServiceContractAddressRelationshipEditModalViewModel extends window.Crm.ViewModels.BaseRelationshipEditModalViewModel {
	table = window.database.CrmService_ServiceContractAddressRelationship;
	lookups: LookupType = {
		serviceContractAddressRelationshipTypes: {$tableName: "CrmService_ServiceContractAddressRelationshipType"}
	};
	relationshipTypeLookup = this.lookups.serviceContractAddressRelationshipTypes;

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
	};

	getQueryForEditing(): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceContractAddressRelationship> {
		return window.database.CrmService_ServiceContractAddressRelationship
			.include("Child");
	};

	getEditableId(relationship: Crm.Service.Rest.Model.ObservableCrmService_ServiceContractAddressRelationship): KnockoutObservable<string> {
		return relationship.ChildId;
	};

	getAutoCompleterOptions(): Select2AutoCompleterOptions {
		return {
			table: "Crm_Address",
			orderBy: ["Name1", "Name2", "Name3", "ZipCode", "City", "Street"],
			mapDisplayObject: window.Helper.Address.mapForSelect2Display,
			customFilter: window.Helper.Address.getSelect2Filter,
			key: "Id"
		};
	};

	getAutoCompleterCaption(): string {
		return "Address";
	};

	createNewEntity(): Crm.Service.Rest.Model.CrmService_ServiceContractAddressRelationship {
		const relationship = super.createNewEntity();
		relationship.ParentId = this.contactId();
		return relationship;
	};
}

namespace("Crm.Service.ViewModels").ServiceContractAddressRelationshipEditModalViewModel = ServiceContractAddressRelationshipEditModalViewModel;