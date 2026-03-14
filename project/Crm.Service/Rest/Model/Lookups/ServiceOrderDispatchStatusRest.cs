namespace Crm.Service.Rest.Model.Lookups
{
	using Crm.Library.Rest;
	using Crm.Service.Model.Lookup;

	[RestTypeFor(DomainType = typeof(ServiceOrderDispatchStatus))]
	public class ServiceOrderDispatchStatusRest : RestEntityLookupWithExtensionValues
	{
		public string Color { get; set; }
	}
}
