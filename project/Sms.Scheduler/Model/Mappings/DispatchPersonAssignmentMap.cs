namespace Sms.Scheduler.Model.Mappings
{
	using Crm.Library.BaseModel.Mappings;
	using System;

	using NHibernate.Mapping.ByCode;

	public class DispatchPersonAssignmentMap : EntityClassMapping<DispatchPersonAssignment>
	{
		public DispatchPersonAssignmentMap()
		{
			Schema("SMS");
			Table("DispatchPersonAssignment");

			Id(a => a.Id, m =>
			{
				m.Column("DispatchPersonAssignmentId");
				m.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
				m.UnsavedValue(Guid.Empty);
			});

			Property(x => x.DispatchKey);
			Property(x => x.ResourceKey);
			ManyToOne(x => x.Dispatch, map =>
			{
				map.Column("DispatchKey");
				map.Fetch(FetchKind.Select);
				map.Lazy(LazyRelation.Proxy);
				map.Cascade(Cascade.None);
				map.Insert(false);
				map.Update(false);
			});
		}
	}
}
