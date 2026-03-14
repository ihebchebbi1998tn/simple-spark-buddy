namespace Crm.Service.Rest.Model.Lookups
{
	using Crm.Library.Rest;
	using Crm.Service.Model.Lookup;

	[RestTypeFor(DomainType = typeof(ServiceContractType))]
	public class ServiceContractTypeRest : RestEntityLookupWithExtensionValues
	{
		public string Color { get; set; } = "#AAAAAA";
		public int GracePeriodInDays { get; set; } = 0;
	}
}
