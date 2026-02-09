namespace Crm.Service.Model.Maps
{
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;
	using NHibernate.Mapping.ByCode.Conformist;

	public class InstallationMap : SubclassMapping<Installation>, IDatabaseMapping
	{
		public InstallationMap()
		{
			DiscriminatorValue("Installation");
			Join("InstallationHead", join =>
				{
					join.Schema("SMS");
					join.Key(x => x.Column("ContactKey"));
					join.Property(x => x.InstallationNo);
					join.Property(x => x.Description);
					join.Property(x => x.ExactPlace);
					join.Property(x => x.ExternalReference);
					join.Property(x => x.Room);
					join.Property(x => x.MaintenanceContractNo);
					join.Property(x => x.LegacyInstallationId);
					join.Property(x => x.SoftwareVersion);
					join.Property(x => x.Priority);
					join.Property(x => x.WarrantyFrom);
					join.Property(x => x.WarrantyUntil);
					join.Property(x => x.KickOffDate);
					join.Property(x => x.TechnicianInformation);
					join.Property(x => x.ManufactureDate);

					join.Property(x => x.StatusKey, map => map.Column("Status"));
					join.Property(x => x.InstallationTypeKey, map => map.Column("InstallationType"));
					join.Property(x => x.ManufacturerKey, map => map.Column("Manufacturer"));
					join.Property(x => x.StatisticsKeyProductTypeKey);
					join.Property(x => x.CustomItemNo);
					join.Property(x => x.CustomItemDesc);

					join.Property(x => x.LocationContactId);
					join.ManyToOne(x => x.LocationCompany, map =>
						{
							map.Column("LocationContactId");
							map.Cascade(Cascade.None);
							map.Fetch(FetchKind.Select);
							map.Lazy(LazyRelation.Proxy);
							map.Insert(false);
							map.Update(false);
						});

					join.Property(x => x.LocationPersonId);
					join.ManyToOne(x => x.LocationContactPerson, map =>
						{
							map.Column("LocationPersonId");
							map.Cascade(Cascade.None);
							map.Fetch(FetchKind.Select);
							map.Lazy(LazyRelation.Proxy);
							map.Insert(false);
							map.Update(false);
						});

					join.Property(x => x.LocationAddressKey);
					join.ManyToOne(x => x.LocationAddress, map =>
						{
							map.Column("LocationAddressKey");
							map.Cascade(Cascade.None);
							map.Fetch(FetchKind.Select);
							map.Lazy(LazyRelation.Proxy);
							map.Insert(false);
							map.Update(false);
						});

					join.Property(x => x.PreferredUser);
					join.ManyToOne(x => x.PreferredUserObj, map =>
						{
							map.Column("PreferredUser");
							map.Cascade(Cascade.None);
							map.Fetch(FetchKind.Select);
							map.Lazy(LazyRelation.Proxy);
							map.Insert(false);
							map.Update(false);
						});
					join.Property(x => x.FolderId, m => m.Column("FolderKey"));
					join.ManyToOne(x => x.ServiceObject, map =>
					{
						map.Column("FolderKey");
						map.Cascade(Cascade.None);
						map.Fetch(FetchKind.Select);
						map.Lazy(LazyRelation.Proxy);
						map.Insert(false);
						map.Update(false);
					});

					join.EntityBag(x => x.AddressRelationships, m =>
					{
						m.Key(k =>
						{
							k.Column("InstallationKey");
							k.NotNullable(true);
						});
						m.Inverse(true);
						m.Lazy(CollectionLazy.Lazy);
						m.Cascade(Cascade.None);
					}, action => action.OneToMany());

					join.Property(x => x.StationKey);
					join.ManyToOne(x => x.Station, map =>
					{
						map.Column("StationKey");
						map.Fetch(FetchKind.Select);
						map.Lazy(LazyRelation.Proxy);
						map.Cascade(Cascade.None);
						map.Insert(false);
						map.Update(false);
					});

					join.Property(x => x.ArticleKey);
					join.ManyToOne(x => x.Article, map =>
					{
						map.Column("ArticleKey");
						map.Fetch(FetchKind.Select);
						map.Lazy(LazyRelation.Proxy);
						map.Cascade(Cascade.None);
						map.Insert(false);
						map.Update(false);
					});

					Set(x => x.AdditionalContacts, map =>
						{
							map.Schema("SMS");
							map.Table("InstallationAdditionalContacts");
							map.Key(km => km.Column("InstallationId"));
							map.Cascade(Cascade.None);
						}, r => r.ManyToMany(m => m.Column("ContactId")));

					Set(x => x.ServiceContractInstallationRelationships, m =>
					{
						m.Key(k =>
						{
							k.Column("InstallationKey");
							k.NotNullable(true);
						});
						m.Inverse(true);
						m.Lazy(CollectionLazy.Lazy);
						m.Cascade(Cascade.None);
					}, action => action.OneToMany());
				});
		}
	}
}
