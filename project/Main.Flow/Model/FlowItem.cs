using Crm.Library.Model;

namespace Main.Flow.Model
{
	using System;

	public class FlowItem : PostingBase
	{
		public FlowItem()
		{
			Category = PostingCategory.FlowItem;
		}

		public virtual Guid RuleKey { get; set; }
		public virtual FlowRule Rule { get; set; }
	}
}
