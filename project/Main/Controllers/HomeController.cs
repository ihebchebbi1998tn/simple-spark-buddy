namespace Main.Controllers
{
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.MobileViewEngine;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;
	using Crm.Library.Services.Interfaces;

	using Main.Services.Interfaces;
	using Main.ViewModels;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Authorize]
	public class HomeController : Controller
	{
		private readonly IUserService userService;
		private readonly IBrowserCapabilities browserCapabilities;
		private readonly IEnumerable<IRedirectProvider> redirectProviders;
		public HomeController(IUserService userService, IBrowserCapabilities browserCapabilities, IEnumerable<IRedirectProvider> redirectProviders)
		{
			this.userService = userService;
			this.browserCapabilities = browserCapabilities;
			this.redirectProviders = redirectProviders;
		}

		[AllowAnonymous]
		public virtual ActionResult Index()
		{
			if (userService.CurrentUser == null)
			{
				return new RedirectResult("~/Main/Account/Login");
			}

			var redirects = redirectProviders.Select(x => x.Index(userService.CurrentUser, browserCapabilities)).Where(x => x != null).Distinct().ToArray();
			if (!redirects.Any())
			{
				return ClientSelection();
			}
			if (redirects.Length == 1)
			{
				var redirect = redirects.First();
				Request.RouteValues["plugin"] = redirect.Plugin;
				Request.RouteValues["controller"] = redirect.Controller;
				Request.RouteValues["action"] = redirect.Action;
				return redirect.ActionResult.Invoke(this);
			}
			return ClientSelection();
		}
		public virtual ActionResult ClientSelection()
		{
			var redirects = redirectProviders.Select(x => x.Index(userService.CurrentUser, browserCapabilities)).Where(x => x != null).Distinct().ToArray();
			var model = new ClientSelectionViewModel
			{
				RedirectProviderResults = redirects.ToList()
			};
			return View("ClientSelection", model);
		}
		public virtual ActionResult MaterialIndex()
		{
			var model = new CrmModel();
			if (!Request.Query.ContainsKey("token"))
			{
				model.CacheManifest = "material.appcache";
			}
			return View("MaterialIndex", model);
		}
		public virtual ActionResult Startup()
		{
			return View("Startup", new CrmModel());
		}
		[RenderAction("MaterialTopMenu", Priority = 15)]
		public virtual ActionResult MaterialTopMenu() => PartialView();
		[RenderAction("MaterialModal", Priority = 10)]
		[RequiredPermission(PermissionName.View, Group = MainPlugin.PermissionGroup.TopSearch)]
		public virtual ActionResult TopSearchModal() => PartialView();
		[RenderAction("MaterialTopMenu", Priority = 10)]
		[RequiredPermission(PermissionName.View, Group = MainPlugin.PermissionGroup.TopSearch)]
		public virtual ActionResult TopSearchTopMenu() => PartialView();
	}
}
