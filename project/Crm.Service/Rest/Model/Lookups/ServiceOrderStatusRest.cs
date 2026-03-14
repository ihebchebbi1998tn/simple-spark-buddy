namespace Crm.Service.Rest.Model.Lookups
{
	using Crm.Library.Rest;
	using Crm.Service.Model.Lookup;

	[RestTypeFor(DomainType = typeof(ServiceOrderStatus))]
	public class ServiceOrderStatusRest : RestEntityLookupWithExtensionValues
	{
		public string Groups { get; set; }
		public string SettableStatuses { get; set; }
	}
}
