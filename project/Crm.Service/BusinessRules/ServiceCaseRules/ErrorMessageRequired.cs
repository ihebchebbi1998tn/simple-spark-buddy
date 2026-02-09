namespace Crm.Service.BusinessRules.ServiceCaseRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class ErrorMessageRequired : RequiredRule<ServiceCase>
	{
		public ErrorMessageRequired()
		{
			Init(x => x.ErrorMessage);
		}
	}
}