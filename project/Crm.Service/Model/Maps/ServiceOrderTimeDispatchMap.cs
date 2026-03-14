namespace Crm.Service.Model.Maps
{
	using System;

	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;

	using GuidCombGeneratorDef = LMobile.Unicore.NHibernate.GuidCombGeneratorDef;

	public class ServiceOrderTimeDispatchMap : EntityClassMapping<ServiceOrderTimeDispatch>
	{
		public ServiceOrderTimeDispatchMap()
		{
			Schema("SMS");
			Table("ServiceOrderTimeDispatch");

			Id(x => x.Id,
				map =>
				{
					map.Column("ServiceOrderTimeDispatchId");
					map.Generator(GuidCombGeneratorDef.Instance);
					map.UnsavedValue(Guid.Empty);
				});

			Property(x => x.ServiceOrderDispatchId);
			ManyToOne(x => x.ServiceOrderDispatch, map =>
			{
				map.Column(nameof(ServiceOrderTimeDispatch.ServiceOrderDispatchId));
				map.Fetch(FetchKind.Select);
				map.Lazy(LazyRelation.Proxy);
				map.Cascade(Cascade.None);
				map.Insert(false);
				map.Update(false);
			});
			Property(x => x.ServiceOrderTimeId);
			ManyToOne(x => x.ServiceOrderTime, map =>
			{
				map.Column(nameof(ServiceOrderTimeDispatch.ServiceOrderTimeId));
				map.Fetch(FetchKind.Select);
				map.Lazy(LazyRelation.Proxy);
				map.Cascade(Cascade.None);
				map.Insert(false);
				map.Update(false);
			});
		}
	}
}
