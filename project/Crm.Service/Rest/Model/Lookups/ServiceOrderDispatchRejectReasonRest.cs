namespace Crm.Service.Rest.Model.Lookups
{
	using Crm.Library.Rest;
	using Crm.Service.Model.Lookup;

	[RestTypeFor(DomainType = typeof(ServiceOrderDispatchRejectReason))]
	public class ServiceOrderDispatchRejectReasonRest : RestEntityLookupWithExtensionValues
	{
		public bool ShowInMobileClient { get; set; }
		public string ServiceOrderStatusKey { get; set; }
	}
}
