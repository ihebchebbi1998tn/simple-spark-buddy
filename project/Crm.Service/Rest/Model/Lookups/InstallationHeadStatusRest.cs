namespace Crm.Service.Rest.Model.Lookups
{
	using Crm.Library.Rest;
	using Crm.Service.Model.Lookup;

	[RestTypeFor(DomainType = typeof(InstallationHeadStatus))]
	public class InstallationHeadStatusRest : RestEntityLookupWithExtensionValues
	{
		public string Color { get; set; } = "#9E9E9E";
	}
}
