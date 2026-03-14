namespace Main.Rest.Model.Lookups
{
	using Crm.Library.Rest;

	using Main.Model.Lookups;

	[RestTypeFor(DomainType = typeof(PaymentInterval))]
	public class PaymentIntervalRest : RestEntityLookupWithExtensionValues
	{
	}
}
