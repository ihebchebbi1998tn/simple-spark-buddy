namespace Crm.Service.BusinessRules.ServiceOrderRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class CustomerContactIdRequired : RequiredRule<ServiceOrderHead>
	{
		public CustomerContactIdRequired()
		{
			Init(x => x.CustomerContactId, propertyNameReplacementKey: "CustomerContact");
		}

		protected override bool IsIgnoredFor(ServiceOrderHead entity)
		{
			return entity.IsTemplate;
		}
	}
}
