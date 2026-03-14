namespace Main.Controllers
{
	using System;
	using System.Collections.Generic;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.EntityConfiguration;
	using Crm.Library.EntityConfiguration.Interfaces;
	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Helper;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services.Interfaces;

	using Main.ViewModels;

	using Microsoft.AspNetCore.Mvc;

	public class LookupDetailsListController : GenericListController<ILookup>
	{
		protected override GenericListViewModel GetGenericListTemplateViewModel()
		{
			var model = new GenericListViewModel
			{
				Title = "Lookup",
				Type = typeof(Type),
				TypeName = "Lookup",
				TypeNameOverride = "LookupDetails",
				PluginName = "Main",
				FilterProperties = new List<FilterDefinition>() { new FilterDefinition() { AllowFilter = true, Caption = "Key", PropertyInfo = typeof(ILookup).GetProperty(nameof(ILookup.Key)) }, new FilterDefinition() { AllowFilter = true, Caption = "Value", PropertyInfo = typeof(ILookup).GetProperty(nameof(ILookup.Value)) } }
			};
			return model;
		}
		public override ActionResult MaterialPrimaryAction()
		{
			return PartialView();
		}
		public LookupDetailsListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<ILookup>> rssFeedProviders, IEnumerable<ICsvDefinition<ILookup>> csvDefinitions, IEntityConfigurationProvider<ILookup> entityConfigurationProvider, IRepository<ILookup> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
			: base(pluginProvider, rssFeedProviders, csvDefinitions, entityConfigurationProvider, repository, appSettingsProvider, resourceManager, restTypeProvider)
		{
		}
	}
}
