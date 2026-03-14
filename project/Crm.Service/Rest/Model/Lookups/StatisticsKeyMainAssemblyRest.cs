namespace Crm.Service.Rest.Model.Lookups
{
	using Crm.Library.Rest;
	using Crm.Service.Model.Lookup;

	[RestTypeFor(DomainType = typeof(StatisticsKeyMainAssembly))]
	public class StatisticsKeyMainAssemblyRest : RestEntityLookupWithExtensionValues
	{
		public string Code { get; set; }
		public string ProductTypeKey { get; set; }
	}
}
