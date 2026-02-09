ko.bindingHandlers.fileSize = {
	update: function (element, valueAccessor) {
		const value = valueAccessor();
		const valueUnwrapped = ko.utils.unwrapObservable(value);
		let number = parseFloat(valueUnwrapped);
		if (isNaN(number) || !number) {
			number = 0;
		}
		let formatted;
		if (number >= 1073741824) {
			formatted = window.Globalize.formatNumber(number / 1073741824, { maximumFractionDigits: 2, minimumFractionDigits: 2 }) + " GB";
		} else if (number >= 1048576) {
			formatted = window.Globalize.formatNumber(number / 1048576, { maximumFractionDigits: 2, minimumFractionDigits: 2 }) + " MB";
		} else {
			formatted = window.Globalize.formatNumber(number / 1024, { maximumFractionDigits: 2, minimumFractionDigits: 2 }) + " KB";
		}
		$(element).text(formatted);
	}
};