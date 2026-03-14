import {registerComponent} from "./componentRegistrar";

registerComponent({
	componentName: "lv-actions",
	template: "Main/Template/LvActions",
	viewModel: {
		createViewModel: function (params, componentInfo) {
			const $element = $(componentInfo.element);
			const $dropdown = $element.find("ul.dropdown-menu");
			const viewModel = {
				evaluateVisible: function () {
					$element.css("visibility", "visible");
					let isVisible;
					$dropdown.children("li").each(function (i, e) {
						if (!isVisible &&
							e.innerHTML.trim() &&
							$(e).css("display") !== "none" &&
							!$(e).hasClass("divider")) {
							isVisible = true;
						}
					});
					$dropdown.children("li.divider").show();
					while ($dropdown.children("li:visible").first().hasClass("divider")) {
						$dropdown.children("li:visible").first().hide();
					}
					while ($dropdown.children("li:visible").last().hasClass("divider")) {
						$dropdown.children("li:visible").last().hide();
					}
					if (!isVisible) {
						$element.css("visibility", "hidden");
					}
					viewModel.observe();
				},
				observe: function () {
					let observer;
					const callback = function () {
						observer.disconnect();
						viewModel.evaluateVisible();
					};
					observer = new window.MutationObserver(callback);
					observer.observe($element[0], {attributes: true, childList: true, subtree: true});
				}
			};
			return viewModel;
		}
	}
});