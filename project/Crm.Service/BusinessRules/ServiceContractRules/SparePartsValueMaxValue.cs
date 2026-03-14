namespace Crm.Service.BusinessRules.ServiceContractRules
{
	using Crm.Library.Validation;
	using Crm.Service.Model;

	public class SparePartsValueMaxValue : Rule<ServiceContract>
	{
		// MaxValue defined by database type decimal(10,2).
		private decimal MaxValue = 99999999.99m;

		protected override bool IsIgnoredFor(ServiceContract contract)
		{
			return !contract.SparePartsValue.HasValue;
		}

		public override bool IsSatisfiedBy(ServiceContract contract)
		{
			return contract.SparePartsValue.Value <= MaxValue;
		}

		protected override RuleViolation CreateRuleViolation(ServiceContract contract)
		{
			return RuleViolation(contract, c => c.SparePartsValue);
		}

		// Constructor
		public SparePartsValueMaxValue()
			: base(RuleClass.MaxValue)
		{
		}
	}
}