namespace Crm.Service.BusinessRules.InstallationRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class InstallationTypeRequired : RequiredRule<Installation>
	{
		public InstallationTypeRequired()
		{
			Init(i => i.InstallationTypeKey, "InstallationType");
		}
	}
}