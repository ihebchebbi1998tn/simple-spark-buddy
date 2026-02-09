namespace Main.Rest.Model.Lookups
{
	using Crm.Library.Rest;

	using Main.Model.Lookups;

	[RestTypeFor(DomainType = typeof(PaymentType))]
	public class PaymentTypeRest : RestEntityLookupWithExtensionValues
	{
	}
}
