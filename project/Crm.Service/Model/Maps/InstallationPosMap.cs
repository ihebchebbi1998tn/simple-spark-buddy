namespace Crm.Service.Model.Maps
{
	using System;

	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;

	public class InstallationPosMap : EntityClassMapping<InstallationPos>
	{
		public InstallationPosMap()
		{
			Schema("SMS");
			Table("InstallationPos");

			Id(x => x.Id, map =>
				{
					map.Column("id");
					map.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
					map.UnsavedValue(Guid.Empty);
				});

			Property(x => x.ArticleId);
			Property(x => x.RelatedInstallationId);
			Property(x => x.InstallationId);
			Property(x => x.PosNo);
			Property(x => x.ItemNo);
			Property(x => x.Description);
			Property(x => x.Supplier);
			Property(x => x.QuantityUnitKey, map => map.Column("QuantityUnit"));
			Property(x => x.Quantity);
			Property(x => x.IsInstalled);
			Property(x => x.InstallDate);
			Property(x => x.Comment);
			Property(x => x.WarrantyStartSupplier);
			Property(x => x.WarrantyEndSupplier);
			Property(x => x.WarrantyStartCustomer);
			Property(x => x.WarrantyEndCustomer);
			Property(x => x.RdsPpClassification);
			Property(x => x.LegacyInstallationId);
			Property(x => x.IsGroupItem);
			Property(x => x.GroupLevel);
			Property(x => x.ReferenceId);
			Property(x => x.RemoveDate);
			Property(x => x.BatchNo);
			Property(x => x.SerialNo);
			Property(x => x.IsSerial);
			ManyToOne(x => x.Article, m =>
			{
				m.Column("ArticleId");
				m.Insert(false);
				m.Update(false);
				m.Fetch(FetchKind.Select);
				m.Lazy(LazyRelation.Proxy);
			});
			ManyToOne(x => x.Installation, m =>
			{
				m.Column("InstallationId");
				m.Insert(false);
				m.Update(false);
				m.Fetch(FetchKind.Select);
				m.Lazy(LazyRelation.Proxy);
			});
			ManyToOne(x => x.RelatedInstallation, m =>
			{
				m.Column("RelatedInstallationId");
				m.Insert(false);
				m.Update(false);
				m.Fetch(FetchKind.Select);
				m.Lazy(LazyRelation.Proxy);
			});
			ManyToOne(x => x.Parent, m =>
			{
				m.Column("ReferenceId");
				m.Insert(false);
				m.Update(false);
				m.Fetch(FetchKind.Select);
				m.Lazy(LazyRelation.Proxy);
			});
			this.EntitySet(x => x.Children,
				m =>
				{
					m.Key(km => km.Column("ReferenceId"));
					m.Inverse(true);
					m.Cascade(Cascade.Remove);
				},
				a => a.OneToMany());
			Property(x => x.QuantityUnitEntryKey);
			ManyToOne(x => x.QuantityUnitEntry, m =>
			{
				m.Column("QuantityUnitEntryKey");
				m.Fetch(FetchKind.Select);
				m.Insert(false);
				m.Update(false);
			});
		}
	}
}
