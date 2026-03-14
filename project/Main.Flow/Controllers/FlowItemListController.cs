namespace Main.Flow.Controllers
{
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

	using Main.Controllers;
	using Main.Flow.Model;

	using Microsoft.AspNetCore.Mvc;

	public class FlowItemListController : GenericListController<FlowItem>
	{
		public FlowItemListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<FlowItem>> rssFeedProviders, IEnumerable<ICsvDefinition<FlowItem>> csvDefinitions, IEntityConfigurationProvider<FlowItem> entityConfigurationProvider, IRepository<FlowItem> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
			: base(pluginProvider,
				rssFeedProviders,
				csvDefinitions,
				entityConfigurationProvider,
				repository,
				appSettingsProvider,
				resourceManager,
				restTypeProvider)
		{
		}

		[RenderAction("FlowItemListTopMenu")]
		public virtual ActionResult TriggerFlowProcessingServiceTopMenu() => PartialView();
	}
}
