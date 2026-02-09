namespace Crm.Service.BusinessRules.ServiceOrderMaterialRules
{
	using Crm.Library.Validation;
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	[Rule]
	public class QuantityNumberRule : NumberRule<ServiceOrderMaterial>
	{
		public QuantityNumberRule()
		{
			Init(c => c.Quantity);
		}
	}
}
