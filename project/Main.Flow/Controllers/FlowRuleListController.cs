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

	public class FlowRuleListController : GenericListController<FlowRule>
	{
		public FlowRuleListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<FlowRule>> rssFeedProviders, IEnumerable<ICsvDefinition<FlowRule>> csvDefinitions, IEntityConfigurationProvider<FlowRule> entityConfigurationProvider, IRepository<FlowRule> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
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

		[RenderAction("FlowRuleItemTemplateActions", Priority = 200)]
		public virtual ActionResult ActionEdit()
		{
			return PartialView();
		}

		[RenderAction("FlowRuleItemTemplateActions", Priority = 100)]
		public virtual ActionResult ActionDelete()
		{
			return PartialView();
		}

		public override ActionResult MaterialPrimaryAction()
		{
			return PartialView();
		}
	}
}
