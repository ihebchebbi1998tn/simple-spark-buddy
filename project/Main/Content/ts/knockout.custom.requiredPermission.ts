;
(function(ko) {
	function requiredPermissionInit(element, valueAccessor, allBindings, viewModel, bindingContext, invert) {
		var didDisplayOnLastUpdate = false;
		var savedNodes;
		ko.computed(function() {
			var rawValue = valueAccessor();
			var permissionName = rawValue.name ? ko.unwrap(rawValue.name) : ko.unwrap(rawValue).split("::")[1];
			var permissionGroup = rawValue.group ? ko.unwrap(rawValue.group) : ko.unwrap(rawValue).split("::")[0];
			var debug = ko.bindingHandlers.requiredPermission.debug();
			var shouldDisplay = window.AuthorizationManager.isAuthorizedForAction(permissionGroup, permissionName);
			if (invert === true) {
				shouldDisplay = !shouldDisplay;
			}
			var isFirstRender = !savedNodes;
			var needsRefresh = isFirstRender || false || (shouldDisplay !== didDisplayOnLastUpdate);
			if (needsRefresh) {
				if (isFirstRender && ko.computedContext.getDependenciesCount()) {
					// @ts-ignore
					savedNodes = ko.utils.cloneNodes(ko.virtualElements.childNodes(element), true);
				}
				if (shouldDisplay) {
					if (!isFirstRender) {
						// @ts-ignore
						ko.virtualElements.setDomNodeChildren(element, ko.utils.cloneNodes(savedNodes));
					}
					ko.applyBindingsToDescendants(bindingContext, element);
				} else {
					ko.virtualElements.emptyNode(element);
				}
				didDisplayOnLastUpdate = shouldDisplay;
			}
			if (debug) {
				var nodes = element instanceof Comment ? ko.virtualElements.childNodes(element) : [element];
				nodes.forEach(function(node) {
					$(node).addClass(invert === true ? "no-required-permission" : "required-permission");
					// @ts-ignore
					$(node).tooltip({ placement: "top", title: permissionGroup + ": " + permissionName });
				});
			}
		}, null, { disposeWhenNodeIsRemoved: element });
		return { 'controlsDescendantBindings': true };
	};
	ko.bindingHandlers.noRequiredPermission = {
		init: function(element, valueAccessor, allBindings, viewModel, bindingContext) {
			return requiredPermissionInit(element, valueAccessor, allBindings, viewModel, bindingContext, true);
		}
	};
	ko.bindingHandlers.requiredPermission = {
		init: function(element, valueAccessor, allBindings, viewModel, bindingContext) {
			return requiredPermissionInit(element, valueAccessor, allBindings, viewModel, bindingContext, false);
		}
	};
	// @ts-ignore
	ko.expressionRewriting.bindingRewriteValidators.noRequiredPermission = false; // Can't rewrite control flow bindings
	// @ts-ignore
	ko.expressionRewriting.bindingRewriteValidators.requiredPermission = false; // Can't rewrite control flow bindings
	ko.virtualElements.allowedBindings.noRequiredPermission = true;
	ko.virtualElements.allowedBindings.requiredPermission = true;
	ko.bindingHandlers.requiredPermission.debug = ko.observable(false);
})(window.ko);