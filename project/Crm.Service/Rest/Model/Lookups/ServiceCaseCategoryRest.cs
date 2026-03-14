namespace Crm.Service.Rest.Model.Lookups
{
	using Crm.Library.Rest;
	using Crm.Service.Model.Lookup;

	[RestTypeFor(DomainType = typeof(ServiceCaseCategory))]
	public class ServiceCaseCategoryRest : RestEntityLookupWithExtensionValues
	{
		public string Color { get; set; } = "#9E9E9E";
	}
}
