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
	
	public class UserAssetListController : GenericListController<UserAsset>
	{
		public UserAssetListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<UserAsset>> rssFeedProviders, IEnumerable<ICsvDefinition<UserAsset>> csvDefinitions, IEntityConfigurationProvider<UserAsset> entityConfigurationProvider, IRepository<UserAsset> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
			: base(pluginProvider, rssFeedProviders, csvDefinitions, entityConfigurationProvider, repository, appSettingsProvider, resourceManager, restTypeProvider)
		{
		}
	}
}
