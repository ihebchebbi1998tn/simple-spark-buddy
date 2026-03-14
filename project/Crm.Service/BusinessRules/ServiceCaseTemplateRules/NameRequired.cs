namespace Crm.Service.BusinessRules.ServiceCaseTemplateRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class NameRequired : RequiredRule<ServiceCaseTemplate>
	{
		public NameRequired()
		{
			Init(x => x.Name);
		}
	}
}