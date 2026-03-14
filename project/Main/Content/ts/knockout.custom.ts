$(function() {
	const writeValueToProperty = function (property, allBindingsAccessor, key, value, checkIfDifferent = false) {
		if (!property || !ko.isWriteableObservable(property)) {
			const propWriters = allBindingsAccessor()['_ko_property_writers'];
			if (propWriters && propWriters[key])
				propWriters[key](value);
		} else if (!checkIfDifferent || property.peek() !== value) {
			property(value);
		}
	};
	// works like the selectedOptions binding (see http://knockoutjs.com/documentation/selectedOptions-binding.html) but with checkboxes, apply binding to a container containing the checkboxes
	ko.bindingHandlers.checkedOptions = {
		init: function(element, valueAccessor, allBindingsAccessor) {
			ko.utils.registerEventHandler(element, "change", function() {
				const value = valueAccessor(), valueToWrite = [];
				// @ts-ignore
				ko.utils.arrayForEach($('input[type=checkbox]', element), function(node) {
					if ($(node).prop('checked'))
						valueToWrite.push($(node).val());
				});
				// selectedOptions binding is using internal writeValueToProperty method, above method is just a copy of this method so the behavior of this binding is the same as of the selectedOptions binding
				//ko.expressionRewriting.writeValueToProperty(value, allBindingsAccessor, 'checkedOptions', valueToWrite);
				writeValueToProperty(value, allBindingsAccessor, 'checkedOptions', valueToWrite);
			});
		},
		update: function(element, valueAccessor) {
			const newValue = ko.utils.unwrapObservable(valueAccessor());
			if (newValue && typeof newValue.length == "number") {
				// @ts-ignore
				ko.utils.arrayForEach($('input[type=checkbox]', element), function(node) {
					const nodeVal = $(node).val();
					const isChecked = ko.utils.arrayFirst(newValue, function (newValueElement) {
						return window.ko.utils.unwrapObservable(newValueElement) == nodeVal;
					}) != null;
					$(node).prop('checked', isChecked);
					//ko.utils.setOptionNodeSelectionState(node, isChecked);
				});
			}
		}
	};

	ko.bindingHandlers.fadeVisible = {
		init: function (element, valueAccessor) {
			const value = valueAccessor();
			$(element).toggle(ko.unwrap(value));
		},
		update: function (element, valueAccessor) {
			const value = valueAccessor();
			ko.unwrap(value) ? $(element).fadeIn() : $(element).fadeOut();
		}
	};
});

window.ko.ItemStatus = {
	Persisted: 0,
	Draft: 1,
	Added: 2,
	Modified: 3,
	Removed: 4
};

const validationOptions = {
	insertMessages: true,
	decorateElement: true,
	errorClass: 'field-validation-error',
	grouping: {deep: true}
};
window.ko.validation.init(validationOptions);

// effects
// @ts-ignore
window.ko.effects = window.ko.effects || {};
// @ts-ignore
window.ko.effects.slideIn = function(element, index, data) {
	if (element.nodeType === 1) {
		$(element).hide();
		$(element).slideDown();
	}
};
// @ts-ignore
window.ko.effects.slideOut = function(element, index, data) {
	if (element.nodeType === 1) {
		$(element).slideUp({
			complete: function() {
				$(element).remove();
			}
		});
	}
};

(function() {
	const templateFromUrlLoader = {
		loadTemplate: function (name, templateConfig, callback) {
			if (templateConfig.url) {
				const fullUrl = window.Helper.Url.resolveUrl("~/" + templateConfig.url);
				$.get(fullUrl, function (markupString) {
					ko.components.defaultLoader.loadTemplate(name, markupString, callback);
				});
			} else {
				callback(null);
			}
		}
	};
	ko.components.loaders.unshift(templateFromUrlLoader);
	const checkPermissionLoader = {
		loadComponent: function (name, componentConfig, callback) {
			if (componentConfig.permission && !window.AuthorizationManager.currentUserHasPermission(componentConfig.permission)) {
				callback({ template: document.createElement("span") });
			} else {
				callback(null);
			}
		}
	}
	ko.components.loaders.unshift(checkPermissionLoader);
})();

(function() {
	const originalClick = ko.bindingHandlers.click;
	const onError = (element, error) => {
		window.Log.error(`Error in click handler of element ${element.outerHTML}`, error);
		window.swal(window.Helper.String.getTranslatedString("Error"), (error as Error).message || error?.toString(), "error");
	}
	ko.bindingHandlers.click = {
		init: function(element, valueAccessor, allBindings, viewModel, bindingContext) {
			let originalHandler = ko.unwrap(valueAccessor());
			let wrappedHandler = function () {
				try {
					let result = originalHandler.apply(this, arguments);
					Promise.resolve(result).catch(error => onError(element, error));
					return result;
				} catch (error) {
					onError(element, error);
					return false;
				}
			};
			originalClick.init(element, ko.computed(function() { return wrappedHandler; }), allBindings, viewModel, bindingContext);
		}
	};
})();