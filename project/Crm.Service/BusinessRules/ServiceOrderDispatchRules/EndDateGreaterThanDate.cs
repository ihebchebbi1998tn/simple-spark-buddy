namespace Crm.Service.BusinessRules.ServiceOrderDispatchRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class EndDateGreaterThanDate : OrderRule<ServiceOrderDispatch>
	{
		public EndDateGreaterThanDate()
		{
			Init(t => t.EndDate, t => t.Date, ValueOrder.FirstValueGreater);
		}
	}
}
