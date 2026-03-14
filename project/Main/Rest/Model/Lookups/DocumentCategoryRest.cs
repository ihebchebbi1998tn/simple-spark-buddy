namespace Main.Rest.Model.Lookups
{
	using Crm.Library.Rest;

	using Main.Model.Lookups;

	[RestTypeFor(DomainType = typeof(DocumentCategory))]
	public class DocumentCategoryRest : RestEntityLookupWithExtensionValues
	{
		public bool OfflineRelevant { get; set; }
	}
}
