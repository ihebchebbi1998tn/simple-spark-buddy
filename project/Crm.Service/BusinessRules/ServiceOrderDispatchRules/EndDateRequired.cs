namespace Crm.Service.BusinessRules.ServiceOrderDispatchRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class EndDateRequired : RequiredRule<ServiceOrderDispatch>
	{
		// Constructor
		public EndDateRequired()
		{
			Init(d => d.EndDate);
		}
	}
}
