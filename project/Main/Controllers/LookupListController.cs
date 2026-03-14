namespace Main.Controllers
{
	using System;
	using System.Collections.Generic;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.EntityConfiguration;
	using Crm.Library.EntityConfiguration.Interfaces;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Helper;
	using Crm.Library.Modularization;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services.Interfaces;

	using Main.Rest.Model;
	using Main.ViewModels;

	using Microsoft.AspNetCore.Mvc;

	public class LookupListController : GenericListController<Type>
	{
		public LookupListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<Type>> rssFeedProviders, IEnumerable<ICsvDefinition<Type>> csvDefinitions, IEntityConfigurationProvider<Type> entityConfigurationProvider, IRepository<Type> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
			: base(pluginProvider, rssFeedProviders, csvDefinitions, entityConfigurationProvider, repository, appSettingsProvider, resourceManager, restTypeProvider)
		{
		}
		protected override GenericListViewModel GetGenericListTemplateViewModel()
		{
			var model = new GenericListViewModel
			{
				Title = "Lookups",
				Type = typeof(Type),
				TypeName = "Lookup",
				TypeNameOverride = "Lookup",
				PluginName = "Main",
				FilterProperties = new List<FilterDefinition>()
				{
					new FilterDefinition()
					{
						AllowFilter = true, 
						Caption = "Name", 
						PropertyInfo = typeof(LookupType).GetProperty(nameof(LookupType.Name))
					},
					new FilterDefinition()
					{
						AllowFilter = true, 
						Caption = "Type", 
						PropertyInfo = typeof(LookupType).GetProperty(nameof(LookupType.FullName))
					}
				}
			};
			return model;
		}

		[RenderAction("LookupDetailsListTopMenu")]
		[RenderAction("LookupListTopMenu")]
		public virtual ActionResult RefreshLookupCacheTopMenu()
		{
			return PartialView();
		}
	}
}
