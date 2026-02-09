namespace Main.Model.Lookups
{
	using Crm.Library.Globalization.Lookup;

	[Lookup("[LU].[MultiFactorAuthenticationMode]")]
	[NotEditable]
	public class MultiFactorAuthenticationMode : EntityLookup<string>
	{
		public const string MandatoryForSpecificUserGroupsKey = "MandatoryForSpecificUserGroups";
		public const string MandatoryForAllUsersKey = "MandatoryForAllUsers";
	}
}
