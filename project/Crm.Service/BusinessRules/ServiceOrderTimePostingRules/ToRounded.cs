namespace Crm.Service.BusinessRules.ServiceOrderTimePostingRules
{
	using Crm.Library.Extensions;
	using Crm.Library.Helper;
	using Crm.Library.Validation;
	using Crm.Service.Model;

	public class ToRounded : Rule<ServiceOrderTimePosting>
	{
		private readonly int minutesInterval;
		protected override bool IsIgnoredFor(ServiceOrderTimePosting timePosting)
		{
			return timePosting.ToAsString.IsNull();
		}

		public override bool IsSatisfiedBy(ServiceOrderTimePosting timePosting)
		{
			return timePosting.To.HasValue && timePosting.To.Value.Minute % minutesInterval == 0;
		}

		protected override RuleViolation CreateRuleViolation(ServiceOrderTimePosting timePosting)
		{
			var resourceParams = new[] { "00", minutesInterval.ToString().PadLeft(2, '0'), (minutesInterval * 2).ToString().PadLeft(2, '0') };
			return RuleViolation(timePosting, t => t.ToAsString, "To", "RoundedMinutesRuleText", resourceParams);
		}

		public ToRounded(IAppSettingsProvider appSettingsProvider)
			: base(RuleClass.Format)
		{
			minutesInterval =  (int)appSettingsProvider.GetValue(ServicePlugin.Settings.ServiceOrderTimePosting.MinutesInterval);
		}
	}
}