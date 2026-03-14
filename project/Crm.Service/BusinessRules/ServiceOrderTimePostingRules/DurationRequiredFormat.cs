namespace Crm.Service.BusinessRules.ServiceOrderTimePostingRules
{
	using Crm.PerDiem.BusinessRules.TimeEntryRules;
	using Crm.Service.Extensions;
	using Crm.Service.Model;

	public class DurationRequiredFormat : BaseDurationRequiredFormat<ServiceOrderTimePosting>
	{
		protected override bool IsIgnoredFor(ServiceOrderTimePosting timeEntry)
		{
			if (timeEntry.IsPrePlanned())
			{
				return true;
			}
			return base.IsIgnoredFor(timeEntry);
		}
	}
}
