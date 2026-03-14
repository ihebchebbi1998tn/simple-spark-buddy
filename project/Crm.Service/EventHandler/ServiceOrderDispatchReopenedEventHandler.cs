namespace Crm.Service.EventHandler
{
	using Crm.Library.Modularization.Events;
	using Crm.Service.Events;
	using Crm.Service.Model;
	using Crm.Service.Model.Lookup;

	public class ServiceOrderDispatchReopenedEventHandler : IEventHandler<EntityModifiedEvent<ServiceOrderDispatch>>
	{
		private readonly IEventAggregator eventAggregator;
		public ServiceOrderDispatchReopenedEventHandler(IEventAggregator eventAggregator)
		{
			this.eventAggregator = eventAggregator;
		}
		public virtual void Handle(EntityModifiedEvent<ServiceOrderDispatch> e)
		{
			if (IsCancelled(e.EntityBeforeChange) && !IsCancelled(e.Entity))
			{
				eventAggregator.Publish(new ServiceOrderDispatchReopenedEvent(e.Entity));
			}
		}
		public virtual bool IsCancelled(ServiceOrderDispatch serviceOrderDispatch)
		{
			return serviceOrderDispatch.StatusKey == ServiceOrderDispatchStatus.CancelledNotCompleteKey || serviceOrderDispatch.StatusKey == ServiceOrderDispatchStatus.CancelledKey;
		}
	}
}
