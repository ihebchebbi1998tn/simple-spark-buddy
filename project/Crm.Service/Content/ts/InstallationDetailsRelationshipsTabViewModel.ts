import {namespace} from "@Main/namespace";
import type {GenericListViewModel} from "@Main/GenericListViewModel";

export class InstallationDetailsRelationshipsTabViewModel extends window.Crm.ViewModels.BaseRelationshipsTabViewModel {
	installationId: string;
	genericInstallationAddressRelationships: GenericListViewModel<Crm.Service.Rest.Model.CrmService_InstallationAddressRelationship, Crm.Service.Rest.Model.ObservableCrmService_InstallationAddressRelationship>;
	installationAddressRelationships: KnockoutObservableArray<Crm.Service.Rest.Model.ObservableCrmService_InstallationAddressRelationship>;
	genericInstallationCompanyRelationships: GenericListViewModel<Crm.Service.Rest.Model.CrmService_InstallationCompanyRelationship, Crm.Service.Rest.Model.ObservableCrmService_InstallationCompanyRelationship>;
	installationCompanyRelationships: KnockoutObservableArray<Crm.Service.Rest.Model.ObservableCrmService_InstallationCompanyRelationship>;
	genericInstallationPersonRelationships: GenericListViewModel<Crm.Service.Rest.Model.CrmService_InstallationPersonRelationship, Crm.Service.Rest.Model.ObservableCrmService_InstallationPersonRelationship>;
	installationPersonRelationships: KnockoutObservableArray<Crm.Service.Rest.Model.ObservableCrmService_InstallationPersonRelationship>;

	constructor(parentViewModel: any) {
		super(parentViewModel);
		this.lookups.installationAddressRelationshipTypes = {$tableName: "CrmService_InstallationAddressRelationshipType"};
		this.lookups.installationCompanyRelationshipTypes = {$tableName: "CrmService_InstallationCompanyRelationshipType"};
		this.lookups.installationPersonRelationshipTypes = {$tableName: "CrmService_InstallationPersonRelationshipType"};

		this.installationId = parentViewModel.installation().Id();

		const ChildAddresses = {
			Selector: "Child.Addresses",
			Operation: "filter(function (a) {return a.IsCompanyStandardAddress == true;})"
		};

		this.genericInstallationAddressRelationships = new window.Main.ViewModels.GenericListViewModel(
			"CrmService_InstallationAddressRelationship",
			["RelationshipTypeKey", "Child.Name1", "Child.Name2", "Child.Name3", "Child.ZipCode", "Child.City", "Child.Street"],
			["ASC", "ASC", "ASC", "ASC", "ASC", "ASC", "ASC"],
			["Child"]);
		this.genericInstallationAddressRelationships.getFilter("ParentId").extend({filterOperator: "==="})(this.installationId);
		this.installationAddressRelationships = this.genericInstallationAddressRelationships.items;
		window.Helper.Distinct.createIndex(this.installationAddressRelationships, "RelationshipTypeKey");
		this.genericInstallationAddressRelationships.infiniteScroll(true);
		this.subViewModels.push(this.genericInstallationAddressRelationships);

		this.genericInstallationCompanyRelationships = new window.Main.ViewModels.GenericListViewModel(
			"CrmService_InstallationCompanyRelationship",
			["RelationshipTypeKey"],
			["ASC"],
			["Child", ChildAddresses, "Child.ResponsibleUserUser", "Child.ParentCompany", "Child.Tags"]);
		this.genericInstallationCompanyRelationships.getFilter("ParentId").extend({filterOperator: "==="})(this.installationId);
		this.installationCompanyRelationships = this.genericInstallationCompanyRelationships.items;
		window.Helper.Distinct.createIndex(this.installationCompanyRelationships, "RelationshipTypeKey");
		this.genericInstallationCompanyRelationships.infiniteScroll(true);
		this.subViewModels.push(this.genericInstallationCompanyRelationships);

		this.genericInstallationPersonRelationships = new window.Main.ViewModels.GenericListViewModel(
			"CrmService_InstallationPersonRelationship",
			["RelationshipTypeKey"],
			["ASC"],
			["Child", "Child.Parent", "Child.Address", "Child.ResponsibleUserUser", "Child.Emails", "Child.Phones", "Child.Tags"]);
		this.genericInstallationPersonRelationships.getFilter("ParentId").extend({filterOperator: "==="})(this.installationId);
		this.installationPersonRelationships = this.genericInstallationPersonRelationships.items;
		window.Helper.Distinct.createIndex(this.installationPersonRelationships, "RelationshipTypeKey");
		this.genericInstallationPersonRelationships.infiniteScroll(true);
		this.subViewModels.push(this.genericInstallationPersonRelationships);
	}

	async init(): Promise<void> {
		this.loading(true);
		await super.init();
		return window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
	}
}

namespace("Crm.Service.ViewModels").InstallationDetailsRelationshipsTabViewModel = InstallationDetailsRelationshipsTabViewModel;