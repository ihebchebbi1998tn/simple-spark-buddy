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

	public class ServiceOrderTimePostingListController : GenericListController<ServiceOrderTimePosting>
	{
		public ServiceOrderTimePostingListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<ServiceOrderTimePosting>> rssFeedProviders, IEnumerable<ICsvDefinition<ServiceOrderTimePosting>> csvDefinitions, IEntityConfigurationProvider<ServiceOrderTimePosting> entityConfigurationProvider, IRepository<ServiceOrderTimePosting> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
			: base(pluginProvider, rssFeedProviders, csvDefinitions, entityConfigurationProvider, repository, appSettingsProvider, resourceManager, restTypeProvider)
		{
		}
		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.ServiceOrderTimePosting)]
		public override ActionResult FilterTemplate() => base.FilterTemplate();

		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.ServiceOrderTimePosting)]
		[RenderAction("ServiceOrderTimePostingListFilterTemplate")]
		public virtual ActionResult JobFilterTemplate() => PartialView("ServiceOrderJobFilterTemplate");

		[RequiredPermission(ServicePlugin.PermissionName.DispatchRelink, Group = ServicePlugin.PermissionGroup.ServiceOrderTimePosting)]
		[RenderAction("DispatchTimePostingTemplateActions")]
		public virtual ActionResult DispatchRelinkTemplateAction() => PartialView();
	}
}
