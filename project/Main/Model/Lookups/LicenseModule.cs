namespace Main.Model.Lookups
{
	using Crm.Library.Globalization.Lookup;

	[Lookup("[LU].[LicenseModule]", "LicenseModuleId")]
	public class LicenseModule : EntityLookup<string>
	{
		[LookupProperty(Shared = true)]
		public virtual string ParentModuleId { get; set; }
	}
}
