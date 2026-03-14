using Microsoft.AspNetCore.Mvc;

namespace Crm.Service.Controllers
{
	using Crm.Library.Model;
	using Microsoft.AspNetCore.Authorization;

	[Authorize]
	public class ReplenishmentOrderItemController : Controller
	{
		[RequiredPermission(ServicePlugin.PermissionGroup.ReplenishmentOrder, false, ServicePlugin.PermissionName.CreateItem, ServicePlugin.PermissionName.EditItem)]
		public virtual ActionResult Edit() => PartialView();
	}
}
