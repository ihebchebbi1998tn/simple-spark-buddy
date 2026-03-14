namespace Crm.Service.BusinessRules.ReplenishmentOrder
{
	using Crm.Library.Validation;
	using Crm.Service.Model;

	public class ItemQuantityPositive : Rule<ReplenishmentOrderItem>
	{
		public override bool IsSatisfiedBy(ReplenishmentOrderItem item)
		{
			return item.Quantity > 0;
		}

		protected override RuleViolation CreateRuleViolation(ReplenishmentOrderItem item)
		{
			return RuleViolation(item, i => i.Quantity);
		}

		// Constructor
		public ItemQuantityPositive()
			: base(RuleClass.Positive)
		{
		}
	}
}
