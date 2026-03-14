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
	
	public class UserSkillListController : GenericListController<UserSkill>
	{
		public UserSkillListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<UserSkill>> rssFeedProviders, IEnumerable<ICsvDefinition<UserSkill>> csvDefinitions, IEntityConfigurationProvider<UserSkill> entityConfigurationProvider, IRepository<UserSkill> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
			: base(pluginProvider, rssFeedProviders, csvDefinitions, entityConfigurationProvider, repository, appSettingsProvider, resourceManager, restTypeProvider)
		{
		}
	}
}
