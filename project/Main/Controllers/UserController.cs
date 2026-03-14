namespace Main.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;
	using Crm.Library.Services.Interfaces;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Authorize]
	public class UserController : Controller
	{
		private readonly IAuthenticationService authenticationService;
		public UserController(IAuthenticationService authenticationService)
		{
			this.authenticationService = authenticationService;
		}
		[RequiredPermission(PermissionName.Create, Group = PermissionGroup.UserAdmin)]
		public virtual ActionResult Create() => PartialView();
		[RequiredPermission(PermissionName.Create, Group = PermissionGroup.UserAdmin)]
		[RenderAction("UserCreateFormExtendedDataBottom")]
		public virtual ActionResult CreatePasswordChangeRequired()
		{
			return authenticationService.PasswordChangeSupported ? PartialView() : new EmptyResult();
		}
		[RequiredPermission(MainPlugin.PermissionName.UserDetail, Group = PermissionGroup.UserAdmin)]
		public virtual ActionResult DetailsTemplate() => PartialView();
		[RenderAction("UserDetailsMaterialTabHeader", Priority = 90)]
		public virtual ActionResult DetailsTabHeader() => PartialView();
		[RenderAction("UserDetailsMaterialTab", Priority = 90)]
		public virtual ActionResult DetailsTab() => PartialView();
		public virtual ActionResult ResetPassword() => PartialView();
		[RequiredPermission(MainPlugin.PermissionName.AssignLicense, Group = PermissionGroup.UserAdmin)]
		[RequiredPermission(MainPlugin.PermissionName.RevokeLicense, Group = PermissionGroup.UserAdmin)]
		public virtual ActionResult AssignLicense() => PartialView();
		
		[RenderAction("UserDetailsMaterialTabHeader", Priority = 30)]
		public virtual ActionResult AssetsTabHeader() => PartialView();
		[RenderAction("UserDetailsMaterialTab", Priority = 30)]
		public virtual ActionResult AssetsTab() => PartialView();

		public virtual ActionResult AddAsset() => PartialView();
	}
}
