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
	using Crm.Model.Lookups;
	using Crm.Service.Model;
	using Crm.Service.Model.Lookup;

	using Main;
	using Main.Controllers;

	using Microsoft.AspNetCore.Mvc;

	using NHibernate.Linq;

	public class InstallationListController : GenericListController<Installation>
	{
		public InstallationListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<Installation>> rssFeedProviders, IEnumerable<ICsvDefinition<Installation>> csvDefinitions, IEntityConfigurationProvider<Installation> entityConfigurationProvider, IRepository<Installation> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
			: base(pluginProvider, rssFeedProviders, csvDefinitions, entityConfigurationProvider, repository, appSettingsProvider, resourceManager, restTypeProvider)
		{
		}
		[RequiredPermission(PermissionName.Read, Group = ServicePlugin.PermissionGroup.Installation)]
		[RenderAction("InstallationItemTemplateActions", Priority = 100)]
		public virtual ActionResult ActionDetails()
		{
			return PartialView();
		}

		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.Installation)]
		public override ActionResult IndexTemplate() => base.IndexTemplate();

		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.Installation)]
		public override ActionResult FilterTemplate() => base.FilterTemplate();
		protected override string GetTitle()
		{
			return "Installations";
		}
		protected override string GetEmptySlate()
		{
			return resourceManager.GetTranslation("PleaseEnterSearchQuery");
		}

		[RequiredPermission(PermissionName.Create, Group = ServicePlugin.PermissionGroup.Installation)]
		public override ActionResult MaterialPrimaryAction()
		{
			return PartialView();
		}
		
		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.Installation)]
		public virtual ActionResult AutocompleteTemplate()
		{
			return PartialView();
		}

		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.Installation)]
		public override ActionResult MaterialMapItemTemplate() => PartialView();
		
		[RenderAction("TopSearchMaterialItemTemplate")]
		[RequiredPermission(nameof(Installation), Group = MainPlugin.PermissionGroup.TopSearch)]
		public virtual ActionResult TopSearchMaterialItemTemplate()
		{
			var model = GetGenericListTemplateViewModel();
			return View("Material/GenericListItem", model);
		}

		public class InstallationCsvDefinition : CsvDefinition<Installation>
		{
			private readonly ILookupManager lookupManager;
			public override IQueryable<Installation> Eager(IQueryable<Installation> query)
			{
				return query
						.Fetch(x => x.LocationCompany)
						.Fetch(x => x.PreferredUserObj)
						.Fetch(x => x.ResponsibleUserObject)
					;
			}

			public InstallationCsvDefinition(IResourceManager resourceManager, ILookupManager lookupManager)
				: base(resourceManager)
			{
				this.lookupManager = lookupManager;
			}
			public override IEnumerable<KeyValuePair<string, Func<Installation, object>>> GetCsv(IEnumerable<Installation> items) {
				var installationTypes = lookupManager.List<InstallationType>();
				var installationStatuses = lookupManager.List<InstallationHeadStatus>();
				var companyTypes = lookupManager.List<CompanyType>();

				Property("Id", x => x.Id);
				Property("InstallationNo", x => x.InstallationNo ?? string.Empty);
				Property("CreateDate", x => x.CreateDate.ToShortDateString());
				Property("Description", x => x.Description);
				Property("InstallationType", x => x.InstallationTypeKey.IsNotNullOrEmpty() ? installationTypes.FirstOrDefault(c => c.Key == x.InstallationTypeKey)?.Value : string.Empty);
				Property(
					"Company",
					x => (string.IsNullOrWhiteSpace(x.LocationCompany.LegacyId)
						? x.LocationCompany.Name
						: x.LocationCompany.LegacyName) +
					(x.LocationCompany.CompanyTypeKey.IsNotNullOrEmpty() && companyTypes.Any(t => t.Key == x.LocationCompany.CompanyTypeKey)
						? " - " + companyTypes.First(t => t.Key == x.LocationCompany.CompanyTypeKey).Value
						: string.Empty));
				Property("ManufactureDate", x => x.ManufactureDate.ToShortDateString());
				Property("KickOffDate", x => x.KickOffDate.ToShortDateString());
				Property("WarrantyFrom", x => x.WarrantyFrom.ToShortDateString());
				Property("WarrantyUntil", x => x.WarrantyUntil.ToShortDateString());
				Property("Status", x => x.StatusKey.IsNotNullOrEmpty() ? installationStatuses.FirstOrDefault(c => c.Key == x.StatusKey)?.Value : string.Empty);
				Property("BackgroundInfo", x => x.BackgroundInfo);
				Property("ResponsibleUser", x => x.ResponsibleUserObject != null ? x.ResponsibleUserObject.DisplayName : x.ResponsibleUser);
				Property("PreferredTechnician", x => x.PreferredUserObj != null ? x.PreferredUserObj.DisplayName : x.PreferredUser);
				
				//Internal Ids
				Property("InstallationTypeKey", x => x.InstallationTypeKey);
				Property("StatusKey", x => x.StatusKey);

				return base.GetCsv(items);
			}
		}
	}
}
