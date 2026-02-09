import {namespace} from "@Main/namespace";
import {HelperInstallation} from "./helper/Helper.Installation";
import {HelperPerson} from "@Crm/helper/Helper.Person";

export class InstallationPersonRelationshipEditModalViewModel extends window.Crm.ViewModels.BaseRelationshipEditModalViewModel {
	mode: string;

	constructor(parentViewModel: any) {
		super(parentViewModel);
		this.table = window.database.CrmService_InstallationPersonRelationship;
		this.lookups = parentViewModel.tabs()["tab-relationships"]().lookups;
		this.relationshipTypeLookup = {$tableName: "CrmService_InstallationPersonRelationshipType"};
		this.setMode(parentViewModel);
	}

	setMode(parentViewModel): void {
		if (parentViewModel instanceof window.Crm.ViewModels.PersonDetailsViewModel) {
			this.mode = "person";
		} else if (parentViewModel instanceof window.Crm.Service.ViewModels.InstallationDetailsViewModel) {
			this.mode = "installation";
		}
	};

	getQueryForEditing(): $data.Queryable<Crm.Service.Rest.Model.CrmService_InstallationPersonRelationship> {
		return window.database.CrmService_InstallationPersonRelationship.include("Parent");
	};

	getEditableId(relationship): KnockoutObservable<string> {
		switch (this.mode) {
			case "person":
				return relationship.ParentId;
			case "installation":
				return relationship.ChildId;
		}
		return super.getEditableId(relationship);
	};

	getAutoCompleterOptions() {
		switch (this.mode) {
			case "person":
				return {
					table: "CrmService_Installation",
					orderBy: ['InstallationNo', 'Description'],
					key: "Id",
					mapDisplayObject: HelperInstallation.mapForSelect2Display,
					customFilter: HelperInstallation.getInstallationAutocompleteFilter
				}
			case "installation":
				return {
					table: "Crm_Person",
					orderBy: ['Surname', 'Firstname'],
					key: "Id",
					mapDisplayObject: HelperPerson.mapForSelect2Display,
					customFilter: HelperPerson.getSelect2Filter
				};

		}

		return super.getAutoCompleterOptions();
	};

	getAutoCompleterCaption(): string {
		switch (this.mode) {
			case "person":
				return "Installation";
			case "installation":
				return "Person";
		}
		return "Unspecified";
	};

	createNewEntity(): any {
		let relationship = super.createNewEntity();
		switch (this.mode) {
			case "installation":
				relationship.ParentId = this.contactId();
				break;
			case "person":
				relationship.ParentId = null;
				relationship.ChildId = this.contactId();
				break;
			default:
				break;
		}
		return relationship;
	};
}

namespace("Crm.Service.ViewModels").InstallationPersonRelationshipEditModalViewModel = InstallationPersonRelationshipEditModalViewModel;
