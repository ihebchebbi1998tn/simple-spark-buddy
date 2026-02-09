namespace Crm.Service.Model.Lookup
{
	using Crm.Library.Globalization.Lookup;

	[Lookup("[SMS].[ServicePriority]", "ServiceNotificationPriorityId")]
	public class ServicePriority : EntityLookup<string>, ILookupWithColor
	{
		[LookupProperty(Shared = true)]
		public virtual string Color { get; set; } = "#9E9E9E";
		[LookupProperty(Shared = true)]
		public virtual bool IsFastLane { get; set; }
		public const string HighKey = "2";
		public const string MiddleKey = "1";
		public const string LowKey = "0";

	}
}
