namespace Main.Controllers
{
	using System.Linq;

	using Crm.Library.Model.Authorization.Interfaces;
	using Crm.Library.Modularization.Menu;
	using Crm.Library.Services.Interfaces;

	using Main.Services.Interfaces;
	using Main.ViewModels;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Authorize]
	public class MenuController : Controller
	{
		private readonly MenuProvider<MaterialMainMenu> materialMainMenuProvider;
		private readonly MenuProvider<MaterialUserProfileMenu> materialUserProfileMenuProvider;
		private readonly IUserService userService;
		private readonly IAuthorizationManager authorizationManager;
		private readonly IMenuEntryService menuEntryService;

		public virtual ActionResult MaterialMainMenu()
		{
			var user = userService.CurrentUser;
			var menuEntries = materialMainMenuProvider.GetMenuEntriesVisibleToUser(user, authorizationManager).ToList();
			menuEntryService.SyncWithDbMenuEntryPriority(menuEntries);

			return PartialView("MaterialMenu", new CrmModelList<MenuEntry> { List = menuEntries });
		}
		public virtual ActionResult MaterialUserProfileMenu()
		{
			var user = userService.CurrentUser;
			var menuEntries = materialUserProfileMenuProvider.GetMenuEntriesVisibleToUser(user, authorizationManager).ToList();

			return PartialView("MaterialMenu", new CrmModelList<MenuEntry> { List = menuEntries });
		}
		public MenuController(IUserService userService, MenuProvider<MaterialMainMenu> materialMainMenuProvider, MenuProvider<MaterialUserProfileMenu> materialUserProfileMenuProvider, IAuthorizationManager authorizationManager, IMenuEntryService menuEntryService)
		{
			this.userService = userService;
			this.materialMainMenuProvider = materialMainMenuProvider;
			this.materialUserProfileMenuProvider = materialUserProfileMenuProvider;
			this.authorizationManager = authorizationManager;
			this.menuEntryService = menuEntryService;
		}
	}
}
