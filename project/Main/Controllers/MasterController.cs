namespace Main.Controllers
{
	using Crm.Library.Helper;
	using Crm.Library.Modularization;

	using Main.ViewModels;

	using Microsoft.AspNetCore.Mvc;

	public class MasterController : Controller
	{
		#region MaterialTitleResource
		[RenderAction("MaterialTitleResource", Priority = 4990)]
		public virtual ActionResult MaterialTitleResourceContentSecurityPolicy() => View("Master/ContentSecurityPolicy");

		[RenderAction("MaterialTitleResource", Priority = 4980)]
		public virtual ActionResult MaterialTitleResourceContentType() => View("Master/ContentType");

		[RenderAction("MaterialTitleResource", Priority = 4970)]
		public virtual ActionResult MaterialTitleResourceInternetExplorerCompatibility() => View("Master/InternetExplorerCompatibility");

		[RenderAction("MaterialTitleResource", Priority = 4960)]
		public virtual ActionResult MaterialTitleResourceViewPort() => View("Master/ViewPort");

		[RenderAction("MaterialTitleResource", Priority = 4950)]
		public virtual ActionResult MaterialTitleResourceTelephoneFormatDetection() => View("Master/TelephoneFormatDetection");

		[RenderAction("MaterialTitleResource", Priority = 4940)]
		public virtual ActionResult MaterialTitleResourceAppDomainAppVirtualPath() => View("Master/AppDomainAppVirtualPath");

		[RenderAction("MaterialTitleResource", Priority = 4930)]
		public virtual ActionResult MaterialTitleResourceCurrentUser() => View("Master/CurrentUser", new CrmModel());

		[RenderAction("MaterialTitleResource", Priority = 4920)]
		public virtual ActionResult MaterialTitleResourceCurrentCulture() => View("Master/CurrentCulture");

		[RenderAction("MaterialTitleResource", Priority = 4920)]
		public virtual ActionResult MaterialTitleResourceCurrentLanguage() => View("Master/CurrentLanguage");

		[RenderAction("MaterialTitleResource", Priority = 4910)]
		public virtual ActionResult MaterialTitleResourceFavicon() => View("Master/Favicon");

		[RenderAction("MaterialTitleResource", Priority = 4900)]
		public virtual ActionResult MaterialTitleResourceMaterialCss() => Content(Url.CssResource("Main", "materialCss"));
		#endregion MaterialTitleResource
		#region MaterialHeadResource
		[RenderAction("MaterialHeadResource", Priority = 9990)]
		public virtual ActionResult MaterialSystemJs() => Content(Url.JsResource("Main", "materialSystemJs"));
		
 		[RenderAction("MaterialHeadResource", Priority = 9980)]
		public virtual ActionResult MaterialHeadResourceJayDataJs() => Content(Url.JsResource("Main", "webSqlPolyfillTs") + Url.JsResource("Main", "jayDataJs") + Url.JsResource("Main", "jayDataTs"));

		[RenderAction("MaterialHeadResource", Priority = 9970)]
		public virtual ActionResult MaterialTs() => Content(Url.JsResource("Main", "mainMaterialTs"));
		
		[RenderAction("MaterialHeadResource", Priority = 9950)]
		public virtual ActionResult MaterialHeadResourceCordovaJs() => View("Master/CordovaJs");
		#endregion MaterialHeadResource
		#region HeadResource
		[RenderAction("HeadResource", Priority = 9990)]
		public virtual ActionResult HeadResourceContentSecurityPolicy() => View("Master/ContentSecurityPolicy");

		[RenderAction("HeadResource", Priority = 9980)]
		public virtual ActionResult HeadResourceContentType() => View("Master/ContentType");

		[RenderAction("HeadResource", Priority = 9970)]
		public virtual ActionResult HeadResourceInternetExplorerCompatibility() => View("Master/InternetExplorerCompatibility");

		[RenderAction("HeadResource", Priority = 9960)]
		public virtual ActionResult HeadResourceViewPort() => View("Master/ViewPort");

		[RenderAction("HeadResource", Priority = 9950)]
		public virtual ActionResult HeadResourceAppDomainAppVirtualPath() => View("Master/AppDomainAppVirtualPath");

		[RenderAction("HeadResource", Priority = 9940)]
		public virtual ActionResult HeadResourceCurrentUser() => View("Master/CurrentUser", new CrmModel());

		[RenderAction("HeadResource", Priority = 9930)]
		public virtual ActionResult HeadResourceCurrentCulture() => View("Master/CurrentCulture");

		[RenderAction("HeadResource", Priority = 9930)]
		public virtual ActionResult HeadResourceCurrentLanguage() => View("Master/CurrentLanguage");

		[RenderAction("HeadResource", Priority = 9920)]
		public virtual ActionResult HeadResourceFavicon() => View("Master/Favicon");

		[RenderAction("HeadResource", Priority = 9910)] 
		public virtual ActionResult HeadResourceCurrentToken() => View("Master/CurrentToken", new CrmModel());

		[RenderAction("HeadResource", Priority = 9870)]
		public virtual ActionResult HeadResourcePrintCss() => Content(Url.CssResource("Main", "printCss", WebExtensions.Media.Print));

		[RenderAction("HeadResource", Priority = 9850)]
		public virtual ActionResult HeadResourceSystemJs() => Content(Url.JsResource("Main", "materialSystemJs"));

		#endregion HeadResource

		#region LoginHeadResource
		[RenderAction("LoginHeadResource", Priority = 9000)]
		public virtual ActionResult LoginHeadResourceAppleIcon() => View("Master/AppleIcon");
		
		[RenderAction("LoginHeadResource", Priority = 8995)]
		public virtual ActionResult LoginHeadResourceAppManifest() => View("Master/AppManifest");
		
		[RenderAction("LoginHeadResource", Priority = 8990)]
		public virtual ActionResult LoginHeadResourceContentSecurityPolicy() => View("Master/ContentSecurityPolicy");

		[RenderAction("LoginHeadResource", Priority = 8985)]
		public virtual ActionResult LoginHeadResourceAppDomainAppVirtualPath() => View("Master/AppDomainAppVirtualPath");

		[RenderAction("LoginHeadResource", Priority = 8980)]
		public virtual ActionResult LoginHeadResourceContentType() => View("Master/ContentType");

		[RenderAction("LoginHeadResource", Priority = 8975)]
		public virtual ActionResult LoginHeadResourceCurrentUser() => View("Master/CurrentUser", new CrmModel());

		[RenderAction("LoginHeadResource", Priority = 8970)]
		public virtual ActionResult LoginHeadResourceInternetExplorerCompatibility() => View("Master/InternetExplorerCompatibility");

		[RenderAction("LoginHeadResource", Priority = 8960)]
		public virtual ActionResult LoginHeadResourceViewPort() => View("Master/ViewPort");

		[RenderAction("LoginHeadResource", Priority = 8950)]
		public virtual ActionResult LoginHeadResourceFavicon() => View("Master/Favicon");

		[RenderAction("LoginHeadResource", Priority = 8940)]
		public virtual ActionResult LoginHeadResourceLoginCss() => Content(Url.CssResource("Main", "loginCss"));

		[RenderAction("LoginHeadResource", Priority = 8930)]
		public virtual ActionResult LoginHeadResourceLoginLess() => Content(Url.CssResource("Main", "loginLess", WebExtensions.Media.None, "11.1"));
		#endregion LoginHeadResource

		#region LoginBodyResource
		[RenderAction("LoginHeadResource", Priority = 8920)]
		public virtual ActionResult LoginHeadResourceLoginJs() => Content(Url.JsResource("Main", "loginJs"));
		#endregion

		#region VideoClientHeadResource

		[RenderAction("VideoClientHeadResource", Priority = 9000)]
		public virtual ActionResult VideoClientHeadResourceVideoClientTs() => Content(Url.JsResource("Main.VideoCall", "videoClientTs"));
		
		[RenderAction("VideoClientHeadResource", Priority = 8990)]
		public virtual ActionResult VideoClientHeadResourceJayDataJs() => Content(Url.JsResource("Main", "webSqlPolyfillTs") + Url.JsResource("Main", "jayDataJs") + Url.JsResource("Main", "jayDataTs"));
		
		#endregion
	}
}
