import {namespace} from "./namespace";

export class LookupEditModalViewModel extends window.Main.ViewModels.ViewModelBase {

	language = ko.observable<Main.Rest.Model.Lookups.Main_Language>(null);
	languages = ko.observableArray<Main.Rest.Model.Lookups.Main_Language>([]);
	lookup = ko.observable(null);
	lookups = ko.observableArray([]);
	lookupType = ko.observable<Main.Rest.Model.LookupType>(null);
	sharedLookupProperties = ko.observableArray<Main.Rest.Model.LookupProperty>([]);
	lookupProperties = ko.observableArray<Main.Rest.Model.LookupProperty>([]);
	isCustomHandledLookup = ko.observable<boolean>(false);
	table: string;

	getLookupPropertyContext(lookupProperty: Main.Rest.Model.LookupProperty, languageKey: string): { Name: string, PropertyTypeName: string, Value: any, IsCustomHandledLookup: KnockoutObservable<boolean> } {
		let lookup = this.lookups().find(x => x.Language() === languageKey);
		if (!lookup) {
			let newLookup = window.database[this.table].defaultType.create();
			newLookup.Language = languageKey;
			this.lookupType().LookupProperties.filter(p => p.Shared).forEach(sharedProperty => {
				if (sharedProperty.Extension) {
					newLookup.ExtensionValues[sharedProperty.Name] = this.lookup().ExtensionValues()[sharedProperty.Name]();
				} else {
					newLookup[sharedProperty.Name] = this.lookup()[sharedProperty.Name]();
				}
			});
			window.database.add(newLookup);
			lookup = newLookup.asKoObservable();
			this.lookups.push(lookup)
		}
		return {
			Name: lookupProperty.Name,
			PropertyTypeName: lookupProperty.PropertyTypeName,
			Value: lookupProperty.Extension ? lookup.ExtensionValues()[lookupProperty.Name] : lookup[lookupProperty.Name],
			IsCustomHandledLookup: this.isCustomHandledLookup
		};
	}

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		this.lookupType(await window.database.GetLookupType(params.fullName));
		this.sharedLookupProperties(this.lookupType().LookupProperties.filter(p => p.Shared && !p.Hidden && ["Key", "Favorite", "SortOrder", "Color"].indexOf(p.Name) === -1))
		this.lookupProperties(this.lookupType().LookupProperties.filter(p => !p.Shared && !p.Hidden && ["CreateUser", "ModifyUser", "IsActive", "Language", "CreateDate", "ModifyDate"].indexOf(p.Name) === -1))
		let currentUser = await window.Helper.User.getCurrentUser();
		this.table = window.Helper.Lookup.parseLookupFullName(params.fullName).table;
		let allLanguages = await window.Helper.Lookup.getLocalizedQuery("Main_Language").toArray();
		let languages = allLanguages.filter(x => x.IsSystemLanguage);
		let languageKeys = allLanguages.map(x => x.Key);
		this.languages(languages);
		this.language(this.languages().find(x => x.Key === currentUser.DefaultLanguageKey) || this.languages()[0]);
		if (`${id}`.length > 0) {
			let lookups = await window.database[this.table].filter("it.Key === this.key && it.Language in this.languages", {key: id, languages: languageKeys}).toArray();
			lookups.forEach(x => window.database.attachOrGet(x));
			this.lookups(lookups.map(x => x.asKoObservable()));
		} else {
			let newLookup = window.database[this.table].defaultType.create();
			newLookup.Language = this.language().Key;
			window.database.add(newLookup);
			this.lookups([newLookup.asKoObservable()])
		}
		this.lookup(this.lookups().find(x => x.Language() === currentUser.DefaultLanguageKey) || this.lookups()[0]);
		this.lookupType().LookupProperties.filter(p => !p.Extension && p.Shared).forEach(sharedProperty => {
			this.lookup()[sharedProperty.Name].subscribe(value => {
				this.lookups().forEach(lookup => {
					if (lookup !== this.lookup()) {
						lookup[sharedProperty.Name](value);
					}
				});
			});
		});
		this.lookupType().LookupProperties.filter(p => p.Extension && p.Shared).forEach(sharedExtensionProperty => {
			this.lookup().ExtensionValues()[sharedExtensionProperty.Name].subscribe(value => {
				this.lookups().forEach(lookup => {
					if (lookup !== this.lookup()) {
						lookup.ExtensionValues()[sharedExtensionProperty.Name](value);
					}
				});
			});
		});
		this.sharedLookupProperties().filter(p => p.Required).forEach(sharedLookupProperty => {
			this.lookup()[sharedLookupProperty.Name].extend({
				required: {
					params: true,
					message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", sharedLookupProperty.Name)
				}
			});
		});
		this.lookup().Key.extend({
			validation: {
				async: true,
				validator: async (key, params, callback) => {
					if (!key) {
						callback(true);
						return;
					}
					let ids = this.lookups().map(x => x.Id());
					let languages = this.languages().map(x => x.Key);
					let count = await window.database[this.table]
						.filter("it.Key === this.key && !(it.Id in this.ids) && it.Language in this.languages", {key, ids, languages})
						.count();
					callback(count === 0);
				},
				message: window.Helper.String.getTranslatedString("RuleViolation.Unique").replace("{0}", window.Helper.String.getTranslatedString("Key"))
			}
		});
	}

	async save(): Promise<void> {
		if (!window.Helper.Database.hasPendingChanges()) {
			$(".modal:visible").modal("hide");
			return;
		}
		this.loading(true);
		let errors = window.ko.validation.group(this.lookups);
		await errors.awaitValidation();
		if (errors().length > 0) {
			this.loading(false);
			errors.showAllMessages();
			return;
		}
		try {
			await window.database.saveChanges();
			$(".modal:visible").modal("hide");
		} catch (e) {
			this.loading(false);
			window.swal(window.Helper.String.getTranslatedString("Error"), (e as Error).message, "error");
		}
	}
}

namespace("Main.ViewModels").LookupEditModalViewModel = LookupEditModalViewModel;