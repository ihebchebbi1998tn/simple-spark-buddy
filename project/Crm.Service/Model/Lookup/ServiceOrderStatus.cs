namespace Crm.Service.Model.Lookup
{
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.Extensions;
	using Crm.Library.Globalization.Lookup;

	[Lookup("[SMS].[ServiceOrderStatus]")]
	[NotEditable]
	public class ServiceOrderStatus : EntityLookup<string>, ILookupWithGroups, ILookupWithSettableStatuses
	{
		public const string NewKey = "New";
		public const string ReadyForSchedulingKey = "ReadyForScheduling";
		public const string ScheduledKey = "Scheduled";
		public const string PartiallyReleasedKey = "PartiallyReleased";
		public const string ReleasedKey = "Released";
		public const string InProgressKey = "InProgress";
		public const string PartiallyCompletedKey = "PartiallyCompleted";
		public const string CompletedKey = "Completed";
		public const string PostProcessingKey = "PostProcessing";
		public const string ReadyForInvoiceKey = "ReadyForInvoice";
		public const string InvoicedKey = "Invoiced";
		public const string ClosedKey = "Closed";

		public const string PreparationGroupKey = "Preparation";
		public const string SchedulingGroupKey = "Scheduling";
		public const string InProgressGroupKey = "InProgress";
		public const string PostProcessingGroupKey = "PostProcessing";
		public const string ClosedGroupKey = "Closed";

		[LookupProperty(Shared = true)]
		public virtual string Groups { get; set; }

		[LookupProperty(Shared = true)]
		public virtual string SettableStatuses { get; set; }
	}

	// Default extension methods
	public static class ServiceOrderStatusExtensions
	{
		public static List<string> GetSettableStatusKeys(this ServiceOrderStatus status)
		{
			return status.SettableStatuses.IsNullOrWhiteSpace()
				? new List<string>()
				: status.SettableStatuses.Split(',').ToList();
		}

		#region SingleStatus

		public static bool IsNew(this ServiceOrderStatus status)
		{
			return status != null && status.Key == ServiceOrderStatus.NewKey;
		}

		public static bool IsReadyForScheduling(this ServiceOrderStatus status)
		{
			return status != null && status.Key == ServiceOrderStatus.ReadyForSchedulingKey;
		}

		public static bool IsScheduled(this ServiceOrderStatus status)
		{
			return status != null && status.Key == ServiceOrderStatus.ScheduledKey;
		}

		public static bool IsPartiallyReleased(this ServiceOrderStatus status)
		{
			return status != null && status.Key == ServiceOrderStatus.PartiallyReleasedKey;
		}

		public static bool IsReleased(this ServiceOrderStatus status)
		{
			return status != null && status.Key == ServiceOrderStatus.ReleasedKey;
		}

		public static bool IsInProgress(this ServiceOrderStatus status)
		{
			return status != null && status.Key == ServiceOrderStatus.InProgressKey;
		}

		public static bool IsPartiallyCompleted(this ServiceOrderStatus status)
		{
			return status != null && status.Key == ServiceOrderStatus.PartiallyCompletedKey;
		}

		public static bool IsCompleted(this ServiceOrderStatus status)
		{
			return status != null && status.Key == ServiceOrderStatus.CompletedKey;
		}

		public static bool IsPostProcessing(this ServiceOrderStatus status)
		{
			return status != null && status.Key == ServiceOrderStatus.PostProcessingKey;
		}

		public static bool IsReadyForInvoice(this ServiceOrderStatus status)
		{
			return status != null && status.Key == ServiceOrderStatus.ReadyForInvoiceKey;
		}

		public static bool IsInvoiced(this ServiceOrderStatus status)
		{
			return status != null && status.Key == ServiceOrderStatus.InvoicedKey;
		}

		public static bool IsClosed(this ServiceOrderStatus status)
		{
			return status != null && status.Key == ServiceOrderStatus.ClosedKey;
		}
		#endregion SingleStatus

		#region StatusGroup

		public static bool BelongsToPreparation(this ServiceOrderStatus status)
		{
			return status.Groups.Split(',').Any(g => g == ServiceOrderStatus.PreparationGroupKey);
		}

		public static bool NotBelongsToPreparation(this ServiceOrderStatus status)
		{
			return !status.BelongsToPreparation();
		}

		public static bool BelongsToScheduling(this ServiceOrderStatus status)
		{
			return status.Groups.Split(',').Any(g => g == ServiceOrderStatus.SchedulingGroupKey);
		}

		public static bool BelongsToInProgress(this ServiceOrderStatus status)
		{
			return status.Groups.Split(',').Any(g => g == ServiceOrderStatus.InProgressGroupKey);
		}

		public static bool BelongsToPostProcessing(this ServiceOrderStatus status)
		{
			return status.Groups.Split(',').Any(g => g == ServiceOrderStatus.PostProcessingGroupKey);
		}

		public static bool BelongsToClosed(this ServiceOrderStatus status)
		{
			return status.Groups.Split(',').Any(g => g == ServiceOrderStatus.ClosedGroupKey);
		}

		#endregion StatusGroup
	}

	public static class ServiceOrderHeadExtensions
	{
		public static bool IsNew(this ServiceOrderHead orderHead)
		{
			return orderHead != null && orderHead.StatusKey == ServiceOrderStatus.NewKey;
		}

		public static bool IsReadyForScheduling(this ServiceOrderHead orderHead)
		{
			return orderHead != null && orderHead.StatusKey == ServiceOrderStatus.ReadyForSchedulingKey;
		}

		public static bool IsScheduled(this ServiceOrderHead orderHead)
		{
			return orderHead != null && orderHead.StatusKey == ServiceOrderStatus.ScheduledKey;
		}

		public static bool IsPartiallyReleased(this ServiceOrderHead orderHead)
		{
			return orderHead != null && orderHead.StatusKey == ServiceOrderStatus.PartiallyReleasedKey;
		}

		public static bool IsReleased(this ServiceOrderHead orderHead)
		{
			return orderHead != null && orderHead.StatusKey == ServiceOrderStatus.ReleasedKey;
		}

		public static bool IsInProgress(this ServiceOrderHead orderHead)
		{
			return orderHead != null && orderHead.StatusKey == ServiceOrderStatus.InProgressKey;
		}

		public static bool IsPartiallyCompleted(this ServiceOrderHead orderHead)
		{
			return orderHead != null && orderHead.StatusKey == ServiceOrderStatus.PartiallyCompletedKey;
		}

		public static bool IsCompleted(this ServiceOrderHead orderHead)
		{
			return orderHead != null && orderHead.StatusKey == ServiceOrderStatus.CompletedKey;
		}

		public static bool IsPostProcessing(this ServiceOrderHead orderHead)
		{
			return orderHead != null && orderHead.StatusKey == ServiceOrderStatus.PostProcessingKey;
		}

		public static bool IsReadyForInvoice(this ServiceOrderHead orderHead)
		{
			return orderHead != null && orderHead.StatusKey == ServiceOrderStatus.ReadyForInvoiceKey;
		}

		public static bool IsInvoiced(this ServiceOrderHead orderHead)
		{
			return orderHead != null && orderHead.StatusKey == ServiceOrderStatus.InvoicedKey;
		}

		public static bool IsClosed(this ServiceOrderHead orderHead)
		{
			return orderHead != null && orderHead.StatusKey == ServiceOrderStatus.ClosedKey;
		}
	}
}
