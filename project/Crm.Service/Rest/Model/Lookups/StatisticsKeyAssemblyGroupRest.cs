namespace Crm.Service.Rest.Model.Lookups
{
	using Crm.Library.Rest;
	using Crm.Service.Model.Lookup;

	[RestTypeFor(DomainType = typeof(StatisticsKeyAssemblyGroup))]
	public class StatisticsKeyAssemblyGroupRest : RestEntityLookupWithExtensionValues
	{
		public string Code { get; set; }
		public string ProductTypeKey { get; set; }
		public string MainAssemblyKey { get; set; }
		public string SubAssemblyKey { get; set; }
	}
}
