namespace Crm.Service.Model.Lookup
{
	using Crm.Library.Globalization.Lookup;

	[Lookup("[SMS].[ServiceOrderTimeStatus]")]
	[NotEditable]
	public class ServiceOrderTimeStatus : EntityLookup<string>, ILookupWithColor
	{
		[LookupProperty(Shared = true)]
		public virtual string Color { get; set; }
		
		public static readonly string CreatedKey = "Created";
		public static readonly string StartedKey = "Started";
		public static readonly string InterruptedKey = "Interrupted";
		public static readonly string FinishedKey = "Finished";
	}
}
