namespace Crm.Service.Model.Lookup
{
    using Crm.Library.Globalization.Lookup;

	[Lookup("[SMS].[ServiceOrderDispatchStatus]", "ServiceOrderDispatchTechnicianStatusId")]
	[NotEditable]
	public class ServiceOrderDispatchStatus : EntityLookup<string>, ILookupWithColor
	{
		[LookupProperty(Shared = true)]
		public virtual string Color { get; set; }
		public const string ScheduledKey = "Scheduled";
		public const string ReleasedKey = "Released";
		public const string ReadKey = "Read";
		public const string InProgressKey = "InProgress";
		public const string SignedByCustomerKey = "SignedByCustomer";
		public const string ClosedNotCompleteKey = "ClosedNotComplete";
		public const string ClosedCompleteKey = "ClosedComplete";
		public const string RejectedKey = "Rejected";
		public const string CancelledKey = "Cancelled";
		public const string CancelledNotCompleteKey = "CancelledNotComplete";
    }

    // Extension methods
    public static class ServiceOrderDispatchExtensions
    {
        public static bool IsScheduled(this ServiceOrderDispatch dispatch)
        {
            return dispatch != null && dispatch.StatusKey == ServiceOrderDispatchStatus.ScheduledKey;
        }
        public static bool IsReleased(this ServiceOrderDispatch dispatch)
        {
            return dispatch != null && dispatch.StatusKey == ServiceOrderDispatchStatus.ReleasedKey;
        }
        public static bool IsRead(this ServiceOrderDispatch dispatch)
        {
            return dispatch != null && dispatch.StatusKey == ServiceOrderDispatchStatus.ReadKey;
        }
        public static bool IsInProgress(this ServiceOrderDispatch dispatch)
        {
            return dispatch != null && dispatch.StatusKey == ServiceOrderDispatchStatus.InProgressKey;
        }
        public static bool IsSignedByCustomer(this ServiceOrderDispatch dispatch)
        {
            return dispatch != null && dispatch.StatusKey == ServiceOrderDispatchStatus.SignedByCustomerKey;
        }
        public static bool IsClosedNotComplete(this ServiceOrderDispatch dispatch)
        {
            return dispatch != null && (dispatch.StatusKey == ServiceOrderDispatchStatus.ClosedNotCompleteKey || dispatch.StatusKey == ServiceOrderDispatchStatus.CancelledNotCompleteKey);
        }
        public static bool IsClosedComplete(this ServiceOrderDispatch dispatch)
        {
            return dispatch != null && (dispatch.StatusKey == ServiceOrderDispatchStatus.ClosedCompleteKey || dispatch.StatusKey == ServiceOrderDispatchStatus.CancelledKey);
        }
        public static bool IsRejected(this ServiceOrderDispatch dispatch)
        {
            return dispatch != null && dispatch.StatusKey == ServiceOrderDispatchStatus.RejectedKey;
        }
    }

    public static class ServiceOrderDispatchStatusExtensions
	{
        public static bool IsScheduled(this ServiceOrderDispatchStatus status)
		{
			return status != null && status.Key == ServiceOrderDispatchStatus.ScheduledKey;
		}

		public static bool IsReleased(this ServiceOrderDispatchStatus status)
		{
			return status != null && status.Key == ServiceOrderDispatchStatus.ReleasedKey;
		}

		public static bool IsRead(this ServiceOrderDispatchStatus status)
		{
			return status != null && status.Key == ServiceOrderDispatchStatus.ReadKey;
		}

		public static bool IsInProgress(this ServiceOrderDispatchStatus status)
		{
			return status != null && status.Key == ServiceOrderDispatchStatus.InProgressKey;
		}

		public static bool IsSignedByCustomer(this ServiceOrderDispatchStatus status)
		{
			return status != null && status.Key == ServiceOrderDispatchStatus.SignedByCustomerKey;
		}

		public static bool IsClosedNotComplete(this ServiceOrderDispatchStatus status)
		{
			return status != null && status.Key == ServiceOrderDispatchStatus.ClosedNotCompleteKey;
		}

		public static bool IsClosedComplete(this ServiceOrderDispatchStatus status)
		{
			return status != null && status.Key == ServiceOrderDispatchStatus.ClosedCompleteKey;
		}

		public static bool IsRejected(this ServiceOrderDispatchStatus status)
		{
			return status != null && status.Key == ServiceOrderDispatchStatus.RejectedKey;
		}
    }
}
