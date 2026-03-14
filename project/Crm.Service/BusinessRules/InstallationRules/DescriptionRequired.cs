namespace Crm.Service.BusinessRules.InstallationRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class DescriptionRequired : RequiredRule<Installation>
	{
		public DescriptionRequired()
		{
			Init(i => i.Description);
		}
	}
}