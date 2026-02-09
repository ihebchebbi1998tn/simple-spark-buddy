using Microsoft.AspNetCore.Mvc;

namespace Crm.Service.Controllers
{
	using Crm.Library.Model;
	using Microsoft.AspNetCore.Authorization;

	[Authorize]
	public class ServiceContractInstallationRelationshipController : Controller
	{
		[RequiredPermission(ServicePlugin.PermissionName.SaveInstallationRelationship, Group = ServicePlugin.PermissionGroup.ServiceContract)]
		public virtual ActionResult EditTemplate()
		{
			return PartialView();
		}
	}
}
