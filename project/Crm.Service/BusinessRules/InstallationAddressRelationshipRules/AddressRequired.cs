namespace Crm.Service.BusinessRules.InstallationAddressRelationshipRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model.Relationships;

	public class AddressRequired : RequiredRule<InstallationAddressRelationship>
	{
		public AddressRequired()
		{
			Init(x => x.ChildId, propertyNameReplacementKey: "Address");
		}
	}
}