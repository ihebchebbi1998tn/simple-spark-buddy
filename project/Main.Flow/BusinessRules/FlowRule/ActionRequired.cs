namespace Main.Flow.BusinessRules.FlowRule
{
	using Crm.Library.Validation.BaseRules;
	using Main.Flow.Model;

	public class ActionRequired : RequiredRule<FlowRule>
	{
		public ActionRequired()
		{
			Init(x => x.Action);
		}
		public override bool IsSatisfiedBy(FlowRule entity)
		{
			return true;
		}
	}
}