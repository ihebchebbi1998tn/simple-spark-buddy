namespace Crm.Service.Rest.Model.Lookups
{
	using Crm.Library.Rest;
	using Crm.Service.Model.Lookup;

	[RestTypeFor(DomainType = typeof(StatisticsKeyProductType))]
	public class StatisticsKeyProductTypeRest : RestEntityLookupWithExtensionValues
	{
		public string Code { get; set; }
	}
}
