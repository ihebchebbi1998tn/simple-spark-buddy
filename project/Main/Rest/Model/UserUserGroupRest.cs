using System;

using Crm.Library.Api.Attributes;
using Crm.Library.Rest;

using Main.Model;

namespace Main.Rest.Model
{
	using Crm.Library.Model;

	[RestTypeFor(DomainType = typeof(UserUserGroup))]

	public class UserUserGroupRest : RestEntity
	{
		public Guid Id { get; set; }
		public Guid UserGroupKey { get; set; }
		public string Username { get; set; }
		[NavigationProperty(nameof(Username))] public UserRest User { get; set; }
		[NavigationProperty(nameof(UserGroupKey))] public UsergroupRest UserGroup { get; set; }

	}
}
