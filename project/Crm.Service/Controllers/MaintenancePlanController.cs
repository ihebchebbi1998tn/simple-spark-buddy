using Microsoft.AspNetCore.Mvc;

namespace Crm.Service.Controllers
{
	using Crm.Library.Model;
	using Microsoft.AspNetCore.Authorization;

	[Authorize]
	public class MaintenancePlanController : Controller
	{
		[RequiredPermission(ServicePlugin.PermissionName.SaveMaintenancePlan, Group = ServicePlugin.PermissionGroup.ServiceContract)]
		public virtual ActionResult EditTemplate()
		{
			return PartialView();
		}
	}
}
