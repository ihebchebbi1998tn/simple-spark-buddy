using Crm.Service.Model;

namespace Crm.Service.Events
{
	using Crm.Library.Modularization.Events;

	public class ServiceContractStatusChangedEvent : IEvent
	{
		public ServiceContract ServiceContract { get; protected set; }

		public ServiceContractStatusChangedEvent(ServiceContract serviceContract)
		{
			ServiceContract = serviceContract;
		}
	}
}