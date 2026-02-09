namespace Crm.Service.BusinessRules.ServiceOrderMaterialRules
{
	using Crm.Article.Model.Enums;
	using Crm.Library.Validation;
	using Crm.Service.Model;

	public class DiscountPercentageBetween0And100 : Rule<ServiceOrderMaterial>
	{
		// Methods
		protected override bool IsIgnoredFor(ServiceOrderMaterial serviceOrderMaterial)
		{
			return serviceOrderMaterial.DiscountType == DiscountType.Absolute;
		}

		public override bool IsSatisfiedBy(ServiceOrderMaterial serviceOrderMaterial)
		{
			return serviceOrderMaterial.Discount >= 0 &&
			       serviceOrderMaterial.Discount <= 100;
		}

		protected override RuleViolation CreateRuleViolation(ServiceOrderMaterial serviceOrderMaterial)
		{
			return RuleViolation(serviceOrderMaterial, m => m.Discount);
		}

		// Constructor
		public DiscountPercentageBetween0And100()
			: base(RuleClass.Format)
		{
		}
	}
}