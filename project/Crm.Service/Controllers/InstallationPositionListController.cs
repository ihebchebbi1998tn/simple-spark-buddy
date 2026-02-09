namespace Crm.Service.Controllers
{
	using System.Collections.Generic;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.EntityConfiguration;
	using Crm.Library.EntityConfiguration.Interfaces;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services.Interfaces;
	using Crm.Service.Model;
	using Microsoft.AspNetCore.Mvc;

	using Main.Controllers;

	using Microsoft.AspNetCore.Authorization;
	[Authorize]
	public class InstallationPositionListController : GenericListController<InstallationPos>
	{
		public InstallationPositionListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<InstallationPos>> rssFeedProviders, IEnumerable<ICsvDefinition<InstallationPos>> csvDefinitions, IEntityConfigurationProvider<InstallationPos> entityConfigurationProvider, IRepository<InstallationPos> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider) : base(pluginProvider, rssFeedProviders, csvDefinitions, entityConfigurationProvider, repository, appSettingsProvider, resourceManager, restTypeProvider)
		{
		}

		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.InstallationPosition)]
		public override ActionResult FilterTemplate()
		{
			return base.FilterTemplate();
		}
	}
}
