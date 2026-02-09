namespace Crm.Service.Model.Maps
{
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;
	using NHibernate.Mapping.ByCode.Conformist;

	public class MaintenancePlanMap : SubclassMapping<MaintenancePlan>, IDatabaseMapping
	{
		public MaintenancePlanMap()
		{
			DiscriminatorValue("MaintenancePlan");

			Join("MaintenancePlan", join =>
			{
				join.Schema("SMS");
				join.Key(x => x.Column("ContactKey"));

				join.Property(x => x.ServiceContractId, m => m.Column("ServiceContractKey"));
				join.Property(x => x.ServiceOrderTemplateId);
				join.Property(x => x.FirstDate);
				join.Property(x => x.NextDate);
				join.Property(x => x.RhythmValue);
				join.Property(x => x.RhythmUnitKey);
				join.Property(x => x.GenerateMaintenanceOrders);
				join.Property(x => x.AllowPrematureMaintenance);

				join.ManyToOne(x => x.ServiceContract, m =>
				{
					m.Column("ServiceContractKey");
					m.Insert(false);
					m.Update(false);
				});
				join.ManyToOne(x => x.ServiceOrderTemplate, map =>
				{
					map.Column("ServiceOrderTemplateId");
					map.Fetch(FetchKind.Select);
					map.Lazy(LazyRelation.Proxy);
					map.Cascade(Cascade.None);
					map.Insert(false);
					map.Update(false);
				});

				join.EntitySet(x => x.ServiceOrders, map =>
				{
					map.Key(km => km.Column("MaintenancePlanId"));
					map.Fetch(CollectionFetchMode.Select);
					map.Lazy(CollectionLazy.Lazy);
					map.Cascade(Cascade.None);
					map.BatchSize(100);
					map.Inverse(true);
				}, action => action.OneToMany());
			});
		}
	}
}
