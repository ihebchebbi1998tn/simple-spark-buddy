namespace Crm.Service.BusinessRules.InstallationRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class InstallationNoMaxLength : MaxLengthRule<Installation>
	{
		public InstallationNoMaxLength()
		{
			Init(i => i.InstallationNo, 30);
		}
	}
}