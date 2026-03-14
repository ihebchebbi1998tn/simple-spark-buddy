namespace Crm.Service.Model.Helpers
{
	using Crm.Service.Model.Lookup;
	using Library.Globalization.Lookup;
	public class DefaultServiceOrderDispatchStatusEvaluator : IServiceOrderDispatchStatusEvaluator
	{
		private readonly ILookupManager lookupManger;
		public DefaultServiceOrderDispatchStatusEvaluator(ILookupManager lookupManger)
		{
			this.lookupManger = lookupManger;
		}
		public virtual CompareResult CompareStatus(string status1, string status2)
		{
			var status1Enum = lookupManger.Get<ServiceOrderDispatchStatus>(status1).SortOrder;
			var status2Enum = lookupManger.Get<ServiceOrderDispatchStatus>(status2).SortOrder;

			if (status1Enum > status2Enum)
			{
				return CompareResult.FirstHasAdvancedStatus;
			}
			if (status1Enum < status2Enum)
			{
				return CompareResult.SecondHasAdvancedStatus;
			}

			return CompareResult.BothHaveSameStatus;
		}
		public virtual bool IsStatusTransitionAllowed(string fromStatus, string toStatus)
		{
			var compareResult = CompareStatus(fromStatus, toStatus);
			if (compareResult == CompareResult.BothHaveSameStatus)
			{
				return true;
			}
			if (toStatus == ServiceOrderDispatchStatus.RejectedKey && !(fromStatus == ServiceOrderDispatchStatus.ReadKey || fromStatus == ServiceOrderDispatchStatus.ReleasedKey || fromStatus == ServiceOrderDispatchStatus.ScheduledKey))
			{
				return false;
			}
			if (fromStatus == ServiceOrderDispatchStatus.CancelledNotCompleteKey || fromStatus == ServiceOrderDispatchStatus.CancelledKey)
			{
				return true;
			}
			if (fromStatus == ServiceOrderDispatchStatus.SignedByCustomerKey && toStatus == ServiceOrderDispatchStatus.InProgressKey)
			{
				return true;
			}
			if (fromStatus == ServiceOrderDispatchStatus.ClosedNotCompleteKey && toStatus == ServiceOrderDispatchStatus.ClosedCompleteKey)
			{
				return false;
			}
			if (compareResult == CompareResult.SecondHasAdvancedStatus)
			{
				return true;
			}
			return false;
		}

		public enum CompareResult
		{
			FirstHasAdvancedStatus,
			SecondHasAdvancedStatus,
			BothHaveSameStatus
		}
	}
}
