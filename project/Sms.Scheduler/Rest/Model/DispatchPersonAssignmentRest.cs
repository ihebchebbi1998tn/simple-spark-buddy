namespace Sms.Scheduler.Rest.Model
{
	using System;

	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;
	using Crm.Service.Rest.Model;

	using Sms.Scheduler.Model;

	[RestTypeFor(DomainType = typeof(DispatchPersonAssignment))]
	public class DispatchPersonAssignmentRest : RestEntityWithExtensionValues
	{
		public Guid DispatchKey { get; set; }
		[NavigationProperty(nameof(DispatchKey))] public virtual ServiceOrderDispatchRest Dispatch { get; set; }
		public string ResourceKey { get; set; }
	}
}
