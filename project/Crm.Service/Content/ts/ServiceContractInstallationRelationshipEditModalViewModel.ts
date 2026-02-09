import {namespace} from "@Main/namespace";

export class ServiceContractInstallationRelationshipEditModalViewModel extends window.Crm.ViewModels.BaseRelationshipEditModalViewModel {
	table = window.database.CrmService_ServiceContractInstallationRelationship;
	serviceContract = ko.observable<Crm.Service.Rest.Model.CrmService_ServiceContract>(null);

	getQueryForEditing(): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceContractInstallationRelationship> {
		return window.database.CrmService_ServiceContractInstallationRelationship
			.include("Child");
	};

	getEditableId(relationship: Crm.Service.Rest.Model.ObservableCrmService_ServiceContractInstallationRelationship): KnockoutObservable<string> {
		return relationship.ChildId;
	};

	getAutoCompleterOptions(): Select2AutoCompleterOptions {
		return {
			customFilter: (query, term) => {
				if (term) {
					query = window.Helper.String.contains(query, term, ["InstallationNo", "Description"]);
				}
				if (this.serviceContract().ServiceObjectId) {
					query = query.filter(function(it) {
							return it.FolderId === this.ServiceObjectId;
						},
						{ ServiceObjectId: this.serviceContract().ServiceObjectId });
				} else {
					if (window.Crm.Service.Settings.ServiceContract.OnlyInstallationsOfReferencedCustomer) {
						query = query.filter(function (it) {
								return it.LocationContactId === this.ParentId;
							},
							{ParentId: this.serviceContract().ParentId});
					}
				}
				return query;
			},
			mapDisplayObject: window.Helper.Installation.mapForSelect2Display,
			orderBy: ["InstallationNo", "Description"],
			table: "CrmService_Installation"
		};
	};

	getAutoCompleterCaption(): string {
		return "Installation";
	};

	createNewEntity(): Crm.Service.Rest.Model.CrmService_ServiceContractInstallationRelationship {
		const relationship = super.createNewEntity();
		relationship.ParentId = this.contactId();
		return relationship;
	};

	showRelationshipTab(): void {
		$(".tab-nav a[href='#tab-installations']").tab("show");
	};

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		let result = await window.database.CrmService_ServiceContract.find(this.contactId());
		this.serviceContract(result);
	};
}

namespace("Crm.Service.ViewModels").ServiceContractInstallationRelationshipEditModalViewModel = ServiceContractInstallationRelationshipEditModalViewModel;