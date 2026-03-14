namespace Crm.Service.EventHandler
{
	using System;
	using System.Linq;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Modularization.Events;
	using Crm.Service.Model;
	using Crm.Service.Model.Lookup;

	public class ServiceOrderPostProcessingEventHandler : IEventHandler<EntityModifiedEvent<ServiceOrderHead>>
	{
		private readonly IRepositoryWithTypedId<ServiceOrderDispatch, Guid> dispatchRepository;
		private readonly IResourceManager resourceManager;
		private readonly IRepository<ServiceOrderTime> serviceOrderTimeRepository;

		public ServiceOrderPostProcessingEventHandler(IRepositoryWithTypedId<ServiceOrderDispatch, Guid> dispatchRepository, IResourceManager resourceManager, IRepository<ServiceOrderTime> serviceOrderTimeRepository)
		{
			this.dispatchRepository = dispatchRepository;
			this.resourceManager = resourceManager;
			this.serviceOrderTimeRepository = serviceOrderTimeRepository;
		}

		public virtual void Handle(EntityModifiedEvent<ServiceOrderHead> e)
		{
			if (e.Entity.Status.BelongsToPostProcessing() && !e.EntityBeforeChange.Status.BelongsToPostProcessing() && !e.Entity.IsTemplate)
			{
				var serviceOrderTimes = e.Entity.ServiceOrderTimes;
				foreach (var serviceOrderTime in serviceOrderTimes)
				{
					serviceOrderTime.InvoiceDuration = (float)serviceOrderTime.Postings.Where(x => x.ServiceOrderTimePostingType == "Invoice").Sum(x => x.DurationInMinutes.GetValueOrDefault()) / 60;
					serviceOrderTimeRepository.SaveOrUpdate(serviceOrderTime);
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
					var newRemark = existingRemark + (existingRemark.IsNotNullOrEmpty() ? "." : string.Empty) + resourceManager.GetTranslation("CorrespondingOrderWasSetToPostProcessing", openDispatch.DispatchedUser.DefaultLanguageKey);
					openDispatch.StatusKey = ServiceOrderDispatchStatus.RejectedKey;
					openDispatch.RejectReasonKey = ServiceOrderDispatchRejectReason.RejectedBySystem;
					openDispatch.RejectRemark = newRemark.Substring(0, Math.Min(newRemark.Length, 500));
					openDispatch.ModifyUser = "ServiceOrderPostProcessingEventHandler";
					openDispatch.CloseDate = DateTime.Now;
					dispatchRepository.SaveOrUpdate(openDispatch);
				}
			}
		}
	}
}
