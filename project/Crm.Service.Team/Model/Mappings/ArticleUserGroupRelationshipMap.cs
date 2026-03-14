namespace Crm.Service.Team.Model.Mappings
{
	using System;

	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;

	using GuidCombGeneratorDef = LMobile.Unicore.NHibernate.GuidCombGeneratorDef;

	public class ArticleUserGroupRelationshipMap : EntityClassMapping<ArticleUserGroupRelationship>
	{
		public ArticleUserGroupRelationshipMap()
		{
			Schema("CRM");
			Table("ArticleUserGroupRelationship");

			Id(
				x => x.Id,
				map =>
				{
					map.Column("ArticleUserGroupRelationshipId");
					map.Generator(GuidCombGeneratorDef.Instance);
					map.UnsavedValue(Guid.Empty);
				});
			Property(x => x.ArticleKey);
			Property(x => x.UserGroupKey);
			Property(x => x.From, map => map.Column("`From`"));
			Property(x => x.To, map => map.Column("`To`"));
			ManyToOne(x => x.Article, m =>
			{
				m.Column("ArticleKey");
				m.Insert(false);
				m.Update(false);
				m.Lazy(LazyRelation.Proxy);
			});
			ManyToOne(x => x.UserGroup, m =>
			{
				m.Column("UserGroupKey");
				m.Insert(false);
				m.Update(false);
				m.Fetch(FetchKind.Select);
				m.Lazy(LazyRelation.Proxy);
			});
		}
	}
}
