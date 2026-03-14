namespace Crm.Service.BusinessRules.ServiceOrderMaterialRules
{
	using Crm.Library.Validation;
	using Crm.Service.Model;

	public class QuantityNotNegative : Rule<ServiceOrderMaterial>
	{
		public override bool IsSatisfiedBy(ServiceOrderMaterial material)
		{
			return material.Quantity >= 0;
		}

		protected override RuleViolation CreateRuleViolation(ServiceOrderMaterial material)
		{
			return RuleViolation(material, m => m.Quantity);
		}

		public QuantityNotNegative()
			: base(RuleClass.NotNegative)
		{
		}
	}
}
