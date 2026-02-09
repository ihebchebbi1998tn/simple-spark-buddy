export class HelperStatisticsKey {

	static async getAvailableLookups(lookups: LookupType, ...entities: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderStatisticsKey[] | Crm.Service.Rest.Model.ObservableCrmService_ServiceCase[]): Promise<void> {
		if(!window.AuthorizationManager || !window.AuthorizationManager.isAuthorizedForAction("StatisticsKey", "View"))
			return;
		let queries = [];
		const tableNames = [
			"CrmService_StatisticsKeyProductType",
			"CrmService_StatisticsKeyMainAssembly",
			"CrmService_StatisticsKeySubAssembly",
			"CrmService_StatisticsKeyAssemblyGroup",
			"CrmService_StatisticsKeyFaultImage",
			"CrmService_StatisticsKeyRemedy",
			"CrmService_StatisticsKeyCause",
			"CrmService_StatisticsKeyWeighting",
			"CrmService_StatisticsKeyCauser"
		];
		for(let tableName of tableNames) {
			queries.push({
				queryable: window.database[tableName],
				method: "count",
				handler: function(count) {
					if (count > 0) {
						let propertyName = HelperStatisticsKey.getJsNameByTable(tableName);
						let lookupDefinition = { $tableName: tableName, $lazy: true };
						//to fill up the lookup table with the key of the value stored part of the entity 
						if (entities.length > 0) {
							const entityPropertyName = HelperStatisticsKey.getModelPropertyNameByTable(entities[0], tableName);
							for (const entity of entities) {
								if (!!window.ko.unwrap(entity[entityPropertyName]))
									lookupDefinition[window.ko.unwrap(entity[entityPropertyName])] = null;
							}
							
						}
						lookups[propertyName] = lookupDefinition;
					}
				}
			});
		}
		await window.Helper.Batch.Execute(queries);
	}

	static getAutocompleteOptions(tableName: string, entity: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderStatisticsKey | Crm.Service.Rest.Model.ObservableCrmService_ServiceCase, additionalOptions: {}): Select2AutoCompleterOptions {
		let options = window.Helper.Lookup.getAutocompleteOptions(tableName);
		options.customFilter = function (query, key) {
			switch (tableName) {
				case "CrmService_StatisticsKeyAssemblyGroup":
					if (!!window.ko.unwrap(entity[HelperStatisticsKey.getModelPropertyNameByTable(entity, "CrmService_StatisticsKeySubAssembly")])) {
						query = query.filter("it.SubAssemblyKey == this.key", { key: window.ko.unwrap(entity[HelperStatisticsKey.getModelPropertyNameByTable(entity, "CrmService_StatisticsKeySubAssembly")]).split(':')[2] })
					}
					break;
				case "CrmService_StatisticsKeySubAssembly":
					if (!!window.ko.unwrap(entity[HelperStatisticsKey.getModelPropertyNameByTable(entity, "CrmService_StatisticsKeyMainAssembly")])) {
						query = query.filter("it.MainAssemblyKey == this.key", { key: window.ko.unwrap(entity[HelperStatisticsKey.getModelPropertyNameByTable(entity, "CrmService_StatisticsKeyMainAssembly")]).split(':')[1] })
					}
					break;
				case "CrmService_StatisticsKeyMainAssembly":
				case "CrmService_StatisticsKeyRemedy":
				case "CrmService_StatisticsKeyCause":
				case "CrmService_StatisticsKeyWeighting":
				case "CrmService_StatisticsKeyCauser":
					if (!!window.ko.unwrap(entity[HelperStatisticsKey.getModelPropertyNameByTable(entity, "CrmService_StatisticsKeyProductType")])) {
						query = query.filter("it.ProductTypeKey == null || it.ProductTypeKey == this.key", { key: window.ko.unwrap(entity[HelperStatisticsKey.getModelPropertyNameByTable(entity, "CrmService_StatisticsKeyProductType")]) })
					}
					break;
				case "CrmService_StatisticsKeyFaultImage":
					if (!!window.ko.unwrap(entity[HelperStatisticsKey.getModelPropertyNameByTable(entity, "CrmService_StatisticsKeyProductType")])) {
						query = query.filter("it.ProductTypeKey == null || it.ProductTypeKey == this.key", { key: window.ko.unwrap(entity[HelperStatisticsKey.getModelPropertyNameByTable(entity, "CrmService_StatisticsKeyProductType")]) })
					}
					if (!!window.ko.unwrap(entity[HelperStatisticsKey.getModelPropertyNameByTable(entity, "CrmService_StatisticsKeyAssemblyGroup")])) {
						query = query.filter("it.AssemblyGroupKey == this.key", { key: window.ko.unwrap(entity[HelperStatisticsKey.getModelPropertyNameByTable(entity, "CrmService_StatisticsKeyAssemblyGroup")]) })
					}
					break;
			}
			return window.Helper.Lookup.queryLookup(query, key);
		}
		return {...options, ...additionalOptions};
	}

	static getJsNameByTable(tableName: string): string {
		let propertyName = tableName.split('_')[1];
		return propertyName[0].toLowerCase() + propertyName.substr(1);
	}

	static getModelPropertyNameByTable(entity: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderStatisticsKey | Crm.Service.Rest.Model.ObservableCrmService_ServiceCase, tableName: string): string {
		let propertyName = tableName.split('_')[1] + "Key";
		if (entity.innerInstance instanceof window.database.CrmService_ServiceOrderStatisticsKey.defaultType) {
			return propertyName.substring(13);
		}
		return propertyName;
	}
}



