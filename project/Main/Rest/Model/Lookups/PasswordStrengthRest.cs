namespace Main.Rest.Model.Lookups
{
	using Crm.Library.Rest;

	[RestTypeFor(DomainType = typeof(PasswordStrength))]
	public class PasswordStrength : RestEntityLookupWithExtensionValues
	{
	}
}
