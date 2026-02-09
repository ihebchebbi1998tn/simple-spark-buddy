namespace Crm.Service.BusinessRules.ServiceOrderRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class ServiceLocationFax : FaxRule<ServiceOrderHead>
	{
		public ServiceLocationFax()
		{
			Init(x => x.ServiceLocationFax);
		}
	}
}
