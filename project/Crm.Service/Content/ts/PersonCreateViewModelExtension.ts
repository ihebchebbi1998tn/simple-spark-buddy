import {Mixin} from "ts-mixer";

export class PersonCreateViewModelExtension extends window.Crm.ViewModels.PersonCreateViewModel {
	getParentAutocompleteOptions(): Select2AutoCompleterOptions {
		if (this.parentType() === "ServiceObject") {
			return {
				table: "CrmService_ServiceObject",
				customFilter: (query, term) => {
					if (term) {
						query = window.Helper.String.contains(query, term, ["ObjectNo", "Name"]);
					}
					return query;
				},
				orderBy: ["Name"],
				joins: [{
					Selector: "Addresses",
					Operation: "filter(function(a) { return a.IsCompanyStandardAddress == true; })"
				}],
				mapDisplayObject: function (o) {
					return {id: o.Id, text: Helper.ServiceObject.getDisplayName(o), item: o};
				},
				onSelect: x => this.onSelectParent(x)
			};
		}
		return super.getParentAutocompleteOptions();
	};
}

window.Crm.ViewModels.PersonCreateViewModel = Mixin(PersonCreateViewModelExtension);