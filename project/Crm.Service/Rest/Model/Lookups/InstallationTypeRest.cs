namespace Crm.Service.Rest.Model.Lookups
{
	using Crm.Library.Rest;
	using Crm.Service.Model.Lookup;

	[RestTypeFor(DomainType = typeof(InstallationType))]
	public class InstallationTypeRest : RestEntityLookupWithExtensionValues
	{
	}
}
