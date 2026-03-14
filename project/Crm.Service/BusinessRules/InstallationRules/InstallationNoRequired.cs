namespace Crm.Service.BusinessRules.InstallationRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class InstallationNoRequired : RequiredRule<Installation>
	{
		public InstallationNoRequired()
		{
			Init(i => i.InstallationNo);
		}
	}
}