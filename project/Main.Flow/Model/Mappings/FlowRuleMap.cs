using NHibernate.Type;
using System;

namespace Main.Flow.Model.Mappings
{
	using Crm.Library.BaseModel.Mappings;

	public class FlowRuleMap : EntityClassMapping<FlowRule>
	{
		public FlowRuleMap()
		{
			Schema("CRM");
			Table("FlowRule");

			Id(a => a.Id, m =>
			{
				m.Column("FlowRuleId");
				m.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
				m.UnsavedValue(Guid.Empty);
			});

			Property(x => x.EntityType);
			Property(x => x.Action, m =>
			{
				m.Type<EnumStringType<Actions>>();
			});
			Property(x => x.Description);
			Property(x => x.Endpoint);
			Property(x => x.Username);
			Property(x => x.Password);
		}
	}
}
