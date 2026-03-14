namespace Crm.Service.Model.Lookup
{
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.BaseModel.Attributes;
	using Crm.Library.Extensions;
	using Crm.Library.Globalization.Lookup;

	[Lookup("[SMS].[ServiceContractStatus]")]
	public class ServiceContractStatus : EntityLookup<string>, ILookupWithSettableStatuses
	{
		public const string InactiveKey = "Inactive";
		public const string ActiveKey = "Active";
		public const string ExpiredKey = "Expired";
		public const string PendingKey = "Pending";

		[LookupProperty(Shared = true)]
		[UI(Hidden = true)]
		public virtual string SettableStatuses { get; set; }
	}

	public static class ServiceContractStatusExtensions
	{
		public static List<string> GetSettableStatusKeys(this ServiceContractStatus status)
		{
			return status.SettableStatuses.IsNullOrWhiteSpace()
				? new List<string>()
				: status.SettableStatuses.Split(',').ToList();
		}

		public static bool IsPendingStatus(this ServiceContractStatus status)
		{
			return status != null && status.Key == ServiceContractStatus.PendingKey;
		}
		public static bool IsInactiveStatus(this ServiceContractStatus status)
		{
			return status != null && status.Key == ServiceContractStatus.InactiveKey;
		}
		public static bool IsActiveStatus(this ServiceContractStatus status)
		{
			return status != null && status.Key == ServiceContractStatus.ActiveKey;
		}
		public static bool IsExpiredStatus(this ServiceContractStatus status)
		{
			return status != null && status.Key == ServiceContractStatus.ExpiredKey;
		}
	}
}
