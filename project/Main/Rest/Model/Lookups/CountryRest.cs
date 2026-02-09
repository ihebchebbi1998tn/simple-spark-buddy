namespace Main.Rest.Model.Lookups
{
	using Crm.Library.Rest;

	using Main.Model.Lookups;

	[RestTypeFor(DomainType = typeof(Country))]
	public class CountryRest : RestEntityLookupWithExtensionValues
	{
		public string Iso2Code { get; set; }
		public string Iso3Code { get; set; }
		public string CallingCode { get; set; }
	}
}
