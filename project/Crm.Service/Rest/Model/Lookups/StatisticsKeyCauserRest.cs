namespace Crm.Service.Rest.Model.Lookups
{
	using Crm.Library.Rest;
	using Crm.Service.Model.Lookup;

	[RestTypeFor(DomainType = typeof(StatisticsKeyCauser))]
	public class StatisticsKeyCauserRest : RestEntityLookupWithExtensionValues
	{
		public string Code { get; set; }
		public string ProductTypeKey { get; set; }
	}
}
