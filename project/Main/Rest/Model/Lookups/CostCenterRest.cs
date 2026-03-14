namespace Main.Rest.Model.Lookups
{
	using Crm.Library.Rest;

	using Main.Model.Lookups;

	[RestTypeFor(DomainType = typeof(CostCenter))]
	public class CostCenterRest : RestEntityLookupWithExtensionValues
	{
	}
}
