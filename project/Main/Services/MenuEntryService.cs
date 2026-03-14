namespace Main.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Modularization.Menu;

	using Main.Services.Interfaces;

	using DbMenuEntry = Main.Model.MenuEntry;

	public class MenuEntryService : IMenuEntryService
	{
		private readonly IRepositoryWithTypedId<DbMenuEntry, Guid> menuEntryRepository;
		private readonly Func<DbMenuEntry> menuEntryFactory;
		private const string MenuEntryModifyUser = nameof(MenuEntryService);

		public virtual void UpdateDbMenuEntries(List<MenuEntry> menuEntries)
		{
			foreach (var menuEntry in menuEntries)
			{
				var category = menuEntry.Category;
				var title = menuEntry.Title;
				var priority = menuEntry.Priority;
				var iconClass = menuEntry.IconClass;

				var menuEntryDb = menuEntryRepository
					.GetAll()
					.FirstOrDefault(x => x.Category == category && x.Title == title);
				if (menuEntryDb == null)
				{
					menuEntryDb = menuEntryFactory();
					menuEntryDb.Category = category;
					menuEntryDb.Title = title;
					menuEntryDb.IconClass = iconClass;
					menuEntryDb.Priority = priority;
					menuEntryDb.CreateUser = MenuEntryModifyUser;
					menuEntryDb.ModifyUser = MenuEntryModifyUser;
					menuEntryRepository.SaveOrUpdate(menuEntryDb);
				}
				else if (menuEntryDb.ModifyUser == MenuEntryModifyUser)
				{
					menuEntryDb.Priority = priority;
					menuEntryDb.IconClass = iconClass;
					menuEntryDb.ModifyUser = MenuEntryModifyUser;
					menuEntryRepository.SaveOrUpdate(menuEntryDb);
				}
			}

			foreach (var menuEntry in menuEntryRepository
				         .GetAll()
				         .ToList()
				         .Where(x => !menuEntries.Exists(y => y.Category == x.Category && y.Title == x.Title)))
			{
				menuEntryRepository.Delete(menuEntry);
			}
		}

		public virtual void SyncWithDbMenuEntryPriority(List<MenuEntry> menuEntries)
		{
			foreach (var menuEntry in menuEntries)
			{
				var category = menuEntry.Category;
				var title = menuEntry.Title;
				var menuEntryDb = menuEntryRepository
					.GetAll()
					.FirstOrDefault(x => x.Category == category && x.Title == title);
				if (menuEntryDb != null)
				{
					menuEntry.Priority = menuEntryDb.Priority;
				}
			}
		}

		public virtual DateTime GetLastModifyDate() =>
			menuEntryRepository
				.GetAll()
				.OrderByDescending(x => x.ModifyDate)
				.FirstOrDefault()?.ModifyDate ?? DateTime.MinValue;

		public MenuEntryService(
			IRepositoryWithTypedId<DbMenuEntry, Guid> menuEntryRepository,
			Func<DbMenuEntry> menuEntryFactory)
		{
			this.menuEntryRepository = menuEntryRepository;
			this.menuEntryFactory = menuEntryFactory;
		}
	}
}
