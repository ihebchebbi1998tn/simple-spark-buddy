namespace Crm.Service.Rest.Model
{
	using System;

	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;
	using Crm.Service.Model;

	[RestTypeFor(DomainType = typeof(ServiceOrderDispatchReportRecipient))]
	public class ServiceOrderDispatchReportRecipientRest : RestEntityWithExtensionValues
	{
		public Guid DispatchId { get; set; }
		public string Email { get; set; }
		public string Language { get; set; }
		public string Locale { get; set; }

		[NavigationProperty(nameof(DispatchId), nameof(ServiceOrderDispatchRest.ReportRecipients))]
		public ServiceOrderDispatchRest Dispatch { get; set; }
	}
}
