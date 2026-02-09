import type {GenericListViewModel} from "@Main/GenericListViewModel";
import {Mixin} from "ts-mixer";

export class PersonDetailsRelationshipsTabViewModelExtension extends window.Crm.ViewModels.PersonDetailsRelationshipsTabViewModel {
	genericInstallationPersonRelationships: GenericListViewModel<Crm.Service.Rest.Model.CrmService_InstallationPersonRelationship, Crm.Service.Rest.Model.ObservableCrmService_InstallationPersonRelationship>;
	installationPersonRelationships: KnockoutObservableArray<Crm.Service.Rest.Model.ObservableCrmService_InstallationPersonRelationship>;

	constructor(parentViewModel: any) {
		super(parentViewModel)
		if (window.AuthorizationManager.currentUserIsAuthorizedForAction("Installation", "ReadInstallationPersonRelationship")) {
			this.lookups.installationPersonRelationshipTypes = {$tableName: "CrmService_InstallationPersonRelationshipType"};
			this.lookups.countries = {$tableName: "Main_Country"};
			this.lookups.regions = {$tableName: "Main_Region"};
			this.lookups.installationHeadStatuses = {$tableName: "CrmService_InstallationHeadStatus"};
			this.lookups.installationTypes = {$tableName: "CrmService_InstallationType"};
			this.genericInstallationPersonRelationships = new window.Main.ViewModels.GenericListViewModel(
				"CrmService_InstallationPersonRelationship",
				["RelationshipTypeKey"], ["ASC"],
				["Parent"]);
			this.genericInstallationPersonRelationships.getFilter("ChildId").extend({filterOperator: "==="})(this.personId);

			this.installationPersonRelationships = this.genericInstallationPersonRelationships.items;
			window.Helper.Distinct.createIndex(this.installationPersonRelationships, "RelationshipTypeKey");
			this.subViewModels.push(this.genericInstallationPersonRelationships);
		}
	}
}

window.Crm.ViewModels.PersonDetailsRelationshipsTabViewModel = Mixin(PersonDetailsRelationshipsTabViewModelExtension);