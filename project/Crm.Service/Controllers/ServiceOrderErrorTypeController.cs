using Microsoft.AspNetCore.Mvc;

namespace Crm.Service.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;

	using Microsoft.AspNetCore.Authorization;

	[Authorize]
	public class ServiceOrderErrorTypeController : Controller
	{

		[RequiredPermission(PermissionName.Create, Group = ServicePlugin.PermissionGroup.ServiceOrderErrorType)]
		[RenderAction("ServiceOrderErrorTypeTabPrimaryAction")]
		public virtual ActionResult PrimaryActionAddServiceOrderErrorType() => PartialView();

		[RequiredPermission(PermissionName.Edit, Group = ServicePlugin.PermissionGroup.ServiceOrderErrorType)]
		public virtual ActionResult EditTemplate() => PartialView();

	}
}
