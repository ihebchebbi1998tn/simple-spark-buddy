namespace Crm.Service.Rest.Model.Lookups
{
	using Crm.Library.Rest;
	using Crm.Service.Model.Lookup;

	[RestTypeFor(DomainType = typeof(ServiceObjectCategory))]
	public class ServiceObjectCategoryRest : RestEntityLookupWithExtensionValues
	{
		public string Color { get; set; } = "#9E9E9E";
	}
}
