(function (ko) {
	const unknownTranslationString = "TranslationMissing" + Date.now();
	ko.bindingHandlers.translatedTitle = {
		update: function (element: HTMLElement, valueAccessor) {
			const value = valueAccessor();
			const valueUnwrapped = ko.utils.unwrapObservable(value.key || value);
			const language = ko.utils.unwrapObservable(value.language);
			const translation = window.Helper.String.getTranslatedString(valueUnwrapped, unknownTranslationString, language);
			const translationMissing = translation === unknownTranslationString;
			if (translationMissing) {
				window.Helper.String.fetchStringTranslation(valueUnwrapped, language).then(function (fetchedTranslation) {
					element.setAttribute("title", fetchedTranslation);
				});
			} else {
				element.setAttribute("title", translation);
			}
		}
	};
})(window.ko);
export {}