import type {ServiceObjectDetailsViewModel} from "./ServiceObjectDetailsViewModel";
import {namespace} from "@Main/namespace";

export class ServiceObjectDetailsPersonsTabViewModel extends window.Main.ViewModels.GenericListViewModel<Crm.Rest.Model.Crm_Person, Crm.Rest.Model.ObservableCrm_Person> {
	serviceObjectId = ko.observable<string>(null);
	lookups: LookupType = {
		countries: {$tableName: "Main_Country"},
		regions: {$tableName: "Main_Region"}
	};
	parentViewModel: ServiceObjectDetailsViewModel;

	constructor(parentViewModel: ServiceObjectDetailsViewModel) {
		const joinTags = {
			Selector: "Tags",
			Operation: "orderBy(function(t) { return t.Name; })"
		};
		super(
			"Crm_Person",
			["Surname", "Firstname"],
			["ASC", "ASC"],
			["Address", "Emails", "Faxes", "Phones", "ResponsibleUserUser", joinTags]);
		this.parentViewModel = parentViewModel;
		const serviceObjectId = parentViewModel.serviceObject().Id();
		this.serviceObjectId(serviceObjectId);
		this.getFilter("ParentId").extend({filterOperator: "==="})(serviceObjectId);
		if (window.Crm.Settings.Person.BusinessTitleIsLookup) {
			this.lookups.businessTitles = {$tableName: "Crm_BusinessTitle"};
		}
	}

	async init(): Promise<void> {
		await super.init();
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		if (window.Crm.Settings.Person.BusinessTitleIsLookup) {
			this.parentViewModel.lookups.businessTitles = this.lookups.businessTitles;
		}
	};
}

namespace("Crm.Service.ViewModels").ServiceObjectDetailsPersonsTabViewModel = ServiceObjectDetailsPersonsTabViewModel;