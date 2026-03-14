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

	using Main.Controllers;

	using Microsoft.AspNetCore.Mvc;

	public class ServiceOrderTimeListController : GenericListController<ServiceOrderTime>
	{
		public ServiceOrderTimeListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<ServiceOrderTime>> rssFeedProviders, IEnumerable<ICsvDefinition<ServiceOrderTime>> csvDefinitions, IEntityConfigurationProvider<ServiceOrderTime> entityConfigurationProvider, IRepository<ServiceOrderTime> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
			: base(pluginProvider, rssFeedProviders, csvDefinitions, entityConfigurationProvider, repository, appSettingsProvider, resourceManager, restTypeProvider)
		{
		}
		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.ServiceOrderTime)]
		public override ActionResult FilterTemplate() => base.FilterTemplate();
	}
}
