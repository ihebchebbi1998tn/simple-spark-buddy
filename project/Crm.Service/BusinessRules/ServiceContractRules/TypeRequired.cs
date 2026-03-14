namespace Crm.Service.BusinessRules.ServiceContractRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class TypeRequired : RequiredRule<ServiceContract>
	{
		public TypeRequired()
		{
			Init(x => x.ContractTypeKey);
		}
	}
}