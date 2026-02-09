namespace Crm.Service.BusinessRules.ServiceContractRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class ValidFromRequired : RequiredRule<ServiceContract>
	{
		// Constructor
		public ValidFromRequired()
		{
			Init(s => s.ValidFrom);
		}
	}
}