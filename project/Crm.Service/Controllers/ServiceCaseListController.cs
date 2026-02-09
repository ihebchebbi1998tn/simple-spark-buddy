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
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services.Interfaces;
	using Crm.Service.Model;
	using Crm.Service.Model.Lookup;

	using Main;
	using Main.Controllers;
	using Main.ViewModels;

	using Microsoft.AspNetCore.Mvc;

	using NHibernate.Linq;

	public class ServiceCaseListController : GenericListController<ServiceCase>
	{
		public ServiceCaseListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<ServiceCase>> rssFeedProviders, IEnumerable<ICsvDefinition<ServiceCase>> csvDefinitions, IEntityConfigurationProvider<ServiceCase> entityConfigurationProvider, IRepository<ServiceCase> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
			: base(pluginProvider, rssFeedProviders, csvDefinitions, entityConfigurationProvider, repository, appSettingsProvider, resourceManager, restTypeProvider)
		{
		}
		[RenderAction("ServiceCaseItemTemplateActions", Priority = 90)]
		public virtual ActionResult ActionComplete()
		{
			return PartialView();
		}

		[RenderAction("ServiceCaseItemTemplateActions", Priority = 100)]
		public virtual ActionResult ActionDetails()
		{
			return PartialView();
		}

		[RenderAction("ServiceCaseItemTemplateActions", Priority = 95)]
		public virtual ActionResult ActionEdit()
		{
			return PartialView();
		}
		protected override string GetTitle()
		{
			return "ServiceCases";
		}

		protected override string GetEmptySlate()
		{
			return resourceManager.GetTranslation("ServiceCaseListEmptySlate");
		}

		public class ServiceCaseCsvDefinition : CsvDefinition<ServiceCase>
		{
			private readonly ILookupManager lookupManager;
			public override IQueryable<ServiceCase> Eager(IQueryable<ServiceCase> query)
			{
				return query
					.Fetch(x => x.AffectedCompany)
					.Fetch(x => x.AffectedInstallation)
					;
			}

			public ServiceCaseCsvDefinition(IResourceManager resourceManager, ILookupManager lookupManager)
				: base(resourceManager)
			{
				this.lookupManager = lookupManager;
			}
			public override IEnumerable<KeyValuePair<string, Func<ServiceCase, object>>> GetCsv(IEnumerable<ServiceCase> items) {
				var serviceCaseStatuses = lookupManager.List<ServiceCaseStatus>();
				var servicePriorities = lookupManager.List<ServicePriority>();
				var serviceCaseCategories = lookupManager.List<ServiceCaseCategory>();

				Property("Id", x => x.Id);
				Property("ServiceCaseNo", x => x.ServiceCaseNo ?? string.Empty);
				Property("CreateDate", x => x.CreateDate);
				Property("ErrorMessage", x => x.ErrorMessage);
				Property("InstallationNo", x => x.AffectedInstallation != null ? x.AffectedInstallation.InstallationNo : string.Empty);
				Property("Status", x => x.StatusKey.ToString().IsNotNullOrEmpty() ? serviceCaseStatuses.FirstOrDefault(c => c.Key == x.StatusKey)?.Value : string.Empty);
				Property("Priority", x => x.PriorityKey.IsNotNullOrEmpty() ? servicePriorities.FirstOrDefault(c => c.Key == x.PriorityKey)?.Value : string.Empty);
				Property("AffectedCompanyKey", x => x.AffectedCompany);
				Property("ServiceCaseCategory", x => x.CategoryKey.IsNotNullOrEmpty() ? serviceCaseCategories.FirstOrDefault(c => c.Key == x.CategoryKey)?.Value : string.Empty);

				//Internal Ids
				Property("StatusKey", x=> x.StatusKey);
				Property("PriorityKey", x => x.PriorityKey);
				Property("CategoryKey", x => x.CategoryKey);

				return base.GetCsv(items);
			}
		}

		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.ServiceCase)]
		public override ActionResult FilterTemplate()
		{
			return base.FilterTemplate();
		}
		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.ServiceCase)]
		public override ActionResult IndexTemplate()
		{
			return base.IndexTemplate();
		}
		[RequiredPermission(PermissionName.Create, Group = ServicePlugin.PermissionGroup.ServiceCase)]
		public override ActionResult MaterialPrimaryAction()
		{
			return PartialView();
		}

		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.ServiceCase)]
		[RequiredPermission(nameof(ServiceCase), Group = PermissionGroup.MaterialDashboard)]
		public virtual ActionResult ServiceCaseDashboardList()
		{
			var model = new GenericListViewModel
			{
				EmptySlate = resourceManager.GetTranslation("ServiceCaseListEmptySlate"),
				IdentifierPropertyName = repository.IdentifierPropertyName,
				PluginName = "Crm.Service",
				Title = "ServiceCases",
				Type = typeof(ServiceCase),
				TypeName = GetTypeName(),
				OrderByProperties = OrderByProperties
			};
			return PartialView(model);
		}
		
		[RenderAction("TopSearchMaterialItemTemplate")]
		[RequiredPermission(nameof(ServiceCase), Group = MainPlugin.PermissionGroup.TopSearch)]
		public virtual ActionResult TopSearchMaterialItemTemplate()
		{
			var model = GetGenericListTemplateViewModel();
			return View("Material/GenericListItem", model);
		}
	}
}
