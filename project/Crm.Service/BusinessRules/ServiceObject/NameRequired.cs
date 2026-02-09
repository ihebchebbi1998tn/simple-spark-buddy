namespace Crm.Service.BusinessRules.ServiceObject
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class NameRequired : RequiredRule<ServiceObject>
	{
		// Constructor
		public NameRequired()
		{
			Init(c => c.Name);
		}
	}
}