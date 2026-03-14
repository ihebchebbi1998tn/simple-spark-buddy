namespace Main.Rest.Model.Lookups
{
	using Crm.Library.Rest;

	using Main.Model.Lookups;

	[RestTypeFor(DomainType = typeof(TimeUnit))]
	public class TimeUnitRest : RestEntityLookupWithExtensionValues
	{
		public int? TimeUnitsPerYear { get; set; }
	}
}
