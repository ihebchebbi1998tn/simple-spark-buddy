namespace Main.Services
{
	using Crm.Library.AutoFac;
	using Crm.Library.Modularization.Menu;

	using Main.Services.Interfaces;

	public class MenuEntriesStartupSync : IAutoActivateDependency
	{
		public MenuEntriesStartupSync(IMenuEntryService menuEntryService, MenuProvider<MaterialMainMenu> materialMainMenuProvider)
		{
			menuEntryService.UpdateDbMenuEntries(materialMainMenuProvider.GetMenuEntries());
		}
	}
}
