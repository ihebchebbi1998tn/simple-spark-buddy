namespace Crm.Service.Model.Maps
{
	using System;

	using Crm.Library.BaseModel.Mappings;
	using Crm.Service.Model.Relationships;

	using NHibernate.Mapping.ByCode;

	public class InstallationCompanyRelationshipMap : EntityClassMapping<InstallationCompanyRelationship>
	{
		public InstallationCompanyRelationshipMap()
		{
			Schema("SMS");
			Table("InstallationCompanyRelationship");

			Id(x => x.Id,
				m =>
				{
					m.Column("InstallationCompanyRelationshipId");
					m.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
					m.UnsavedValue(Guid.Empty);
				});

			Property(x => x.RelationshipTypeKey, m => m.Column("RelationshipType"));
			Property(x => x.ParentId, m => m.Column("InstallationKey"));

			ManyToOne(x => x.Parent,
				m =>
				{
					m.Column("InstallationKey");
					m.Insert(false);
					m.Update(false);
					m.Lazy(LazyRelation.Proxy);
				});
			Property(x => x.ChildId, m => m.Column("CompanyKey"));
			ManyToOne(x => x.Child,
				m =>
				{
					m.Column("CompanyKey");
					m.Insert(false);
					m.Update(false);
					m.Lazy(LazyRelation.Proxy);
				});
			Property(x => x.Information);
		}
	}
}
