import {namespace} from "./namespace";
import {Breadcrumb} from "./breadcrumbs";

interface IncompleteLookup {
	Key: string,
	MissingLanguages: string[]
}
export class LookupDetailsListIndexViewModel extends window.Main.ViewModels.GenericListViewModel<$data.Entity, $data.Entity> {
	private lookupType: any;
	lookupProperties = ko.observableArray<Main.Rest.Model.LookupProperty[]>([]);
	exceptions = [{Name: "DynamicFormLocalization", HasCustomKey: true, CustomKeyProperty: "Id"}];
	incompleteLookups: IncompleteLookup[] = [];
	languages: any;
	canOpenDetailsModal: KnockoutObservable<boolean> = ko.observable(false);

	constructor() {
		super(null, ["Favorite", "SortOrder"], ["DESC", "ASC"]);
	}

	getLookupPropertyContext(lookupProperty: Main.Rest.Model.LookupProperty, lookup: any): { Name: string, PropertyTypeName: string, Value: any } {
		return {
			Name: lookupProperty.Name,
			PropertyTypeName: lookupProperty.PropertyTypeName,
			Value: lookupProperty.Extension ? lookup.ExtensionValues()[lookupProperty.Name] : lookup[lookupProperty.Name]
		};
	}

	isIncomplete(key: string): boolean {
		return !!this.incompleteLookups.find(l => l.Key === key);
	}

	getMissingLanguages(key: string): string {
		return this.incompleteLookups.find(l => l.Key === key).MissingLanguages.join(", ");
	}

	findIncompleteLookups(lookups: Array<any>): void {
		let groupedLookups = lookups.reduce(function (res, lookup) {
			res[lookup.Key] = res[lookup.Key] || [];
			res[lookup.Key].push(lookup);
			return res;
		}, {});

		Object.getOwnPropertyNames(groupedLookups).forEach(key => {
			let lookupGroup = groupedLookups[key];
			let missingLanguages = this.languages.$array.filter(lang => {
				return lookupGroup.map(l => l.Language).indexOf(lang.Key) === -1 && lang.Key !== null && lang.IsSystemLanguage;
			}).map(x => x.Value);
			if (missingLanguages.length > 0) {
				this.incompleteLookups.push({Key: key, MissingLanguages: missingLanguages});
			}
		});
	}

	applyFilters(query: $data.Queryable<any>): $data.Queryable<any> {
		let exception = this.exceptions.find(x => x.Name === this.lookupType.Name);
		let keyFilter = null;
		if (exception && exception.HasCustomKey && exception.CustomKeyProperty) {
			if (this.filters.Key && this.filters.Key()) {
				keyFilter = this.filters.Key;
				delete this.filters.Key;
			}
		}
		query = super.applyFilters(query).filter("it.Language === this.language", {language: this.currentUser().DefaultLanguageKey})
		if (keyFilter) {
			this.filters.Key = keyFilter;
		}
		return query;
	}

	async initItems(items: any[]): Promise<any[]> {
		let exception = this.exceptions.find(x => x.Name === this.lookupType.Name);
		if (exception && exception.HasCustomKey && exception.CustomKeyProperty) {
			let KeyProperty = exception.CustomKeyProperty;
			if (this.filters.Key && this.filters.Key()) {
				items = items.filter(item => {
					return item[KeyProperty]().indexOf(this.filters.Key().Value) !== -1;
				});
			}
			items.forEach(item => {
				item.Key = ko.observable(item[KeyProperty]());
			});
		}
		return items;
	}

	getLookupColor(lookup: any): string {
		if (lookup.Color) {
			return lookup.Color;
		}
		return "#9E9E9E"
	}

	async addMissingLookups(items: Array<any>): Promise<KnockoutObservableArray<any>> {
		let viewModel = this;
		let additionalProperties = this.lookupType.LookupProperties.filter(p => !p.Hidden && ["CreateUser", "ModifyUser", "IsActive", "Language", "CreateDate", "ModifyDate", "Value", "Key", "Favorite", "SortOrder"].indexOf(p.Name) === -1).map(x => x.Name);
		let uniqueKeys = [...new Set(items.map(x => x.Key()))];
		let localizedKeys = [...new Set(items.filter(x => x.Language() == viewModel.currentUser().DefaultLanguageKey).map(x => x.Key()))];
		if (uniqueKeys.length === localizedKeys.length && uniqueKeys.every(el => localizedKeys.indexOf(el) !== -1)) {
			await window.database[viewModel.entityType].filter("it.Language === this.language", {language: this.currentUser().DefaultLanguageKey}).orderByDescending("it.Favorite").orderBy("it.SortOrder").orderBy("it.Key").take(viewModel.pageSize).toArray(viewModel.items);
			return viewModel.items;
		}
		let missingKeys = uniqueKeys.filter(x => localizedKeys.indexOf(x) === -1);
		missingKeys.forEach(function (key) {
			let lookup = window.database[viewModel.entityType][viewModel.entityType].create();
			lookup.Key = key;
			lookup.Language = viewModel.currentUser().DefaultLanguageKey;
			lookup.Value = window.Helper.String.getTranslatedString('Unspecified');
			for (const propertyName of additionalProperties) {
				let item = items.find(i => i.Key() === key);
				let isExtension = !item[propertyName];
				if (isExtension && item.ExtensionValues()) {
					lookup.ExtensionValues[propertyName] = item.ExtensionValues()[propertyName]();
				} else {
					lookup[propertyName] = item[propertyName]();
				}
			}
			window.database.add(lookup);
		});

		await window.database.saveChanges();
		await window.database[viewModel.entityType].filter("it.Language === this.language", {language: this.currentUser().DefaultLanguageKey}).orderByDescending("it.Favorite").orderBy("it.SortOrder").orderBy("it.Key").take(viewModel.pageSize).toArray(viewModel.items);
		return viewModel.items;
	}

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		this.lookupType = await window.database.GetLookupType(id);
		this.entityType = window.Helper.Lookup.parseLookupFullName(id).table;
		this.pageTitle = window.Helper.String.getTranslatedString(window.Helper.Lookup.parseLookupFullName(id).table);
		await super.init(id, params);
		this.lookupProperties(this.lookupType.LookupProperties.filter(p => !p.Hidden && ["CreateUser", "ModifyUser", "IsActive", "Language", "CreateDate", "ModifyDate", "Value", "Key", "Favorite", "SortOrder"].indexOf(p.Name) === -1))
		await this.setBreadcrumbs();
		if (this.exceptions.map(x => x.Name).indexOf(this.lookupType.Name) === -1) {
			let arr = await window.database[this.entityType].orderByDescending("it.Favorite").orderBy("it.SortOrder").orderBy("it.Key").toArray();
			await this.addMissingLookups(arr.map(x => x.asKoObservable()));
			this.languages = await window.Helper.Lookup.getLocalizedArrayMap("Main_Language");
			this.findIncompleteLookups(arr);
		}
		this.canOpenDetailsModal((this.exceptions.map(x => x.Name).indexOf(this.lookupType.Name) === -1) || this.lookupType.IsEditable);
	}

	async refreshLookupCache(): Promise<void> {
		await window.Main.ViewModels.LookupListIndexViewModel.prototype.refreshLookupCache.call(this);
	}

	async remove(lookup: any): Promise<void> {
		try {
			await window.Helper.Confirm.confirmDelete();
		} catch (e) {
			return;
		}
		this.loading(true);

		let isUsed = await window.database.IsLookupUsed(lookup.Key(), this.lookupType.FullName);
		if (isUsed) {
			this.loading(false);
			window.swal(window.Helper.String.getTranslatedString("Error"), window.Helper.String.getTranslatedString("LookupDeletionDenied"), "error");
			return;
		}
		
		let lookups = await window.database[this.entityType].filter("it.Key === this.key", {key: lookup.Key()}).toArray();
		lookups.forEach(lookup => window.database.remove(lookup));
		try {
			await window.database.saveChanges();
		} catch (e) {
			this.loading(false);
			let errorMessage = (e as Error).message;
			let parsedErrorMessage = window.Helper.String.tryExtractErrorMessageValue(errorMessage) ?? errorMessage;
			lookups.forEach(lookup => window.database.detach(lookup));
			window.swal(window.Helper.String.getTranslatedString("Error"), parsedErrorMessage, "error"); 
		}
	}

	async setBreadcrumbs(): Promise<void> {
		await window.breadcrumbsViewModel.setCustomBreadcrumbs([
			new Breadcrumb(window.Helper.String.getTranslatedString("Lookups"), "WebAPI::Lookup", "#/Main/LookupList/IndexTemplate"),
			new Breadcrumb(this.pageTitle, null, window.location.hash)
		]);
	}
}

namespace("Main.ViewModels").LookupDetailsListIndexViewModel = LookupDetailsListIndexViewModel;