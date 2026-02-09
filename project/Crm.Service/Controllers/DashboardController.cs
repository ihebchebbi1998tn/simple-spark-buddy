using Microsoft.AspNetCore.Mvc;

namespace Crm.Service.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;
	using Crm.Service.Model;

	using Microsoft.AspNetCore.Authorization;
	using PermissionGroup = ServicePlugin.PermissionGroup;

	[Authorize]
	public class DashboardController : Controller
	{
		[RenderAction("DashboardMiniChart", Priority = 900)]
		[RequiredPermission(PermissionName.Index, Group = PermissionGroup.Dispatch)]
		[RequiredPermission(nameof(ServiceOrderDispatch), Group = Crm.Library.Model.Authorization.PermissionGroup.MaterialDashboard)]
		public virtual ActionResult DueDispatches()
		{
			return PartialView();
		}

		[RenderAction("DashboardMiniChart", Priority = 1000)]
		[RequiredPermission(PermissionName.Index, Group = PermissionGroup.Dispatch)]
		[RequiredPermission(nameof(ServiceOrderDispatch), Group = Crm.Library.Model.Authorization.PermissionGroup.MaterialDashboard)]
		public virtual ActionResult NewDispatches()
		{
			return PartialView();
		}

		[RenderAction("Dashboard", Priority = 500)]
		[RequiredPermission(PermissionName.Index, Group = PermissionGroup.Dispatch)]
		[RequiredPermission(nameof(ServiceOrderDispatch), Group = Crm.Library.Model.Authorization.PermissionGroup.MaterialDashboard)]
		public virtual ActionResult TodaysDispatches()
		{
			return PartialView();
		}

		[RenderAction("Dashboard", Priority = 1500)]
		[RequiredPermission(PermissionName.Index, Group = PermissionGroup.ServiceCase)]
		[RequiredPermission(nameof(ServiceCase), Group = Crm.Library.Model.Authorization.PermissionGroup.MaterialDashboard)]
		public virtual ActionResult ServiceCaseDashboardSection()
		{
			return PartialView();
		}
	}
}
