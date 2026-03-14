namespace Crm.Service.BusinessRules.ServiceCaseRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class ErrorMessageMaxLength : MaxLengthRule<ServiceCase>
	{
		public ErrorMessageMaxLength()
		{
			Init(x => x.ErrorMessage, 500);
		}
	}
}