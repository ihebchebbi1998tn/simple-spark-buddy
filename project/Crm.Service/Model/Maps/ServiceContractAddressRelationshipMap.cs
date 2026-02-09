namespace Crm.Service.Model.Maps
{
	using System;

	using Crm.Library.BaseModel.Mappings;
	using Crm.Service.Model.Relationships;

	using NHibernate.Mapping.ByCode;

	public class ServiceContractAddressRelationshipMap : EntityClassMapping<ServiceContractAddressRelationship>
	{
		public ServiceContractAddressRelationshipMap()
		{
			Schema("SMS");
			Table("ServiceContractAddressRelationship");

			Id(x => x.Id, m =>
			{
				m.Column("ServiceContractAddressRelationshipId");
				m.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
				m.UnsavedValue(Guid.Empty);
			});

			Property(x => x.RelationshipTypeKey, m => m.Column("RelationshipType"));

			Property(x => x.ParentId, m => m.Column("ServiceContractKey"));
			ManyToOne(x => x.Parent, m =>
			{
				m.Column("ServiceContractKey");
				m.Insert(false);
				m.Update(false);
				m.Lazy(LazyRelation.Proxy);
			});
			Property(x => x.ChildId, m => m.Column("AddressKey"));
			ManyToOne(x => x.Child, m =>
			{
				m.Column("AddressKey");
				m.Insert(false);
				m.Update(false);
				m.Lazy(LazyRelation.Proxy);
			});
			Property(x => x.Information);
		}
	}
}