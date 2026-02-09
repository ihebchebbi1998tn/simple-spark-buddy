namespace Main.Rest.Model
{
	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;

	[RestTypeFor(DomainType = typeof(Crm.Library.Model.Usergroup))]
	public class UsergroupRest : RestEntityWithExtensionValues
	{
		public string Name { get; set; }
		[NavigationProperty(nameof(UserUserGroupRest.UserGroupKey))] public UserUserGroupRest[] Members { get; set; }
		[NotReceived, RestrictedField] public string[] UsersIds { get; set; }
		public bool MultiFactorAuthenticationMandatory { get; set; }
	}
}
