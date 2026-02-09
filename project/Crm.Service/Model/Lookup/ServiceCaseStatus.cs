namespace Crm.Service.Model.Lookup
{
	using Crm.Library.BaseModel.Attributes;
	using Crm.Library.Globalization.Lookup;

	[Lookup("[SMS].[ServiceNotificationStatus]", "ServiceNotificationStatusId")]
	public class ServiceCaseStatus : EntityLookup<int>, ILookupWithColor, ILookupWithGroups, ILookupWithSettableStatuses
	{
		public const string ClosedGroupKey = "Closed";
		public const string InProgressGroupKey = "InProgress";

		[LookupProperty(Shared = true)]
		public virtual string Color { get; set; } = "#9E9E9E";

		[LookupProperty(Shared = true)]
		[UI(Hidden = true)]
		public virtual string Groups { get; set; }

		[LookupProperty(Shared = true)]
		[UI(Hidden = true)]
		public virtual string SettableStatuses { get; set; }
	}
}
