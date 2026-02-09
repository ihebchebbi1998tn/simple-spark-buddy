namespace Crm.Service.BusinessRules.ServiceContractRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class WarrantyFromNotBeforeWarrantyFrom : OrderRule<Installation>
	{
		public WarrantyFromNotBeforeWarrantyFrom()
		{
			Init(c => c.WarrantyUntil, c => c.WarrantyFrom, ValueOrder.FirstValueGreaterOrEqual);
		}
	}
}