namespace Crm.Service.Model.Maps
{
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;
	using NHibernate.Mapping.ByCode.Conformist;

	public class ServiceObjectMap : SubclassMapping<ServiceObject>, IDatabaseMapping
	{
		public ServiceObjectMap()
		{
			DiscriminatorValue("ServiceObject");

			ManyToOne(x => x.StandardAddress,
				m =>
				{
					m.Formula("(SELECT TOP 1 a.AddressId FROM Crm.Address a WHERE a.CompanyKey = ContactId AND a.IsCompanyStandardAddress = 1 AND a.IsActive = 1)");
					m.Insert(false);
					m.Update(false);
				});
			Property(x => x.StandardAddressId,
				m =>
				{
					m.Formula("(SELECT TOP 1 a.AddressId FROM Crm.Address a WHERE a.CompanyKey = ContactId AND a.IsCompanyStandardAddress = 1 AND a.IsActive = 1)");
					m.Insert(false);
					m.Update(false);
				});

			Join("ServiceObject", join =>
			{
				join.Schema("SMS");
				join.Table("ServiceObject");
				join.Key(x => x.Column("ContactKey"));

				join.Property(x => x.ObjectNo);
				join.Property(x => x.CategoryKey, c => c.Column("Category"));
			});

			this.EntitySet(x => x.Installations, m =>
			{
				m.Key(k =>
				{
					k.Column("FolderKey");
					k.NotNullable(false);
				});
				m.Schema("SMS");
				m.Table("InstallationHead");
				m.Inverse(true);
				m.Lazy(CollectionLazy.Lazy);
				m.Cascade(Cascade.None);
				m.BatchSize(25);
			}, action => action.OneToMany());
		}
	}
}
