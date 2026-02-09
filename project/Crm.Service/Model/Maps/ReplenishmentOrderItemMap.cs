namespace Crm.Service.Model.Maps
{
	using System;

	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;

	public class ReplenishmentOrderItemMap : EntityClassMapping<ReplenishmentOrderItem>
	{
		public ReplenishmentOrderItemMap()
		{
			Schema("SMS");
			Table("ReplenishmentOrderItem");

			Id(x => x.Id, map =>
			{
				map.Column("ReplenishmentOrderItemId");
				map.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
				map.UnsavedValue(Guid.Empty);
			});

			ManyToOne(x => x.ReplenishmentOrder, m =>
			{
				m.Column("ReplenishmentOrderKey");
				m.Insert(false);
				m.Update(false);
				m.Fetch(FetchKind.Select);
				m.Lazy(LazyRelation.Proxy);
			});

			Property(x => x.MaterialNo, map => map.Column("ArticleNo"));
			Property(x => x.Description);
			Property(x => x.Quantity);
			Property(x => x.ReplenishmentOrderId, map => map.Column("ReplenishmentOrderKey"));
			Property(x => x.QuantityUnitKey);
			Property(x => x.Remark);
			
			Property(x => x.ArticleId);
			ManyToOne(x => x.Article, map =>
			{
				map.Column("ArticleId");
				map.Fetch(FetchKind.Select);
				map.Lazy(LazyRelation.Proxy);
				map.Cascade(Cascade.None);
				map.Insert(false);
				map.Update(false);
			});

			this.EntitySet(x => x.ServiceOrderMaterials, map =>
			{
				map.Key(km => km.Column("ReplenishmentOrderItemId"));
				map.Fetch(CollectionFetchMode.Select);
				map.Lazy(CollectionLazy.Lazy);
				map.Cascade(Cascade.Persist);
				map.Inverse(true);
			}, action => action.OneToMany());
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
