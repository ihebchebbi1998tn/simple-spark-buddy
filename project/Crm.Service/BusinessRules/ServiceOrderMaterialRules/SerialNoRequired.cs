namespace Crm.Service.BusinessRules.ServiceOrderMaterialRules
{
	using Crm.Library.Extensions;
	using Crm.Library.Validation;
	using Crm.Service.Model;

	public class SerialNoRequired : Rule<ServiceOrderMaterial>
	{
		public override bool IsSatisfiedBy(ServiceOrderMaterial entity) => !string.IsNullOrEmpty(entity.SerialNo) || entity.SerialId.IsNotNullOrDefault();
		protected override bool IsIgnoredFor(ServiceOrderMaterial entity) => !entity.IsSerial || entity.ServiceOrderMaterialType != "Used" || entity.Quantity == 0;
		protected override RuleViolation CreateRuleViolation(ServiceOrderMaterial entity) => RuleViolation(entity, x => x.SerialNo, propertyNameReplacementKey: "SerialNoRequired", errorMessageKey: "SerialNoRequired");

		public SerialNoRequired()
			: base(RuleClass.Required)
		{
		}
	}
}
