namespace Sms.Scheduler.Model.Mappings
{
	using System;

	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Type;

	public class AbsenceMap : EntityClassMapping<Absence>
	{
		public AbsenceMap()
		{
			Schema("SMS");
			Table("Absence");
			
			Id(a => a.Id, m =>
			{
				m.Column("AbsenceId");
				m.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
				m.UnsavedValue(Guid.Empty);
			});
			
			Property(x => x.Description);
			Property(x => x.TimeEntryTypeKey);
			Property(x => x.ResponsibleUser);
			Property(x => x.From, map => map.Column("`From`"));
			Property(x => x.To, map => map.Column("`To`"));
		}
	}
}
