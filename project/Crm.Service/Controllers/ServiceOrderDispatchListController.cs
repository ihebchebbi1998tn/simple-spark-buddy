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

	using Main;
	using Main.Controllers;
	using Main.ViewModels;

	using Microsoft.AspNetCore.Mvc;

	using NHibernate.Linq;

	public class ServiceOrderDispatchListController : GenericListController<ServiceOrderDispatch>
	{
		public ServiceOrderDispatchListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<ServiceOrderDispatch>> rssFeedProviders, IEnumerable<ICsvDefinition<ServiceOrderDispatch>> csvDefinitions, IEntityConfigurationProvider<ServiceOrderDispatch> entityConfigurationProvider, IRepository<ServiceOrderDispatch> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
			: base(pluginProvider, rssFeedProviders, csvDefinitions, entityConfigurationProvider, repository, appSettingsProvider, resourceManager, restTypeProvider)
		{
		}
		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.Dispatch)]
		public virtual ActionResult AutocompleteTemplate()
		{
			return PartialView();
		}

		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.Dispatch)]
		public override ActionResult FilterTemplate()
		{
			return base.FilterTemplate();
		}

		protected override string GetTypeName()
		{
			return "ServiceOrderDispatch";
		}

		protected override string GetTitle()
		{
			return "ServiceOrderDispatches";
		}

		protected override string GetEmptySlate()
		{
			if (!repository.GetAll().Any()) return resourceManager.GetTranslation("NoServiceOrderDispatchInfo");
			return resourceManager.GetTranslation("SearchCriteriaYieldedNoResults");
		}

		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.Dispatch)]
		public override ActionResult IndexTemplate()
		{
			return base.IndexTemplate();
		}

		[RequiredPermission(ServicePlugin.PermissionName.AppointmentConfirmation, Group = ServicePlugin.PermissionGroup.Dispatch)]
		[RenderAction("ServiceOrderDispatchItemTemplateActions", Priority = 90)]
		public virtual ActionResult TemplateActionAppointmentConfirmation()
		{
			return PartialView();
		}

		[RequiredPermission(ServicePlugin.PermissionName.AppointmentRequest, Group = ServicePlugin.PermissionGroup.Dispatch)]
		[RenderAction("ServiceOrderDispatchItemTemplateActions", Priority = 100)]
		public virtual ActionResult TemplateActionAppointmentRequest()
		{
			return PartialView();
		}

		[RequiredPermission(ServicePlugin.PermissionName.ConfirmScheduled, Group = ServicePlugin.PermissionGroup.Dispatch)]
		[RenderAction("ServiceOrderDispatchItemTemplateActions", Priority = 70)]
		public virtual ActionResult TemplateActionConfirmAppointment()
		{
			return PartialView();
		}

		[RenderAction("ServiceOrderDispatchItemTemplateActions", Priority = 75)]
		[RenderAction("ServiceOrderDispatchItemTemplateActions", Priority = 85)]
		public virtual ActionResult TemplateActionDivider()
		{
			return PartialView("ListDivider");
		}

		[RequiredPermission(ServicePlugin.PermissionName.RejectScheduled, Group = ServicePlugin.PermissionGroup.Dispatch)]
		[RenderAction("ServiceOrderDispatchItemTemplateActions", Priority = 50)]
		public virtual ActionResult TemplateActionReject()
		{
			return PartialView();
		}

		[RequiredPermission(ServicePlugin.PermissionName.Cancel, Group = ServicePlugin.PermissionGroup.Dispatch)]
		[RenderAction("ServiceOrderDispatchItemTemplateActions", Priority = 50)]
		public virtual ActionResult TemplateActionCancel()
		{
			return PartialView();
		}

		[RequiredPermission(ServicePlugin.PermissionName.Reschedule, Group = ServicePlugin.PermissionGroup.Dispatch)]
		[RenderAction("ServiceOrderDispatchItemTemplateActions", Priority = 60)]
		public virtual ActionResult TemplateActionReschedule()
		{
			return PartialView();
		}

		[RequiredPermission(PermissionName.Read, Group = ServicePlugin.PermissionGroup.Dispatch)]
		[RenderAction("ServiceOrderDispatchItemTemplateActions", Priority = 80)]
		public virtual ActionResult TemplateActionViewDetails()
		{
			return PartialView();
		}

		[RequiredPermission(ServicePlugin.PermissionName.DownloadReport, Group = ServicePlugin.PermissionGroup.Dispatch)]
		[RenderAction("ServiceOrderDispatchItemTemplateActions", Priority = 60)]
		public virtual ActionResult TemplateActionDownloadReport()
		{
			return PartialView();
		}
		[RequiredPermission(PermissionName.Create, Group = ServicePlugin.PermissionGroup.Dispatch)]
		[RenderAction("ServiceOrderDispatchItemTemplateActions", Priority = 65)]
		public virtual ActionResult TemplateActionScheduleFollowUpDispatch()
		{
			return PartialView();
		}
		[RequiredPermission(PermissionName.Create, Group = ServicePlugin.PermissionGroup.ServiceOrder)]
		[RenderAction("ServiceOrderDispatchItemTemplateActions", Priority = 75)]
		public virtual ActionResult TemplateActionScheduleFollowUpServiceOrder()
		{
			return PartialView();
		}
		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.Dispatch)]
		public virtual ActionResult TodaysDispatches()
		{
			var model = new GenericListViewModel
			{
				EmptySlate = resourceManager.GetTranslation("TodaysDispatchesEmptySlate"),
				IdentifierPropertyName = repository.IdentifierPropertyName,
				PluginName = "Crm.Service",
				Title = "TodaysDispatches",
				Type = typeof(ServiceOrderDispatch),
				TypeName = GetTypeName()
			};
			return PartialView(model);
		}
		
		[RenderAction("TopSearchMaterialItemTemplate")]
		[RequiredPermission(nameof(ServiceOrderDispatch), Group = MainPlugin.PermissionGroup.TopSearch)]
		public virtual ActionResult TopSearchMaterialItemTemplate()
		{
			var model = GetGenericListTemplateViewModel();
			return View("Material/GenericListItem", model);
		}
		[RenderAction("ServiceOrderDispatchListFilterTemplate")]
		public virtual ActionResult ServiceOrderDispatchListFilterTemplate() => PartialView();

		public class ServiceOrderDispatchCsvDefinition : CsvDefinition<ServiceOrderDispatch>
		{
			private readonly ILookupManager lookupManager;
			public override IQueryable<ServiceOrderDispatch> Eager(IQueryable<ServiceOrderDispatch> query)
			{
				return query
					.Fetch(x => x.DispatchedUser)
					.Fetch(x => x.OrderHead)
					;
			}

			public ServiceOrderDispatchCsvDefinition(IResourceManager resourceManager, ILookupManager lookupManager)
				: base(resourceManager)
			{
				this.lookupManager = lookupManager;
			}
			public override IEnumerable<KeyValuePair<string, Func<ServiceOrderDispatch, object>>> GetCsv(IEnumerable<ServiceOrderDispatch> items) {
				var serviceOrderTypes = lookupManager.List<ServiceOrderType>();
				var serviceOrderDispatchStatuses = lookupManager.List<ServiceOrderDispatchStatus>();

				Property("Id", x => x.Id);
				Property("DispatchNo", x => x.DispatchNo != null ? x.DispatchNo : string.Empty);
				Property("CreateDate", x => x.CreateDate.ToShortDateString());
				Property("ServiceOrder", x => x.OrderHead.OrderNo);
				Property("T_Type", x => x.OrderHead.TypeKey.IsNotNullOrEmpty() ? serviceOrderTypes.FirstOrDefault(c => c.Key == x.OrderHead.TypeKey)?.Value : string.Empty);
				Property("Status", x => x.StatusKey.IsNotNullOrEmpty() ? serviceOrderDispatchStatuses.FirstOrDefault(c => c.Key == x.StatusKey)?.Value : string.Empty);
				Property("Date", x => x.Date.ToShortDateString());
				Property("EndDateCaption", x => x.EndDate.ToShortDateString());
				Property("Time", x => x.Date.ToShortTimeString());
				Property("ErrorMessage", x => x.OrderHead.ErrorMessage);
				Property("Company", x => x.OrderHead.CustomerContact);
				Property("CompanyPhone", x => x.OrderHead.CustomerContact?.PrimaryPhoneData ?? string.Empty);
				Property("Initiator", x => x.OrderHead.Initiator);
				Property("InitiatorPhone", x => x.OrderHead.Initiator?.PrimaryPhoneData ?? string.Empty);
				Property("InitiatorPerson", x => x.OrderHead.InitiatorPerson);
				Property("InitiatorPersonPhone", x => x.OrderHead.InitiatorPerson?.PrimaryPhoneData ?? string.Empty);
				Property("ServiceLocationResponsibleContact", x => x.OrderHead.ServiceLocationResponsiblePerson ?? string.Empty);
				Property("ServiceLocationPhone", x => x.OrderHead.ServiceLocationPhone.IsNotNullOrEmpty() ? x.OrderHead.ServiceLocationPhone : x.OrderHead.ServiceLocationMobile ?? string.Empty);
				Property("InstallationNo", x => x.OrderHead.AffectedInstallation?.InstallationNo);
				Property("Duration", x => x.EndDate.Subtract(x.Date).TotalHours + " " + resourceManager.GetTranslation("DurationHours"));
				Property("Technician", x => x.DispatchedUser?.DisplayName);

				//Internal Ids
				Property("TypeKey", x => x.OrderHead != null ? x.OrderHead.TypeKey : string.Empty);
				Property("StatusKey", x => x.StatusKey);

				return base.GetCsv(items);
			}
		}
	}
}
