namespace Sms.Scheduler.Model.Mappings
{
	using Crm.Library.BaseModel.Mappings;
	using System;
	public class DispatchArticleAssignmentMap : EntityClassMapping<DispatchArticleAssignment>
	{
		public DispatchArticleAssignmentMap()
		{
			Schema("SMS");
			Table("DispatchArticleAssignment");

			Id(a => a.Id, m =>
			{
				m.Column("DispatchArticleAssignmentId");
				m.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
				m.UnsavedValue(Guid.Empty);
			});

			Property(x => x.DispatchKey);
			Property(x => x.ResourceKey);
		}
	}
}
