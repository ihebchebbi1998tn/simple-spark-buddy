namespace Main.Controllers
{
	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Authorize]
	public class TemplateController : Controller
	{
		public virtual ActionResult BarcodeScanner()
		{
			return PartialView();
		}
		[RequiredPermission("RecentPage", Group = PermissionGroup.WebApiRead)]
		public virtual ActionResult Breadcrumbs()
		{
			return PartialView();
		}
		
		public virtual ActionResult DateFilter()
		{
			return PartialView();
		}
		public virtual ActionResult EmptyStateBox()
		{
			return PartialView();
		}
		public virtual ActionResult FloatingActionButton()
		{
			return PartialView();
		}
		public virtual ActionResult FlotChart()
		{
			return PartialView();
		}
		public virtual ActionResult FormElement()
		{
			return PartialView();
		}
		public virtual ActionResult FullCalendarWidget()
		{
			return PartialView();
		}		
		public virtual ActionResult GenericListSelection()
		{
			return PartialView();
		}
		public virtual ActionResult GetIcsLink()
		{
			return PartialView();
		}
		public virtual ActionResult GetRssLink()
		{
			return PartialView();
		}
		public virtual ActionResult InlineEditor()
		{
			return PartialView("InlineEdit/InlineEditor");
		}
		public virtual ActionResult LicensingAlert()
		{
			return PartialView();
		}
		public virtual ActionResult LvActions()
		{
			return PartialView();
		}

		public virtual ActionResult MiniChart()
		{
			return PartialView();
		}

		public virtual ActionResult PmbBlock()
		{
			return PartialView("InlineEdit/PmbBlock");
		}
		public virtual ActionResult PmbbEdit()
		{
			return PartialView("InlineEdit/PmbbEdit");
		}
		public virtual ActionResult PmbbEditEntry()
		{
			return PartialView("InlineEdit/PmbbEditEntry");
		}
		public virtual ActionResult PmbbView()
		{
			return PartialView("InlineEdit/PmbbView");
		}
		public virtual ActionResult PmbbViewEntry()
		{
			return PartialView("InlineEdit/PmbbViewEntry");
		}
		public virtual ActionResult ScaleFilter()
		{
			return PartialView();
		}
		public virtual ActionResult SignaturePad()
		{
			return PartialView("SignaturePad");
		}
		public virtual ActionResult StatusChooser()
		{
			return PartialView("StatusChooser");
		}
		[RequiredPermission(PermissionName.View, Group = MainPlugin.PermissionGroup.TopSearch)]
		public virtual ActionResult TopSearch()
		{
			return PartialView();
		}
		public virtual ActionResult CountWidget()
		{
			return PartialView();
		}
		public virtual ActionResult CollapsibleBlock()
		{
			return PartialView("CollapsibleBlock");
		}
		public virtual ActionResult CollapsibleBlockHeader()
		{
			return PartialView("CollapsibleBlockHeader");
		}
		public virtual ActionResult CollapsibleBlockContent()
		{
			return PartialView("CollapsibleBlockContent");
		}
		public virtual ActionResult Block()
		{
			return PartialView("Block");
		}
		public virtual ActionResult BlockHeader()
		{
			return PartialView("BlockHeader");
		}
		public virtual ActionResult BlockContent()
		{
			return PartialView("BlockContent");
		}

		public virtual ActionResult BarcodeFormat()
		{
			return PartialView("BarcodeFormat");
		}

		[AllowAnonymous]
		[RenderAction("MaterialTitleResource", "TemplateHeadResource", Priority = 50)]
		public virtual ActionResult TemplateReportCss()
		{
			return Content(Url.CssResource("Main", "templateReportCss"));
		}

		[AllowAnonymous]
		[RenderAction("MaterialTitleResource", "TemplateHeadResource", Priority = 50)]
		public virtual ActionResult TemplateComponentCss()
		{
			return Content(Url.CssResource("Main", "templateComponentCss"));
		}

		[AllowAnonymous]
		[RenderAction("TemplateHeadResource", Priority = 10000)]
		public virtual ActionResult TemplateReportJs()
		{
			return Content(Url.JsResource("Main", "webSqlPolyfillTs") + Url.JsResource("Main", "jayDataJs") + Url.JsResource("Main", "jayDataTs") + Url.JsResource("Main", "templateReportJs"));
		}
		[AllowAnonymous]
		[RenderAction("TemplateHeadResource", Priority = 60)]
		public virtual ActionResult TemplateReportMaterialCss()
		{
			return Content(Url.CssResource("Main", "materialCss"));
		}
		public virtual ActionResult TimeRangeFilter()
		{
			return PartialView();
		}
		public virtual ActionResult Barcode()
		{
			return PartialView();
		}
	}
}
