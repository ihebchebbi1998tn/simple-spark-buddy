namespace Crm.Service.BusinessRules.MaintenancePlans
{
	using Crm.Library.Validation;
	using Crm.Service.Model;

	public class RhythmValuePositive : Rule<MaintenancePlan>
	{
		protected override bool IsIgnoredFor(MaintenancePlan maintenancePlan)
		{
			return maintenancePlan.RhythmValue == null;
		}

		public override bool IsSatisfiedBy(MaintenancePlan maintenancePlan)
		{
			return maintenancePlan.RhythmValue.GetValueOrDefault() > 0;
		}

		protected override RuleViolation CreateRuleViolation(MaintenancePlan maintenancePlan)
		{
			return RuleViolation(maintenancePlan, m => m.RhythmValue, "Interval");
		}

		// Constructor
		public RhythmValuePositive()
			: base(RuleClass.Positive)
		{
		}
	}
}