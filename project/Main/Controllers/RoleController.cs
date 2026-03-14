namespace Main.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Modularization;

	using Main.ViewModels;

	using Microsoft.AspNetCore.Mvc;

	public class RoleController : Controller
	{
		[RequiredPermission(MainPlugin.PermissionName.AssignRole, Group = PermissionGroup.UserAdmin)]
		public virtual ActionResult Assign() => PartialView();
		[RequiredPermission(MainPlugin.PermissionName.CreateRole, Group = PermissionGroup.UserAdmin)]
		public virtual ActionResult Create() => PartialView();
		[RequiredPermission(MainPlugin.PermissionName.EditRole, Group = PermissionGroup.UserAdmin)]
		public virtual ActionResult Details() => PartialView();
		[RenderAction("RoleDetailsTab", Priority = 100)]
		public virtual ActionResult DetailsTab() => PartialView();
		[RenderAction("RoleDetailsTabHeader", Priority = 100)]
		public virtual ActionResult DetailsTabHeader() => PartialView();
		[RenderAction("RoleDetailsTab", Priority = 90)]
		public virtual ActionResult UsersTab()
		{
			var model = new GenericListViewModel
			{
				Type = typeof(User),
				FilterRoute = "Main/UserList/FilterTemplate"
			};
			return PartialView(model);
		}
		[RenderAction("RoleDetailsTabHeader", Priority = 90)]
		public virtual ActionResult UsersTabHeader() => PartialView();
	}
}
