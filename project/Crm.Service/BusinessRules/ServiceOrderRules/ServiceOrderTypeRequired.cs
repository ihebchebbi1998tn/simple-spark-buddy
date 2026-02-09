namespace Crm.Service.BusinessRules.ServiceOrderRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class ServiceOrderTypeRequired : RequiredRule<ServiceOrderHead>
	{
		public ServiceOrderTypeRequired()
		{
			Init(x => x.TypeKey, propertyNameReplacementKey: "ServiceOrderType");
		}
	}
}
