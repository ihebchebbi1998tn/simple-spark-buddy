namespace Crm.Service.BusinessRules.ServiceOrderRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class Name3MaxLength : MaxLengthRule<ServiceOrderHead>
	{
		public Name3MaxLength()
		{
			Init(d => d.Name3, 180);
		}
	}
}