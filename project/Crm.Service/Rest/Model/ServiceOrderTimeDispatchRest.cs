namespace Crm.Service.Rest.Model
{
	using System;

	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;
	using Crm.Service.Model;

	[RestTypeFor(DomainType = typeof(ServiceOrderTimeDispatch))]
	public class ServiceOrderTimeDispatchRest : RestEntityWithExtensionValues
	{
		public Guid ServiceOrderDispatchId { get; set; }
		public Guid ServiceOrderTimeId { get; set; }
		[NavigationProperty(nameof(ServiceOrderDispatchId), InverseProperty = nameof(ServiceOrderDispatchRest.ServiceOrderTimeDispatches))]
		public ServiceOrderDispatchRest ServiceOrderDispatch { get; set; }
		[NavigationProperty(nameof(ServiceOrderTimeId), InverseProperty = nameof(ServiceOrderTimeRest.ServiceOrderTimeDispatches))]
		public ServiceOrderTimeRest ServiceOrderTime { get; set; }
	}
}
