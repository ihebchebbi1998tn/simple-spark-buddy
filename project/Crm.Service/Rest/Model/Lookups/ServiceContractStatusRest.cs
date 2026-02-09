namespace Crm.Service.Rest.Model.Lookups
{
	using Crm.Library.Rest;
	using Crm.Service.Model.Lookup;

	[RestTypeFor(DomainType = typeof(ServiceContractStatus))]
	public class ServiceContractStatusRest : RestEntityLookupWithExtensionValues
	{
		public string SettableStatuses { get; set; }
	}
}
