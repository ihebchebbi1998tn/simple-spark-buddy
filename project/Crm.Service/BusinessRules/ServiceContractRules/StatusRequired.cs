namespace Crm.Service.BusinessRules.ServiceContractRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class StatusRequired : RequiredRule<ServiceContract>
	{
		public StatusRequired()
		{
			Init(x => x.StatusKey);
		}
	}
}