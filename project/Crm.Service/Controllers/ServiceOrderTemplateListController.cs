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
	using Main.ViewModels;

	using Microsoft.AspNetCore.Mvc;

	public class ServiceOrderTemplateListController : GenericListController<ServiceOrderHead>
	{
		private readonly IEntityConfigurationProvider<ServiceOrderTemplate> serviceOrderTemplateEntityConfigurationProvider;

		public ServiceOrderTemplateListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<ServiceOrderHead>> rssFeedProviders, IEnumerable<ICsvDefinition<ServiceOrderHead>> csvDefinitions, IEntityConfigurationProvider<ServiceOrderHead> entityConfigurationProvider, IRepository<ServiceOrderHead> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider, IEntityConfigurationProvider<ServiceOrderTemplate> serviceOrderTemplateEntityConfigurationProvider)
			: base(pluginProvider, rssFeedProviders, csvDefinitions, entityConfigurationProvider, repository, appSettingsProvider, resourceManager, restTypeProvider)
		{
			this.serviceOrderTemplateEntityConfigurationProvider = serviceOrderTemplateEntityConfigurationProvider;
		}
		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.ServiceOrderTemplate)]
		public override ActionResult FilterTemplate()
		{
			return base.FilterTemplate();
		}

		protected override string GetEmptySlate()
		{
			return resourceManager.GetTranslation("NoServiceOrderTemplates");
		}

		protected override GenericListViewModel GetGenericListTemplateViewModel()
		{
			var model = base.GetGenericListTemplateViewModel();
			model.FilterProperties = serviceOrderTemplateEntityConfigurationProvider.FilterableProperties;
			model.OrderByProperties = serviceOrderTemplateEntityConfigurationProvider.SortableProperties;
			model.TypeNameOverride = "ServiceOrderTemplate";
			return model;
		}

		protected override string GetTitle()
		{
			return "ServiceOrderTemplates";
		}

		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.ServiceOrderTemplate)]
		public override ActionResult IndexTemplate()
		{
			return base.IndexTemplate();
		}

		[RequiredPermission(PermissionName.Create, Group = ServicePlugin.PermissionGroup.ServiceOrderTemplate)]
		public override ActionResult MaterialPrimaryAction()
		{
			return PartialView();
		}
		
		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.ServiceOrderTemplate)]
		public override ActionResult Selection() => base.Selection();
	}
}
