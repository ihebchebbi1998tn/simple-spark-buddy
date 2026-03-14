;(function() {
	// add active class for opened items
	$(document).on("show.bs.collapse", ".collapse", function(e) {
		$(this).closest(".panel").find(".panel-heading").first().addClass("active");
	}).on("hide.bs.collapse", ".collapse", function(e) {
		if (!$.contains(this, e.target)) {
			$(this).closest(".panel").find(".panel-heading").first().removeClass("active");
		}
	});
	// hide alerts instead of removing from dom
	$(document).on("click", "[data-hide]", function() {
		$(this).closest("." + $(this).attr("data-hide")).trigger("close.bs.alert").hide().trigger("closed.bs.alert");
		return false;
	});
	// show floating button in iOS when
	if ((navigator.userAgent.match(/(iPad|iPhone|iPod).*/g))) {
		var trigger = function () {
			$(this).trigger("hide.bs.dropdown");
		};
		// ... modal closes
		$(document).on("hidden.bs.modal", trigger);
		// ... sweet alert closes
		$(document).on("blur", ".showSweetAlert", trigger);
		// ... genericlist searched
		$(document).on("genericlist.searched", trigger);
	}

	// fixes popovers for ios
	if ((navigator.userAgent.match(/(iPad|iPhone|iPod).*/g))) {
		$(document).on("click touchstart", "a[data-toggle='popover'][data-trigger='focus']", function() {
			// @ts-ignore
			$("a[data-toggle='popover'][data-trigger='focus']").not(this).popover("hide");
			// @ts-ignore
			$(this).popover("show");
			return false;
		});
		$(document).on("click touchstart", function() {
			// @ts-ignore
			$("a[data-toggle='popover'][data-trigger='focus']").popover("hide");
		});
	}

	// add padding to scroll containers so datepicker overflows become visible
	$(document).on("dp.show dp.update", ".mCSB_container .date-picker", function() {
		$(this).parents(".mCSB_container").css("padding-bottom", $(this).parent().find(".bootstrap-datetimepicker-widget").height() + "px");
	});
	$(document).on("dp.hide", ".mCSB_container .date-picker", function() {
		$(this).parents(".mCSB_container").css("padding-bottom", "0");
	});
})();
window.scrollToSelector = function(selector) {
	setTimeout(function() {
		var element = $(selector).first();
		if (element.length === 0) {
			return;
		}
		window.scroll(window.scrollX, element.offset().top - $("#header").height());
	});
}