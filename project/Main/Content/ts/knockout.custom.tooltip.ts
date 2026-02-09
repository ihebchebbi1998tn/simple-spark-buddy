;
(function(ko) {

	var isIOS = /iPad|iPhone|iPod/.test(navigator.platform);

	ko.bindingHandlers.tooltip = {
		defaults: { // https://getbootstrap.com/docs/3.3/javascript/#tooltips
			container: "body",
			delay: {
				show: 500, 
				hide: 100
			},
			trigger: "hover"
		},
		init: function(element, valueAccessor) {
			var value = ko.unwrap(valueAccessor());
			var options = $.extend({}, ko.bindingHandlers.tooltip.defaults, typeof value === "object" ? value : {});
			if (!value) {
				return;
			}
			$(element).tooltip(options);

			ko.utils.domNodeDisposal.addDisposeCallback(element, function() {
				$(element).tooltip("hide");
			});

			$(element).on("click",
				function () {
					$(element).tooltip("hide");
				});

			// workaround for ios https://github.com/twbs/bootstrap/issues/16028
			if (isIOS) {
				$(element).css("cursor", "pointer");
				$(element).addClass("tooltip-ios");
				$(element).on("press",
					function() {
						$(this).tooltip("show");
					});
			}
		}
	};

	if (isIOS) {
		$(document).on("touchstart",
			function(e: any) {
				$(".tooltip-ios").each(function() {
					if (!$(this).is(e.target) &&
						$(this).has(e.target).length === 0 &&
						$(".tooltip").has(e.target).length === 0) {
						$(this).tooltip("hide");
					}
				});
			});
	}

})(window.ko);