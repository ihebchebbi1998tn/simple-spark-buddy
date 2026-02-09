import {namespace} from "./namespace";

export class LookupListIndexViewModel extends window.Main.ViewModels.GenericListViewModel<Main.Rest.Model.LookupType, Main.Rest.Model.ObservableLookupType> {
	private lookupTypes: Array<Main.Rest.Model.LookupType>;

	getItemGroup(item: any): ItemGroup {
		return {
			title: this.getTranslatedName(item)[0]
		};
	}

	async getSearchResults(): Promise<Main.Rest.Model.LookupType[]> {
		let result = this.lookupTypes;
		if (this.filters.Name && this.filters.Name()) {
			const nameFilter = ko.unwrap(this.filters["Name"]);
			result = result.filter(x => this.getTranslatedName(x).toLowerCase().indexOf(nameFilter.Value.toLowerCase()) !== -1);
		}
		if (this.filters.FullName && this.filters.FullName()) {
			const filter = ko.unwrap(this.filters["FullName"]);
			result = result.filter(x => x.FullName.toLowerCase().indexOf(filter.Value.toLowerCase()) !== -1);
		}
		return result;
	}

	getTranslatedName = (item) => window.Helper.String.getTranslatedString(window.Helper.Lookup.parseLookupFullName(window.ko.unwrap(item.FullName)).table);

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		this.lookupTypes = await window.database.GetLookupTypes().toArray();
		this.lookupTypes = this.lookupTypes.sort((a, b) => this.getTranslatedName(a).localeCompare(this.getTranslatedName(b)));
		await super.init(id, params);
	}

	async refreshLookupCache(): Promise<void> {
		this.loading(true);
		await fetch(window.Helper.Url.resolveUrl("~/Main/Lookup/RefreshLookupCache"), {redirect: "manual"});
		this.showSnackbar(window.Helper.String.getTranslatedString("Success"));
		this.loading(false);
	}
}

namespace("Main.ViewModels").LookupListIndexViewModel = LookupListIndexViewModel;