namespace Crm.Service.EventHandler
{
	using System;

	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Globalization.Resource;
	using Crm.Service.Model.Lookup;

	using log4net;

	using Library.Data.Domain.DataInterfaces;
	using Library.Modularization.Events;
	using Model;
	using Model.Extensions;
	using Model.Helpers;

	public class ServiceOrderDispatchCreateEventHandler : IEventHandler<EntityCreatedEvent<ServiceOrderDispatch>>
	{
		private readonly IRepository<ServiceOrderHead> serviceOrderRepository;
		private readonly IRepository<ServiceOrderDispatch> dispatchRepository;
		private readonly IServiceOrderStatusEvaluator serviceOrderStatusEvaluator;
		private readonly ILookupManager lookupManager;
		private readonly IResourceManager resourceManager;
		private readonly ILog logger;

		public ServiceOrderDispatchCreateEventHandler(IRepository<ServiceOrderHead> serviceOrderRepository, IServiceOrderStatusEvaluator serviceOrderStatusEvaluator, IRepository<ServiceOrderDispatch> dispatchRepository, ILookupManager lookupManager, IResourceManager resourceManager, ILog logger)
		{
			this.serviceOrderRepository = serviceOrderRepository;
			this.serviceOrderStatusEvaluator = serviceOrderStatusEvaluator;
			this.dispatchRepository = dispatchRepository;
			this.lookupManager = lookupManager;
			this.resourceManager = resourceManager;
			this.logger = logger;
		}
		public virtual void Handle(EntityCreatedEvent<ServiceOrderDispatch> e)
		{
			var serviceOrder = e.Entity.OrderHead;
			var dispatch = e.Entity;
			var serviceOrderStatus = lookupManager.Get<ServiceOrderStatus>(serviceOrder.StatusKey);
			if (serviceOrderStatus.IsClosed())
			{
				var existingRemark = dispatch.Remark != null ? dispatch.Remark + Environment.NewLine : null;
				var newRemark = existingRemark + resourceManager.GetTranslation("CorrespondingOrderWasClosed", dispatch.DispatchedUser.DefaultLanguageKey);
				dispatch.StatusKey = ServiceOrderDispatchStatus.ClosedCompleteKey;
				dispatch.ModifyUser = "ServiceOrderDispatchCreateEventHandler";
				dispatch.Remark = newRemark.Substring(0, Math.Min(newRemark.Length, 500));
				dispatchRepository.SaveOrUpdate(dispatch);
				return;
			}

			logger.DebugFormat("Service Order No: {0} | New Dispatch Created | Id: {1} | Dispatch Status: {2}",
				e.Entity.OrderHead.OrderNo, e.Entity.Id, e.Entity.StatusKey);
			var oldStatusKey = e.Entity.OrderHead.StatusKey;

			serviceOrderStatusEvaluator.DynamicUpdateOrderStatus(e.Entity.OrderHead);
			logger.DebugFormat("Service Order No: {0} | Old Status: {1} | New Status: {2}",
				e.Entity.OrderHead.OrderNo, oldStatusKey, e.Entity.OrderHead);
			serviceOrderRepository.SaveOrUpdate(e.Entity.OrderHead);
		}
	}
}
