///<reference types="jquery"/>
///<reference types="knockout"/>

import moment from "moment";
import {FirebasePlugin as FirebasePluginType} from "../js/cordova/android/plugins/cordova-plugin-firebasex/types";

import {AccountUserProfileViewModel as AccountUserProfileViewModelType} from "../ts/AccountUserProfileViewModel";
import {AccountUserProfileDevicesTabViewModel as AccountUserProfileDevicesTabViewModelType} from "../ts/AccountUserProfileDevicesTabViewModel";
import {Breadcrumb} from "../ts/breadcrumbs";
import {
	DashboardCalendarWidgetViewModel as DashboardCalendarWidgetViewModelType
} from "../ts/DashboardCalendarWidgetViewModel";
import {DashboardIndexViewModel as DashboardIndexViewModelType} from "../ts/DashboardIndexViewModel";
import {DefaultViewModel as DefaultViewModelType} from "../ts/DefaultViewModel";
import {FloatingActionButtonViewModel as FloatingActionButtonViewModelType} from "../ts/FloatingActionButtonViewModel";
import {InlineEditorViewModel} from "../ts/InlineEditorViewModel"
import {Logger} from "log4javascript";
import {MenuBreadcrumbsViewModel} from "../ts/knockout.component.breadcrumbs";
import {namespace} from "../ts/namespace";
import swal from "sweetalert";
import {
	BackgroundServiceIndexViewModel as BackgroundServiceIndexViewModelType
} from "../ts/BackgroundServiceIndexViewModel";
import JobDetails from "../ts/knockout.component.JobDetails"
import {LogListIndexViewModel as LogListIndexViewModelType} from "../ts/LogListIndexViewModel";
import {HelperLookup} from "../ts/helper/Helper.Lookup";
import {HelperConfirm} from "../ts/helper/Helper.Confirm";
import {HelperBatch} from "../ts/helper/Helper.Batch";
import {HelperCulture} from "../ts/helper/Helper.Culture";
import {HelperDate} from "../ts/helper/Helper.Date";
import {HelperDatabase} from "../ts/helper/Helper.Database";
import {HelperDistinct} from "../ts/helper/Helper.Distinct";
import {HelperDOM} from "../ts/helper/Helper.DOM";
import {HelperNumber} from "../ts/helper/Helper.Number";
import {HelperObject} from "../ts/helper/Helper.Object";
import {HelperPosting} from "../ts/helper/Helper.Posting";
import {HelperVisibility} from "../ts/helper/Helper.Visibility";
import {HelperMap} from "../ts/helper/Helper.Map";
import {HelperMessage} from "../ts/helper/Helper.Message";
import {HelperNavigation} from "../ts/helper/Helper.Navigation";
import {SiteEditViewModel} from "../ts/SiteEditViewModel";
import {SiteThirdPartySoftwareViewModel} from "../ts/SiteThirdPartySoftwareViewModel";
import {UserCreateViewModel} from "../ts/UserCreateViewModel";
import {UserDetailsViewModel} from "../ts/UserDetailsViewModel";
import {UserGroupDetailsViewModel as UserGroupDetailsViewModelType} from "../ts/UserGroupDetailsViewModel";
import {UserGroupCreateViewModel} from "../ts/UserGroupCreateViewModel";
import {UserGroupListIndexViewModel as UserGroupListIndexViewModelType} from "../ts/UserGroupsListIndexViewModel";
import {UserListIndexViewModel as UserListIndexViewModelType} from "../ts/UserListIndexViewModel";
import {HelperUser} from "../ts/helper/Helper.User";
import {HelperUrl} from "../ts/helper/Helper.Url";
import {HelperString} from "../ts/helper/Helper.String";
import {AuthorizationManager} from "../ts/AuthorizationManager";
import {GenericListViewModel} from "../ts/GenericListViewModel";
import {GenericListChartViewModel as GenericListChartViewModelType} from "../ts/GenericListViewModel.Chart";
import {GenericListMapViewModel as GenericListMapViewModelType} from "../ts/GenericListViewModel.Map";
import {
	TemplateGetIcsLinkModalViewModel as TemplateGetIcsLinkModalViewModelType
} from "../ts/TemplateGetIcsLinkModalViewModel";
import {LookupEditModalViewModel} from "../ts/LookupEditModalViewModel";
import {LookupListIndexViewModel as LookupListIndexViewModelType} from "../ts/LookupListIndexViewModel";
import {NumberingService} from "../ts/NumberingService";
import {VisibilityViewModel} from "../ts/VisibilityViewModel";
import {OnlineTopMenuViewModel as OnlineTopMenuViewModelType} from "../ts/OnlineTopMenuViewModel";
import {PmbbViewModel as PmbbViewModelType} from "../ts/PmbbViewModel";
import { TopSearchViewModel as TopSearchViewModelType } from "../ts/TopSearchViewModel";
import {ViewModelBase as ViewModelBaseType} from "../ts/ViewModelBase";
import {HomeStartupViewModel as HomeStartupViewModelType} from "../ts/StartupViewModel";
import {MainMenuViewModel as MainMenuViewModelType} from "../ts/MainMenuViewModel";
import type {BarcodeScanner} from "../ts/knockout.component.barcodeScanner";
import type {HubConnection} from "@microsoft/signalr";
import * as signalR from "@microsoft/signalr";
import {SignalRAppender} from "../ts/Log";
import {HelperImage} from "../ts/helper/Helper.Image";
import {CollapsibleBlockViewModel as CollapsibleBlockViewModelType} from "../ts/CollapsibleBlockViewModel";
import {
	CollapsibleBlockHeaderViewModel as CollapsibleBlockHeaderViewModelType
} from "../ts/CollapsibleBlockHeaderViewModel";
import {
	CollapsibleBlockContentViewModel as CollapsibleBlockContentViewModelType
} from "../ts/CollapsibleBlockContentViewModel";
import {BlockViewModel as BlockViewModelType} from "../ts/BlockViewModel";
import {BlockHeaderViewModel as BlockHeaderViewModelType} from "../ts/BlockHeaderViewModel";
import {BlockContentViewModel as BlockContentViewModelType} from "../ts/BlockContentViewModel";
import type {HelperBarcode} from "@Main/helper/Helper.Barcode";
import {HelperDownload} from "../ts/helper/Helper.Download";
import type {SiteDetailsMenuEntriesTabViewModel as SiteDetailsMenuEntriesTabViewModelType} from "@Main/SiteDetailsMenuEntriesTabViewModel";

export type StorageProvider = { name: "oData" | "webSql" | "InMemory", supportedSetOperations: any }

export type WakeLockType = "screen"
export interface WakeLockSentinel extends EventTarget {
	readonly released: boolean;
	readonly type: WakeLockType;
	release(): Promise<undefined>;
	onrelease: ((this: WakeLockSentinel, ev: Event) => any) | null;
}
export interface WakeLock {
	request(type: WakeLockType): Promise<WakeLockSentinel>;
}

declare global {
	type valueof<T> = T[keyof T]
	type SignalrState = valueof<typeof window.signalR.HubConnectionState>
	type HelperType = typeof Helper;

	type LookupType = {
		[key: string]: {
			$tableName?: string,
			$filterExpression?: string,
			$filterParameters?: {},
			$array?: any[],
			$lazy?: boolean
		}
	}

	interface Navigator {
		readonly wakeLock: WakeLock;
	}

	interface Window {
		barcodeScanner: BarcodeScanner;
		Breadcrumb: typeof Breadcrumb;
		client: {
			userAgent: () => "iOS" | "Android" | "Windows Phone" | "OfflineClient" | "Windows" | "Unknown";
			isMobileDevice: () => boolean;
		}
		cordova: {
			exec(success: (data: any) => any, fail: (err: any) => any, service: string, action: string, args?: any[]): void;
			platformId: string;
			version: string;
			define: {
				moduleMap: { [key: string]: any }
			};
			require(moduleName: string): any;
			plugins: {
				fileOpener2: any;
				printer: {
					print(callback: (res: boolean) => void): void;
				}
			}
			InAppBrowser: any;
		}
		ics: any;
		namespace: typeof namespace;
		jobDetails: JobDetails;
		breadcrumbsViewModel: MenuBreadcrumbsViewModel;
		FirebasePlugin: typeof FirebasePluginType;
		CordovaCall: {
			receiveCall(caller: string),
			on(eventName: string, callback),
			endCall()
		};
		Log: Logger;
		AuthorizationManager: AuthorizationManager;

		database: Default.Container & {
			add(item: $data.Entity): $data.Entity;
			add(initData: {}): $data.Entity;
			addMany(items: $data.Entity[]): $data.Entity[];
			attach(item: any): void;
			attach(item: {}): void;
			attachOrGet(item: $data.Entity): $data.Entity;
			attachOrGet(item: {}): $data.Entity;
			detach(item: $data.Entity): void;
			detach(item: {}): void;
			remove(item: $data.Entity): void;
			remove(item: {}): void;
			processEntityTypeAfterEventHandler({data: any});
			storageProvider: StorageProvider;
			batchExecuteQuery(x: any);
		};
		oDataDatabase: Default.Container & {
			add(item: $data.Entity): $data.Entity;
			add(initData: {}): $data.Entity;
			addMany(items: $data.Entity[]): $data.Entity[];
			attach(item: any): void;
			attach(item: {}): void;
			attachOrGet(item: $data.Entity): $data.Entity;
			attachOrGet(item: {}): $data.Entity;
			detach(item: $data.Entity): void;
			detach(item: {}): void;
			remove(item: $data.Entity): void;
			remove(item: {}): void;
			processEntityTypeAfterEventHandler({data: any});
			storageProvider: StorageProvider;
			batchExecuteQuery(x: any);
		};
		powermanagement: {
			acquire(): void
			release(): void
		};
		scrollToSelector: (selector: string) => void;
		sqlitePlugin: {
			openDatabase: any;
		}
		swal: typeof swal;
		Main: typeof Main;
		$data: {
			storageProviders: any;
			createGuid(): { value: string }
		};
		signalR: typeof signalR;
		SignalRAppender: SignalRAppender;
		ko: CustomKnockoutStatic;
		moment: typeof moment;
		NumberingService: typeof NumberingService;
		VisibilityViewModel: typeof VisibilityViewModel;
	}

	interface JQuery {
		collapse: Function;
		modal: Function;
		tab: Function;
	}

	interface JQueryStatic {
		connection: {
			MessageHub?: HubConnection
			ProfilingHub?: HubConnection
		};
		growl: growl;
	}

	interface KnockoutValidationErrors {
		scrollToError(): void;
		expandCollapsiblesWithErrors(): void;
	}

// typings for Bootstrap Growl - v2.0.1
// https://github.com/mouse0270/bootstrap-growl
	namespace growl {
		interface Content {
			message?: string;
			title?: string;
			icon?: string;
			url?: string;
		}

		interface Options {
			type?: string;
			allow_dismiss?: boolean;
			element?: string;
			placement?: {
				from?: string,
				align?: string
			};
			offset?: string;
			spacing?: number;
			z_index?: number;
			delay?: number;
			timer?: number;
			url_target?: string;
			mouse_over?: false;
			animate?: {
				enter?: string,
				exit?: string
			};
			onShow?: null;
			onShown?: null;
			onHide?: null;
			onHidden?: null;
			icon_type?: string;
			template?: string;
		}
	}

	interface growl {
		(content: growl.Content | string, options: growl.Options): growl;

		error(options: growl.Options): void;

		notice(options: growl.Options): void;

		warning(options: growl.Options): void;

		close(): growl;

		$template;
	}

	export type Bookmark<T extends $data.Entity> = {
		Category: string,
		Name: string,
		Key: string,
		Expression?: (query: $data.Queryable<T>) => $data.Queryable<T>;
		ApplyFilters?: () => void;
	}

	export type BulkAction = {
		Name: string,
		Action?: any,
		Modal?: {
			Target: string,
			Route: string
		}
	}

	export type ItemGroup = {
		css?: string;
		subtitle?: string;
		title: string;
	}

	export type Select2AutoCompleter = {
		data?: { id: string, text: string }[] | any,
		autocompleteOptions?: Select2AutoCompleterOptions,
		allowClear?: boolean
		default?: string[]
		nested?: boolean
		nestedProperty?: string
	}
	export type Select2AutoCompleterOptions = {
		table?: string,
		orderBy?: string[],
		/**
		 * Applies a custom filter when queries the database
		 * @param query - One item from the provided list
		 * @param term - Content of the Input element
		 */
		customFilter?: (query: any, term: string) => any,
		mapDisplayObject?: (item: any) => Select2AutoCompleterResult,
		getElementByIdQuery?: Function,
		onResult?: Function,
		onSelect?: Function,
		placeholder?: string,
		templateResult?: (param: any) => string | JQuery,
		templateResultId?: string,
		noResults?: string,
		joins?: (string | { Selector: string, Operation: string })[],
		confirmChange?: Function,
		key?: string
	}
	export type Select2AutoCompleterResult = {
		id: string,
		item?: any,
		text: string
	}
	type InlineEditorViewModelType = typeof InlineEditorViewModel;
	type MenuBreadcrumbsViewModelType = typeof MenuBreadcrumbsViewModel;
	type SiteEditViewModelType = typeof SiteEditViewModel;
	type SiteThirdPartySoftwareViewModelType = typeof SiteThirdPartySoftwareViewModel;
	type UserCreateViewModelType = typeof UserCreateViewModel;
	type UserDetailsViewModelType = typeof UserDetailsViewModel;
	type UserGroupCreateViewModelType = typeof UserGroupCreateViewModel;
	type LookupEditModalViewModelType = typeof LookupEditModalViewModel;
	namespace Main {
		namespace Settings {
			let ActiveDirectoryEndpoint: string;
			let CefToPdfPath: string;
			namespace Cordova {
				let AndroidSalesAppLink: string;
				let AndroidServiceAppLink: string;
				let AppleIosSalesAppLink: string;
				let AppleIosServiceAppLink: string;
				let Windows10SalesAppLink: string;
				let Windows10ServiceAppLink: string;
			}
			namespace Email {
				let AttachmentMaxSize: string;
				let SenderImpersonation: boolean;
			}
			namespace Geocoder {
				namespace Address {
					let BatchSize: string;
				}
				let BingMapsApiKey: string;
				let GeocoderService: string;
				let GoogleMapsApiKey: string;
				let GoogleMapsApiVersion: string;
				let MapQuestApiKey: string;
				let YahooMapsApiKey: string;
				let YahooMapsApiSecret: string;
			}
			namespace Maintenance {
				let AmountOfRecentPagesToKeep: string;
				let CommandTimeout: string;
				let ErrorLogDeprecationDays: string;
				let FragmentationLevel1: string;
				let FragmentationLevel2: string;
				let LogDeprecationDays: string;
				let MessageDeprecationDays: string;
				let PostingDeprecationDays: string;
				let ReplicatedClientDeprecationDays: string;
			}
			let MapTileLayerUrl: string;
			let MaxFileLengthInKb: string;
		let MinPasswordStrength: string;
			namespace OpenId {
				let Authority: string;
				let CallbackUrl: string;
				let ClaimType: string;
				let ClientId: string;
				let ClientSecret: string;
				let LogoutUri: string;
				let ResponseType: string;
				let Scope: string;
				let UseTokenLifetime: boolean;
			}
			namespace Posting {
				let MaxRetries: string;
				let RetryAfter: string;
			}
			let RedisConfiguration: string;
			namespace Report {
				let FooterHeight: string;
				let FooterSpacing: string;
				let HeaderHeight: string;
				let HeaderSpacing: string;
			}
			namespace Site {
				let HostEditable: boolean;
				let PluginsEditable: boolean;
			}
			let SoftDelete: boolean;
			let UseActiveDirectoryAuthenticationService: boolean;
			let UseOpenIdAuthentication: boolean;
			namespace Task {
				let AttentionTaskTypeKey: string
			}
		}
		namespace ViewModels {
			let AccountUserProfileViewModel: typeof AccountUserProfileViewModelType;
			let AccountUserProfileDevicesTabViewModel: typeof AccountUserProfileDevicesTabViewModelType;
			let BackgroundServiceIndexViewModel: typeof BackgroundServiceIndexViewModelType;
			let DashboardCalendarWidgetViewModel: typeof DashboardCalendarWidgetViewModelType;
			let DashboardIndexViewModel: typeof DashboardIndexViewModelType;
			let DefaultViewModel: typeof DefaultViewModelType;
			let FloatingActionButtonViewModel: typeof FloatingActionButtonViewModelType;
			let HomeStartupViewModel: typeof HomeStartupViewModelType;
			let MainMenuViewModel: typeof MainMenuViewModelType;
			let OnlineTopMenuViewModel: typeof OnlineTopMenuViewModelType;
			let InlineEditorViewModel: InlineEditorViewModelType;
			let GenericListViewModel: GenericListViewModelType;
			let GenericListChartViewModel: typeof GenericListChartViewModelType;
			let GenericListMapViewModel: typeof GenericListMapViewModelType;
			let LogListIndexViewModel: typeof LogListIndexViewModelType;
			let MenuBreadcrumbsViewModel: MenuBreadcrumbsViewModelType;
			let PmbbViewModel: typeof PmbbViewModelType;
			let SiteEditViewModel: SiteEditViewModelType;
			let SiteThirdPartySoftwareViewModel: SiteThirdPartySoftwareViewModelType;
			let TemplateGetIcsLinkModalViewModel: typeof TemplateGetIcsLinkModalViewModelType;
			let TopSearchViewModel: typeof TopSearchViewModelType;
			let UserCreateViewModel: UserCreateViewModelType;
			let UserDetailsViewModel: UserDetailsViewModelType;
			let UserGroupDetailsViewModel: typeof UserGroupDetailsViewModelType;
			let UserGroupCreateViewModel: UserGroupCreateViewModelType;
			let UserGroupListIndexViewModel: typeof UserGroupListIndexViewModelType;
			let UserListIndexViewModel: typeof UserListIndexViewModelType;
			let ViewModelBase: typeof ViewModelBaseType;
			let LookupEditModalViewModel: LookupEditModalViewModelType;
			let LookupListIndexViewModel: typeof LookupListIndexViewModelType;
			let CollapsibleBlockViewModel: typeof CollapsibleBlockViewModelType;
			let CollapsibleBlockHeaderViewModel: typeof CollapsibleBlockHeaderViewModelType;
			let CollapsibleBlockContentViewModel: typeof CollapsibleBlockContentViewModelType;
			let BlockViewModel: typeof BlockViewModelType;
			let BlockHeaderViewModel: typeof BlockHeaderViewModelType;
			let BlockContentViewModel: typeof BlockContentViewModelType;
			let SiteDetailsMenuEntriesTabViewModel: typeof SiteDetailsMenuEntriesTabViewModelType;
		}
	}

	type GenericListViewModelType = typeof GenericListViewModel;

	namespace Helper {

		/* Top Level functions */
		function getMetadata(name: string): string;

		function resolveUrl(url: string): string;

		function getTranslatedString(alienString: string, defaultString?: string, language?: string): null | string;

		function limitStringWithEllipses(longString: string, maxLength?: number): any;

		function objectToString(obj: any): string;

		function htmlEntities(unsafeText: any): string;

		namespace mousePosition {
			let X: number;
			let Y: number;
		}

		function log(...params: any[]): void;

		function blockElement(element: JQuery, unblock?: boolean): void;

		function createLink(element: HTMLElement, removeClass: string): void;

		function generateGuid(): string;

		/* Nested Helpers */
		let Batch: typeof HelperBatch;
		let Confirm: typeof HelperConfirm;
		let Culture: typeof HelperCulture;
		let DOM: typeof HelperDOM;
		let Database: typeof HelperDatabase;
		let Date: typeof HelperDate;
		let Distinct: typeof HelperDistinct;
		let Image: typeof HelperImage;
		let Lookup: typeof HelperLookup;
		let Message: typeof HelperMessage;
		let Navigation: typeof HelperNavigation;
		let Number: typeof HelperNumber;
		let Object: typeof HelperObject;
		let Posting: typeof HelperPosting;
		let String: typeof HelperString;
		let Url: typeof HelperUrl;
		let User: typeof HelperUser;
		let Visibility: typeof HelperVisibility;
		let Map: typeof HelperMap | undefined;
		let Barcode: typeof HelperBarcode | undefined;
		let Download: typeof HelperDownload | undefined;
	}


	interface CustomKnockoutValidationErrors extends KnockoutValidationErrors {
		awaitValidation(): Promise<void>;

		switchToError(): void;
	}

	interface CustomKnockoutValidationStatic extends KnockoutValidationStatic {
		addRule<T>(observable: KnockoutObservable<T>, rule: KnockoutValidationRule & {
			propertyName: string
		}): KnockoutObservable<T>;

		group(obj: any, options?: any): CustomKnockoutValidationErrors;
	}

	interface CustomKnockoutStatic extends KnockoutStatic {
		custom: {
			paging: any
		}
		ItemStatus: any;
		validation: CustomKnockoutValidationStatic;
		validationRules: {
			add(entityName, rule): void
		}
	}

	interface ExtensionValues<T> {
		ExtensionValues: T
	}
}