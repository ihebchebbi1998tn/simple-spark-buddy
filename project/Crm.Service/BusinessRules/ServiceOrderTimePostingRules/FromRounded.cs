namespace Crm.Service.BusinessRules.ServiceOrderTimePostingRules
{
	using Crm.Library.Extensions;
	using Crm.Library.Helper;
	using Crm.Library.Validation;
	using Crm.Service.Model;

	public class FromRounded : Rule<ServiceOrderTimePosting>
	{
		private readonly int minutesInterval;
		protected override bool IsIgnoredFor(ServiceOrderTimePosting timePosting)
		{
			return timePosting.FromAsString.IsNull();
		}

		public override bool IsSatisfiedBy(ServiceOrderTimePosting timePosting)
		{
			return timePosting.From.HasValue && timePosting.From.Value.Minute % minutesInterval == 0;
		}

		protected override RuleViolation CreateRuleViolation(ServiceOrderTimePosting timePosting)
		{
			var resourceParams = new[] { "00", minutesInterval.ToString().PadLeft(2, '0'), (minutesInterval * 2).ToString().PadLeft(2, '0') };
			return RuleViolation(timePosting, t => t.FromAsString, "From", "RoundedMinutesRuleText", resourceParams);
		}

		public FromRounded(IAppSettingsProvider appSettingsProvider)
			: base(RuleClass.Format)
		{
			minutesInterval = (int)appSettingsProvider.GetValue(ServicePlugin.Settings.ServiceOrderTimePosting.MinutesInterval);
		}
	}
}