ko.bindingHandlers.money = {
	update: function (element, valueAccessor) {
		var value = valueAccessor();
		var valueUnwrapped = ko.utils.unwrapObservable(value);
		var number = parseFloat(valueUnwrapped);
		if (isNaN(number) || !number) {
			number = 0;
		}
		var formatted = window.Globalize.formatNumber(number, { maximumFractionDigits: 2, minimumFractionDigits: 2 });
		$(element).text(formatted);
	}
};
