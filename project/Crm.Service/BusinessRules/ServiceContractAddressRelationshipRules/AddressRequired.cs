namespace Crm.Service.BusinessRules.ServiceContractAddressRelationshipRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model.Relationships;

	public class AddressRequired : RequiredRule<ServiceContractAddressRelationship>
	{
		public AddressRequired()
		{
			Init(x => x.ChildId, propertyNameReplacementKey: "Address");
		}
	}
}