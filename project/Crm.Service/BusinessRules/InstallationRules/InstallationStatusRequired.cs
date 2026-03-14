namespace Crm.Service.BusinessRules.InstallationRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class InstallationStatusRequired : RequiredRule<Installation>
	{
		public InstallationStatusRequired()
		{
			Init(i => i.StatusKey, "Status");
		}
	}
}