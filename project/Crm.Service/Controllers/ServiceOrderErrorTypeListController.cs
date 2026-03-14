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
	using Crm.Library.Modularization;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services.Interfaces;
	using Crm.Service.Model;

	using Main.Controllers;

	using Microsoft.AspNetCore.Mvc;

	public class ServiceOrderErrorTypeListController : GenericListController<ServiceOrderErrorType>
	{
		public ServiceOrderErrorTypeListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<ServiceOrderErrorType>> rssFeedProviders, IEnumerable<ICsvDefinition<ServiceOrderErrorType>> csvDefinitions, IEntityConfigurationProvider<ServiceOrderErrorType> entityConfigurationProvider, IRepository<ServiceOrderErrorType> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
			: base(pluginProvider, rssFeedProviders, csvDefinitions, entityConfigurationProvider, repository, appSettingsProvider, resourceManager, restTypeProvider)
		{
		}
		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.ServiceOrderErrorType)]
		public override ActionResult FilterTemplate() => base.FilterTemplate();


		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.ServiceOrderErrorType)]
		public override ActionResult IndexTemplate() => base.IndexTemplate();


		[RequiredPermission(PermissionName.Edit, Group = ServicePlugin.PermissionGroup.ServiceOrderErrorType)]
		[RenderAction("ServiceOrderErrorTypeListActions", Priority = 200)]
		public virtual ActionResult ActionEdit()
		{
			return PartialView();
		}

		[RequiredPermission(PermissionName.Delete, Group = ServicePlugin.PermissionGroup.ServiceOrderErrorType)]
		[RenderAction("ServiceOrderErrorTypeListActions", Priority = 100)]
		public virtual ActionResult ActionDelete()
		{
			return PartialView();
		}

	}
}
