namespace Crm.Service.EventHandler
{
	using System;
	using System.Linq;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Modularization.Events;
	using Crm.Service.Model;
	using Crm.Service.Model.Lookup;

	public class ServiceOrderClosedEventHandler : IEventHandler<EntityModifiedEvent<ServiceOrderHead>>
	{
		private readonly IRepositoryWithTypedId<ServiceOrderHead, Guid> serviceOrderRepository;
		private readonly IRepositoryWithTypedId<ServiceOrderDispatch, Guid> dispatchRepository;
		private readonly ILookupManager lookupManager;
		private readonly IResourceManager resourceManager;
		public ServiceOrderClosedEventHandler(IRepositoryWithTypedId<ServiceOrderHead, Guid> serviceOrderRepository, ILookupManager lookupManager, IRepositoryWithTypedId<ServiceOrderDispatch, Guid> dispatchRepository, IResourceManager resourceManager)
		{
			this.serviceOrderRepository = serviceOrderRepository;
			this.lookupManager = lookupManager;
			this.dispatchRepository = dispatchRepository;
			this.resourceManager = resourceManager;
		}

		public virtual void Handle(EntityModifiedEvent<ServiceOrderHead> e)
		{
			var isClosed = lookupManager.Get<ServiceOrderStatus>(e.Entity.StatusKey).BelongsToClosed();
			var wasClosed = lookupManager.Get<ServiceOrderStatus>(e.EntityBeforeChange.StatusKey).BelongsToClosed();

			if (isClosed && !wasClosed && !e.Entity.Closed.HasValue && !e.Entity.IsTemplate)
			{
				e.Entity.Closed = DateTime.UtcNow;
				serviceOrderRepository.SaveOrUpdate(e.Entity);
			}

			if (!isClosed)
			{
				return;
			}

			var openDispatches = dispatchRepository.GetAll()
				.Where(
					x => x.OrderId == e.Entity.Id
					    && x.StatusKey != ServiceOrderDispatchStatus.ClosedCompleteKey
					    && x.StatusKey != ServiceOrderDispatchStatus.ClosedNotCompleteKey
						&& x.StatusKey != ServiceOrderDispatchStatus.RejectedKey
						&& x.StatusKey != ServiceOrderDispatchStatus.CancelledKey
						&& x.StatusKey != ServiceOrderDispatchStatus.CancelledNotCompleteKey);

			foreach (var openDispatch in openDispatches)
			{
				var existingRemark = openDispatch.Remark != null ? openDispatch.Remark + Environment.NewLine : null;
				var newRemark = existingRemark + (existingRemark.IsNotNullOrEmpty() ? "." : string.Empty) + resourceManager.GetTranslation("CorrespondingOrderWasClosed", openDispatch.DispatchedUser.DefaultLanguageKey);
				openDispatch.StatusKey = ServiceOrderDispatchStatus.RejectedKey;
			    openDispatch.RejectReasonKey = ServiceOrderDispatchRejectReason.RejectedBySystem;
				openDispatch.RejectRemark = newRemark.Substring(0, Math.Min(newRemark.Length, 500));
			    openDispatch.ModifyUser = "ServiceOrderClosedEventHandler";
				openDispatch.CloseDate = DateTime.Now;
                dispatchRepository.SaveOrUpdate(openDispatch);
			}
		}
	}
}
