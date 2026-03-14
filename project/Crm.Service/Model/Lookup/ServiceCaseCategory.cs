namespace Crm.Service.Model.Lookup
{
	using Crm.Library.Globalization.Lookup;

	[Lookup("[SMS].[ServiceNotificationCategory]", "ServiceNotificationCategoryId")]
	public class ServiceCaseCategory : EntityLookup<string>, ILookupWithColor
	{
		[LookupProperty(Shared = true)]
		public virtual string Color { get; set; } = "#9E9E9E";
	}
}