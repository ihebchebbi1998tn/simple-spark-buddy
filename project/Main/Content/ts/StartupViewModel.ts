import { DefaultViewModel } from "./DefaultViewModel";
import { namespace } from "./namespace";
import { HelperString } from "./helper/Helper.String";
import { HelperUrl } from "./helper/Helper.Url";
import { HelperCulture } from "./helper/Helper.Culture";
import Log from "../ts/Logger";
import { HelperDatabase } from "./helper/Helper.Database";
import type {WakeLockSentinel} from "../@types";

export class StartupViewModelStep {
	cancel?: Function;
	cancellable: KnockoutObservable<boolean>;
	completedItems?: KnockoutObservable<number>;
	completion?: KnockoutObservable<number>;
	description: KnockoutComputed<string>;
	details?: KnockoutObservable<string>;
	detailsCompletion?: KnockoutObservable<number>;
	once?: boolean;
	run: Function;
	parallelizable?: number;
	priority: number;
	progress?: Function;
	status: KnockoutObservable<string>;
	then?: Function;
	totalItems?: KnockoutObservable<number>;
	callback?: Function;
	subCompletedItems?: KnockoutObservable<number>;
	subTotalItems?: KnockoutObservable<number>;
}

export class HomeStartupViewModel extends DefaultViewModel {

	redirectUrl = ko.observable<string>("/Main/Dashboard/IndexTemplate");
	cancellable = ko.observable<boolean>(false);
	steps = ko.observableArray<StartupViewModelStep>([]);
	errorMessage = ko.observable<string>(null);
	showErrorDetails = ko.observable<boolean>(false);
	wakeLock: WakeLockSentinel = null;
	
	constructor() {
		super();

		let waitForApplicationCachePromise = null;
		const onlineStatusStep: StartupViewModelStep = {
			cancellable: ko.observable(false),
			description: ko.pureComputed( () => HelperString.getTranslatedString( "OnlineStatus")),
			run: function () {
				// @ts-ignore
				const cache = window.jakeCache || (navigator.userAgent.indexOf("AppBoundDomains") === -1 ? window.applicationCache : null);
				if (cache && $("html").attr("manifest")) {
					waitForApplicationCachePromise = waitForApplicationCachePromise || window.Crm.Offline.Bootstrapper.waitForApplicationCache();
					const d = $.Deferred();
					waitForApplicationCachePromise.progress(d.resolve).then(d.resolve).fail(d.reject);
					return d.promise();
				}
				return $.ajax({
					type: "HEAD",
					url: HelperUrl.resolveUrl("~/Main/Account/Login"),
					cache: false
				});
			},
			status: ko.observable(null),
			priority: 990,
			parallelizable: 1
		};
		this.steps.push(onlineStatusStep);


		const serverConnectionStep: StartupViewModelStep = {
			cancellable: ko.observable(true),
			description: ko.pureComputed(HelperString.getTranslatedString.bind(null, "ServerConnection")),
			// @ts-ignore
			run: window.Crm.SignalR.establishConnection,
			status: ko.observable(null),
			priority: 985,
			parallelizable: 1
		};
		this.steps.push(serverConnectionStep);

		// @ts-ignore
		const cache = window.jakeCache || (navigator.userAgent.indexOf("AppBoundDomains") === -1 ? window.applicationCache : null);
		if (cache && $("html").attr("manifest")) {
			const applicationDataStep: StartupViewModelStep = {
				cancellable: ko.observable(false),
				completedItems: ko.observable(null),
				completion: ko.observable(null),
				description: ko.pureComputed(HelperString.getTranslatedString.bind(null, "ApplicationFiles")),
				progress: function (progress, file, files) {
					this.completion(progress);
					this.completedItems(file);
					this.totalItems(files);
				},
				run: () => {
					waitForApplicationCachePromise = waitForApplicationCachePromise || window.Crm.Offline.Bootstrapper.waitForApplicationCache();
					return waitForApplicationCachePromise.always(() => {
						this.cancellable(true);
					});
				},
				status: ko.observable(null),
				totalItems: ko.observable(null),
				priority: 980
			};
			this.steps.push(applicationDataStep);
		}

		const initializeAuthorizationManager: StartupViewModelStep = {
			cancellable: ko.observable(false),
			description: ko.pureComputed(HelperString.getTranslatedString.bind(null, "InitializePermissions")),
			once: true,
			run: () => {
				const d = $.Deferred();
				window.AuthorizationManager.initPromise().then(d.resolve);
				Log.debug("Authorization manager successfully initialized", );
				return d.promise();
			},
			status: ko.observable(null),
			priority: 680
		};
		this.steps.push(initializeAuthorizationManager);

		const loadSettingsStep: StartupViewModelStep = {
			cancellable: ko.observable(false),
			description: ko.pureComputed(HelperString.getTranslatedString.bind(null, "LoadSettings")),
			once: true,
			run: window.Crm.Offline.Bootstrapper.initializeSettings,
			status: ko.observable(null),
			priority: 970,
			parallelizable: 2
		};
		this.steps.push(loadSettingsStep);

		const loadLicensingStep: StartupViewModelStep = {
			cancellable: ko.observable(false),
			description: ko.pureComputed(window.Helper.String.getTranslatedString.bind(null, "LoadLicenseData")),
			run: window.Crm.Offline.Bootstrapper.loadLicensing,
			status: ko.observable(null),
			priority: 968
		};
		this.steps.push(loadLicensingStep);
		
		const loadTranslationsStep: StartupViewModelStep = {
			cancellable: ko.observable(false),
			description: ko.pureComputed(HelperString.getTranslatedString.bind(null, "LoadTranslations")),
			once: true,
			run: function () {
				let d = $.Deferred();
				window.Crm.Offline.Bootstrapper.initializeTranslations().then(function () {
					HelperCulture.initialize().then(d.resolve).catch(d.reject);
				}).fail(d.reject);
				return d.promise();
			},
			status: ko.observable(null),
			priority: 965,
			parallelizable: 2
		};
		this.steps.push(loadTranslationsStep);

		const initDatabaseStep: StartupViewModelStep = {
			cancellable: ko.observable(false),
			description: ko.pureComputed(HelperString.getTranslatedString.bind(null, "InitDatabase")),
			once: true,
			run: function() {
				let d = $.Deferred();
				window.Helper.Database.initialize().then(d.resolve).catch(d.reject);
				return d.promise();
			},
			status: ko.observable(null),
			priority: 960
		};
		this.steps.push(initDatabaseStep);

		const initDatabaseModel: StartupViewModelStep = {
			cancellable: ko.observable(false),
			description: ko.pureComputed(window.Helper.String.getTranslatedString.bind(null, "InitDatabase")),
			once: true,
			run: function() {
				// initialize observable classes
				// @ts-ignore
				window.database._storageModel.map(x => x._LogicalType.create().asKoObservable());
				return $.Deferred().resolve().promise();
			},
			status: ko.observable(null),
			priority: 779 // should be after DatabaseMigration and DataSynchronization
		};
		this.steps.push(initDatabaseModel);

		window.addEventListener('storage', (event) => {
			if (event.key == HelperDatabase.getStoragePrefix() + 'logout-event') {
				window.location.href = HelperUrl.resolveUrl("~/Main/Account/Login?returnUrl=" + location.pathname + encodeURIComponent(location.hash));
			}
		});

		const initRulesStep: StartupViewModelStep = {
			cancellable: ko.observable(false),
			description: ko.pureComputed( () => HelperString.getTranslatedString("InitRules")),
			once: true,
			run: () => {
				const d = $.Deferred();
				fetch(HelperUrl.resolveUrl("~/Main/Model/GetRules"), {
					cache: "force-cache"
				})
					.then( (response) => response.json())
					.then( (rules) => {
						rules.forEach(entityRules => {
							entityRules.Value.forEach((rule) => {
								window.ko.validationRules.add(entityRules.Key, function (entity) {
									let property = entity[rule.PropertyName];
									if (!property && window.ko.unwrap(entity.ExtensionValues)) {
										property = entity.ExtensionValues()[rule.PropertyName];
									}
									if (property) {
										if (rule.RuleClass === 2) {
											window.ko.validation.addRule(property, {
												rule: "required",
												message: HelperString.getTranslatedString("RuleViolation.Required").replace("{0}", HelperString.getTranslatedString(rule.PropertyNameReplacementKey || rule.PropertyName)),
												params: true,
												propertyName: rule.PropertyName
											});
										} else if (rule.RuleClass === 3) {
											window.ko.validation.addRule(property, {
												rule: "digit",
												message: HelperString.getTranslatedString("RuleViolation.Digit").replace("{0}", HelperString.getTranslatedString(rule.PropertyNameReplacementKey || rule.PropertyName)),
												params: true,
												propertyName: rule.PropertyName
											});
										} else if (rule.RuleClass === 4) {
											window.ko.validation.addRule(property, {
												rule: "number",
												message: HelperString.getTranslatedString("RuleViolation.Number").replace("{0}", HelperString.getTranslatedString(rule.PropertyNameReplacementKey || rule.PropertyName)),
												params: true,
												propertyName: rule.PropertyName
											});
										} else if (rule.RuleClass === 5) {
											window.ko.validation.addRule(property, {
												rule: "pattern",
												message: HelperString.getTranslatedString("RuleViolation.Format").replace("{0}", HelperString.getTranslatedString(rule.PropertyNameReplacementKey || rule.PropertyName)),
												params: rule.RegularExpression,
												propertyName: rule.PropertyName
											});
										} else if (rule.RuleClass === 8) {
											window.ko.validation.addRule(property, {
												rule: "minLength",
												message: HelperString.getTranslatedString("RuleViolation.MinLength").replace("{0}", HelperString.getTranslatedString(rule.PropertyNameReplacementKey || rule.PropertyName)),
												params: rule.MaxLength,
												propertyName: rule.PropertyName
											});
										} else if (rule.RuleClass === 9) {
											window.ko.validation.addRule(property, {
												rule: "maxLength",
												message: HelperString.getTranslatedString("RuleViolation.MaxLength").replace("{0}", HelperString.getTranslatedString(rule.PropertyNameReplacementKey || rule.PropertyName)),
												params: rule.MaxLength,
												propertyName: rule.PropertyName
											});
										} else if (rule.RuleClass === 13) {
											window.ko.validation.addRule(property, {
												rule: "min",
												message: HelperString.getTranslatedString("RuleViolation.NotNegative").replace("{0}", HelperString.getTranslatedString(rule.PropertyNameReplacementKey || rule.PropertyName)),
												params: 0,
												propertyName: rule.PropertyName
											});
										} else {
											Log.error("Unknown RuleClass " + rule.RuleClass);
										}
									}
								});
							});
						});
					})
					.then(d.resolve);
				return d.promise();
			},
			status: ko.observable(null),
			priority: 973,
			parallelizable: 2
		};
		this.steps.push(initRulesStep);
	}
	
	cancel(): void {
		let firstCancellableStep;
		this.steps().forEach(function (step) {
			if (step.cancellable() === true && step.status() !== "success") {
				if (!firstCancellableStep) {
					firstCancellableStep = step;
				}
				step.status("cancelled");
			}
		});
		if (firstCancellableStep && firstCancellableStep.cancel) {
			firstCancellableStep.cancel();
		}
	}

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		if ("wakeLock" in navigator && !document.hidden) {
			try {
				this.wakeLock = await navigator.wakeLock.request("screen");
			} catch (e) {
				window.Log.info("Requesting WakeLock failed/denied", e);
			}
		}
		this.initFromMainMenuViewModel()
		this.initFromAccountUserProfileViewModel()
		this.initFromTaskTopMenuViewModel();
		
		$('#menu-trigger').addClass("hidden");
		if (!!params && !!params.redirectUrl) {
			this.redirectUrl(params.redirectUrl);
		}
		if (HomeStartupViewModel.initialized) {
			this.steps.remove((step) => {
				return step.once === true;
			});
		}
		const sorted = this.steps().sort((s1, s2) => s2.priority - s1.priority);
		const grouped: Array<Array<StartupViewModelStep>> = [];
		for (let i = 0; i < sorted.length; i++) {
			const step = sorted[i];
			if (i > 0 && sorted[i - 1].parallelizable && step.parallelizable && sorted[i - 1].parallelizable === step.parallelizable) {
				grouped[grouped.length - 1].push(step);
			} else {
				grouped.push([step]);
			}
		}
		
		// @ts-ignore
		async.eachSeries(grouped, (group, groupCallback) => {
			// @ts-ignore
			async.each(group, (step, callback) => {
				step.callback = function () {
					callback?.();
					callback = null;
				}
				if (step.status() === "cancelled") {
					step.callback();
					return;
				}
				step.status("loading");
				step.cancel = function () {
					step.callback();
				};
				step.run()
					.progress((step.progress || function () { }).bind(step))
					.then((step.then || function () { }).bind(step))
					.then(function () {
						step.status("success");
					})
					.then(function() {
						step.callback();
					})
					.fail((e) => {
						if (e === 200) {
							window.location.href = HelperUrl.resolveUrl("~/Main/Account/Login?returnUrl=" + location.pathname + encodeURIComponent(location.hash));
						} else if (e === 0) {
							step.status("error");
							this.cancel();
							step.callback();
						} else {
							step.status("error");
							var errorMessage = JSON.stringify(e);
							if (errorMessage === "{}") {
								var e2 = {};
								Object.getOwnPropertyNames(e).forEach(function (p) {
									e2[p] = e[p];
								});
								errorMessage = JSON.stringify(e2);
							}
							window.Log.error(errorMessage);
							var $alert = $('div.alert-dismissable');
							if (!!$alert) {
								$alert.show();
							}
							this.errorMessage(errorMessage);
						}
					});
			},  () => {
				groupCallback();
			});
		}, () =>{
			setTimeout(function() { document.dispatchEvent(new Event("Initialized")) });
			if ($(".profile-pic").length > 0 && !HomeStartupViewModel.initialized) {
				HelperDatabase.registerEventHandlers({}, {
					Main_User: {
						"afterCreate": this.refreshProfile,
						"afterUpdate": this.refreshProfile
					},
					Main_Site: {
						"afterCreate": this.refreshProfileMenu,
						"afterUpdate": this.refreshProfileMenu
					},
				});
			}
			HomeStartupViewModel.initialized = true;
			this.hideStartupPage();
			$('#menu-trigger').removeClass("hidden");
		});
	}

	static initialized = false;

	dispose(): void {
		if (this.wakeLock !== null){
			this.wakeLock.release();
			this.wakeLock = null;
		}
	}
	retry(): void {
		window.location.reload();
	}

	getRedirectUrl(): string {
		let redirectUrl = this.redirectUrl();
		if (localStorage["lastPage"] != null) {
			return HelperUrl.resolveUrl(localStorage["lastPage"]);
		}
		else {
			return HelperUrl.resolveUrl(redirectUrl);
		}
	}
	
	hideStartupPage(): void {
		const url = this.getRedirectUrl();
		// @ts-ignore
		window.database.cacheReset();
		window.database.stateManager.reset();
		window.history.replaceState(null, null, window.location.origin + window.location.pathname + "#" + url);
		$(document).trigger("hashchange");
	}
	
	async refreshProfile(): Promise<void> {
		const currentUserName = window.Helper.User.getCurrentUserName();
		const user = await window.database.Main_User.include("expandAvatar").find(currentUserName);
		const avatar = user.Avatar;
		let avatarUrl = HelperUrl.resolveUrl("~/Plugins/Main/Content/img/avatar.jpg");
		if (avatar !== null) {
			const blob = HelperString.base64toBlob(avatar);
			avatarUrl = window.URL.createObjectURL(blob);
		}
		window.Helper.Database.saveToLocalStorage("avatar", avatar);
		window.Helper.Database.saveToLocalStorage("displayName", window.Helper.User.getDisplayName(user));
		window.Helper.Database.saveToLocalStorage("email", user.Email);
		if (window.Main.Settings.UseActiveDirectoryAuthenticationService) {
			window.Helper.Database.saveToLocalStorage("adName", user.AdName);
		}
		window.Helper.User.currentUser = user;
		$(".img > div", $(".profile-pic")).css("background-image", "url(" + avatarUrl + ")");
		$(".profile-info #user-display-name").text(window.Helper.User.getDisplayName(user));
	};

	async refreshProfileMenu(table?: any, site?: Main.Rest.Model.Main_Site): Promise<void> {
		if (!site) {
			site = await window.database.Main_Site.GetCurrentSite().first();
		}
		const logo = site.ExtensionValues.MaterialLogoBase64;
		let logoUrl = HelperUrl.resolveUrl("~/static-dist/Main/style/img/profile-menu.png");
		if (logo !== null) {
			const blob = HelperString.base64toBlob(logo);
			logoUrl = window.URL.createObjectURL(blob);
		}

		$("a", $(".profile-menu")).first().css("background", "url(" + logoUrl + ") no-repeat center center").css("background-size", "100%");
		$("header .logo a").text(site.Name);
	}
	
	initFromAccountUserProfileViewModel(): void {
		const initUserProfilePic: StartupViewModelStep = {
			cancellable: ko.observable(false),
			description: ko.pureComputed(HelperString.getTranslatedString.bind(null, "InitCurrentUser")),
			run: () => {
				const d = $.Deferred();
				this.refreshProfile()
					.then( () => this.refreshProfileMenu() )
					.then(d.resolve);
				return d.promise();
			},
			status: ko.observable(null),
			priority: 590,
			parallelizable: 4
		};

		if ($(".profile-pic").length > 0) {
			this.steps.push(initUserProfilePic);
		}
	}
	
	initFromMainMenuViewModel(): void {
		const $sidebar = $("#sidebar");
		if ($sidebar.length) {
			let viewModel;
			const element = $sidebar[0];
			const context = ko.contextFor(element);
			if (context) {
				viewModel = context.$root;
				viewModel.loading(true);
			} else {
				viewModel = new window.Main.ViewModels.MainMenuViewModel();
			}
			const initMainMenuMenu: StartupViewModelStep = {
				cancellable: window.ko.observable(false),
				description: window.ko.pureComputed( () => HelperString.getTranslatedString("MainMenu")),
				run: () => {
					viewModel.loading(false);
					if (!context) {
						window.ko.applyBindings(viewModel, element);
					}
					return $.Deferred().resolve().promise();
				},
				status: window.ko.observable(null),
				priority: 0
			};
			this.steps.push(initMainMenuMenu);
		}
	}
	
	initFromTaskTopMenuViewModel() {
		const initTaskTopMenu: StartupViewModelStep = {
			cancellable: ko.observable(false),
			description: ko.pureComputed(() => HelperString.getTranslatedString("InitializeTaskTopMenuViewModel")),
			run: () => {
				if (!window.database.Crm_Task || !window.AuthorizationManager.isAuthorizedForAction("Task", "Index")) {
					return $.Deferred().resolve().promise();
				}
				const d = $.Deferred();
				const $container = $("#tasks-top-menu");
				if ($container.length) {
					const element = $container[0];
					const context = ko.contextFor(element);
					if (context) {
						context.$root.refresh().then(d.resolve);
					} else {
						const viewModel = new window.Crm.ViewModels.TaskTopMenuViewModel();
						viewModel.init().then(function() {
							ko.applyBindings(viewModel, element);
						}).then(d.resolve);
					}
				}
				return d.promise();
			},
			status: ko.observable(null),
			priority: 570,
			parallelizable: 4
		};
		this.steps.push(initTaskTopMenu);
	}
	
}

namespace("Main.ViewModels").HomeStartupViewModel = HomeStartupViewModel;


export class HomeStartupDialogViewModel extends HomeStartupViewModel {
	constructor() {
		super();
	}
}

namespace("Main.ViewModels").HomeStartupDialogViewModel = HomeStartupDialogViewModel