namespace Crm.Service.Model.Maps
{
	using System;

	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;

	public class ServiceOrderErrorCauseMap : EntityClassMapping<ServiceOrderErrorCause>
	{
		public ServiceOrderErrorCauseMap()
		{
			Schema("SMS");
			Table("ServiceOrderErrorCauses");

			Id(x => x.Id, map =>
			{
				map.Column("ServiceOrderErrorCauseId");
				map.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
				map.UnsavedValue(Guid.Empty);
			});

			Property(x => x.StatisticsKeyCauseKey);
			Property(x => x.InternalRemark);
			Property(x => x.Description);
			Property(x => x.IsSuspected);
			Property(x => x.IsConfirmed);
			Property(x => x.IsExported);

			Property(x => x.ParentServiceOrderErrorCauseId);
			ManyToOne(x => x.ParentServiceOrderErrorCause,
				m =>
				{
					m.Column("ParentServiceOrderErrorCauseId");
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
			Property(x => x.ServiceOrderErrorTypeId);
			ManyToOne(x => x.ServiceOrderErrorType,
				m =>
				{
					m.Column("ServiceOrderErrorTypeId");
					m.Fetch(FetchKind.Select);
					m.Insert(false);
					m.Update(false);
				});

			this.EntitySet(x => x.ChildServiceOrderErrorCauses,
				m =>
				{
					m.Key(km => km.Column("ParentServiceOrderErrorCauseId"));
					m.Inverse(true);
				},
				a => a.OneToMany());
		}
	}
}
