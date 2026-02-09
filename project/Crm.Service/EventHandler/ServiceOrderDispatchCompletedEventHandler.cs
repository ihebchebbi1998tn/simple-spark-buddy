namespace Crm.Service.EventHandler
{
	using Crm.Library.Modularization.Events;
	using Crm.Service.Events;
	using Crm.Service.Model;
	using Crm.Service.Model.Lookup;

	public class ServiceOrderDispatchCompletedEventHandler : IEventHandler<EntityCreatedEvent<ServiceOrderDispatch>>, IEventHandler<EntityModifiedEvent<ServiceOrderDispatch>>
	{
		private readonly IEventAggregator eventAggregator;
		public ServiceOrderDispatchCompletedEventHandler(IEventAggregator eventAggregator)
		{
			this.eventAggregator = eventAggregator;
		}
		public virtual void Handle(EntityCreatedEvent<ServiceOrderDispatch> e)
		{
			if (IsCompleted(e.Entity))
			{
				eventAggregator.Publish(new ServiceOrderDispatchCompletedEvent(e.Entity));
			}
		}
		public virtual void Handle(EntityModifiedEvent<ServiceOrderDispatch> e)
		{
			if (IsCompleted(e.Entity) && !IsCompleted(e.EntityBeforeChange))
			{
				eventAggregator.Publish(new ServiceOrderDispatchCompletedEvent(e.Entity));
			}
		}
		public virtual bool IsCompleted(ServiceOrderDispatch serviceOrderDispatch)
		{
			return serviceOrderDispatch.StatusKey == ServiceOrderDispatchStatus.ClosedNotCompleteKey || serviceOrderDispatch.StatusKey == ServiceOrderDispatchStatus.ClosedCompleteKey;
		}
	}
}