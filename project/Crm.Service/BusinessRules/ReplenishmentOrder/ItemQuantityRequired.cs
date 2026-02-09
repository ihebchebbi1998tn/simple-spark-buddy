namespace Crm.Service.BusinessRules.ReplenishmentOrder
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class ItemQuantityRequired : RequiredRule<ReplenishmentOrderItem>
	{
		// Constructor
		public ItemQuantityRequired()
		{
			Init(i => i.Quantity);
		}
	}
}