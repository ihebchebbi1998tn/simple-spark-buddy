namespace Crm.Service.Events
{
	using System.Collections.Generic;

	using Crm.Library.Modularization.Events;
	using Crm.Service.Model;

	public class ServiceCaseInformationChangedEvent : IEvent
	{
		public ServiceCase ServiceCase { get; protected set; }
		public string ModifiedFieldsJson { get; protected set; }
		
		public ServiceCaseInformationChangedEvent(ServiceCase serviceCase, string modifiedFields)
		{
			ServiceCase = serviceCase;
			ModifiedFieldsJson = modifiedFields;
		}
	}
}
