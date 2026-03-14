namespace Crm.Service.BusinessRules.ServiceContractRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class ManufactureDateNotBeforeKickoffDate : OrderRule<Installation>
	{
		public ManufactureDateNotBeforeKickoffDate()
		{
			Init(c => c.KickOffDate, c => c.ManufactureDate, ValueOrder.FirstValueGreaterOrEqual);
		}
	}
}