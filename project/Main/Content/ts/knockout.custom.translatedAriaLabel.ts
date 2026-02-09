/** If the label text is visible on screen, authors SHOULD use aria-labelledby and SHOULD NOT use aria-label. */
(function (ko) {
	const unknownTranslationString = "TranslationMissing" + Date.now();
	ko.bindingHandlers.translatedAriaLabel = {
		update: function (element: HTMLElement, valueAccessor) {
			const value = valueAccessor();
			const valueUnwrapped = ko.utils.unwrapObservable(value.key || value);
			const language = ko.utils.unwrapObservable(value.language);
			const translation = window.Helper.String.getTranslatedString(valueUnwrapped, unknownTranslationString, language);
			const translationMissing = translation === unknownTranslationString;
			if (translationMissing) {
				window.Helper.String.fetchStringTranslation(valueUnwrapped, language).then(function (fetchedTranslation) {
					element.setAttribute("aria-label", fetchedTranslation);
				});
			} else {
				element.setAttribute("aria-label", translation);
			}
		}
	};
})(window.ko);
export {};