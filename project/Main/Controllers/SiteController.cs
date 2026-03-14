namespace Main.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Authorize]
	public class SiteController : Controller
	{
		[HttpGet]
		[RequiredPermission(PermissionName.Settings, Group = MainPlugin.PermissionGroup.Site)]
		public virtual ActionResult Details()
		{
			return PartialView();
		}

		[HttpGet]
		[RequiredPermission(PermissionName.Settings, Group = MainPlugin.PermissionGroup.Site)]
		public virtual ActionResult Edit()
		{
			return PartialView();
		}

		[RenderAction("SiteTab")]
		public virtual ActionResult DetailsTab() => PartialView();

		[RenderAction("SiteTabHeader")]
		public virtual ActionResult DetailsTabHeader() => PartialView();
		[RenderAction("SiteTab")]
		public virtual ActionResult LicenseTab() => PartialView();

		[RenderAction("SiteTabHeader")]
		public virtual ActionResult LicenseTabHeader() => PartialView();

		[RenderAction("SiteTab")]
		public virtual ActionResult MenuEntriesTab() => PartialView();

		[RenderAction("SiteTabHeader")]
		public virtual ActionResult MenuEntriesTabHeader() => PartialView();

		[RequiredPermission(PermissionName.View, Group = MainPlugin.PermissionGroup.ThirdPartySoftware)]
		public virtual ActionResult ThirdPartySoftware() => PartialView();
	}
}
