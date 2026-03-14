namespace Main.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Authorize]
	public class VisibilityController : Controller
	{
		[RequiredPermission(PermissionName.Edit, Group = PermissionGroup.Visibility)]
		public virtual ActionResult Edit()
		{
			return PartialView();
		}
		
		[RequiredPermission(PermissionName.Edit, Group = PermissionGroup.Visibility)]
		public virtual ActionResult Selection()
		{
			return PartialView();
		}
	}
}
