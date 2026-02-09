(function (ko) {
	const originalInit = ko.bindingHandlers.text.init;
	const originalUpdate = ko.bindingHandlers.text.update;

	ko.bindingHandlers.text = {
		init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
			const val = ko.unwrap(valueAccessor());
			const wrapText = !element.classList || element.classList.length === 0 || !element.classList.contains("text-nowrap");

			if (val && val.value && typeof (val) === "object") {
				const valueUnwrapped = ko.utils.unwrapObservable(val.value);
				if (!valueUnwrapped) {
					return;
				}
				if (wrapText) {
					element.style.whiteSpace = val.whiteSpace || "pre-line";
					element.style.wordBreak = val.wordBreak || "break-word";
				}
				// Create a unique ID for the element
				const uniqueId = element.id || "content-" + Math.random().toString(36).slice(2, 9);
				element.id = uniqueId;

				// Remove any existing toggle button
				const existingToggle = document.getElementById("toggle-" + uniqueId);
				if (existingToggle) existingToggle.remove();

				// Add the toggle button if expand is enabled
				if (!val.expand) {
					if (val.limit) {
						ko.utils.setTextContent(element, valueUnwrapped.length > val.limit ?
							window.Helper.String.limitStringWithEllipses(valueUnwrapped, val.limit) : valueUnwrapped);
					} else {
						ko.utils.setTextContent(element, valueUnwrapped);
					}
					return;
				}
				val.limit = val.limit || 300;
				// Create a unique ID for the element using a counter or a more unique approach
				if (val.expand) {
					const toggle = document.createElement("a");
					toggle.id = "toggle-" + uniqueId;
					toggle.href = "javascript:void(0);";
					toggle.textContent = window.Helper.String.getTranslatedString("more");
					element.after(toggle);
				}
			} else {
				if (element.style && wrapText) {
					element.style.whiteSpace = "pre-line";
					element.style.wordBreak = "break-word";
				}
				originalInit(element, valueAccessor, allBindings, viewModel, bindingContext);
			}
		},
		update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
			const val = ko.unwrap(valueAccessor());
			if (val && val.value && typeof (val) === "object") {
				const valueUnwrapped = ko.utils.unwrapObservable(val.value || val);
				if (!valueUnwrapped) {
					return;
				}

				const toggle = document.getElementById("toggle-" + element.id);
				if (!toggle) {
					ko.utils.setTextContent(element, valueUnwrapped);
					return;
				}

				val.expand = !!val.expand;
				val.limit = val.limit || 300;
				val.collapsed = val.collapsed !== undefined ? val.collapsed : true;

				// Update toggle click handler
				const toggleClickHandler = function () {
					val.collapsed = !val.collapsed;
					toggle.textContent = val.collapsed
						? window.Helper.String.getTranslatedString("more")
						: window.Helper.String.getTranslatedString("less");
					ko.utils.setTextContent(
						element,
						val.collapsed && valueUnwrapped.length > val.limit
							? window.Helper.String.limitStringWithEllipses(valueUnwrapped, val.limit)
							: valueUnwrapped
					);
				};

				toggle.removeEventListener("click", toggleClickHandler); // Cleanup
				toggle.addEventListener("click", toggleClickHandler);

				// Set initial text and toggle visibility
				toggle.style.display = valueUnwrapped.length > val.limit ? "inline" : "none";
				ko.utils.setTextContent(
					element,
					val.collapsed && valueUnwrapped.length > val.limit
						? window.Helper.String.limitStringWithEllipses(valueUnwrapped, val.limit)
						: valueUnwrapped
				);
			} else {
				originalUpdate(element, valueAccessor, allBindings, viewModel, bindingContext);
			}
		}
	};
})(window.ko);