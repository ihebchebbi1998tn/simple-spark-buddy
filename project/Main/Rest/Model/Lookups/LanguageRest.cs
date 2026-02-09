namespace Main.Rest.Model.Lookups
{
	using Crm.Library.Rest;

	using Main.Model.Lookups;

	[RestTypeFor(DomainType = typeof(Language))]
	public class LanguageRest : RestEntityLookupWithExtensionValues
	{
		public string DefaultLocale { get; set; }
		public bool IsSystemLanguage { get; set; }
	}
}
