import type {GenericListViewModel} from "@Main/GenericListViewModel";
import {Mixin} from "ts-mixer";
import type {CompanyDetailsViewModel} from "@Crm/CompanyDetailsViewModel";

export class CompanyDetailsRelationshipsTabViewModelExtension extends window.Crm.ViewModels.CompanyDetailsRelationshipsTabViewModel {
	genericInstallationCompanyRelationships: GenericListViewModel<Crm.Service.Rest.Model.CrmService_InstallationCompanyRelationship, Crm.Service.Rest.Model.ObservableCrmService_InstallationCompanyRelationship>;
	installationCompanyRelationships: KnockoutObservableArray<Crm.Service.Rest.Model.ObservableCrmService_InstallationCompanyRelationship>;

	constructor(parentViewModel: CompanyDetailsViewModel) {
		super(parentViewModel)
		if (window.AuthorizationManager.currentUserIsAuthorizedForAction("Installation", "ReadInstallationCompanyRelationship")) {
			this.lookups.installationCompanyRelationshipTypes = {$tableName: "CrmService_InstallationCompanyRelationshipType"};
			this.lookups.countries = {$tableName: "Main_Country"};
			this.lookups.regions = {$tableName: "Main_Region"};
			this.lookups.installationHeadStatuses = {$tableName: "CrmService_InstallationHeadStatus"};
			this.lookups.installationTypes = {$tableName: "CrmService_InstallationType"};
			this.lookups.statisticsKeyProductTypes = {$tableName: "CrmService_StatisticsKeyProductType"};
			this.genericInstallationCompanyRelationships = new window.Main.ViewModels.GenericListViewModel(
				"CrmService_InstallationCompanyRelationship",
				["RelationshipTypeKey"], ["ASC"],
				["Parent"]);
			this.genericInstallationCompanyRelationships.getFilter("ChildId").extend({filterOperator: "==="})(this.companyId);

			this.installationCompanyRelationships = this.genericInstallationCompanyRelationships.items;
			window.Helper.Distinct.createIndex(this.installationCompanyRelationships, "RelationshipTypeKey");
			this.genericInstallationCompanyRelationships.pageSize(5);
			this.subViewModels.push(this.genericInstallationCompanyRelationships);
		}
	}
}

window.Crm.ViewModels.CompanyDetailsRelationshipsTabViewModel = Mixin(CompanyDetailsRelationshipsTabViewModelExtension);