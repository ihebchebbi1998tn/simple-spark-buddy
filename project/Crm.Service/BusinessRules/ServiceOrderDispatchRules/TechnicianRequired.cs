namespace Crm.Service.BusinessRules.ServiceOrderDispatchRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class TechnicianRequired : RequiredRule<ServiceOrderDispatch>
	{
		// Constructor
		public TechnicianRequired()
		{
			Init(d => d.DispatchedUsername, propertyNameReplacementKey: "DispatchedUser");
		}
	}
}
