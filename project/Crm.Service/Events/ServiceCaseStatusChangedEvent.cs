using Crm.Service.Model;

namespace Crm.Service.Events
{
	using Crm.Library.Modularization.Events;

	public class ServiceCaseStatusChangedEvent : IEvent
	{
		public ServiceCase ServiceCase { get; protected set; }
		public string NoteText { get; protected set; }

		public ServiceCaseStatusChangedEvent(ServiceCase serviceCase, string noteText)
		{
			ServiceCase = serviceCase;
			NoteText = noteText;
		}
	}
}
