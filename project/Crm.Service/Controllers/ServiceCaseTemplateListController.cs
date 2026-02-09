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

	public class ServiceCaseTemplateListController : GenericListController<ServiceCaseTemplate>
	{
		[RequiredPermission(PermissionName.Read, Group = ServicePlugin.PermissionGroup.ServiceCaseTemplate)]
		[RenderAction("ServiceCaseTemplateItemTemplateActions", Priority = 100)]
		public virtual ActionResult ActionDetails()
		{
			return PartialView();
		}
		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.ServiceCaseTemplate)]
		public override ActionResult FilterTemplate()
		{
			return base.FilterTemplate();
		}
		protected override string GetTitle()
		{
			return "ServiceCaseTemplates";
		}
		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.ServiceCaseTemplate)]
		public override ActionResult IndexTemplate()
		{
			return base.IndexTemplate();
		}
		[RequiredPermission(PermissionName.Create, Group = ServicePlugin.PermissionGroup.ServiceCaseTemplate)]
		public override ActionResult MaterialPrimaryAction()
		{
			return PartialView();
		}
		public ServiceCaseTemplateListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<ServiceCaseTemplate>> rssFeedProviders, IEnumerable<ICsvDefinition<ServiceCaseTemplate>> csvDefinitions, IEntityConfigurationProvider<ServiceCaseTemplate> entityConfigurationProvider, IRepository<ServiceCaseTemplate> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
			: base(pluginProvider, rssFeedProviders, csvDefinitions, entityConfigurationProvider, repository, appSettingsProvider, resourceManager, restTypeProvider)
		{
		}
	}
}
