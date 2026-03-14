namespace Crm.Service.Model.Maps
{
	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Cfg.MappingSchema;
	using NHibernate.Mapping.ByCode;

	public class ServiceOrderTimeMap : EntityClassMapping<ServiceOrderTime>
	{
		public ServiceOrderTimeMap()
		{
			Schema("SMS");
			Table("ServiceOrderTimes");

			Id(x => x.Id, map =>
				{
					map.Column("id");
					map.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
					map.UnsavedValue(HbmUnsavedValueType.Undefined.ToNullValue());
				});

			Property(x => x.CompleteDate);
			Property(x => x.TimeZone);
			Property(x => x.CompleteUser);
			Property(x => x.OrderId);
			Property(x => x.PosNo);
			Property(x => x.ItemNo);
			Property(x => x.Description);
			Property(x => x.Comment);
			Property(x => x.EstimatedDuration);
			Property(x => x.ActualDuration);
			Property(x => x.InvoiceDuration);
			Property(x => x.Price);
			Property(x => x.TotalValue);
			Property(x => x.Discount, m => m.Column("DiscountPercent"));
			Property(x => x.DiscountType);
			Property(x => x.HasTool);
			Property(x => x.CreatedLocal);
			Property(x => x.TransferDate);
			Property(x => x.InstallationPosId);
			Property(x => x.CausingItemNo);
			Property(x => x.CausingItemSerialNo);
			Property(x => x.CausingItemPreviousSerialNo);
			Property(x => x.Diagnosis);
			Property(x => x.IsActive);

			Property(x => x.StatusKey, map => map.Column("Status"));
			Property(x => x.LocationKey, map => map.Column("Location"));
			Property(x => x.CategoryKey, map => map.Column("Category"));
			Property(x => x.PriorityKey, map => map.Column("Priority"));
			Property(x => x.NoCausingItemSerialNoReasonKey, map => map.Column("NoCausingItemSerialNoReason"));
			Property(x => x.NoCausingItemPreviousSerialNoReasonKey, map => map.Column("NoCausingItemPreviousSerialNoReason"));

			Property(x => x.IsCostLumpSum);
			Property(x => x.IsMaterialLumpSum);
			Property(x => x.IsTimeLumpSum);
			Property(x => x.InvoicingTypeKey);

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
			Property(x => x.InstallationId);
			ManyToOne(x => x.Installation, map =>
			{
				map.Column("InstallationId");
				map.Fetch(FetchKind.Select);
				map.Lazy(LazyRelation.Proxy);
				map.Cascade(Cascade.None);
				map.Insert(false);
				map.Update(false);
			});
			ManyToOne(x => x.ServiceOrderHead,
				m =>
				{
					m.Column("OrderId");
					m.Fetch(FetchKind.Select);
					m.Insert(false);
					m.Update(false);
				});

			this.EntitySet(x => x.ServiceOrderTimeDispatches,
				m =>
				{
					m.Key(km => km.Column(nameof(ServiceOrderTimeDispatch.ServiceOrderTimeId)));
					m.Inverse(true);
				},
				a => a.OneToMany());

			this.EntitySet(x => x.Postings,
				m =>
					{
						m.Key(km => km.Column("OrderTimesId"));
						m.Inverse(true);
					},
				a => a.OneToMany());

			this.EntitySet(x => x.ServiceCases,
				m =>
					{
						m.Key(km => km.Column("ServiceOrderTimeId"));
						m.Inverse(true);
					},
				a => a.OneToMany());

			this.EntitySet(x => x.ServiceOrderMaterials,
				m =>
					{
						m.Key(km => km.Column("ServiceOrderTimeId"));
						m.Inverse(true);
					},
				a => a.OneToMany());


			this.EntitySet(x => x.ServiceOrderErrorTypes,
				m =>
				{
					m.Key(km => km.Column("ServiceOrderTimeId"));
					m.Inverse(true);
				},
				a => a.OneToMany());
		}
	}
}
