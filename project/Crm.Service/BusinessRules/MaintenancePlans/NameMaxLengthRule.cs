namespace Crm.Service.BusinessRules.MaintenancePlans
{
	using Crm.Library.Extensions;
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class NameMaxLengthRule : MaxLengthRule<MaintenancePlan>
	{
		// Methods
		protected override bool IsIgnoredFor(MaintenancePlan maintenancePlan)
		{
			return maintenancePlan.Name.IsNullOrEmpty();
		}

		// Constructor
		public NameMaxLengthRule()
		{
			Init(m => m.Name, 120);
		}
	}
}