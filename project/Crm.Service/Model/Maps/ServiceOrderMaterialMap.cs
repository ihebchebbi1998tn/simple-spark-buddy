namespace Crm.Service.Model.Maps
{
	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Cfg.MappingSchema;
	using NHibernate.Mapping.ByCode;
	using NHibernate.Type;

	public class ServiceOrderMaterialMap : EntityClassMapping<ServiceOrderMaterial>
	{
		public ServiceOrderMaterialMap()
		{
			Schema("SMS");
			Table("ServiceOrderMaterial");

			Id(x => x.Id, map =>
				{
					map.Column("id");
					map.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
					map.UnsavedValue(HbmUnsavedValueType.Undefined.ToNullValue());
				});
			Version(x => x.Version, map => map.Type(new Int64Type()));

			Property(x => x.OrderId);
			Property(x => x.PosNo);
			Property(x => x.ItemNo);
			Property(x => x.Description);
			Property(x => x.ArticleTypeKey);
			Property(x => x.ParentServiceOrderMaterialId);
			Property(x => x.ParentServiceOrderMaterialVersion, map => map.Type<Int64Type>());
			Property(x => x.Quantity);
			Property(x => x.QuantityUnitKey, map => map.Column("QuantityUnit"));
			Property(x => x.FromLocation, map => map.Column("FromLocationNo"));
			Property(x => x.FromStorageArea);
			Property(x => x.FromWarehouse);
			Property(x => x.ServiceOrderMaterialType);
			Property(x => x.ToLocation, map => map.Column("ToLocationNo"));
			Property(x => x.ToWarehouse);
			Property(x => x.Price, map => map.Column("HourlyRate"));
			Property(x => x.TotalValue);
			Property(x => x.Discount, m => m.Column("DiscountPercent"));
			Property(x => x.DiscountType);
			Property(x => x.Status);
			Property(x => x.TransferDate);
			Property(x => x.BuiltIn);
			Property(x => x.IsSerial);
			Property(x => x.InternalRemark);
			Property(x => x.ExternalRemark);
			Property(x => x.CommissioningStatusKey);
			Property(x => x.ServiceOrderTimeId);
			Property(x => x.BatchNo);
			Property(x => x.IsBatch);
			Property(x => x.ReplenishmentOrderItemId);
			ManyToOne(x => x.ReplenishmentOrderItem, map =>
			{
				map.Column("ReplenishmentOrderItemId");
				map.Fetch(FetchKind.Select);
				map.Lazy(LazyRelation.Proxy);
				map.Cascade(Cascade.None);
				map.Insert(false);
				map.Update(false);
			});
			Property(x => x.IsActive);

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
			ManyToOne(x => x.ServiceOrderTime,
				m =>
					{
						m.Column("ServiceOrderTimeId");
						m.Fetch(FetchKind.Select);
						m.Insert(false);
						m.Update(false);
					});
			ManyToOne(x => x.ServiceOrderHead,
				m =>
					{
						m.Column("OrderId");
						m.Fetch(FetchKind.Select);
						m.Insert(false);
						m.Update(false);
					});
			ManyToOne(x => x.ServiceOrderDispatch,
				m =>
				{
					m.Column("DispatchId");
					m.Fetch(FetchKind.Select);
					m.Insert(false);
					m.Update(false);
				});
			Property(x => x.CreatedLocal);
			Property(x => x.DispatchId);
			Property(x => x.SignedByCustomer);

			Property(x => x.SerialNo);
			Property(x => x.PreviousSerialNo);
			Property(x => x.SerialId);
			ManyToOne(x => x.Serial, map =>
			{
				map.Column("SerialId");
				map.Fetch(FetchKind.Select);
				map.Lazy(LazyRelation.Proxy);
				map.Cascade(Cascade.None);
				map.Insert(false);
				map.Update(false);
			});
			Property(x => x.NoPreviousSerialNoReasonKey);
			Property(x => x.IsInstalled);

			this.EntitySet(x => x.ChildServiceOrderMaterials, map =>
			{
				map.Key(km => km.Column("ParentServiceOrderMaterialId"));
				map.Fetch(CollectionFetchMode.Select);
				map.Lazy(CollectionLazy.Lazy);
				map.Cascade(Cascade.None);
				map.Inverse(true);
			}, action => action.OneToMany());
			this.EntitySet(x => x.DocumentAttributes, map =>
			{
				map.Key(km => km.Column("ServiceOrderMaterialId"));
				map.Fetch(CollectionFetchMode.Select);
				map.Lazy(CollectionLazy.Lazy);
				map.Cascade(Cascade.None);
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
