ko.bindingHandlers.lookupValue = {
	update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
		const value = valueAccessor();
		const valueUnwrapped = ko.unwrap(value);
		if (valueUnwrapped === null || valueUnwrapped === undefined) {
			$(element).text("");
			return;
		}
		const lookups = ko.unwrap(allBindingsAccessor().lookups);
		let lookupValue = ko.unwrap(allBindingsAccessor().defaultValue) || "";
		const prefix = ko.unwrap(allBindingsAccessor().prefix || "");
		const suffix = ko.unwrap(allBindingsAccessor().suffix || "");
		let selectedLookup;
		if (Array.isArray(lookups)) {
			selectedLookup = ko.utils.arrayFirst(lookups, function (lookup) {
				return valueUnwrapped === ko.unwrap(lookup.Key);
			});
		} else {
			selectedLookup = lookups[valueUnwrapped];
		}
		if (selectedLookup) {
			lookupValue = prefix + ko.unwrap(selectedLookup.Value) + suffix;
		}
		$(element).text(lookupValue);
	}
};
