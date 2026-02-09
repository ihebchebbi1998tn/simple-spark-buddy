namespace Main.Flow.Rest.Model
{
	using System;

	using Crm.Library.Rest;
	using Crm.Library.Rest.Model;

	using Main.Flow.Model;

	[RestTypeFor(DomainType = typeof(FlowItem))]
	public class FlowItemRest : PostingBaseRest
	{
		public virtual Guid RuleKey { get; set; }
	}
}
