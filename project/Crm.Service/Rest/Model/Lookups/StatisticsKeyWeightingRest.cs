namespace Crm.Service.Rest.Model.Lookups
{
	using Crm.Library.Rest;
	using Crm.Service.Model.Lookup;

	[RestTypeFor(DomainType = typeof(StatisticsKeyWeighting))]
	public class StatisticsKeyWeightingRest : RestEntityLookupWithExtensionValues
	{
		public string Code { get; set; }
		public string ProductTypeKey { get; set; }
	}
}
