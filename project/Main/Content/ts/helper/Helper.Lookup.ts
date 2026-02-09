import {HelperCulture} from "./Helper.Culture";
import {HelperString} from "./Helper.String";

export class HelperLookup {

	static defaultLanguage: string = null;

	static getLookupQuery(lookupName:string , key: string, language: string): $data.Queryable<any> {
		return window.database[lookupName]
		.select("it.Value")
		.filter("it.Key === this.key && it.Language === language", { key: key, language: language });
	}

	static async getLanguage(language: string | null): Promise<string> {
		if (language) {
			return language;
		} else {
			return HelperCulture.languageCulture();
		}
	}

	static initializeArray(tableName: string | $data.EntitySet<any, any>, lookups: any[]): any[] {
		const result = [];
		if (lookups.length > 1) {
			//for knockout.options binding use this dummy instead of optionsCaption
			const dummy = (typeof tableName === "string" ? window.database[tableName] : tableName).defaultType.create();
			dummy.Value = HelperString.getTranslatedString("PleaseSelect");
			dummy.Key = null;
			result.push(dummy);
		}
		result.push(...lookups);
		return result;
	}

	static getLocalizedQuery(tableName: string | $data.EntitySet<any, any> | $data.Queryable<any>, language: string = null, filterExpression: string = null, filterParameters: {} = null): $data.Queryable<any> {
		language = language || window.Helper.Lookup.defaultLanguage;
		let table;
		if (typeof tableName === "string") {
			table = window.database[tableName];
			if (!table) {
				throw new Error("table not found: " + tableName);
			}
		} else {
			table = tableName;
		}

		let query = table.filter("it.Language === this.language", {language: language});
		if (!!filterExpression) {
			query = query.filter(filterExpression, filterParameters);
		}
		return query
		.orderByDescending("it.Favorite")
		.orderBy("it.SortOrder")
		.orderBy("it.Value");
	}

	static getLocalized(tableName: string | $data.EntitySet<any, any>, language: string = null, filterExpression: string = null, filterParameters: {} = null): Promise<any[]> {
		return HelperLookup.getLocalizedQuery(tableName, language, filterExpression, filterParameters)
		.toArray()
		.then(function (lookups) {
			return HelperLookup.initializeArray(tableName, lookups);
		});
	}

	static async getLocalizedArrayMap(tableName: string | $data.EntitySet<any, any>, language: string = null, filterExpression: string = null, filterParameters: {} = null, target: {} = null): Promise<any> {
		let result = await this.getLocalized(tableName, language, filterExpression, filterParameters);
		return HelperLookup.mapLookups(result, target);
	}

	static async getLocalizedArrayMaps(lookupMap: LookupType, language: string = null): Promise<any> {
		const queries = [];
		Object.keys(lookupMap).forEach(function (key) {
			const target = lookupMap[key];
			if (!target.$tableName || (target.$array && !target.$lazy) || !window.database[target.$tableName]) {
				return;
			}
			if (target.$lazy) {
				const keys = Object.keys(target).filter((x) => !x.startsWith("$"));
				const keysWithoutValues = keys.filter(key => target[key] === null);
				if (keysWithoutValues.length > 0) {
					queries.push({
						tableName: target.$tableName,
						key: key,
						queryable: HelperLookup.getLocalizedQuery(target.$tableName, language, "it.Key in keys", {keys: keysWithoutValues}),
						method: "toArray"
					});
				}
			} else {
				queries.push({
					tableName: target.$tableName,
					key: key,
					queryable: HelperLookup.getLocalizedQuery(target.$tableName, language, target.$filterExpression, target.$filterParameters),
					method: "toArray"
				});
			}
		});
		if (!queries.length) {
			return;
		}
		let results = await window.database.batchExecuteQuery(queries)
		results.forEach(function (lookups, i) {
			const tableName = queries[i].tableName;
			const key = queries[i].key;
			const result = HelperLookup.initializeArray(tableName, lookups);
			HelperLookup.mapLookups(result, window.ko.unwrap(lookupMap[key]));
		});
		return lookupMap;
	}
	static mapLookups(lookups: any, target?: any): any {
		const map = lookups.reduce(function (m, entry) {
			m[entry.Key] = entry;
			return m;
		}, target || {});
		if (map["$array"] && !map["$lazy"] === true) {
			throw new Error("lookup contains a key '$array'");
		}
		map["$array"] = lookups;
		return map;
	}
	static setDefaultLanguage(language: string): void {
		HelperLookup.defaultLanguage = language;
	}

	static toArray(obj: {}, sortProperty: string): any[] {
		let result = Object.keys(obj).map(function (x) { return obj[x]; });
		if (sortProperty) {
			result = result.sort((a, b) => {
				if (a[sortProperty] > b[sortProperty]) {
					return -1;
				} else if (a[sortProperty] > b[sortProperty]) {
					return 1;
				}
				return 0;
			});
		}
		return result;
	}

	static getDefaultLookupValueSingleSelect<T>(lookupMap: any, dbDefaultKey: T = null): T {
		if (lookupMap.$array.length === 1) {
			return lookupMap.$array[0].Key;
		}
		const favorites = lookupMap.$array.filter((x) => x.Favorite);
		if (favorites.length === 1) {
			return favorites[0].Key;
		}
		if (favorites.length > 1) {
			return favorites.reduce(function (prev, current) {
				return (prev.SortOrder < current.SortOrder) ? prev : current
			}).Key;
		}
		const dbDefault = lookupMap[dbDefaultKey];
		return dbDefault ? dbDefaultKey : null;
	}

	static getLookupColor(lookups: any, key: string | KnockoutObservable<string>): string {
		if (lookups) {
			const unwrappedKey = ko.unwrap(key);
			if (unwrappedKey !== null && unwrappedKey !== undefined) {
				const value = lookups[unwrappedKey];
				if (value && ko.unwrap(value.Color)) {
					return ko.unwrap(value.Color);
				}
			}
		}
		return "#9E9E9E";
	}

	static getLookupValue(lookup: any, key: string | number): string {
		if (lookup) {
			const unwrappedKey = ko.unwrap(key);
			if (unwrappedKey !== null && unwrappedKey !== undefined) {
				const value = lookup[unwrappedKey];
				if (value) {
					return ko.unwrap(value.Value);
				}
			}
		}
		return "";
	}

	static queryLookupValue(lookupName: string, key: string, language: string) {
		return HelperLookup.getLanguage(language).then(function (lang) {
			const result = $.Deferred();
			HelperLookup.getLookupQuery(lookupName, key, lang)
			.single(it => true)
			.then(function (value) {
				result.resolve(value);
			})
			// @ts-ignore
			.fail(function () {
				result.resolve(key);
			});
			return result.promise();
		});
	}

	static parseLookupFullName(fullName: string): any {
		if (fullName === "Crm.Library.Globalization.Language"){
			return { fullName, plugin: "Main", pluginFullName: "Main", model: "Language", table: "Main_Language" };
		}
		let match = /(?<plugin>.*?)(\.Model\.Lookups\.|\.Model\.Lookup\.|\.Lookups\.|\.Model\.)(?<model>.*)/.exec(fullName);
		let pluginFullName = match.groups.plugin; 
		let plugin = pluginFullName.replace(/\./g, "");
		let model = match.groups.model;
		let table = plugin + "_" + model;
		return {
			fullName, plugin, pluginFullName, model, table
		};
	}

	static batchQueryLookupValue(options: any, language: string): any {
		return HelperLookup.getLanguage(language).then(function (lang) {
			const queries = options.map(function (o) {
				return {
					queryable: HelperLookup.getLookupQuery(o.lookupName, o.key, lang),
					method: "single"
				};
			});
			return window.database.batchExecuteQuery(queries);
		});
	}

	static mapLookupForSelect2Display(lookup: any): Select2AutoCompleterResult {
		return {
			id: lookup.Key,
			item: lookup,
			text: lookup.Value
		};
	}

	static queryDefaultLookupKey(lookupName: string, defaultKey: string): Promise<string> {
		return HelperLookup.getLocalizedQuery(lookupName)
			.take(1)
			.toArray()
			.then(function(results){
				if (results.length > 0){
					return results[0].Key;
				}
				return defaultKey;
			})
	}

	static queryLookup(lookupName: string | $data.EntitySet<any, any> | $data.Queryable<any>, key: string = null, filterExpression: string = null, filterParameters: {} = null): $data.Queryable<any> {
		if (!filterExpression && !filterParameters && key) {
			const filterExpressions: string[] = [];
			filterParameters = {};
			const words = window.Helper.String.parseWords(key);
			words.forEach((word, i) => {
				filterExpressions.push("it.Value.toLowerCase().contains(this.word" + i + ")");
				filterParameters["word" + i] = word;
			});
			filterExpression = filterExpressions.join(" && ");
			return HelperLookup.getLocalizedQuery(lookupName, null, filterExpression, filterParameters);

		} else if (filterExpression && filterParameters) {
			return HelperLookup.getLocalizedQuery(lookupName, null, filterExpression, filterParameters);
		} else {
			return HelperLookup.getLocalizedQuery(lookupName);
		}
	}
	static getLookupByKeyQuery(lookupName: string, key: string): $data.Queryable<any> {
		return HelperLookup.getLocalizedQuery(lookupName, null, Array.isArray(key) ? "it.Key in this.Key" : "it.Key === this.Key", { Key: key });
	}

	static getAutocompleteOptions(tableName: string): Select2AutoCompleterOptions {
		return {
			customFilter: HelperLookup.queryLookup,
			table: tableName,
			mapDisplayObject: HelperLookup.mapLookupForSelect2Display,
			getElementByIdQuery: HelperLookup.getLookupByKeyQuery
		};
	}

	static getMissingLookupText(lookupTypeTranslationKey: string): string {
		return HelperString.getTranslatedString("MissingLookup").replace("{0}", HelperString.getTranslatedString(lookupTypeTranslationKey));
	}

	static filterLookupWithParent(query: $data.Queryable<any>, term: string, parent: any): $data.Queryable<any> {
		query = query.filter("it.Language === this.language", { language: (document.getElementById("meta.CurrentLanguage") as HTMLMetaElement).content });
		if (term) {
			query = window.Helper.String.contains(query, term, ["Value.toLowerCase()"]);
		}
		if (parent) {
			parent = parent.Value || parent;
			query = query.filter("it.Language === this.language && it.ParentName == this.parent", { language: (document.getElementById("meta.CurrentLanguage") as HTMLMetaElement).content, parent: parent });
		}
		return query
			.orderByDescending("it.Favorite")
			.orderBy("it.SortOrder")
			.orderBy("it.Value");
	}
}



