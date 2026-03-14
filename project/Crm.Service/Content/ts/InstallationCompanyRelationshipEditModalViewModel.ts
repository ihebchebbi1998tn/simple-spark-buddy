import {namespace} from "@Main/namespace";
import {HelperInstallation} from "./helper/Helper.Installation";
import {HelperCompany} from "@Crm/helper/Helper.Company";

export class InstallationCompanyRelationshipEditModalViewModel extends window.Crm.ViewModels.BaseRelationshipEditModalViewModel {
	mode: string;

	constructor(parentViewModel: any) {
		super(parentViewModel);
		this.table = window.database.CrmService_InstallationCompanyRelationship;
		this.lookups = parentViewModel.tabs()["tab-relationships"]().lookups;
		this.relationshipTypeLookup = {$tableName: "CrmService_InstallationCompanyRelationshipType"};
		this.setMode(parentViewModel);
	}

	setMode(parentViewModel): void {
		if (parentViewModel instanceof window.Crm.ViewModels.CompanyDetailsViewModel) {
			this.mode = "company";
		} else if (parentViewModel instanceof window.Crm.Service.ViewModels.InstallationDetailsViewModel) {
			this.mode = "installation";
		}
	};

	getQueryForEditing(): $data.Queryable<Crm.Service.Rest.Model.CrmService_InstallationCompanyRelationship> {
		return window.database.CrmService_InstallationCompanyRelationship.include("Parent");
	};

	getEditableId(relationship): KnockoutObservable<string> {
		switch (this.mode) {
			case "company":
				return relationship.ParentId;
			case "installation":
				return relationship.ChildId;
		}
		return super.getEditableId(relationship);
	};

	getAutoCompleterOptions() {
		switch (this.mode) {
			case "company":
				return {
					table: "CrmService_Installation",
					orderBy: ['InstallationNo', 'Description'],
					key: "Id",
					mapDisplayObject: HelperInstallation.mapForSelect2Display,
					customFilter: HelperInstallation.getInstallationAutocompleteFilter
				}
			case "installation":
				return {
					table: "Crm_Company",
					orderBy: ["Name"],
					key: "Id",
					mapDisplayObject: HelperCompany.mapForSelect2Display,
					customFilter: HelperCompany.getSelect2Filter
				};

		}

		return super.getAutoCompleterOptions();
	};

	getAutoCompleterCaption(): string {
		switch (this.mode) {
			case "company":
				return "Installation";
			case "installation":
				return "Company";
		}
		return "Unspecified";
	};

	createNewEntity(): any {
		let relationship = super.createNewEntity();
		switch (this.mode) {
			case "installation":
				relationship.ParentId = this.contactId();
				break;
			case "company":
				relationship.ParentId = null;
				relationship.ChildId = this.contactId();
				break;
			default:
				break;
		}
		return relationship;
	};
}

namespace("Crm.Service.ViewModels").InstallationCompanyRelationshipEditModalViewModel = InstallationCompanyRelationshipEditModalViewModel;
