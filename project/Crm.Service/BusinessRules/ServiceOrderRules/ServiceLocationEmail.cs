namespace Crm.Service.BusinessRules.ServiceOrderRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class ServiceLocationEmail : EmailRule<ServiceOrderHead>
	{
		public ServiceLocationEmail()
		{
			Init(x => x.ServiceLocationEmail);
		}
	}
}
