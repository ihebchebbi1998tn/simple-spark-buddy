using Crm.Library.EntityConfiguration;

using Main.Flow.Model;

namespace Main.Flow.Configuration
{
	public class FlowRuleConfiguration : EntityConfiguration<FlowRule>
	{
		public override void Initialize()
		{
			Property(x => x.CreateDate, m => m.Sortable());
			Property(x => x.ModifyDate, m => m.Sortable());
			Property(x => x.EntityType, c => c.Filterable(f => f.Caption("EntityType")));
			Property(x => x.Username, c => c.Filterable(f => f.Caption("Username")));
			Property(x => x.Endpoint, c => c.Filterable(f => f.Caption("Endpoint")));
			Property(x => x.CreateUser, m => m.Filterable(x => x.Definition(new UserFilterDefinition())));
			Property(x => x.CreateDate, m => m.Filterable(x => x.Definition(new DateFilterDefinition())));
		}
		public FlowRuleConfiguration(IEntityConfigurationHolder<FlowRule> entityConfigurationHolder)
			: base(entityConfigurationHolder)
		{
		}
	}
}
