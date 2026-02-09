namespace Crm.Service.BusinessRules.ServiceOrderMaterialRules
{
	using Crm.Library.Validation;
	using Crm.Service.Model;

	[Rule]
	public class SerialQuantity : Rule<ServiceOrderMaterial>
	{
		public override bool IsSatisfiedBy(ServiceOrderMaterial material)
		{
			return material.Quantity == 0 || material.Quantity == 1;
		}

		protected override bool IsIgnoredFor(ServiceOrderMaterial entity) => !entity.IsSerial || entity.ServiceOrderMaterialType != "Used";

		protected override RuleViolation CreateRuleViolation(ServiceOrderMaterial material)
		{
			return RuleViolation(material, c => c.Quantity);
		}

		// Constructor
		public SerialQuantity()
			: base(RuleClass.MaxValue)
		{
		}
	}
}
