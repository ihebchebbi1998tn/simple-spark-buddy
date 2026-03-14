namespace Sms.Scheduler.Rest.Model
{
	using System;

	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;

	using Main.Rest.Model;

	using Sms.Scheduler.Model;

	[RestTypeFor(DomainType = typeof(Absence))]
	public class AbsenceRest : RestEntityWithExtensionValues
	{
		public DateTime From { get; set; }
		public DateTime To { get; set; }
		public string TimeEntryTypeKey { get; set; }
		public string Description { get; set; }
		public string ResponsibleUser { get; set; }
		[NavigationProperty(nameof(ResponsibleUser))] public virtual UserRest ResponsibleUserObject { get; set; }
	}
}
