namespace Crm.Service.Team.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using Crm.Library.Modularization;
	using Crm.Library.Model;

	public class UserGroupDetailsController : Controller
	{
		[RenderAction("UserGroupDetailsMaterialTabHeader", Priority = 80)]
		public virtual ActionResult ArticlesTabHeader() => PartialView();
		[RenderAction("UserGroupDetailsMaterialTab", Priority = 80)]
		public virtual ActionResult ArticlesTab() => PartialView();

		[RenderAction("UserGroupDetailsMaterialTabHeader", Priority = 70)]
		[RequiredPermission(ServicePlugin.PermissionName.DispatchesTab, Group = ServicePlugin.PermissionGroup.ServiceOrder)]
		public virtual ActionResult DispatchesTabHeader() => PartialView();

		[RenderAction("UserGroupDetailsMaterialTab", Priority = 70)]
		[RequiredPermission(ServicePlugin.PermissionName.DispatchesTab, Group = ServicePlugin.PermissionGroup.ServiceOrder)]
		public virtual ActionResult DispatchesTab() => PartialView();
	}
}
