namespace Crm.Service.EventHandler
{
	using System;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Modularization.Events;
	using Crm.Service.Model;

	public class TransferReportRecipientsFromServiceOrderToDispatch : IEventHandler<EntityCreatedEvent<ServiceOrderDispatch>>
	{
		private readonly Func<ServiceOrderDispatchReportRecipient> serviceOrderDispatchReportRecipientFactory;
		private readonly IRepositoryWithTypedId<ServiceOrderDispatchReportRecipient, Guid> serviceOrderDispatchReportRecipientRepository;

		public TransferReportRecipientsFromServiceOrderToDispatch(Func<ServiceOrderDispatchReportRecipient> serviceOrderDispatchReportRecipientFactory, IRepositoryWithTypedId<ServiceOrderDispatchReportRecipient, Guid> serviceOrderDispatchReportRecipientRepository)
		{
			this.serviceOrderDispatchReportRecipientFactory = serviceOrderDispatchReportRecipientFactory;
			this.serviceOrderDispatchReportRecipientRepository = serviceOrderDispatchReportRecipientRepository;
		}

		public virtual void Handle(EntityCreatedEvent<ServiceOrderDispatch> e)
		{
			var dispatch = e.Entity;
			foreach (var reportRecipient in dispatch.OrderHead.ReportRecipients)
			{
				var dispatchReportRecipient = serviceOrderDispatchReportRecipientFactory();
				dispatchReportRecipient.DispatchId = dispatch.Id;
				dispatchReportRecipient.Email = reportRecipient;
				serviceOrderDispatchReportRecipientRepository.SaveOrUpdate(dispatchReportRecipient);
			}
		}
	}
}
