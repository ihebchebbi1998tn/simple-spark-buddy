;(function($data: any, ko: any) {
	ko.punches.enableAll();
	// override ko.validation config defaults so the plugin can be used with the material theme
	ko.validation.configuration.insertMessages = false;
	ko.validation.configuration.errorElementClass = "has-error";
	ko.validation.registerExtenders();
	var validationGroup = ko.validation.group;
	ko.validation.group = function() {
		var result = validationGroup.apply(this, arguments);
		result.awaitValidation = function() {
			var deferred = $.Deferred();
			if (result.isValidating()) {
				var subscription = result.isValidating.subscribe(function(isValidating) {
					if (!isValidating) {
						subscription.dispose();
						deferred.resolve();
					}
				});
			} else {
				deferred.resolve();
			}
			return deferred.promise();
		}
		result.isValidating = ko.pureComputed(function() {
			return result.filter(function(x) { return x.isValidating(); }).length > 0;
		});
		result.scrollToError = function() {
			const errorElement = getFirstErrorElement();
			if (errorElement.length > 0) {
				setTimeout(function() {
					window.scroll(window.scrollX, errorElement.offset().top - $("#header").height());
				});
			}
		};
		result.switchToError = function() {
			var modalContent = $("#modal .modal-content");
			var modalTabNav = modalContent.find(".tab-nav");
			var activeTab = modalContent.find(".tab-pane:visible");
			var inactiveTabs = modalContent.find(".tab-pane").not(":visible");
			var inactiveTabsWithError = inactiveTabs.find(".has-error");
			if (inactiveTabsWithError.length > 0) {
				modalTabNav.find("li.active").removeClass("active");
				var firstInactiveTabWithError = $(inactiveTabsWithError[0]).parents(".tab-pane");
				modalTabNav.find("a[href='#" + firstInactiveTabWithError.attr("id") + "']").closest("li").addClass("active");
				activeTab.removeClass("active").hide();
				firstInactiveTabWithError.show();
			}
		};
		result.expandCollapsiblesWithErrors = function () {
			const errorElement = getFirstErrorElement();
			if (errorElement.length === 0) {
				return;
			}

			const form = errorElement.closest("form.root-form");
			const collapsibleBlocks = $("collapsible-block", form);
			const collapsiblePanels = $(".panel-collapse", form);

			if (collapsibleBlocks.length === 0 && collapsiblePanels.length === 0) {
				return;
			}

			collapsibleBlocks.each(function () {
				let errorElement = $(".has-error", this).first();
				if (errorElement.length === 0) {
					errorElement = $(".alert-danger", this).first();
				}
				if (errorElement.length > 0) {
					const header = $(".collapsible-block", this);
					const viewModel = ko.contextFor(header.get(0)).$data;
					viewModel.collapsed(false);
				}
			});

			collapsiblePanels.each(function () {
				let errorElement = $(".has-error", this).first();
				if (errorElement.length === 0) {
					errorElement = $(".alert-danger", this).first();
				}
				if (errorElement.length > 0) {
					const content = $("> div", this);
					content.addClass("in");
					content.attr("aria-expanded", "true");
					content.css("height", "");

					const toggle = $(this).prev().find('a[data-toggle="collapse"]');
					toggle.removeClass("collapsed");
					toggle.attr("aria-expanded", "true");
				}
			});
		};

		function getFirstErrorElement() {
			let errorElement = $(".has-error").first();
			if (errorElement.length === 0) {
				errorElement = $(".alert-danger").first();
			}
			return errorElement;
		}
		return result;
	}
	if (!$data) {
		return;
	}
	ko.validationRules = {
		add: function(entityName, rule) {
			ko.validationRules[entityName] = ko.validationRules[entityName] || [];
			ko.validationRules[entityName].push(rule);
		}
	};
	// @ts-ignore
	$data.Entity.prototype.addValidationRules = function() {
		if (this.outerInstance instanceof $data.KoObservableEntity) {
			if (this.context) {
				this.context.__kv_traversed = true;
			}
			var observableEntity = this.outerInstance;
			(ko.validationRules[this.getType().name] || []).forEach(function(rule) {
				rule(observableEntity);
			});
		}
	};
	var asKoObservable = $data.Entity.prototype.asKoObservable;
	$data.Entity.prototype.asKoObservable = function() {
		var observableEntity = asKoObservable.apply(this, arguments);
		var isTracked = window.database.stateManager.trackedEntities.find(function(trackedEntity) {
			return trackedEntity.data === observableEntity.innerInstance;
		});
		this.outerInstance = observableEntity;
		if (isTracked) {
			this.addValidationRules();
		}
		return observableEntity;
	};
	var trackEntity = $data.EntitySet.prototype._trackEntity;
	$data.EntitySet.prototype._trackEntity = function(entity) {
		entity.addValidationRules();
		return trackEntity.apply(this, arguments);
	};
})(window.$data, window.ko);