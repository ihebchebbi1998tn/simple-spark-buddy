namespace Crm.Service.BusinessRules.ServiceOrderTimePostingRules
{
	using Crm.Library.Extensions;
	using Crm.Library.Helper;
	using Crm.Library.Validation;
	using Crm.Service.Model;

	public class DurationRounded : Rule<ServiceOrderTimePosting>
	{
		private readonly int minutesInterval;
		protected override bool IsIgnoredFor(ServiceOrderTimePosting timePosting)
		{
			return timePosting.DurationInMinutes == null || timePosting.FromAsString.IsNotNullOrEmpty() && timePosting.ToAsString.IsNotNullOrEmpty();
		}

		public override bool IsSatisfiedBy(ServiceOrderTimePosting timePosting)
		{
			return timePosting.DurationInMinutes % minutesInterval == 0;
		}

		protected override RuleViolation CreateRuleViolation(ServiceOrderTimePosting timePosting)
		{
			var resourceParams = new[] { "00", minutesInterval.ToString().PadLeft(2, '0'), (minutesInterval * 2).ToString().PadLeft(2, '0') };
			return RuleViolation(timePosting, t => t.DurationAsString, "Duration", "RoundedMinutesRuleText".WithArgs(resourceParams));
		}

		public DurationRounded(IAppSettingsProvider appSettingsProvider)
			: base(RuleClass.Format)
		{
			minutesInterval = (int)appSettingsProvider.GetValue(ServicePlugin.Settings.ServiceOrderTimePosting.MinutesInterval);
		}
	}
}