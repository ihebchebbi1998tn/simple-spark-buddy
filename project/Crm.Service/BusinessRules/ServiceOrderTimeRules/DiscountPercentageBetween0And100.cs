namespace Crm.Service.BusinessRules.ServiceOrderTimeRules
{
	using Crm.Article.Model.Enums;
	using Crm.Library.Validation;
	using Crm.Service.Model;

	public class DiscountPercentageBetween0And100 : Rule<ServiceOrderTime>
	{
		// Methods
		protected override bool IsIgnoredFor(ServiceOrderTime serviceOrderTime)
		{
			return serviceOrderTime.DiscountType == DiscountType.Absolute;
		}

		public override bool IsSatisfiedBy(ServiceOrderTime serviceOrderTime)
		{
			return serviceOrderTime.Discount >= 0 &&
			       serviceOrderTime.Discount <= 100;
		}

		protected override RuleViolation CreateRuleViolation(ServiceOrderTime serviceOrderTime)
		{
			return RuleViolation(serviceOrderTime, m => m.Discount);
		}

		// Constructor
		public DiscountPercentageBetween0And100()
			: base(RuleClass.Format)
		{
		}
	}
}