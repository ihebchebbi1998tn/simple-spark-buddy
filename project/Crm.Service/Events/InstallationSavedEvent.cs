namespace Crm.Service.Events
{
	using Crm.Service.Model;
	using Crm.Library.Modularization.Events;

	public class InstallationSavedEvent : IEvent
	{
		public Installation Installation { get; protected set; }

		public InstallationSavedEvent(Installation installation)
		{
			Installation = installation;
		}
	}
}