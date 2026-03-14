namespace Crm.Service.Team.Rest.Model
{
	using System;

	using Crm.Article.Rest.Model;
	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;
	using Crm.Service.Team.Model;

	using Main.Rest.Model;

	[RestTypeFor(DomainType = typeof(ArticleUserGroupRelationship))]

	public class ArticleUserGroupRelationshipRest : RestEntityWithExtensionValues
	{
		public virtual Guid ArticleKey { get; set; }
		public virtual Guid UserGroupKey { get; set; }
		public virtual DateTime? From { get; set; }
		public virtual DateTime? To { get; set; }

		[NavigationProperty(nameof(ArticleKey))]
		public virtual ArticleRest Article { get; set; }

		[NavigationProperty(nameof(UserGroupKey))]
		public virtual UsergroupRest UserGroup { get; set; }
	}
}
