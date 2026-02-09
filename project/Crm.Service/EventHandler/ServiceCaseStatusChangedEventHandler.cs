namespace Crm.Service.EventHandler
{
	using Crm.Library.Modularization.Events;
	using Crm.Service.Events;
	using Crm.Service.Model;

	public class ServiceCaseStatusChangedEventHandler : IEventHandler<EntityModifiedEvent<ServiceCase>>
	{
		private readonly IEventAggregator eventAggregator;
		public ServiceCaseStatusChangedEventHandler(IEventAggregator eventAggregator)
		{
			this.eventAggregator = eventAggregator;
		}

		public virtual void Handle(EntityModifiedEvent<ServiceCase> e)
		{
			if (e.Entity.StatusKey != e.EntityBeforeChange.StatusKey)
			{
				eventAggregator.Publish(new ServiceCaseStatusChangedEvent(e.Entity, e.Entity.Status.Value));
			}
		}
	}
}
