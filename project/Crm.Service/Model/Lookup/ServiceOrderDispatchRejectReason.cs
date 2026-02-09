namespace Crm.Service.Model.Lookup
{
	using Crm.Library.BaseModel.Attributes;
	using Crm.Library.Globalization.Lookup;

	[Lookup("[SMS].[ServiceOrderDispatchRejectReason]")]
	public class ServiceOrderDispatchRejectReason : EntityLookup<string>
	{
		public const string FalseAlarm = "FalseAlarm";
		public const string ConflictingDates = "ConflictingDates";
		public const string CustomerNotAccessible = "CustomerNotAccessible";
		public const string InstallationNotAccessible = "InstallationNotAccessible";
		public const string RejectedBySystem = "RejectedBySystem";

		[LookupProperty(Shared = true)]
		public virtual bool ShowInMobileClient { get; set; }
		[LookupProperty(Shared = true, Column = "ServiceOrderStatus")]
		[UI(Hidden = true)]
		public virtual string ServiceOrderStatusKey { get; set; }
	}
}
