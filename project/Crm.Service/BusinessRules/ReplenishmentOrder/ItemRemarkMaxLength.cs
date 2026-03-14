namespace Crm.Service.BusinessRules.ReplenishmentOrder
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class ItemRemarkMaxLength : MaxLengthRule<ReplenishmentOrderItem>
	{
		// Constructor
		public ItemRemarkMaxLength()
		{
			Init(i => i.Remark, 500);
		}
	}
}