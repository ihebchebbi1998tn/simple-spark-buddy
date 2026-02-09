namespace Crm.Service.Events
{
	using Crm.Library.Modularization.Events;
	using Crm.Service.Model;

	public class ServiceOrderDispatchCompletedEvent : IEvent
	{
		public ServiceOrderDispatch ServiceOrderDispatch { get; protected set; }

		public ServiceOrderDispatchCompletedEvent(ServiceOrderDispatch serviceOrderDispatch)
		{
			ServiceOrderDispatch = serviceOrderDispatch;
		}
	}
}