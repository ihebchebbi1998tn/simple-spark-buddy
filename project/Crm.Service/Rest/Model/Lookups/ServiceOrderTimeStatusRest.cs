namespace Crm.Service.Rest.Model.Lookups
{
	using Crm.Library.Rest;
	using Crm.Service.Model.Lookup;

	[RestTypeFor(DomainType = typeof(ServiceOrderTimeStatus))]
	public class ServiceOrderTimeStatusRest : RestEntityLookupWithExtensionValues
	{
		public string Color { get; set; }
	}
}
