namespace Crm.Service.BusinessRules.MaintenancePlans
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class NameRequired : RequiredRule<MaintenancePlan>
	{
		//Constructor
		public NameRequired()
		{
			Init(m => m.Name);
		}
	}
}