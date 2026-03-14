namespace Crm.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Modularization;

	using Main;

	using Microsoft.AspNetCore.Mvc;

	public class UserGroupController : Controller
	{
		[RequiredPermission(MainPlugin.PermissionName.AddUserGroup, Group = PermissionGroup.UserAdmin)]
		public virtual ActionResult Create()
		{
			return PartialView();
		}
		public virtual ActionResult DetailsTemplate() => PartialView();
		[RenderAction("UserGroupDetailsMaterialTabHeader", Priority = 90)]
		public virtual ActionResult DetailsTabHeader() => PartialView();
		[RenderAction("UserGroupDetailsMaterialTab", Priority = 90)]
		public virtual ActionResult DetailsTab() => PartialView();
	}
}
