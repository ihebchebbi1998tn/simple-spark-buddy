namespace Crm.Service.BusinessRules.ServiceOrderMaterialRules
{
	using Crm.Library.Validation;
	using Crm.Service.Model;

	[Rule]
	public class QuantityMaxValue : Rule<ServiceOrderMaterial>
	{
		public override bool IsSatisfiedBy(ServiceOrderMaterial material)
		{
			return material.Quantity < 10000000000000000;
		}

		protected override RuleViolation CreateRuleViolation(ServiceOrderMaterial material)
		{
			return RuleViolation(material, c => c.Quantity);
		}

		// Constructor
		public QuantityMaxValue()
			: base(RuleClass.MaxValue)
		{
		}
	}
}
