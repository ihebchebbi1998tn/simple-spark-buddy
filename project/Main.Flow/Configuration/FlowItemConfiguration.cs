using System;
using System.Linq;
using Crm.Library.EntityConfiguration;
using Crm.Library.Model;
using Main.Flow.Model;

namespace Main.Flow.Configuration
{
	public class FlowItemConfiguration : EntityConfiguration<FlowItem>
	{
		public override void Initialize()
		{
			Property(x => x.CreateDate, m => m.Sortable());
			Property(x => x.ModifyDate, m => m.Sortable());
			var stateDropDownFilter = new DropDownFilterDefinition(Enum.GetValues(typeof(PostingState)).Cast<PostingState>().ToDictionary(s => s.ToString(), s => s.ToString()));
			Property(x => x.PostingState, m => m.Filterable(x => x.Definition(stateDropDownFilter)));
			Property(x => x.CreateUser, m => m.Filterable(x => x.Definition(new UserFilterDefinition())));
			Property(x => x.CreateDate, m => m.Filterable(x => x.Definition(new DateFilterDefinition())));
			var typeDropDownFilter = new DropDownFilterDefinition(Enum.GetValues(typeof(PostingType)).Cast<PostingType>().ToDictionary(t => t.ToString(), t => t.ToString()));
			Property(x => x.PostingType, m => m.Filterable(x => x.Definition(typeDropDownFilter)));
		}
		public FlowItemConfiguration(IEntityConfigurationHolder<FlowItem> entityConfigurationHolder)
			: base(entityConfigurationHolder)
		{
		}
	}
}
