namespace Crm.Service.Rest.Model.Lookups
{
	using Crm.Library.Rest;
	using Crm.Service.Model.Lookup;

	[RestTypeFor(DomainType = typeof(StatisticsKeyFaultImage))]
	public class StatisticsKeyFaultImageRest : RestEntityLookupWithExtensionValues
	{
		public string Code { get; set; }
		public string ProductTypeKey { get; set; }
		public string AssemblyGroupKey { get; set; }
		public string InstallationTypeKey { get; set; }
	}
}
