namespace Crm.Service.BusinessRules.ServiceContractRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class ContractNoMaxLength : MaxLengthRule<ServiceContract>
	{
		// Constructor
		public ContractNoMaxLength()
		{
			Init(s => s.ContractNo, 20);
		}
	}
}