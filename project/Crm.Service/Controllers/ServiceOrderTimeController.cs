using Microsoft.AspNetCore.Mvc;

namespace Crm.Service.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Modularization;
	using Microsoft.AspNetCore.Authorization;

	[Authorize]
	public class ServiceOrderTimeController : Controller
	{
		[RequiredPermission(ServicePlugin.PermissionName.TimeEdit, Group = ServicePlugin.PermissionGroup.ServiceOrder)]
		public virtual ActionResult EditTemplate() => PartialView();

		[RenderAction("DispatchJobsTabPrimaryAction")]
		public virtual ActionResult PrimaryActionAddServiceOrderTime() => PartialView();
	}
}
