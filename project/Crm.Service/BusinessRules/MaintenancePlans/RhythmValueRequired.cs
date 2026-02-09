namespace Crm.Service.BusinessRules.MaintenancePlans
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class RhythmValueRequired : RequiredRule<MaintenancePlan>
	{
		// Constructor
		public RhythmValueRequired()
		{
			Init(m => m.RhythmValue, "Interval");
		}
	}
}