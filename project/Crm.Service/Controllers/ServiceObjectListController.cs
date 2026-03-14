namespace Crm.Service.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.EntityConfiguration;
	using Crm.Library.EntityConfiguration.Interfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services.Interfaces;
	using Crm.Service.Model;
	using Crm.Service.Model.Lookup;

	using Main.Controllers;

	using Microsoft.AspNetCore.Mvc;

	using NHibernate.Linq;

	public class ServiceObjectListController : GenericListController<ServiceObject>
	{
		public ServiceObjectListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<ServiceObject>> rssFeedProviders, IEnumerable<ICsvDefinition<ServiceObject>> csvDefinitions, IEntityConfigurationProvider<ServiceObject> entityConfigurationProvider, IRepository<ServiceObject> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
			: base(pluginProvider, rssFeedProviders, csvDefinitions, entityConfigurationProvider, repository, appSettingsProvider, resourceManager, restTypeProvider)
		{
		}
		[RequiredPermission(PermissionName.Read, Group = ServicePlugin.PermissionGroup.ServiceObject)]
		[RenderAction("ServiceObjectItemTemplateActions", Priority = 100)]
		public virtual ActionResult ActionDetails()
		{
			return PartialView();
		}

		protected override string GetTitle()
		{
			return "ServiceObjects";
		}
		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.ServiceObject)]
		public override ActionResult FilterTemplate() => base.FilterTemplate();
		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.ServiceObject)]
		public override ActionResult IndexTemplate() => base.IndexTemplate();

		[RequiredPermission(PermissionName.Create, Group = ServicePlugin.PermissionGroup.ServiceObject)]
		public override ActionResult MaterialPrimaryAction()
		{
			return PartialView();
		}
		
		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.ServiceObject)]
		public override ActionResult MaterialMapItemTemplate() => PartialView();

		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.ServiceObject)]
		public override ActionResult Selection() => base.Selection();

		public class ServiceObjectCsvDefinition : CsvDefinition<ServiceObject>
		{
			private readonly ILookupManager lookupManager;
			public override IQueryable<ServiceObject> Eager(IQueryable<ServiceObject> query)
			{
				return query.Fetch(x => x.ResponsibleUserObject);
			}

			public ServiceObjectCsvDefinition(IResourceManager resourceManager, ILookupManager lookupManager)
				: base(resourceManager)
			{
				this.lookupManager = lookupManager;
			}
			public override IEnumerable<KeyValuePair<string, Func<ServiceObject, object>>> GetCsv(IEnumerable<ServiceObject> items) {
				var serviceObjectCategories = lookupManager.List<ServiceObjectCategory>();

				Property("Id", x => x.Id);
				Property("ObjectNo", x => x.ObjectNo ?? string.Empty);
				Property("Name", x => x.Name ?? string.Empty);
				Property("BackgroundInfo", x => x.BackgroundInfo ?? string.Empty);
				Property("CreateDate", x => x.CreateDate.ToShortDateString());
				Property("ServiceObjectCategory", x => x.CategoryKey.IsNotNullOrEmpty() ? serviceObjectCategories.FirstOrDefault(c => c.Key == x.CategoryKey)?.Value : string.Empty);
				Property("ResponsibleUser", x => x.ResponsibleUserObject != null ? x.ResponsibleUserObject.DisplayName : x.ResponsibleUser);

				//Internal Ids
				Property("CategoryKey", x => x.CategoryKey);

				return base.GetCsv(items);
			}
		}
	}
}
