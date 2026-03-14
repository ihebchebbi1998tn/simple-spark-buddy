namespace Crm.Service.BusinessRules.ServiceContractRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class EndDateNotBeforeStartDate : OrderRule<ServiceContract>
	{
		// Constructor
		public EndDateNotBeforeStartDate()
		{
			Init(s => s.ValidTo, s => s.ValidFrom, ValueOrder.FirstValueGreaterOrEqual);
		}
	}
}