namespace Crm.Service.Model.Relationships
{
	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Model;
	using Crm.Service.Model.Lookup;

	public class ServiceContractAddressRelationship : LookupRelationship<ServiceContract, Address, ServiceContractAddressRelationshipType>
	{
	}
}
