namespace Crm.Service.Model.Relationships
{
	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Model;
	using Crm.Service.Model.Lookup;

	public class InstallationPersonRelationship : LookupRelationship<Installation, Person, InstallationPersonRelationshipType>
	{
	}
}
