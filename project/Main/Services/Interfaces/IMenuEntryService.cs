namespace Main.Services.Interfaces
{
	using System;
	using System.Collections.Generic;

	using Crm.Library.AutoFac;
	using Crm.Library.Modularization.Menu;

	public interface IMenuEntryService: IDependency
	{
		void UpdateDbMenuEntries(List<MenuEntry> menuEntries);
		void SyncWithDbMenuEntryPriority(List<MenuEntry> menuEntries);
		DateTime GetLastModifyDate();
	}
}
