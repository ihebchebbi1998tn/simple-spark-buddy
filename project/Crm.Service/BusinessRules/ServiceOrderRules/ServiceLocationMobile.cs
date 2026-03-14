namespace Crm.Service.BusinessRules.ServiceOrderRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class ServiceLocationMobile : FaxRule<ServiceOrderHead>
	{
		public ServiceLocationMobile()
		{
			Init(x => x.ServiceLocationMobile);
		}
	}
}
