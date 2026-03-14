using Microsoft.AspNetCore.Mvc;

namespace Crm.Service.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;

	using Microsoft.AspNetCore.Authorization;

	[Authorize]
	public class ServiceOrderErrorCauseController : Controller
	{

		[RequiredPermission(PermissionName.Edit, Group = ServicePlugin.PermissionGroup.ServiceOrderErrorCause)]
		public virtual ActionResult EditTemplate() => PartialView();

	}
}
