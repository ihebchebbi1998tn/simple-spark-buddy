namespace Crm.Service.Rest.Model
{
	using System;

	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;
	using Crm.Service.Model;

	[RestTypeFor(DomainType = typeof(ServiceOrderStatisticsKey))]
	public class ServiceOrderStatisticsKeyRest : RestEntityWithExtensionValues
	{
		[NavigationProperty(nameof(DispatchId))]
		public ServiceOrderDispatchRest Dispatch { get; set; }
		public Guid? DispatchId { get; set; }
		[NavigationProperty(nameof(ServiceOrderId), InverseProperty = nameof(ServiceOrderHeadRest.ServiceOrderStatisticsKeys))]
		public ServiceOrderHeadRest ServiceOrder { get; set; }
		public Guid ServiceOrderId { get; set; }
		public string ProductTypeKey { get; set; }
		public string MainAssemblyKey { get; set; }
		public string SubAssemblyKey { get; set; }
		public string AssemblyGroupKey { get; set; }
		public string FaultImageKey { get; set; }
		public string RemedyKey { get; set; }
		public string CauseKey { get; set; }
		public string WeightingKey { get; set; }
		public string CauserKey { get; set; }
	}
}
