namespace Crm.Service.BusinessRules.ServiceContractRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class ContractNoRequired : RequiredRule<ServiceContract>
	{
		// Constructor
		public ContractNoRequired()
		{
			Init(c => c.ContractNo);
		}
	}
}