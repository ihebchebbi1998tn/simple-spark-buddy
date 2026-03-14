(function (ko) {
	const unknownTranslationString = "TranslationMissing" + Date.now();
	ko.bindingHandlers.translatedText = {
		format: function(translation, args) {
			if (!translation || !Array.isArray(args) || args.length === 0) {
				return translation;
			}
			for (var i = 0; i < args.length; i++) {
				translation = translation.replace("{" + i + "}", ko.unwrap(args[i]));
			}
			return translation;
		},
		update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
			const value = valueAccessor();
			const valueUnwrapped = ko.utils.unwrapObservable(value.key || value);
			const language = ko.utils.unwrapObservable(value.language) || window.defaultLanguage;
			const args = ko.utils.unwrapObservable(value.args);
			const translation = window.Helper.String.getTranslatedString(valueUnwrapped, unknownTranslationString, language);
			const translationMissing = translation == unknownTranslationString;
			if (translationMissing) {
				window.Helper.String.fetchStringTranslation(valueUnwrapped, language).then(function (fetchedTranslation) {
					$(element).text(ko.bindingHandlers.translatedText.format(fetchedTranslation, args));
				});
			} else {
				$(element).text(ko.bindingHandlers.translatedText.format(translation, args));
			}
		}
	};
})(window.ko);
