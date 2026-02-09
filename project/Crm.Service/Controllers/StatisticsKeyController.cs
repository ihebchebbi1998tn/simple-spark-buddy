namespace Crm.Service.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Microsoft.AspNetCore.Mvc;

	public class StatisticsKeyController : Controller
	{
		public StatisticsKeyController()
		{
		}

		[RequiredPermission(PermissionName.View, Group = ServicePlugin.PermissionGroup.StatisticsKey)]
		public virtual ActionResult InfoTemplate()
		{
			return PartialView();
		}

		[RequiredPermission(PermissionName.Edit, Group = ServicePlugin.PermissionGroup.StatisticsKey)]
		public virtual ActionResult EditTemplate()
		{
			return PartialView();
		}
	}
}
