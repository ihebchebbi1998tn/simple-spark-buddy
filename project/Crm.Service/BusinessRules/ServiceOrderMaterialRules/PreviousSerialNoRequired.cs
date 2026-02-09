namespace Crm.Service.BusinessRules.ServiceOrderMaterialRules
{
	using Crm.Library.Validation;
	using Crm.Service.Model;

	public class PreviousSerialNoRequired : Rule<ServiceOrderMaterial>
	{
		public override bool IsSatisfiedBy(ServiceOrderMaterial entity)
		{
			return string.IsNullOrEmpty(entity.PreviousSerialNo) == false || string.IsNullOrEmpty(entity.NoPreviousSerialNoReasonKey) == false;
		}
		protected override bool IsIgnoredFor(ServiceOrderMaterial entity)
		{
			return entity.IsSerial == false || entity.Quantity == 0 || entity.ServiceOrderMaterialType != "Used";
		}
		protected override RuleViolation CreateRuleViolation(ServiceOrderMaterial entity)
		{
			return RuleViolation(entity, x => x.PreviousSerialNo, propertyNameReplacementKey: "PreviousSerialNoRequired", errorMessageKey: "PreviousSerialNoRequired");
		}

		public PreviousSerialNoRequired()
			: base(RuleClass.Required)
		{
		}
	}
}
