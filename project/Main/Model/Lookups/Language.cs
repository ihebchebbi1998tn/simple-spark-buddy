namespace Main.Model.Lookups
{
	using Crm.Library.Globalization.Lookup;

	[Lookup("[LU].[Language]", "LangId")]
	public class Language : EntityLookup<string>
	{
		[LookupProperty(Shared = true)]
		public virtual string DefaultLocale { get; set; }
		[LookupProperty(Shared = true)]
		public virtual bool IsSystemLanguage { get; set; }

		// Members
		public static readonly Language None = new Language { Key = string.Empty, Value = "None", Language = "en" };
		public static readonly Language German = new Language { Key = "de", Value = "German", Language = "en" };
		public static readonly Language English = new Language { Key = "en", Value = "English", Language = "en" };
		public static readonly Language French = new Language { Key = "fr", Value = "Francais", Language = "en" };
		public static readonly Language Hungarian = new Language { Key = "hu", Value = "Hungarian", Language = "en" };
		public static readonly Language Spanish = new Language { Key = "es", Value = "Spanish", Language = "en" };
	}
}
