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

	public class ServiceOrderExpensePostingListController : GenericListController<ServiceOrderExpensePosting>
	{
		public ServiceOrderExpensePostingListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<ServiceOrderExpensePosting>> rssFeedProviders, IEnumerable<ICsvDefinition<ServiceOrderExpensePosting>> csvDefinitions, IEntityConfigurationProvider<ServiceOrderExpensePosting> entityConfigurationProvider, IRepository<ServiceOrderExpensePosting> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
			: base(pluginProvider, rssFeedProviders, csvDefinitions, entityConfigurationProvider, repository, appSettingsProvider, resourceManager, restTypeProvider)
		{
		}
		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.ServiceOrderTimePosting)]
		public override ActionResult FilterTemplate() => base.FilterTemplate();


		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.ServiceOrderExpensePosting)]
		public override ActionResult IndexTemplate() => base.IndexTemplate();

		[RequiredPermission(PermissionName.Edit, Group = ServicePlugin.PermissionGroup.ServiceOrderExpensePosting)]
		[RenderAction("ServiceOrderExpensePostingsListActions", Priority = 200)]
		public virtual ActionResult ActionEdit()
		{
			return PartialView();
		}

		[RequiredPermission(PermissionName.Delete, Group = ServicePlugin.PermissionGroup.ServiceOrderExpensePosting)]
		[RenderAction("ServiceOrderExpensePostingsListActions", Priority = 100)]
		public virtual ActionResult ActionDelete()
		{
			return PartialView();
		}
	}
}
