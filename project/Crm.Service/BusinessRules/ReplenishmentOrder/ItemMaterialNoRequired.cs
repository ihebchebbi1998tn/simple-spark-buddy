namespace Crm.Service.BusinessRules.ReplenishmentOrder
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class ItemMaterialNoRequired : RequiredRule<ReplenishmentOrderItem>
	{
		// Constrcutor
		public ItemMaterialNoRequired()
		{
			Init(i => i.MaterialNo, "Material");
		}
	}
}