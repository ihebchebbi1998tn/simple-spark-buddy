namespace Crm.Service.BusinessRules.ServiceContractRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class ManufactureDateNotBeforeWarrantyFrom : OrderRule<Installation>
	{
		public ManufactureDateNotBeforeWarrantyFrom()
		{
			Init(c => c.WarrantyFrom, c => c.ManufactureDate, ValueOrder.FirstValueGreaterOrEqual);
		}
	}
}