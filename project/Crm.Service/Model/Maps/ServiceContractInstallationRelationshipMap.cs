namespace Crm.Service.Model.Maps
{
	using System;

	using Crm.Library.BaseModel.Mappings;
	using Crm.Service.Model.Relationships;

	using NHibernate.Mapping.ByCode;
	using NHibernate.Type;

	public class ServiceContractInstallationRelationshipMap : EntityClassMapping<ServiceContractInstallationRelationship>
	{
		public ServiceContractInstallationRelationshipMap()
		{
			Schema("SMS");
			Table("ServiceContractInstallationRelationship");

			Id(x => x.Id, m =>
				{
					m.Column("ServiceContractInstallationRelationshipId");
					m.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
					m.UnsavedValue(Guid.Empty);
				});

			Property(x => x.ParentId, m => m.Column("ServiceContractKey"));
			ManyToOne(x => x.Parent, m =>
				{
					m.Column("ServiceContractKey");
					m.Insert(false);
					m.Update(false);
					m.Lazy(LazyRelation.Proxy);
				});
			Property(x => x.ChildId, m => m.Column("InstallationKey"));
			ManyToOne(x => x.Child, m =>
				{
					m.Column("InstallationKey");
					m.Insert(false);
					m.Update(false);
					m.Lazy(LazyRelation.Proxy);
				});
			Property(x => x.TimeAllocation, m => m.Type<TimeAsTimeSpanType>());
			Property(x => x.Information);
		}
	}
}