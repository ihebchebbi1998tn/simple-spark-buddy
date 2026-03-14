import { namespace } from "@Main/namespace";

export default class LookupEditModalViewModelExtension extends window.Main.ViewModels.LookupEditModalViewModel {
	selectedErrorTypes = ko.observableArray<string>([]);
	numberingSequences: any[];
	currentLookupMaintenanceOrder: KnockoutObservable<boolean> = ko.observable<boolean>(false);
	existingMaintenanceOrderTypes: KnockoutObservableArray<any> = ko.observableArray<any>([]);
	isMaintenanceOrderEditable: KnockoutComputed<boolean> = ko.pureComputed<boolean>(() => this.existingMaintenanceOrderTypes().length == 0 || this.currentLookupMaintenanceOrder());
	maintenanceOrderValidationMessage: KnockoutComputed<string> = ko.pureComputed<string>(() => window.Helper.String.getTranslatedString("MaintenanceOrderTypeAlreadyExists").replace("{1}", this.existingMaintenanceOrderTypes().map(x => `${x.Value}(${x.Key})`).join(", ")));

	extendedLookups: LookupType = {
		errorTypes: { $tableName: "CrmService_StatisticsKeyFaultImage" }
	};

	async init(id?: string, params?: { [key: string]: string }) {
		await super.init(id, params);
		if (this.lookup().ErrorTypes) {
			await window.Helper.Lookup.getLocalizedArrayMaps(this.extendedLookups);
		}
		if (ko.unwrap(this.lookup().ErrorTypes)) {
			this.selectedErrorTypes(this.lookup().ErrorTypes().split(","));
		}
		if (params.fullName === "Crm.Service.Model.Lookup.ServiceOrderType") {
			this.numberingSequences = await fetch(window.Helper.Url.resolveUrl("~/Main/NumberingSequences.json")).then(x => x.json());
			this.extendedLookups.serviceOrderTypes = await window.Helper.Lookup.getLocalizedArrayMap("CrmService_ServiceOrderType");
			this.existingMaintenanceOrderTypes(this.extendedLookups.serviceOrderTypes.$array.filter(x => x.MaintenanceOrder));
			this.currentLookupMaintenanceOrder(this.existingMaintenanceOrderTypes().filter(x => x.Key == id).length === 1);
		}
	};

	getErrorTypesFromKeys(keys: string): Select2AutoCompleterResult[] {
		return this.extendedLookups.errorTypes.$array
			.filter(x => keys != null ? keys.split(',').indexOf(x.Key) !== -1 : [])
			.map(window.Helper.Lookup.mapLookupForSelect2Display);
	};

	getSelectedOptions(keys: string): string[] {
		return keys != null ? keys.split(',') : [];
	};

	async save(): Promise<void> {
		if (this.lookup().ErrorTypes) {
			await this.saveErrorTypeRelationship();
			this.lookup().ErrorTypes(this.selectedErrorTypes().join(','));
		}
		await super.save();
	};

	async saveErrorTypeRelationship(): Promise<void> {
		const selectedErrorTypeKeys = this.selectedErrorTypes();
		const statisticsKeyCauseId = this.lookup().Id();
		const errorTypeEntities = await window.database.CrmService_StatisticsKeyFaultImage
			.filter("it.Key in this.keys && it.Language == this.language",
				{
					keys: selectedErrorTypeKeys,
					language: this.language().Key
				})
			.toArray();
		const selectedErrorTypeIds = errorTypeEntities.map(et => et.Id);
		// @ts-ignore
		const existingRelationships = await window.database.CrmService_ErrorCauseTypeRelationship
			.filter("it.StatisticsKeyCauseKey === this.id", { id: statisticsKeyCauseId })
			.toArray();
		const existingErrorTypeIds = existingRelationships.map(rel => rel.ErrorTypeKey);
		const newErrorTypeIds = selectedErrorTypeIds.filter(
			id => !existingErrorTypeIds.includes(id)
		);
		const removedErrorTypeIds = existingErrorTypeIds.filter(
			id => !selectedErrorTypeIds.includes(id)
		);
		for (const errorTypeId of newErrorTypeIds) {
			// @ts-ignore
			const relationship = window.database.CrmService_ErrorCauseTypeRelationship.defaultType.create();
			relationship.StatisticsKeyCauseKey = statisticsKeyCauseId;
			relationship.ErrorTypeKey = errorTypeId;
			window.database.add(relationship);
		}
		for (const errorTypeId of removedErrorTypeIds) {
			const relationshipToDelete = existingRelationships.find(
				rel => rel.ErrorTypeKey === errorTypeId
			);
			if (relationshipToDelete) {
				window.database.remove(relationshipToDelete);
			}
		}
	}
}
namespace("Main.ViewModels").LookupEditModalViewModel = LookupEditModalViewModelExtension;