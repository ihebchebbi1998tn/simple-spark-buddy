namespace Crm.Service.Model.Extensions
{
	using System.Linq;

	using Crm.Service.Model.Lookup;

	public static class ServiceCaseStatusExtensions
	{
		public static bool BelongsToClosed(this ServiceCaseStatus status)
		{
			return status.Groups.Split(',').Any(g => g == ServiceCaseStatus.ClosedGroupKey);
		}

		public static bool BelongsToInProgress(this ServiceCaseStatus status)
		{
			return status.Groups.Split(',').Any(g => g == ServiceCaseStatus.InProgressGroupKey);
		}
	}
}