namespace Crm.Service.BusinessRules.ServiceOrderRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class ServiceLocationPhone : PhoneRule<ServiceOrderHead>
	{
		public ServiceLocationPhone()
		{
			Init(x => x.ServiceLocationPhone);
		}
	}
}
