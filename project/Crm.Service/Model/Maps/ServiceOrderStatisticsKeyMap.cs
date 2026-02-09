namespace Crm.Service.Model.Maps
{
	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Cfg.MappingSchema;
	using NHibernate.Mapping.ByCode;

	using GuidCombGeneratorDef = LMobile.Unicore.NHibernate.GuidCombGeneratorDef;

	public class ServiceOrderStatisticsKeyMap : EntityClassMapping<ServiceOrderStatisticsKey>
	{
		public ServiceOrderStatisticsKeyMap()
		{
			Schema("SMS");
			Table("ServiceOrderStatisticsKey");

			Id(x => x.Id,
				map =>
				{
					map.Column("ServiceOrderStatisticsKeyId");
					map.Generator(GuidCombGeneratorDef.Instance);
					map.UnsavedValue(HbmUnsavedValueType.Undefined.ToNullValue());
				});

			Property(x => x.ProductTypeKey);
			Property(x => x.MainAssemblyKey);
			Property(x => x.SubAssemblyKey);
			Property(x => x.AssemblyGroupKey);
			Property(x => x.FaultImageKey);
			Property(x => x.RemedyKey);
			Property(x => x.CauseKey);
			Property(x => x.WeightingKey);
			Property(x => x.CauserKey);

			Property(x => x.DispatchId);
			ManyToOne(x => x.Dispatch,
				m =>
				{
					m.Column("DispatchId");
					m.Fetch(FetchKind.Select);
					m.Insert(false);
					m.Update(false);
				});
			Property(x => x.ServiceOrderId);
			ManyToOne(x => x.ServiceOrder,
				m =>
				{
					m.Column("ServiceOrderId");
					m.Fetch(FetchKind.Select);
					m.Insert(false);
					m.Update(false);
				});
		}
	}
}
