namespace Crm.Project.BusinessRules.ProjectRules
{
	using Crm.Library.Validation;
	using Crm.Service.Model;

	public class PriceNotNegative : Rule<ServiceContract>
	{
		public override bool IsSatisfiedBy(ServiceContract contract)
		{
			return contract.Price == null || contract.Price >= 0;
		}

		protected override RuleViolation CreateRuleViolation(ServiceContract contract)
		{
			return RuleViolation(contract, p => p.Price);
		}

		public PriceNotNegative()
			: base(RuleClass.NotNegative)
		{
		}
	}
}