namespace Crm.Service.EventHandler
{
	using System;
	using System.Linq;
	using Crm.Service.Enums;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Helper;
	using Crm.Library.Modularization.Events;
	using Crm.Library.Services.Interfaces;
	using Crm.Service.Model;
	using Crm.Service.Model.Lookup;
	using Crm.Service.Model.Relationships;
	using Crm.Service.Services.Interfaces;
	using Crm.Library.Extensions;

	using Main;
		using System.Globalization;

		public class ServiceOrderAttentionTasksEventHandler : IEventHandler<EntityModifiedEvent<ServiceOrderHead>>, IEventHandler<EntityCreatedEvent<ServiceOrderDispatch>>, IEventHandler<EntityModifiedEvent<ServiceOrderDispatch>>
	{
		private readonly IRepositoryWithTypedId<ServiceContractInstallationRelationship, Guid> contractInstallationRelationshipRepository;
		private readonly IAttentionTaskService attentionTaskService;
		private readonly IUserService userService;
		private readonly IResourceManager resourceManager;
		private readonly IAppSettingsProvider appSettingsProvider;
		public ServiceOrderAttentionTasksEventHandler(IRepositoryWithTypedId<ServiceContractInstallationRelationship, Guid> contractInstallationRelationshipRepository, IAttentionTaskService attentionTaskService, IUserService userService, IResourceManager resourceManager, IAppSettingsProvider appSettingsProvider)
		{

			this.contractInstallationRelationshipRepository = contractInstallationRelationshipRepository;
			this.attentionTaskService = attentionTaskService;
			this.userService = userService;
			this.resourceManager = resourceManager;
			this.appSettingsProvider = appSettingsProvider;
		}
		public virtual void Handle(EntityModifiedEvent<ServiceOrderHead> e)
		{
			var user = userService.GetUser(e.Entity.ModifyUser);
			var translationCulture = CultureInfo.GetCultureInfo(user?.DefaultLanguageKey ?? "en");
			var serviceOrder = e.Entity;
			var dispatches = serviceOrder.Dispatches;
			if (appSettingsProvider.GetValue(MainPlugin.Settings.Task.AttentionTaskTypeKey).IsNotNullOrEmpty() && serviceOrder.Status.BelongsToPostProcessing() && !e.EntityBeforeChange.Status.BelongsToPostProcessing() && !serviceOrder.IsTemplate)
			{
				foreach (var dispatch in dispatches)
				{
					if (!dispatch.TimePostings.Any())
					{
						attentionTaskService.CreateAttentionTaskForDispatch(dispatch, user, resourceManager.GetTranslation("NoTimesReportedForDispatch", translationCulture).WithArgs(dispatch.DispatchedUser.DisplayName, dispatch.DispatchNo.IsNotNullOrEmpty() ? dispatch.DispatchNo : dispatch.OrderHead.OrderNo, dispatch.Date.ToShortDateString()));
					}
				}
				var maintenanceOrderGenerationMode = appSettingsProvider.GetValue(ServicePlugin.Settings.ServiceContract.MaintenanceOrderGenerationMode);
				var installation = serviceOrder.AffectedInstallation;
				if (maintenanceOrderGenerationMode == MaintenanceOrderGenerationMode.OrderPerInstallation && installation != null)
				{
					var customerContractInstallationNos = contractInstallationRelationshipRepository.GetAll().Where(x => x.Parent.ParentId == serviceOrder.CustomerContactId && x.Parent.ValidFrom.HasValue && x.Parent.ValidFrom <= serviceOrder.Reported && x.Parent.ValidTo.HasValue && x.Parent.ValidTo >= serviceOrder.Reported).Select(x => x.Child.InstallationNo).ToList();
					if (customerContractInstallationNos.Contains(installation.InstallationNo))
					{
						attentionTaskService.CreateAttentionTaskForServiceOrder(serviceOrder, user, resourceManager.GetTranslation("ValidMaintenanceContractForTheCustomer", translationCulture));
					}
					else
					{
						if (CheckInstallationWarranty(serviceOrder, installation))
						{
							attentionTaskService.CreateAttentionTaskForServiceOrder(serviceOrder, user, resourceManager.GetTranslation("InstallationUnderWarranty", translationCulture).WithArgs(installation.InstallationNo));
						}
					}
				}
				if (maintenanceOrderGenerationMode == MaintenanceOrderGenerationMode.JobPerInstallation)
				{
					if (CheckForValidMaintenanceContract(serviceOrder))
					{
						attentionTaskService.CreateAttentionTaskForServiceOrder(serviceOrder, user, resourceManager.GetTranslation("ValidMaintenanceContractForTheCustomer", translationCulture));
					}
					else
					{
						CheckForInstallationsWarranties(serviceOrder);
					}
				}
			}
		}
		public virtual void CheckForInstallationsWarranties(ServiceOrderHead serviceOrder)
		{
			var user = userService.GetUser(serviceOrder.ModifyUser);
			var translationCulture = CultureInfo.GetCultureInfo(user?.DefaultLanguageKey ?? "en");
			var installations = serviceOrder.ServiceOrderTimes.Where(x => x.Installation != null).Select(x => x.Installation).Distinct().ToList();
			if (appSettingsProvider.GetValue(ServicePlugin.Settings.AttentionTasks.CheckAllInstallationsWarranties))
			{
				foreach (var installation in installations)
				{
					if (CheckInstallationWarranty(serviceOrder, installation))
					{
						attentionTaskService.CreateAttentionTaskForServiceOrder(serviceOrder, user, resourceManager.GetTranslation("InstallationUnderWarranty", translationCulture).WithArgs(installation.InstallationNo));
					}
				}
			}
			else
			{
				var installation = installations.Where(x => CheckInstallationWarranty(serviceOrder, x)).FirstOrDefault();
				if (installation != null) attentionTaskService.CreateAttentionTaskForServiceOrder(serviceOrder, user, resourceManager.GetTranslation("InstallationUnderWarranty", translationCulture).WithArgs(installation.InstallationNo));
			}
		}
		public virtual bool CheckInstallationWarranty(ServiceOrderHead serviceOrder, Installation installation)
		{
			return installation.WarrantyFrom.HasValue && installation.WarrantyFrom.Value <= serviceOrder.Reported && installation.WarrantyUntil.HasValue && installation.WarrantyUntil.Value > serviceOrder.Reported;
		}
		public virtual bool CheckForValidMaintenanceContract(ServiceOrderHead serviceOrder)
		{
			var orderInstallationIds = serviceOrder.ServiceOrderTimes.Where(x => x.InstallationId.HasValue).Select(x => x.InstallationId.Value).Distinct().ToList();
			var customerContractInstallationIds = contractInstallationRelationshipRepository.GetAll().Where(x => x.Parent.ParentId == serviceOrder.CustomerContactId && x.Parent.ValidFrom.HasValue && x.Parent.ValidFrom <= serviceOrder.Reported && x.Parent.ValidTo.HasValue && x.Parent.ValidTo >= serviceOrder.Reported).Select(x => x.ChildId).ToList();

			if (appSettingsProvider.GetValue(ServicePlugin.Settings.AttentionTasks.CheckAllInstallationsWarranties))
			{
				return orderInstallationIds.All(x => customerContractInstallationIds.Contains(x));
			}
			return orderInstallationIds.Any(x => customerContractInstallationIds.Contains(x));
		}
		public virtual void Handle(EntityModifiedEvent<ServiceOrderDispatch> e)
		{
			var user = userService.GetUser(e.Entity.ModifyUser);
			var translationCulture = CultureInfo.GetCultureInfo(user?.DefaultLanguageKey ?? "en");
			if (appSettingsProvider.GetValue(MainPlugin.Settings.Task.AttentionTaskTypeKey).IsNotNullOrEmpty())
			{
				var dispatch = e.Entity;
				var dispatchBeforeChange = e.EntityBeforeChange;
				if (dispatch.IsRejected() && !dispatchBeforeChange.IsRejected() && dispatch.OrderHead != null)
				{
					attentionTaskService.CreateAttentionTaskForServiceOrder(dispatch.OrderHead, user, resourceManager.GetTranslation("DispatchWasRejected", translationCulture).WithArgs(dispatch.DispatchNo.IsNotNullOrEmpty() ? dispatch.DispatchNo : dispatch.OrderHead.OrderNo, dispatch.DispatchedUsername));
				}
			}
		}
		public virtual void Handle(EntityCreatedEvent<ServiceOrderDispatch> e)
		{
			var user = userService.GetUser(e.Entity.CreateUser);
			var translationCulture = CultureInfo.GetCultureInfo(user?.DefaultLanguageKey ?? "en");
			if (appSettingsProvider.GetValue(MainPlugin.Settings.Task.AttentionTaskTypeKey).IsNotNullOrEmpty())
			{
				var dispatch = e.Entity;
				if (dispatch.IsRejected() && dispatch.OrderHead != null)
				{
					attentionTaskService.CreateAttentionTaskForServiceOrder(dispatch.OrderHead, user, resourceManager.GetTranslation("DispatchWasRejected", translationCulture).WithArgs(dispatch.DispatchNo.IsNotNullOrEmpty() ? dispatch.DispatchNo : dispatch.OrderHead.OrderNo, dispatch.DispatchedUsername));
				}
			}
		}
	}
}
