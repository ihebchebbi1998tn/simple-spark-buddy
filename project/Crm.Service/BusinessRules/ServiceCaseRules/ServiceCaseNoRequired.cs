namespace Crm.Service.BusinessRules.ServiceCaseRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class ServiceCaseNoRequired : RequiredRule<ServiceCase>
	{
		public ServiceCaseNoRequired()
		{
			Init(x => x.ServiceCaseNo);
		}
	}
}