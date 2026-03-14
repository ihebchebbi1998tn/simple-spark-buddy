using Microsoft.AspNetCore.Mvc;

namespace Crm.Service.Controllers
{
	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;
	using Microsoft.AspNetCore.Authorization;

	[Authorize]
	public class ServiceOrderTimePostingController : Controller
	{
		[RequiredPermission(ServicePlugin.PermissionName.TimePostingsEdit, Group = ServicePlugin.PermissionGroup.ServiceOrder)]
		public virtual ActionResult EditTemplate() => PartialView();
		
		[RequiredPermission(ServicePlugin.PermissionName.TimePostingAdd, Group = ServicePlugin.PermissionGroup.ServiceOrder)]
		[RenderAction("PerDiemReportOverviewEntry", Priority = 100)]
		public virtual ActionResult PerDiemReportOverviewEntry()
		{
			return PartialView();
		}

		[AllowAnonymous]
		[RenderAction("PerDiemReportResource", Priority = 90)]
		public virtual ActionResult PerDiemReportResource()
		{
			return Content(Url.JsResource("Crm.Service", "servicePerDiemReportJs"));
		}

		[RenderAction("DispatchTimePostingsTabPrimaryAction")]
		public virtual ActionResult PrimaryActionAddServiceOrderTimePosting() => PartialView();

		[RenderAction("ServiceOrderTimePostingsTabPrimaryAction")]
		public virtual ActionResult PrimaryActionAddServiceOrderTimePostingPrePlanned() => PartialView();

		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.Dispatch)]
		[RenderAction("TimeEntryTemplateActions")]
		public virtual ActionResult TemplateActionViewDispatch() => PartialView();

		[RenderAction("TimeEntryItemTemplateExtension")]
		public virtual ActionResult TimeEntryItemTemplateExtension() => PartialView();

		[RequiredPermission(ServicePlugin.PermissionName.DispatchRelink, Group = ServicePlugin.PermissionGroup.ServiceOrderTimePosting)]
		public virtual ActionResult DispatchRelink() => PartialView();
	}
}
