using Microsoft.AspNetCore.Mvc;

namespace Crm.Service.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Microsoft.AspNetCore.Authorization;

	[Authorize]
	public class InstallationPositionController : Controller
	{
		[RequiredPermission(PermissionName.Edit, Group = ServicePlugin.PermissionGroup.InstallationPosition)]
		public virtual ActionResult EditTemplate()
		{
			return PartialView();
		}
	}
}
