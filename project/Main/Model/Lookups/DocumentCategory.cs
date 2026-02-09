namespace Main.Model.Lookups
{
	using Crm.Library.Globalization.Lookup;

	[Lookup("[LU].[DocumentCategory]")]
	public class DocumentCategory : EntityLookup<string>
	{
		[LookupProperty(Shared = true)]
		public virtual bool OfflineRelevant { get; set; }
	}
}
