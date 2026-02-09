namespace Main
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Security.Cryptography;
	using System.Text;

	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Licensing;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Offline;
	using Crm.Library.Offline.Interfaces;
	using Crm.Library.Services.Interfaces;
	using Crm.Library.Unicore;

	using Main.Model.Lookups;
	using Main.Services.Interfaces;

	public class MaterialCacheManifestRegistrar : CacheManifestRegistrar<MaterialCacheManifest>
	{
		public static Dictionary<string, string> Themes = new Dictionary<string, string>
		{
			{ "pink", "#E91E63" },
			{ "purple", "#9C27B0" },
			{ "blue", "#2196F3" },
			{ "lightblue", "#03A9F4" },
			{ "cyan", "#00BCD4" },
			{ "teal", "#009688" },
			{ "green", "#4CAF50" },
			{ "orange", "#FF9800" },
			{ "bluegray", "#607D8B" }
		};
		private readonly IClientSideGlobalizationService clientSideGlobalizationService;
		private readonly IUserService userService;
		private readonly ILicensingService licensingService;
		private readonly ILookupManager lookupManager;
		private readonly IMenuEntryService menuEntryService;
		public MaterialCacheManifestRegistrar(IClientSideGlobalizationService clientSideGlobalizationService, IUserService userService, IPluginProvider pluginProvider, ILicensingService licensingService, ILookupManager lookupManager, IMenuEntryService menuEntryService)
			: base(pluginProvider)
		{
			this.clientSideGlobalizationService = clientSideGlobalizationService;
			this.userService = userService;
			this.licensingService = licensingService;
			this.lookupManager = lookupManager;
			this.menuEntryService = menuEntryService;
		}
		protected override void Initialize()
		{
			using var hasher = SHA256.Create();
			CacheComment(() => $"# license hash: {licensingService.GetUserLicenseCount(UnicoreDefaults.CommonDomainId.ToString()).GetHashCode()^licensingService.GetExpireDate(UnicoreDefaults.CommonDomainId.ToString()).GetHashCode()}");
			CacheComment(() => $"# last modify date of menu entry {menuEntryService.GetLastModifyDate()}");
			CacheComment(() =>
			{
				var languages = lookupManager.List<Language>().Where(x => x.IsSystemLanguage).OrderBy(x => x.Key).Select(x => x.Key).ToArray();
				var languagesHash = string.Concat(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(string.Join("|", languages))).Select(x => x.ToString("x2")));
				return $"# languages hash: {languagesHash}";
			});

			CacheJs( "cordovaIosJs");
			CacheJs( "cordovaAndroidJs");
			CacheJs( "webSqlPolyfillTs");
			CacheJs( "jayDataJs");
			CacheJs( "jayDataTs");
			CacheJs( "loginJs");
			CacheJs( "mainMaterialTs");
			CacheJs( "materialSystemJs");
			CacheCss( "loginCss");
			CacheCss( "loginLess");
			CacheCss( "materialCss");
			CacheCss( "templateReportCss");

			Cache("Account", "RemoveDevice");
			Cache("Account", "UserProfile");
			Cache("Account", "SetUpTotpAuthenticator");
			Cache("Barcode", "Preview");
			Cache("Dashboard", "IndexTemplate");
			Cache("Dashboard", "CalendarWidgetTemplate");
			Cache("Error", "Template");
			Cache("Home", "Index");
			Cache("Home", "MaterialIndex");
			Cache("Home", "Startup");
			Cache("Model", "GetRules");
			Cache("Posting", "DetailsTemplate");
			Cache("Posting", "Edit");
			Cache("Posting", "MaterialPostingsTabHeader");
			Cache("Posting", "MaterialPostingsTab");
			Cache("PostingList", "IndexTemplate");
			Cache("PostingList", "FilterTemplate");
			Cache("PostingList", "SkipTransactions");
			Cache("PostingList", "TriggerPostingServiceTopMenu");
			Cache("Template", "DateFilter");
			Cache("Template", "EmptyStateBox");
			Cache("Template", "BarcodeScanner");
			Cache("Template", "FloatingActionButton");
			Cache("Template", "FlotChart");
			Cache("Template", "FormElement");
			Cache("Template", "FullCalendarWidget");
			Cache("Template", "GenericListSelection");
			Cache("Template", "GetIcsLink");
			Cache("Template", "GetRssLink");
			Cache("Template", "InlineEditor");
			Cache("Template", "LvActions");
			Cache("Template", "MiniChart");
			Cache("Template", "PmbBlock");
			Cache("Template", "PmbbEdit");
			Cache("Template", "PmbbEditEntry");
			Cache("Template", "PmbbView");
			Cache("Template", "PmbbViewEntry");
			Cache("Template", "SignaturePad");
			Cache("Template", "TopSearch");
			Cache("Template", "Breadcrumbs");
			Cache("Template", "LicensingAlert");
			Cache("Template", "CountWidget");
			Cache("Template", "CollapsibleBlock");
			Cache("Template", "CollapsibleBlockHeader");
			Cache("Template", "CollapsibleBlockContent");
			Cache("Template", "Block");
			Cache("Template", "BlockHeader");
			Cache("Template", "BlockContent");
			Cache("Template", "StatusChooser");
			Cache("User","AssignLicense");
			Cache("Visibility", "Edit");
			Cache("Visibility", "Selection");
			Cache(() => $"~/Main/Authorization/{userService.CurrentUser.Id}.json");
			Cache(() => $"~/Main/Licensing.json");
			Cache("~/api/$metadata");
			Cache("~/Plugins/Main/Content/img/avatar.jpg");
			Cache("~/Plugins/Main/Content/img/lmobile-block-excited.svg");
			Cache("~/Plugins/Main/Content/img/lmobile-block-happy.svg");
			Cache("~/Plugins/Main/Content/img/lmobile-block-sad.svg");
			Cache("~/Plugins/Main/Content/img/marker_pin3.png");
			Cache("~/Plugins/Main/Content/img/marker_shadow.png");
			Cache("~/Plugins/Main/Content/img/app-logo-sales.webp");
			Cache("~/Plugins/Main/Content/img/app-logo-service.webp");
			Cache("~/Plugins/Main/Content/img/favicon.ico");
			Cache("~/Plugins/Main/Content/img/paperclip.png");
			Cache("~/static-dist/Main/style/fonts/glyphicons-halflings-regular.woff2");
			Cache("~/static-dist/Main/style/fonts/Material-Design-Iconic-Font.woff2?v=2.2.0");
			Cache("~/static-dist/Main/style/fonts/Material-Design-Iconic-Font.woff?v=2.2.0");
			Cache("~/static-dist/Main/style/fonts/Material-Design-Iconic-Font.ttf?v=2.2.0");
			Cache("~/Plugins/Main/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-300.woff");
			Cache("~/Plugins/Main/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-300.woff2");
			Cache("~/Plugins/Main/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-300italic.woff");
			Cache("~/Plugins/Main/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-300italic.woff2");
			Cache("~/Plugins/Main/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-600.woff");
			Cache("~/Plugins/Main/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-600.woff2");
			Cache("~/Plugins/Main/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-600italic.woff");
			Cache("~/Plugins/Main/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-600italic.woff2");
			Cache("~/Plugins/Main/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-700.woff");
			Cache("~/Plugins/Main/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-700.woff2");
			Cache("~/Plugins/Main/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-700italic.woff");
			Cache("~/Plugins/Main/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-700italic.woff2");
			Cache("~/Plugins/Main/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-800.woff");
			Cache("~/Plugins/Main/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-800.woff2");
			Cache("~/Plugins/Main/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-800italic.woff");
			Cache("~/Plugins/Main/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-800italic.woff2");
			Cache("~/Plugins/Main/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-italic.woff");
			Cache("~/Plugins/Main/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-italic.woff2");
			Cache("~/Plugins/Main/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-regular.woff");
			Cache("~/Plugins/Main/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-regular.woff2");
			Cache("~/static-dist/Main/style/fonts/roboto/Roboto-Bold-webfont.ttf");
			Cache("~/static-dist/Main/style/fonts/roboto/Roboto-Light-webfont.ttf");
			Cache("~/static-dist/Main/style/fonts/roboto/Roboto-Medium-webfont.ttf");
			Cache("~/static-dist/Main/style/fonts/roboto/Roboto-Regular-webfont.ttf");
			Cache("~/static-dist/Main/style/fonts/roboto/Roboto-Thin-webfont.ttf");
			Cache("~/Plugins/Main/Content/style/img/icons/search.png");
			Cache("~/static-dist/Main/style/img/icons/search-2.png");
			Cache("~/static-dist/Main/style/img/icons/search-2@2x.png");
			Cache("~/static-dist/Main/style/img/profile-menu.png");
			Cache("~/static-dist/Main/style/img/select.png");
			Cache("~/static-dist/Main/style/img/select@2x.png");
			Cache("~/static-dist/Main/style/select2-spinner.gif");
			Cache("~/static-dist/Main/style/img/icons/pin.png");
			Cache("~/static-dist/Main/style/img/icons/unpin.png");
			Cache("~/Main/Resources.json");
			Cache("~/Main/Settings.json");
			Cache("~/Main/Resource/GetGlobalizationData?format=json");
			Cache("~/Main/Resource/ListLocales?format=json");
			Cache("~/static-dist/cldr-data/supplemental/likelySubtags.json");
			Cache("~/static-dist/cldr-data/supplemental/currencyData.json");
			Cache("~/static-dist/cldr-data/supplemental/timeData.json");
			Cache("~/static-dist/cldr-data/supplemental/weekData.json");
			Cache("~/static-dist/cldr-data/supplemental/numberingSystems.json");
			Cache("~/static-dist/cldr-data/supplemental/plurals.json");
			Cache("~/static-dist/cldr-data/supplemental/ordinals.json");
			var cultureResourcePath = clientSideGlobalizationService.GetCurrentCultureOrDefaultResourcePath();
			Cache($"{cultureResourcePath}/ca-gregorian.json");
			Cache($"{cultureResourcePath}/currencies.json");
			Cache($"{cultureResourcePath}/dateFields.json");
			Cache($"{cultureResourcePath}/languages.json");
			Cache($"{cultureResourcePath}/localeDisplayNames.json");
			Cache($"{cultureResourcePath}/numbers.json");
			Cache($"{cultureResourcePath}/scripts.json");
			Cache($"{cultureResourcePath}/territories.json");
			Cache($"{cultureResourcePath}/timeZoneNames.json");
			Cache($"{cultureResourcePath}/variants.json");
		}
	}
}
