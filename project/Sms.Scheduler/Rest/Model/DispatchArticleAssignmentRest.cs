namespace Sms.Scheduler.Rest.Model
{
	using System;

	using Crm.Library.Rest;
	using Sms.Scheduler.Model;

	[RestTypeFor(DomainType = typeof(DispatchArticleAssignment))]
	public class DispatchArticleAssignmentRest : RestEntityWithExtensionValues
	{
		public Guid DispatchKey { get; set; }
		public Guid ResourceKey { get; set; }
	}
}
