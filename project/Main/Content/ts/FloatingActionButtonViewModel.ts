import {namespace} from "./namespace";

export class FloatingActionButtonViewModel {
	$element: JQuery;
	href: string;
	icon: string;
	title: string;
	multiButtonMainIcon: string;
	position: string;
	showWhenDropdownIsToggled: boolean;
	showDropdownEventHandler = () => this.hide();
	hideDropdownEventHandler = () => this.show();
	inlineEditorClosedEventHandler = () => this.show()

	constructor(params, componentInfo) {
		this.$element = $(componentInfo.element);
		this.href = params.href;
		this.icon = ko.unwrap(params.icon) || "plus";
		this.title = window.Helper.String.getTranslatedString(params.title || "T_AddNew");
		this.multiButtonMainIcon = ko.unwrap(params.multiButtonMainIcon) || "plus";
		this.position = params.position || "bottom";
		this.showWhenDropdownIsToggled = params.showWhenDropdownIsToggled;
		if (this.showWhenDropdownIsToggled === undefined || this.showWhenDropdownIsToggled === null) {
			this.showWhenDropdownIsToggled = false;
		}
	}

	init() {
		if (!this.showWhenDropdownIsToggled) {
			$(document).on("show.bs.dropdown", this.showDropdownEventHandler);
			$(document).on("hide.bs.dropdown", this.hideDropdownEventHandler);
		}
		$(document).on("inlineEditorClosed", this.inlineEditorClosedEventHandler);
	};

	mfbAfterRender() {
		const lis = this.$element.find(".mfb-component__list>li").filter(function (i, li) {
			return $(li).css("display").toLowerCase() !== "none";
		});
		if (lis.length === 0) {
			this.$element.hide();
			this.observeComponentList();
			return;
		}
		let mainButton = null;
		if (lis.length === 1) {
			mainButton = lis.find("a");
		} else {
			lis.find("a").addClass("mfb-component__button--child btn-primary")
				.find("i").addClass("mfb-component__child-icon");
		}

		if (!mainButton) {
			const aParent = document.createElement("div");
			aParent.innerHTML = '<a href="#" onclick="return false;"></a>';
			mainButton = aParent.firstChild;
		}

		const $mainButton = $(mainButton);
		$mainButton.find("i").remove();
		const iconParent = document.createElement("div");
		iconParent.innerHTML = '<i class="zmdi zmdi-' + (lis.length === 1 ? this.icon : this.multiButtonMainIcon) + '"></i>';
		// @ts-ignore
		$mainButton.append(iconParent.firstChild);

		this.$element.find(".mfb-component__button--main").remove();
		$mainButton.insertBefore(this.$element.find(".mfb-component__list"))
			.addClass("mfb-component__button--main btn-primary animated bounceIn")
			.removeAttr("data-mfb-label");
		const is = $mainButton.find("i");
		let $clone;
		// @ts-ignore
		if (lis.length > 1 && window.Modernizr && window.Modernizr.touch) {
			const cloneParent = document.createElement("div");
			cloneParent.innerHTML = '<i class="zmdi zmdi-close"></i>';
			$clone = $(cloneParent.firstChild);

			const ul = this.$element.find("ul").first();
			$mainButton.on("click", function () {
				if (ul.attr("data-mfb-state") === "open") {
					ul.attr("data-mfb-state", "closed");
				} else {
					ul.attr("data-mfb-state", "open");
				}
			});
			lis.on("click", function () {
				ul.attr("data-mfb-state", "closed");
			});
			this.$element.find('[data-mfb-toggle="hover"]').attr("data-mfb-toggle", "click");
		} else {
			$clone = is.clone();
		}
		$clone.insertAfter(is)
			.addClass("mfb-component__main-icon--active");
		is.addClass("mfb-component__main-icon--resting");
		this.$element.show();
		this.observeComponentList();
	};

	observeComponentList() {
		const componentList = this.$element.find(".mfb-component__list")[0];
		let observer;
		const callback = () => {
			observer.disconnect();
			this.mfbAfterRender();
		};
		observer = new window.MutationObserver(callback);
		observer.observe(componentList, {attributes: true, childList: true, subtree: true});
	};

	show() {
		this.$element.show();
	};

	hide() {
		const activeModal = $(".modal:visible");
		if (activeModal.length === 0) {
			this.$element.hide();
		}
	};

	dispose() {
		if (!this.showWhenDropdownIsToggled) {
			$(document).off("show.bs.dropdown", this.showDropdownEventHandler);
			$(document).off("hide.bs.dropdown", this.hideDropdownEventHandler);
		}
		$(document).off("inlineEditorClosed", this.inlineEditorClosedEventHandler);
	};
}

namespace("Main.ViewModels").FloatingActionButtonViewModel = FloatingActionButtonViewModel;