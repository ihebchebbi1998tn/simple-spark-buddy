import {namespace} from "./namespace";

(function($) {
	const app = $.sammy("#content", function () {
		let lastState = null;
		this.around(async function (callback) {
			let result = true;
			if (window.history.state !== null) {
				result = await checkPendingChanges();
				if (!result && lastState !== null) {
					window.history.replaceState({}, "", lastState.path);
				}
				if (result) {
					lastState = window.history.state;
				}
			}
			if (result) {
				progress.start();
				window.Helper.Database.clearTrackedEntities();
				updateMainMenu();
				closeMainMenu();
				closeDropDownMenu();
				closeModal();
				callback();
			}
		});

		const dummyViewModel = function () {
			this.init = function () {
				return Promise.resolve();
			};
		};

		function clearContainer() {
			$("#content .container").each(function(i, element) {
				const context = ko.contextFor(element);
				const viewModel = context ? context.$root : null;
				if (viewModel) {
					if (viewModel.dispose) {
						viewModel.dispose();
					}
					if (ko.isObservable(viewModel.tabs)) {
						const tabs = ko.unwrap(viewModel.tabs);
						Object.keys(tabs).forEach(function(key) {
							const tab = ko.unwrap(tabs[key]);
							if (tab && tab.dispose && tab.dispose !== viewModel.dispose) {
								tab.dispose();
							}
						});
					}
				}
				emptyDomNode(element);
			});
		}
		function focusOnInitialInput() {
			if (/(iPad|iPhone|iPod)/.test(navigator.userAgent)) {
				return;
			}
			const selector = $(".modal.in").length > 0 ? ".modal.in" : "#content";
			const visibleInputs = $(selector).find("input:visible, select:visible, textarea:visible");
			const $inputsWithInitialFocusAttribute = visibleInputs.filter("[data-initial-focus]");
			if ($inputsWithInitialFocusAttribute.length > 0) {
				$inputsWithInitialFocusAttribute.first().trigger("focus");
			} else if(visibleInputs.length > 0 && ($(visibleInputs[0]).is("input") || $(visibleInputs[0]).is("textarea")) && !$(visibleInputs[0]).hasClass("date-picker") && !$(visibleInputs[0]).val()) {
				$(visibleInputs[0]).trigger("focus");
			}
		}
		async function apply(template, context, plugin, controller, action, id) {
			const routeId = currentRouteId = new Date().getTime();
			if (action.indexOf("Template") !== -1) {
				action = action.substr(0, action.indexOf("Template"));
			}
			$("#content").css("visibility", "hidden");
			const viewModelName = plugin + ".ViewModels." + controller + action + "ViewModel";
			const viewModelConstructor = namespace(viewModelName);
			const viewModel = typeof (viewModelConstructor) === "function" ? new viewModelConstructor() : new dummyViewModel();
			viewModel.plugin = plugin;
			viewModel.controller = controller;
			viewModel.action = action;
			id = id || context.params.id;
			id = $.isNumeric(id) && parseInt(id).toString() === id ? parseInt(id) : id;
			window.Log.debug("Initializing viewmodel " + viewModelName + "...");
			attachProgressToViewModel(viewModel);
			clearContainer();
			
			try {
				await viewModel.init(id, context.params)
				if (routeId !== currentRouteId) {
					window.Log.debug("Initialized viewmodel " + viewModelName + " obsolete, skipped applying bindings.");
					if (viewModel.dispose) {
						viewModel.dispose();
					}
					return;
				}
				window.scrollTo(0, 0);
				app.swap(template, () => {});
				addTabViewModels(context.$element(), viewModel);
				window.Log.debug("Initialized viewmodel " + viewModelName + ", applying bindings...");
				window.ko.applyBindings(viewModel, $(".container", context.$element()[0])[0]);
				setTimeout(function() {
					if (!!viewModel.errors && !!viewModel.errors.showAllMessages) {
						viewModel.errors.showAllMessages(false);
					}
					initRightNav(viewModel, context.$element());
					initTopMenu(viewModel, context.$element());
					$("#content").css("visibility", "visible");
					if (viewModel.action === 'Create') {
						focusOnInitialInput();
					}
					const tabName = context.params ? context.params.tab : null;
					initTabs(context.$element(), tabName);
					if (viewModel.loading) {
						viewModel.loading(false);
					}
					if (loadings.every(x => x() === false)) {
						window.requestAnimationFrame(progress.end.bind(progress));
					}
				});
			} catch (e) {
				handleViewModelInitFail(viewModel, viewModelName, e, id, context.params);
			}
		}

		// apply modal viewmodels to modals
		$(document).on("show.bs.modal", "#xlModal, #lgModal, #modal, #smModal", async function (event) {
			// @ts-ignore
			const route = $.isPlainObject(event.relatedTarget) ? event.relatedTarget.route : $(event.relatedTarget).attr("data-route");
			if (!route) {
				return;
			}
			const routeParts = route.split("/");
			// @ts-ignore
			const url = window.Sammy.EventContext.prototype.getUrl({
				plugin: routeParts[0],
				controller: routeParts[1],
				action: routeParts[2]
			});
			if (routeParts[2].indexOf("Template") !== -1) {
				routeParts[2] = routeParts[2].substr(0, routeParts[2].indexOf("Template"));
			}
			let id = null;
			let params = {};
			if (routeParts[3]) {
				const paramsRoutePart = routeParts.slice(3).join("/");
				const ix = paramsRoutePart.indexOf("?");
				if (ix >= 0) {
					id = paramsRoutePart.substr(0, ix);
					// @ts-ignore
					params = app._parseQueryString(paramsRoutePart.substr(ix));
				} else {
					id = paramsRoutePart;
				}
			}
			id = $.isNumeric(id) && parseInt(id).toString() === id ? parseInt(id) : id;
			const $oldModalContent = $(".modal-content", this);
			const $modalContent = $(document.createElement("div")).addClass("modal-content");
			// @ts-ignore
			const context = ko.contextFor(event.relatedTarget);
			// @ts-ignore
			const viewModel = $.isPlainObject(event.relatedTarget) ? event.relatedTarget.viewModel : (context ? context.$root : null);
			$modalContent.hide();
			progress.start();
			let result = await $.get(url);
			$modalContent.html(result);
			const modalViewModelName = routeParts[0] + ".ViewModels." + routeParts[1] + routeParts[2] + "ModalViewModel";
			const modalViewModelConstructor = namespace(modalViewModelName);
			if (typeof modalViewModelConstructor === "function") {
				const modalViewModel = new modalViewModelConstructor(viewModel);
				attachProgressToViewModel(modalViewModel);
				try {
					await modalViewModel.init(id, params)
					if (modalViewModel.loading) {
						modalViewModel.loading(false);
					}
					$oldModalContent.replaceWith($modalContent);
					window.ko.applyBindings(modalViewModel, $modalContent.get(0));
					$modalContent.show();
					window.requestAnimationFrame(focusOnInitialInput);
					$(document).one("hide.bs.modal", "#xlModal, #lgModal, #modal, #smModal", function (event) {
						const element = $modalContent.get(0);
						disposeElement(element);
					});
					if (loadings.every(x => x() === false)) {
						window.requestAnimationFrame(progress.end.bind(progress));
					}
				} catch (e) {
					handleViewModelInitFail(modalViewModel, modalViewModelName, e, id, params);
				}
			} else {
				$oldModalContent.replaceWith($modalContent);
				window.ko.applyBindings(viewModel, $modalContent.get(0));
				$modalContent.show();
				if (loadings.every(x => x() === false)) {
					window.requestAnimationFrame(progress.end.bind(progress));
				}
			}
			const zIndex = 1010 + (10 * $('.modal:visible').length);
			$(this).css('z-index', zIndex);
			queueMicrotask(() => {
				$('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
			});
		});

		// apply right nav viewmodels to right nav
		$(document).on("sidebar.opened", "#right-nav", async function (event, relatedTarget) {
			const route = $.isPlainObject(relatedTarget) ? relatedTarget.route : $(relatedTarget).attr("data-route");
			if (!route) {
				return;
			}
			const routeParts = route.split("/");
			// @ts-ignore
			const url = window.Sammy.EventContext.prototype.getUrl({
				plugin: routeParts[0],
				controller: routeParts[1],
				action: routeParts[2]
			});
			if (routeParts[2].indexOf("Template") !== -1) {
				routeParts[2] = routeParts[2].substr(0, routeParts[2].indexOf("Template"));
			}
			let id = null;
			let params = {};
			if (routeParts[3]) {
				const ix = routeParts[3].indexOf("?");
				if (ix >= 0) {
					id = routeParts[3].substr(0, ix);
					// @ts-ignore
					params = app._parseQueryString(routeParts[3].substr(ix));
				} else {
					id = routeParts[3];
				}
			}
			id = $.isNumeric(id) && parseInt(id).toString() === id ? parseInt(id) : id;
			let $oldRightNav = $(".mCSB_container", this);
			if ($oldRightNav.length === 0) {
				$oldRightNav = $(this);
			}
			const $rightNavContent = $(document.createElement("div")).addClass("right-nav-content");
			const context = ko.contextFor(relatedTarget);
			const viewModel = $.isPlainObject(relatedTarget) ? relatedTarget.viewModel : (context ? context.$data : null);
			$oldRightNav.html("");
			$rightNavContent.hide();
			progress.start();
			let result = await $.get(url);
			$rightNavContent.html(result);
			const rightNavViewModelName = routeParts[0] + ".ViewModels." + routeParts[1] + routeParts[2] + "RightNavViewModel";
			const RightNavViewModelConstructor = namespace(rightNavViewModelName);
			if (typeof RightNavViewModelConstructor === "function") {
				const rightNavViewModel = new RightNavViewModelConstructor(viewModel);
				attachProgressToViewModel(rightNavViewModel);
				try {
					await rightNavViewModel.init(id, params);
					if (rightNavViewModel.loading) {
						rightNavViewModel.loading(false);
					}
					$oldRightNav.append($rightNavContent);
					window.ko.applyBindings(rightNavViewModel, $rightNavContent.get(0));
					$rightNavContent.show();
					$(document).one("sidebar.closed", "#right-nav", function (event) {
						const element = $rightNavContent.get(0);
						disposeElement(element);
					});
					if (loadings.every(x => x() === false)) {
						window.requestAnimationFrame(progress.end.bind(progress));
					}
				} catch (e) {
					handleViewModelInitFail(rightNavViewModel, rightNavViewModelName, e, id, params);
				}
			} else {
				$oldRightNav.append($rightNavContent);
				window.ko.applyBindings(viewModel, $rightNavContent.get(0));
				$rightNavContent.show();
				if (loadings.every(x => x() === false)) {
					window.requestAnimationFrame(progress.end.bind(progress));
				}
			}
		});

		function errorHandler(e) {
			window.Log.error("sammy route failed to load template: status " + e.status + ": " + e.statusText);
			if (e.status === 404) {
				app.runRoute("get", "#/Main/Error/Template/404");
			}
		}

		let currentRouteId = 0;

		this.get("#/:plugin/:controller/:action/:id", async function (context) {
			const routeId = currentRouteId = new Date().getTime();
			try {
				let template = await context.load(context.getUrl(context.params));
				if (routeId !== currentRouteId) {
					window.Log.debug("Route id changed, aborting route");
					return;
				}
				await updateBreadcrumbs();
				await apply(template, context, context.params.plugin, context.params.controller, context.params.action, context.params.id);
			} catch (e) {
				errorHandler(e);
			}
		});
		this.get("#/:plugin/:controller/:action", async function (context) {
			const routeId = currentRouteId = new Date().getTime();
			try {
				let template = await context.load(context.getUrl(context.params));
				if (routeId !== currentRouteId) {
					window.Log.debug("Route id changed, aborting route");
					return;
				}
				await updateBreadcrumbs();
				await apply(template, context, context.params.plugin, context.params.controller, context.params.action, null);
			} catch (e) {
				errorHandler(e);
			}
		});
		this.get("#/:plugin/:controller", async function (context) {
			const routeId = currentRouteId = new Date().getTime();
			try {
				let template = await context.load(context.getUrl(context.params));
				if (routeId !== currentRouteId) {
					window.Log.debug("Route id changed, aborting route");
					return;
				}
				await updateBreadcrumbs();
				await apply(template, context, context.params.plugin, context.params.controller, "Index", null);
			} catch (e) {
				errorHandler(e);
			}
		});
		this.notFound = function (verb, path) {
			const materialIndex = window.Helper.Url.resolveUrl("~/Main/Home/MaterialIndex");
			if (path !== materialIndex) {
				window.Log.warn("404 Not Found: " + verb + " " + path);
			}
		};
	});
	$.ajaxSetup({ cache: true });
	// @ts-ignore
	const progress = new Mprogress({template: 3, parent: "#header"}).start();
	// @ts-ignore
	$(document).hammer({ domEvents: true });

	// handle swipe for main menu
	$(document)
		.on("swiperight",
			function(e) {
				if ($("aside.toggled").length === 1) {
					return;
				}
				// @ts-ignore
				const gesture = (e.originalEvent || e).gesture;
				const x = (gesture.srcEvent.x || gesture.srcEvent.pageX);
				const startX = x - gesture.deltaX;
				const swipeStartPositionUnknown = x === 0;
				const swipedFromEdge = (swipeStartPositionUnknown || startX < 30);
				if (swipedFromEdge && !$("#menu-trigger").hasClass("open")) {
					$("#menu-trigger").trigger("click");
				}
			});
	$("#sidebar").on("swipeleft", function(e) {
		if ($("#menu-trigger").hasClass("open")) {
			$("#menu-trigger").trigger("click");
		}
	});

	// handle trigger of sidebars
	let preventClosePromise = null;
	const isClosePrevented = function ($element) {
		const deferred = $.Deferred();
		const closingEvent = $.Event("sidebar.closing");
		$element.trigger(closingEvent, deferred);
		if (closingEvent.isDefaultPrevented() === false) {
			deferred.resolve();
		}
		preventClosePromise = deferred.promise();
		preventClosePromise.always(function () {
			setTimeout(function () {
				preventClosePromise = null;
			}, 0);
		});
		return preventClosePromise;
	};
	$("#main, .top-menu:not(.section-menu)").on("click", "[data-trigger]", function(e) {
		e.preventDefault();
		e.stopPropagation();
		const x = $(this).data("trigger");
		const $x = $(x);
		const triggerOpen = !$(this).hasClass("open") && $(this).closest(x).length === 0;
		if (triggerOpen) {
			$x.addClass("toggled");
			$(".open[data-trigger=\"" + x + "\"]").removeClass("open");
			$(this).addClass("open");
			$("#header").addClass("sidebar-toggled");
			$x.trigger("sidebar.opened", $(this));
			closeTopMenuDropDownMenu();
		} else {
			isClosePrevented($x).then(function() {
				$x.removeClass("toggled");
				$(".open[data-trigger=\"" + x + "\"]").removeClass("open");
				$("#header").removeClass("sidebar-toggled");
				$x.trigger("sidebar.closed");
			});
		}
	});

	let mouseDown = null;
	$(document).on("mousedown", function(e) {
		mouseDown = e;
	});
	$(document).on("mouseup", function(e) {
		if (preventClosePromise || !mouseDown) {
			return;
		}
		if ($(e.target).parents(".modal").length > 0 || $(e.target).parents(".modal-footer").length > 0) {
			return;
		}
		if ($(e.target).parents(".select2-container").length > 0) {
			return;
		}
		const $toggled = $(".sidebar.toggled");
		const id = $toggled.attr("id");
		const $trigger = $(e.target).closest("[data-trigger]");
		if ($toggled.length > 0 && $(e.target).closest(".toggled").length === 0 && ($trigger.length === 0 || $trigger.data("trigger") !== "#" + id) && $(mouseDown.target).closest(".toggled").length === 0) {
			isClosePrevented($toggled).then(function() {
				$(".open[data-trigger=\"#" + id + "\"]").removeClass("open");
				$toggled.removeClass("toggled");
				$("#header").removeClass("sidebar-toggled");
				$toggled.trigger("sidebar.closed");
			});
		}
		mouseDown = null;
	});

	function checkPendingChanges(): Promise<boolean> {
		return new Promise((resolve, reject) => {
			if (window.Helper.Database.hasPendingChanges()) {
				const activeModal = $(".modal:visible").last();
				if (activeModal.length > 0) {
					activeModal.hide();
				}
				showPendingChangesAlert(function (isConfirm) {
					if (activeModal.length > 0) {
						activeModal.show();
					}
					if (isConfirm) {
						resolve(true);
					} else {
						resolve(false);
					}
				});
			} else {
				resolve(true);
			}
		})
	}

	function showPendingChangesAlert(callback) {
		window.swal({
			title: "",
			text: window.Helper.String.getTranslatedString("PendingChangesWarningText"),
			type: "warning",
			showCancelButton: true,
			confirmButtonText: window.Helper.String.getTranslatedString("Discard"),
			cancelButtonText: window.Helper.String.getTranslatedString("Cancel")
		}, callback);
	}

	function updateMainMenu() {
		$("#sidebar li.active").removeClass("active");
		$("#sidebar li a.active").removeClass("active");
		const $activeLink = $($("#sidebar a").filter(function (i, x) {
			const $link = $(x);
			const href = $link.attr("href");
			const location = window.location.hash;
			const isActive = href !== "#" && location.indexOf(href) !== -1;
			return isActive;
		}).toArray().sort(function (a, b) {
			return $(b).attr("href").length - $(a).attr("href").length;
		}).slice(0, 1));
		if ($activeLink.length === 1) {
			const $subMenu = $activeLink.parents(".sub-menu");
			const isSubMenuItem = $subMenu.length === 1;
			if (isSubMenuItem) {
				$activeLink.addClass("active");
				$subMenu.addClass("active");
			} else {
				$activeLink.parent().addClass("active");
			}
		}
		$(".sub-menu.active:not(.toggled) ul").show();
		$(".sub-menu.active:not(.toggled)").addClass("toggled");
		$(".sub-menu").each(function(i, x) {
			if ($("li", x).length === 0) {
				$(x).hide();
			} else {
				$(x).show();
			}
		});
	}

	function closeMainMenu() {
		if ($("#header").hasClass("sidebar-toggled")) {
			$("body").removeClass("modal-open");
			$(".sidebar.toggled").removeClass("toggled");
			$("#header").removeClass("sidebar-toggled");
			$("#menu-trigger").removeClass("open");
		}
	}

	function closeDropDownMenu() {
		$('.dropdown.open').click();
	}

	function closeTopMenuDropDownMenu() {
		$('.top-menu .dropdown.open').click();
	}

	function closeModal() {
		$(".modal:visible").last().modal("hide");
	}

	const loadings = [];
	let animationFrameRequestId = 0;

	function attachProgressToViewModel(viewModel) {
		if (!!viewModel.loading && window.ko.isObservable(viewModel.loading)) {
			viewModel.loading(true);
			if (loadings.indexOf(viewModel.loading) === -1) {
				loadings.push(viewModel.loading);
				const dispose = viewModel.dispose;
				viewModel.dispose = function() {
					loadings.splice(loadings.indexOf(viewModel.loading), 1);
					if (dispose) {
						dispose.apply(this, arguments);
					}
				};
			}
			viewModel.loading.subscribe(function(loading) {
				if (loadings.some(x => x())) {
					window.cancelAnimationFrame(animationFrameRequestId);
					animationFrameRequestId = window.requestAnimationFrame(progress.start.bind(progress));
				} else {
					window.cancelAnimationFrame(animationFrameRequestId);
					animationFrameRequestId = window.requestAnimationFrame(progress.end.bind(progress));
				}
			});
		}
	}
	// @ts-ignore
	window.Sammy.EventContext.prototype.getUrl = function getUrl(params) {
		let url = "/";
		if (!!params.plugin) {
			url += params.plugin + "/";
		}
		url += params.controller || "Home";
		url += "/";
		url += params.action || "Index";
		return window.Helper.Url.resolveUrl("~" + url);
	}

	function saveParamsToUrl(params) {
		let url = window.location.href;
		if (url.indexOf("?") !== -1) {
			url = url.substr(0, url.indexOf("?"));
		}
		params = window.Helper.Object.removeEmptyParams(params);
		url += "?" + $.param(params, true);
		window.history.replaceState(null, null, url);
	}

	function initRightNav(viewModel, container) {
		if (!viewModel.$$rightNav) {
			viewModel.$$rightNav = $("#right-nav", container).detach().html();
		}
		const rightNav = viewModel.$$rightNav;

		const $container = $(document.createElement("div"));
		$container.html(rightNav);
		const $rightNav = $("#right-nav .mCSB_container").length > 0 ? $("#right-nav .mCSB_container") : $("#right-nav");
		$rightNav.html("");
		$rightNav.append($container);

		const contentElement = $rightNav.children().last()[0];
		window.ko.applyBindings(viewModel, contentElement);
	}

	function addTabViewModels(container, viewModel) {
		if (!viewModel || !window.ko.isWriteableObservable(viewModel.tabs)) {
			return;
		}
		$("ul.tab-nav", container).each(function(i, tabNav) {
			const tabs = $("a[role=tab]", tabNav);
			tabs.each(function (j, tab) {
				const tabId = tab.getAttribute("aria-controls");
				viewModel.tabs()[tabId] = window.ko.observable(null);
			});
		});
	}

	function initTabs(container, tabName) {
		$("ul.tab-nav", container).each(function (i, tabNav) {
			const tabs = $("a[role=tab]", tabNav);
			let tab = tabs.first();
			if (tabName) {
				const initTab = tabs.filter("[href='#" + tabName + "']").first();
				if (initTab.length > 0) {
					tab = initTab;
				}
			} else {
				const activeTab = tabs.filter("[aria-expanded='true']").first();
				if (activeTab.length > 0) {
					tab = activeTab;
				}
			}
			if (!!tab) {
				$(tab).tab("show");
			}
		});
	}

	function initTopMenu(viewModel, container, append: boolean = false) {
		const exludeFromDropdownList = ["Filter", "ServiceReport"];

		if (!viewModel.$$topMenu) {
			const $topmostListItems = $("ul.top-menu:not(.generic-list-actions) > li:not(.keep-in-place)", container).detach();
			$topmostListItems.each(function (i, li) {
				viewModel.$$topMenu += li.outerHTML;
			});			
		}
		const topMenu = viewModel.$$topMenu;

		const $newTopMenu = $(document.createElement("div"));
		$newTopMenu.html(topMenu);
		const $topMenuActionsContainer = $("#header .top-menu-actions");
		const $topMenuOverflowMenu = $("#header ul.menu-more-vert");
		if (append !== true) {
			$topMenuActionsContainer.html("");
			$("li:not(.main-action)", $topMenuOverflowMenu).each(function (i, li) {
				$(li).remove();
			});
		}
		const $lis = $newTopMenu.children("li");
		$lis.each(function(i, li) {
			const $li = $(li);
			const actionTitle = li.title ? li.title : $li.data("original-title");
			const isExludedFromDropdown = exludeFromDropdownList.some((el) => window.Helper.getTranslatedString(el) === actionTitle);
			const isOverflowAction = !!$li.text().trim() && !$li.hasClass("dropdown") && !$li.hasClass("prevent-overflow");

			const isDivider = $li.hasClass("divider");
			if (isOverflowAction || isDivider) {
				$topMenuOverflowMenu.append($li);
			} else if (window.client.isMobileDevice() && !isExludedFromDropdown) {
				$topMenuOverflowMenu.prepend($li);
				let actionLabelElement = $("<span></span>").text(actionTitle);
				$($li.children()[0]).append(actionLabelElement);
			} else {
				const actionLabelElement = $("<div class='small'></div>").text(actionTitle);
				$($li.children()[0]).append(actionLabelElement);
				$topMenuActionsContainer.append($li);
			}
			window.ko.applyBindings(viewModel, li);
		});

		function hideFirstOrLastDividers() {
			let result = false;
			const overflowMenuPoints = $topMenuOverflowMenu.find("li:not([style*='display: none'])");
			if (overflowMenuPoints.length === 0) {
				return result;
			}
			const $firstMenuPoint = $(overflowMenuPoints[0]);
			if ($firstMenuPoint.hasClass("divider")) {
				$firstMenuPoint.hide();
				// @ts-ignore
				overflowMenuPoints.splice(0, 1);
				result = true;
			}
			const $lastMenuPoint = $(overflowMenuPoints[overflowMenuPoints.length - 1]);
			if ($lastMenuPoint.hasClass("divider")) {
				$lastMenuPoint.hide();
				// @ts-ignore
				overflowMenuPoints.splice(overflowMenuPoints.length - 1, 1);
				result = true;
			}
			return result;
		}

		while (hideFirstOrLastDividers()) { }

		toggleOverflowMenu();
		const observer = new window.MutationObserver(toggleOverflowMenu);
		observer.observe($topMenuOverflowMenu[0], { childList: true, subtree: true });
	}


	function handleViewModelInitFail(viewModel, viewModelName, e, id?: string, params?: any) {
		window.Log.error("failed initializing viewmodel " + viewModelName, id, params, e);
		app.runRoute("get", "#/Main/Error/Template/500");
		if (viewModel && viewModel.dispose) {
			viewModel.dispose();
		}
	}

	window.breadcrumbsViewModel = null;

	$(document).one('Initialized', (e) => {
		window.breadcrumbsViewModel = new window.Main.ViewModels.MenuBreadcrumbsViewModel($("breadcrumbs")[0]);
	});

	$(document).on("keydown", "form input", function (e) {
		if (e.keyCode === 13) {
			e.preventDefault();
			const container = $(this).closest(".modal-content")[0] || $(this).closest(".right-nav-content")[0] || $(this).closest(".container")[0];
			const $saveAction = !container ? null : $(container).find("button[type=submit],button[data-bind*=save],button[data-bind*=submit],#apply-generic-list-filter");
			if ($saveAction !== null) {
				e.target.blur();
				$saveAction.trigger("click");
			}
		}
	});

	$(document).on("keydown", function (e) {
		const keyboardContext = $(".keyboard-context:visible");

		if (keyboardContext.length > 0) {
			keyboardContext.each((i, value) => {
				const viewModel = ko.contextFor($(value)[0])?.$root;
				if (viewModel && viewModel.keyDown) {
					viewModel.keyDown(e.keyCode);
				}
			});
		}
	});

	async function updateBreadcrumbs() {
		if (window.breadcrumbsViewModel) {
			await window.breadcrumbsViewModel.update();
		}
	}

	// apply tab viewmodels to tabs
	let $mostRecentTab;
	$(document).on('show.bs.tab', async function (e): Promise<void> {
		const viewModel = ko.contextFor(e.target).$root;
		// @ts-ignore
		if (!!e.relatedTarget) {
			// @ts-ignore
			const relatedTab = $(e.relatedTarget).attr("href");
			const relatedTabId = $(relatedTab).attr("id");
			const $relatedTab = $(relatedTab);
			$relatedTab.hide();
		}
		const tab = $(e.target).attr("href");
		const tabId = $(tab).attr("id") || $(e.target).attr("id");
		if (tab.indexOf("#tab-") !== 0 || !tabId) {
			return;
		}
		const $tab = $mostRecentTab = $(tab);

		function showTab() {
			if ($tab !== $mostRecentTab) {
				return;
			}
			if (viewModel.tabs) {
				initRightNav(viewModel.tabs()[tabId](), $tab);
				initTopMenu(viewModel.tabs()[tabId](), $tab);
				if (viewModel.tabs()[tabId]() !== viewModel) {
					initTopMenu(viewModel, $tab, true);
				}
			}
			$tab.fadeIn();
			if (ko.isWriteableObservable(viewModel.previousTabId) && ko.isObservable(viewModel.currentTabId)) {
				viewModel.previousTabId(ko.unwrap(viewModel.currentTabId));
			}
			if (ko.isWriteableObservable(viewModel.currentTab)) {
				viewModel.currentTab(null);
			}
			if (ko.isWriteableObservable(viewModel.currentTabId)) {
				viewModel.currentTabId(null);
			}
			if (ko.isWriteableObservable(viewModel.currentTab)) {
				viewModel.currentTab(viewModel.tabs()[tabId]());
			}
			if (ko.isWriteableObservable(viewModel.currentTabId)) {
				viewModel.currentTabId(tabId);
			}
			const params = window.Helper.Url.getCriteriaFromUrl();
			// @ts-ignore
			params.tab = tabId;
			saveParamsToUrl(params);
		}
		if (!viewModel.tabs || viewModel.tabs()[tabId]() === viewModel) {
			showTab();
			return;
		}
		$tab.hide();
		progress.start();
		if (viewModel.tabs && viewModel.tabs()[tabId]) {
			if (viewModel.tabs()[tabId]() && viewModel.tabs()[tabId]().dispose) {
				viewModel.tabs()[tabId]().dispose();
			}
			viewModel.tabs()[tabId](null);
		}
		const tabName = tabId.substring(4);
		let camelCasedTabName = tabName.replace(/-([a-z])/g, function (g) {
			return g[1].toUpperCase();
		});
		camelCasedTabName = camelCasedTabName.charAt(0).toUpperCase() + camelCasedTabName.slice(1);
		const tabViewModelName = viewModel.plugin + ".ViewModels." + viewModel.controller + viewModel.action + camelCasedTabName + "TabViewModel";
		const tabViewModelConstructor = namespace(tabViewModelName);
		if (typeof tabViewModelConstructor === "function") {
			const tabViewModel = new tabViewModelConstructor(viewModel);
			tabViewModel.parentViewModel = viewModel;
			attachProgressToViewModel(tabViewModel);
			try {
				await tabViewModel.init();
				viewModel.tabs()[tabId](tabViewModel);
				if (tabViewModel.loading) {
					tabViewModel.loading(false);
				}
				showTab();
				if (loadings.every(x => x() === false)) {
					window.requestAnimationFrame(progress.end.bind(progress));
				}
			} catch(e) {
				handleViewModelInitFail(tabViewModel, tabViewModelName, e);
			}
		} else {
			viewModel.tabs()[tabId](viewModel);
			showTab();
			window.requestAnimationFrame(progress.end.bind(progress));
		}
		return;
	});

	function emptyDomNode(domNode) {
		while (domNode && domNode.firstChild) {
			ko.removeNode(domNode.firstChild);
		}
	}
	function disposeElement(element) {
		const context = ko.contextFor(element);
		const viewModel = context ? context.$root : null;
		if (viewModel && viewModel.dispose) {
			viewModel.dispose();
		}
	}
	function toggleOverflowMenu() {
		setTimeout(function () {
			const $topMenuOverflowMenu = $("#header ul.menu-more-vert");
			const $topMenuOverflowContainer = $("#header li.menu-more-vert");
			const hasOverflowMenu = $("li:not([style*='display: none']) > a", $topMenuOverflowMenu).length > 0;
			if (hasOverflowMenu) {
				$topMenuOverflowContainer.show();
			} else {
				$topMenuOverflowContainer.hide();
			}
		}, 500);
	}

	$(document).on('hidden.bs.modal', '.modal', function () {
		$('.modal:visible').length && $(document.body).addClass('modal-open');
	});

	$(document).on("hide.bs.modal", "#xlModal, #lgModal, #modal, #smModal", function (event) {
		const element = $(".modal-content", event.target).get(0);
		//Perhaps there's a better way but for now, only checking pending changes when closing the last modal
		if (!$(element)?.data?.("keepchanges") && !$(".modal-header", element)?.data?.("keepchanges") && $('.modal:visible').length === 1 && window.Helper.Database.hasPendingChanges()) {
			event.preventDefault();
			event.stopImmediatePropagation();
			const activeModal = $(".modal:visible").last();
			activeModal.hide();
			setTimeout(function() {
					showPendingChangesAlert(function(isConfirm) {
						if (isConfirm) {
							window.Helper.Database.clearTrackedEntities();
							disposeElement(element);
							activeModal.modal("hide");
						} else {
							activeModal.show();
						}
					});
				},
				0);
			return false;
		}
		emptyDomNode(element);
		toggleOverflowMenu();
		return true;
	});
	$(document).on("sidebar.closed", "#right-nav", function(event) {
		const element = $(".right-nav-content", event.target).get(0);
		emptyDomNode(element);
	});

	$(document).on("click", "#sidebar a.active, #sidebar li.active > a", function () {
		window.location.hash = $(this).attr("href");
		$.sammy("#content").refresh();
	});

	$(function () {
		if (window.location.hash.indexOf("#/Main/Home/Startup") !== 0) {
			window.location.hash = "/Main/Home/Startup?redirectUrl=" + encodeURIComponent(window.location.hash.substr(1));
		}
		app.run();
	});

	$(document).on("click", ".generic-list-header #viewMode-setting-dropdown:has(.toggle-switch)", function (e) {
		e.stopPropagation();
	});

	$(document).on("click", ".generic-list-actions [data-view]", function (e) {
		e.preventDefault();
		const dataView = $(this).attr("data-view");
		$("[data-view]", $(this).closest("ul")).closest("li").removeClass("active");
		$(this).closest("li").addClass("active");
		// @ts-ignore
		$("full-calendar div#calendar-widget").fullCalendar("changeView", dataView);
	});

	$(document).on("show.bs.collapse", "collapsible-block .collapse", function (e) {
		const viewModel = getCollapsiblBlockViewModel(this);
		viewModel.collapsed(false);
	})

	$(document).on("hide.bs.collapse", "collapsible-block .collapse", function (e) {
		const viewModel = getCollapsiblBlockViewModel(this);
		viewModel.collapsed(true);
	});

	function getCollapsiblBlockViewModel(collapseElement) {
		const header = $(collapseElement).closest(".collapsible-block");
		return ko.contextFor(header.get(0)).$data;
	}

	$(document).on("ready", function (e) {
		const mainActions = $("#top-menu-actions .top-menu > li:not(.menu-more-vert)");
		mainActions.each(function (i, li) {
			$(li).addClass("main-action");
		});

		overflowMainActions();
	});

	$(window).on("resize", function () {
		const windowHeight = $(window).height();
		if ($("dashboard-calendar-widget").length) {
			let calendarHeight = windowHeight - 500;
			if (windowHeight > 900) {
				calendarHeight = windowHeight - 400;
			}
			// @ts-ignore
			$("dashboard-calendar-widget #calendar-widget").fullCalendar('option', 'height', calendarHeight);
		}

		overflowMainActions();
	});

	function overflowMainActions() {
		const width = $(window).width();
		const $topMenuOverflowMenu = $("#header ul.menu-more-vert");
		if (width < 900) {
			const mainActions = $("#top-menu-actions > .top-menu > li.main-action");
			mainActions.each(function (i, li) {
				const isOverflowAction = !$(li).hasClass("dropdown") && !$(li).hasClass("prevent-overflow");
				if (isOverflowAction) {
					$topMenuOverflowMenu.append($(li));
					$("a > div.small", $(li)).removeClass("small");
				}
			});
		} else {
			const mainActions = $("#header ul.menu-more-vert > li.main-action");
			mainActions.each(function (i, li) {
				$("#top-menu-actions > .top-menu > .menu-more-vert").after($(li));
				$("a > div", $(li)).addClass("small");
			});
		}
	}

})(jQuery);
