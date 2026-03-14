namespace Crm.Service.BusinessRules.ServiceContractInstallationRelationshipRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model.Relationships;

	public class InstallationRequired : RequiredRule<ServiceContractInstallationRelationship>
	{
		public InstallationRequired()
		{
			Init(x => x.ChildId, propertyNameReplacementKey: "Installation");
		}
	}
}