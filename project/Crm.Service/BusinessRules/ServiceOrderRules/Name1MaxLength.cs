namespace Crm.Service.BusinessRules.ServiceOrderRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class Name1MaxLength : MaxLengthRule<ServiceOrderHead>
	{
		public Name1MaxLength()
		{
			Init(d => d.Name1, 450);
		}
	}
}