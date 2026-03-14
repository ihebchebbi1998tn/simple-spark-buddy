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
	using Crm.Service.Enums;
	using Crm.Service.Model;
	using Crm.Service.Model.Lookup;

	using Main;
	using Main.Controllers;

	using Microsoft.AspNetCore.Mvc;

	using NHibernate.Linq;

	public class ServiceOrderHeadListController : GenericListController<ServiceOrderHead>
	{
		public ServiceOrderHeadListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<ServiceOrderHead>> rssFeedProviders, IEnumerable<ICsvDefinition<ServiceOrderHead>> csvDefinitions, IEntityConfigurationProvider<ServiceOrderHead> entityConfigurationProvider, IRepository<ServiceOrderHead> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
			: base(pluginProvider,
				rssFeedProviders,
				csvDefinitions,
				entityConfigurationProvider,
				repository,
				appSettingsProvider,
				resourceManager,
				restTypeProvider)
		{
		}
		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.ServiceOrder)]
		public override ActionResult FilterTemplate() => base.FilterTemplate();

		protected override string GetTypeName()
		{
			return "ServiceOrder";
		}
		protected override string GetTitle()
		{
			return "SearchServiceOrders";
		}
		protected override string GetEmptySlate()
		{
			return resourceManager.GetTranslation("PleaseEnterSearchQuery");
		}

		[RequiredPermission(PermissionName.Read, Group = ServicePlugin.PermissionGroup.ServiceOrder), RenderAction("ServiceOrderItemTemplateActions", Priority = 65)]
		public virtual ActionResult TemplateActionDetails() => PartialView();

		[RequiredPermission(PermissionName.Create, Group = ServicePlugin.PermissionGroup.Dispatch), RenderAction("ServiceOrderItemTemplateActions", Priority = 60)]
		public virtual ActionResult TemplateActionSchedule() => PartialView();

		[RenderAction("ServiceOrderItemTemplateActions", Priority = 55)]
		public virtual ActionResult TemplateActionDivider() => PartialView("ListDivider");

		[RequiredPermission(PermissionName.Edit, Group = ServicePlugin.PermissionGroup.ServiceOrder), RenderAction("ServiceOrderItemTemplateActions", Priority = 50)]
		public virtual ActionResult TemplateActionClose() => PartialView();

		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.ServiceOrder)]
		public override ActionResult IndexTemplate() => base.IndexTemplate();

		[RequiredPermission(PermissionName.Create, Group = ServicePlugin.PermissionGroup.ServiceOrder)]
		public override ActionResult MaterialPrimaryAction()
		{
			return PartialView();
		}

		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.ServiceOrder)]
		public override ActionResult MaterialMapItemTemplate() => PartialView();

		[RenderAction("TopSearchMaterialItemTemplate")]
		[RequiredPermission(nameof(ServiceOrderHead), Group = MainPlugin.PermissionGroup.TopSearch)]
		public virtual ActionResult TopSearchMaterialItemTemplate()
		{
			var model = GetGenericListTemplateViewModel();
			return View("Material/GenericListItem", model);
		}
		
		public class ServiceOrderHeadCsvDefinition : CsvDefinition<ServiceOrderHead>
		{
			private readonly ILookupManager lookupManager;
			private readonly IAppSettingsProvider appSettingsProvider;
			public override IQueryable<ServiceOrderHead> Eager(IQueryable<ServiceOrderHead> query)
			{
				return query
					.Fetch(x => x.AffectedInstallation)
					.Fetch(x => x.CustomerContact)
					.Fetch(x => x.Initiator)
					.Fetch(x => x.PreferredTechnician)
					.Fetch(x => x.PreferredTechnicianUsergroup)
					.Fetch(x => x.ResponsibleUserObject)
					.Fetch(x => x.ServiceCase)
					.Fetch(x => x.ServiceContract)
					.Fetch(x => x.UserGroup)
					;
			}

			public ServiceOrderHeadCsvDefinition(IResourceManager resourceManager, ILookupManager lookupManager, IAppSettingsProvider appSettingsProvider)
				: base(resourceManager)
			{
				this.lookupManager = lookupManager;
				this.appSettingsProvider = appSettingsProvider;
			}
			public override IEnumerable<KeyValuePair<string, Func<ServiceOrderHead, object>>> GetCsv(IEnumerable<ServiceOrderHead> items) {
				var serviceOrderTypes = lookupManager.List<ServiceOrderType>();
				var serviceOrderStatuses = lookupManager.List<ServiceOrderStatus>();
				var servicePriorities = lookupManager.List<ServicePriority>();

				Property("Id", x => x.Id);
				Property("OrderNo", x => x.OrderNo ?? string.Empty);
				Property("CreateDate", x => x.CreateDate);
				Property("Reported", x => x.Reported);
				Property("PurchaseOrderNo", x => x.PurchaseOrderNo);
				var maintenanceOrderGenerationMode = appSettingsProvider.GetValue(ServicePlugin.Settings.ServiceContract.MaintenanceOrderGenerationMode);
				if (maintenanceOrderGenerationMode == MaintenanceOrderGenerationMode.JobPerInstallation)
				{
					Property("InstallationNo", x => x.ServiceOrderTimes.Where(t => t.InstallationId.HasValue).Select(t => t.Installation.InstallationNo).Join(", "));
				}
				else
				{
					Property("InstallationNo", x => x.AffectedInstallation?.InstallationNo);
				}
				Property("T_Type", x => x.TypeKey.IsNotNullOrEmpty() ? serviceOrderTypes.FirstOrDefault(c => c.Key == x.TypeKey)?.Value : string.Empty);
				Property("Status", x => x.StatusKey.IsNotNullOrEmpty() ? serviceOrderStatuses.FirstOrDefault(c => c.Key == x.StatusKey)?.Value : string.Empty);
				Property("Priority", x => x.PriorityKey.IsNotNullOrEmpty() ? servicePriorities.FirstOrDefault(c => c.Key == x.PriorityKey)?.Value : string.Empty);
				Property("ErrorMessage", x => x.ErrorMessage);
				Property("UserGroup", x => x.UserGroup);
				Property("ResponsibleUser", x => x.ResponsibleUserObject != null ? x.ResponsibleUserObject.DisplayName : x.ResponsibleUser);
				Property("PreferredTechnicianUsergroup", x => x.PreferredTechnicianUsergroup != null ? x.PreferredTechnicianUsergroup.Name : x.PreferredTechnicianUsergroupKey);
				Property("PreferredTechnician", x => x.PreferredTechnician?.DisplayName);
				Property("Company", x => x.CustomerContact);
				Property("CompanyPhone", x => x.CustomerContact?.PrimaryPhoneData ?? string.Empty);
				Property("Initiator", x => x.Initiator);
				Property("InitiatorPhone", x => x.Initiator?.PrimaryPhoneData ?? string.Empty);
				Property("InitiatorPerson", x => x.InitiatorPerson);
				Property("InitiatorPersonPhone", x => x.InitiatorPerson?.PrimaryPhoneData ?? string.Empty);
				Property("ServiceLocationResponsibleContact", x => x.ServiceLocationResponsiblePerson ?? string.Empty);
				Property("ServiceLocationPhone", x => x.ServiceLocationPhone.IsNotNullOrEmpty() ? x.ServiceLocationPhone : x.ServiceLocationMobile ?? string.Empty);
				Property("ServiceCase", x => x.ServiceCase);
				Property("ServiceContract", x => x.ServiceContract?.ContractNo);

				//Internal Ids
				Property("TypeKey", x => x.TypeKey);
				Property("StatusKey", x => x.StatusKey);
				Property("PriorityKey", x => x.PriorityKey);
				//ServiceLocationPhone

				return base.GetCsv(items);
			}
		}
	}
}
