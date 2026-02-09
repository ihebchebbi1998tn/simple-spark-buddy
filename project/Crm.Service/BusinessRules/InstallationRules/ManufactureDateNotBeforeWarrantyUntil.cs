namespace Crm.Service.BusinessRules.ServiceContractRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class ManufactureDateNotBeforeWarrantyUntil : OrderRule<Installation>
	{
		public ManufactureDateNotBeforeWarrantyUntil()
		{
			Init(c => c.WarrantyUntil, c => c.ManufactureDate, ValueOrder.FirstValueGreaterOrEqual);
		}
	}
}