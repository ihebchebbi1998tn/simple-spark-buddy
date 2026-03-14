namespace Crm.Service.Model.Lookup
{
	using Crm.Library.Globalization.Lookup;

	[Lookup("[SMS].[InstallationHeadStatus]")]
	public class InstallationHeadStatus : EntityLookup<string>, ILookupWithColor
	{
		[LookupProperty(Shared = true)]
		public virtual string Color { get; set; } = "#9E9E9E";
	}
}
