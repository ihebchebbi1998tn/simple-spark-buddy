namespace Main.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Authorize]
	public class BarcodeController : Controller
	{
		[RequiredPermission(PermissionName.Settings, Group = MainPlugin.PermissionGroup.UserAccount)]
		public virtual ActionResult Preview() => PartialView();
		
	}
}
