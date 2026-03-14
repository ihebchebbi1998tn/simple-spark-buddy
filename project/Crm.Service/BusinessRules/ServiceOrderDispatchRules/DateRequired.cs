namespace Crm.Service.BusinessRules.ServiceOrderDispatchRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class DateRequired : RequiredRule<ServiceOrderDispatch>
	{
		// Constructor
		public DateRequired()
		{
			Init(d => d.Date);
		}
	}
}