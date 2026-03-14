namespace Crm.Service.Rest.Model.Lookups
{
	using Crm.Library.Rest;
	using Crm.Service.Model.Lookup;

	[RestTypeFor(DomainType = typeof(ServiceCaseStatus))]
	public class ServiceCaseStatusRest : RestEntityLookupWithExtensionValues<int>
	{
		public string Color { get; set; } = "#9E9E9E";
		public string Groups { get; set; }
		public string SettableStatuses { get; set; }
	}
}
