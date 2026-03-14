namespace Crm.Service.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Modularization;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Authorize]
	public class ServiceOrderMaterialController : Controller
	{
		[RequiredPermission(ServicePlugin.PermissionGroup.ServiceOrder,
			false,
			ServicePlugin.PermissionName.EditCost,
			ServicePlugin.PermissionName.EditMaterial)]
		public virtual ActionResult EditTemplate() => PartialView();
		[RequiredPermission(ServicePlugin.PermissionName.EditMaterial, Group = ServicePlugin.PermissionGroup.ServiceOrder)]
		public virtual ActionResult ReportScheduledMaterial() => PartialView();
		[RenderAction("ServiceOrderMaterialListTopMenu")]
		[RequiredPermission(ServicePlugin.PermissionName.EditMaterial, Group = ServicePlugin.PermissionGroup.ServiceOrder)]
		public virtual ActionResult ReportScheduledMaterialTabHeaderMenu() => PartialView();
	}
}
