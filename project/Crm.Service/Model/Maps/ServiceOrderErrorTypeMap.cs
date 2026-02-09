namespace Crm.Service.Model.Maps
{
	using System;

	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;

	public class ServiceOrderErrorTypeMap : EntityClassMapping<ServiceOrderErrorType>
	{
		public ServiceOrderErrorTypeMap()
		{
			Schema("SMS");
			Table("ServiceOrderErrorTypes");

			Id(x => x.Id, map =>
			{
				map.Column("ServiceOrderErrorTypeId");
				map.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
				map.UnsavedValue(Guid.Empty);
			});

			Property(x => x.StatisticsKeyFaultImageKey);
			Property(x => x.InternalRemark);
			Property(x => x.Description);
			Property(x => x.IsSuspected);
			Property(x => x.IsConfirmed);
			Property(x => x.IsMainErrorType);
			Property(x => x.IsExported);

			Property(x => x.OrderId);
			ManyToOne(x => x.ServiceOrderHead,
				m =>
				{
					m.Column("OrderId");
					m.Fetch(FetchKind.Select);
					m.Insert(false);
					m.Update(false);
				});

			Property(x => x.DispatchId);
			ManyToOne(x => x.ServiceOrderDispatch,
			m =>
			{
				m.Column("DispatchId");
				m.Fetch(FetchKind.Select);
				m.Insert(false);
				m.Update(false);
			});

			Property(x => x.ParentServiceOrderErrorTypeId);
			ManyToOne(x => x.ParentServiceOrderErrorType,
			m =>
			{
				m.Column("ParentServiceOrderErrorTypeId");
				m.Fetch(FetchKind.Select);
				m.Insert(false);
				m.Update(false);
			});

			Property(x => x.ServiceOrderTimeId);
			ManyToOne(x => x.ServiceOrderTime, map =>
			{
				map.Column("ServiceOrderTimeId");
				map.Fetch(FetchKind.Select);
				map.Lazy(LazyRelation.Proxy);
				map.Cascade(Cascade.None);
				map.Insert(false);
				map.Update(false);
			});

			Property(x => x.ServiceCaseId);
			ManyToOne(x => x.ServiceCase, map =>
			{
				map.Column("ServiceCaseId");
				map.Fetch(FetchKind.Select);
				map.Lazy(LazyRelation.Proxy);
				map.Cascade(Cascade.None);
				map.Insert(false);
				map.Update(false);
			});

			this.EntitySet(x => x.ChildServiceOrderErrorTypes,
				m =>
				{
					m.Key(km => km.Column("ParentServiceOrderErrorTypeId"));
					m.Inverse(true);
				},
				a => a.OneToMany());
			this.EntitySet(x => x.ServiceOrderErrorCauses,
				m =>
				{
					m.Key(km => km.Column("ServiceOrderErrorTypeId"));
					m.Inverse(true);
				},
				a => a.OneToMany());
		}
	}
}
