namespace Main.Controllers
{
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.AutoFac;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.EntityConfiguration;
	using Crm.Library.EntityConfiguration.Interfaces;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Helper;
	using Crm.Library.Modularization.Extensions;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services.Interfaces;

	using Main.ViewModels;

	using Microsoft.AspNetCore.Mvc;

	public abstract class GenericListController<TItem> : RestController, IDependency
		where TItem : class
	{
		protected readonly IRssFeedProvider<TItem> rssFeedProvider;
		protected readonly IEnumerable<SortDefinition> OrderByProperties;
		protected readonly IEnumerable<FilterDefinition> FilterProperties;
		protected readonly IRepository<TItem> repository;
		protected readonly IEnumerable<ICsvDefinition<TItem>> csvDefinitions;
		protected readonly IAppSettingsProvider appSettingsProvider;
		protected readonly IPluginProvider pluginProvider;
		protected readonly IResourceManager resourceManager;

		protected GenericListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<TItem>> rssFeedProviders, IEnumerable<ICsvDefinition<TItem>> csvDefinitions, IEntityConfigurationProvider<TItem> entityConfigurationProvider, IRepository<TItem> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
			: base(restTypeProvider)
		{
			this.pluginProvider = pluginProvider;
			rssFeedProvider = pluginProvider.SortByPluginDependencyDescending(rssFeedProviders).FirstOrDefault();
			this.csvDefinitions = csvDefinitions;
			this.repository = repository;
			this.appSettingsProvider = appSettingsProvider;
			this.resourceManager = resourceManager;

			OrderByProperties = entityConfigurationProvider.SortableProperties;
			FilterProperties = entityConfigurationProvider.FilterableProperties;
		}

		public virtual ActionResult GenericListHeader()
		{
			var model = GetGenericListTemplateViewModel();
			model.Title = null;
			return PartialView("Material/GenericListHeader", model);
		}

		protected virtual GenericListViewModel GetGenericListViewModel()
		{
			var model = GetGenericListTemplateViewModel();
			model.EmptySlate = GetEmptySlate();
			return model;
		}

		protected virtual GenericListViewModel GetGenericListTemplateViewModel()
		{
			var model = new GenericListViewModel
			{
				IdentifierPropertyName = repository.IdentifierPropertyName,
				Title = GetTitle(),
				Type = typeof(TItem),
				TypeName = GetTypeName(),
				PluginName = pluginProvider.FindPluginDescriptor(GetType())?.PluginName,
				OrderByProperties = OrderByProperties,
				FilterProperties = FilterProperties,
				MapTileLayerUrl = appSettingsProvider.GetValue(MainPlugin.Settings.System.Maps.MapTileLayerUrl),
				IsCsvExportable = csvDefinitions.Any(),
				IsRssSubscribable = rssFeedProvider != null
			};
			return model;
		}

		public virtual ActionResult IndexTemplate()
		{
			var model = GetGenericListTemplateViewModel();
			return PartialView("Material/GenericList", model);
		}

		public virtual ActionResult FilterTemplate()
		{
			var model = GetGenericListTemplateViewModel();
			return PartialView("Material/GenericListRightNav", model);
		}

		public virtual ActionResult MaterialItemTemplate()
		{
			var model = GetGenericListTemplateViewModel();
			return View(model);
		}

		public virtual ActionResult MaterialItemThumbnail()
		{
			return PartialView();
		}

		public virtual ActionResult MaterialPrimaryAction()
		{
			return new EmptyResult();
		}

		protected virtual string GetTypeName()
		{
			return typeof(TItem).Name;
		}

		protected virtual string GetTitle()
		{
			return typeof(TItem).Name;
		}

		protected virtual string GetEmptySlate()
		{
			return resourceManager.GetTranslation($"{typeof(TItem).Name}EmptySlate");
		}

		public virtual ActionResult MaterialMapItemTemplate()
		{
			return new EmptyResult();
		}

		public virtual ActionResult Selection()
		{
			var model = GetGenericListTemplateViewModel();
			return PartialView("Material/GenericListSelection", model);
		}
	}
}
