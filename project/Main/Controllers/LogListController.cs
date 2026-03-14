namespace Main.Controllers
{
	using System.Collections.Generic;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.EntityConfiguration;
	using Crm.Library.EntityConfiguration.Interfaces;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Helper;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services.Interfaces;

	using Main.Model;

	public class LogListController : GenericListController<Log>
	{
		protected override string GetTitle()
		{
			return "LogEntries";
		}
		protected override string GetEmptySlate()
		{
			return resourceManager.GetTranslation("LogEmptySlate");
		}
		public LogListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<Log>> rssFeedProviders, IEnumerable<ICsvDefinition<Log>> csvDefinitions, IEntityConfigurationProvider<Log> entityConfigurationProvider, IRepository<Log> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
			: base(pluginProvider, rssFeedProviders, csvDefinitions, entityConfigurationProvider, repository, appSettingsProvider, resourceManager, restTypeProvider)
		{
		}
	}
}
