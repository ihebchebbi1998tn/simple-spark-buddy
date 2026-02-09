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

	public class ReplenishmentOrderItemListController : GenericListController<ReplenishmentOrderItem>
	{
		public ReplenishmentOrderItemListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<ReplenishmentOrderItem>> rssFeedProviders, IEnumerable<ICsvDefinition<ReplenishmentOrderItem>> csvDefinitions, IEntityConfigurationProvider<ReplenishmentOrderItem> entityConfigurationProvider, IRepository<ReplenishmentOrderItem> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
			: base(pluginProvider, rssFeedProviders, csvDefinitions, entityConfigurationProvider, repository, appSettingsProvider, resourceManager, restTypeProvider)
		{
		}
		protected override string GetEmptySlate()
		{
			return resourceManager.GetTranslation("ReplenishmentOrderHasNoItems");
		}

		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.ReplenishmentOrder)]
		public override ActionResult FilterTemplate()
		{
			return base.FilterTemplate();
		}

		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.ReplenishmentOrder)]
		public override ActionResult IndexTemplate()
		{
			var model = GetGenericListTemplateViewModel();
			return PartialView(model);
		}

		[RequiredPermission(ServicePlugin.PermissionName.CreateItem, Group = ServicePlugin.PermissionGroup.ReplenishmentOrder)]
		public override ActionResult MaterialPrimaryAction()
		{
			return PartialView();
		}

		[RenderAction("ReplenishmentOrderItemListTopMenu")]
		public virtual ActionResult TopMenuClose() => PartialView();
	}
}
