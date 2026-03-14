namespace Crm.Service.Events
{
	using Crm.Library.Modularization.Events;
	using Crm.Service.Model;

	public class ServiceOrderDispatchReopenedEvent : IEvent
	{
		public ServiceOrderDispatch ServiceOrderDispatch { get; protected set; }

		public ServiceOrderDispatchReopenedEvent(ServiceOrderDispatch serviceOrderDispatch)
		{
			ServiceOrderDispatch = serviceOrderDispatch;
		}
	}
}
