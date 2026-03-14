namespace Crm.Service.BusinessRules.MaintenancePlans
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class RhythmUnitRequired : RequiredRule<MaintenancePlan>
	{
		public RhythmUnitRequired()
		{
			Init(m => m.RhythmUnitKey, "Unit");
		}
	}
}