namespace Main.Services
{
	using Crm.Library.MobileViewEngine;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.Interfaces;
	using Crm.Library.Model.Authorization.PermissionIntegration;

	using Main.Services.Interfaces;

	using Microsoft.AspNetCore.Mvc;

	public class MaterialRedirectProvider : IRedirectProvider
	{
		private readonly IAuthorizationManager authorizationManager;
		public MaterialRedirectProvider(IAuthorizationManager authorizationManager)
		{
			this.authorizationManager = authorizationManager;
		}

		public virtual string AvailableClients(User user)
		{
			if (authorizationManager.IsAuthorizedForAction(user, PermissionGroup.Login, PermissionName.MaterialClientOnline))
			{
				return PermissionName.MaterialClientOnline;
			}
			return null;
		}

		public virtual RedirectProviderResult Index(User user, IBrowserCapabilities browserCapabilities)
		{
			if (authorizationManager.IsAuthorizedForAction(user, PermissionGroup.Login, PermissionName.MaterialClientOnline))
			{
				return new RedirectProviderResult
				{
					Name = "MaterialClientOnline",
					Plugin = "Main",
					Controller = "Home",
					Action = "MaterialIndex",
					ActionResult = x => x.MaterialIndex(),
					Hash = "/Main/Home/Startup/online",
					Icon = "home"
				};
			}
			return null;
		}
		public virtual ActionResult RedirectAfterLogin(User user, IBrowserCapabilities browserCapabilities, string returnUrl)
		{
			if (authorizationManager.IsAuthorizedForAction(user, PermissionGroup.Login, PermissionName.MaterialClientOnline))
			{
				return new RedirectResult("~/Main/Home/MaterialIndex");
			}
			return null;
		}
	}
}
