namespace Crm.Service.BusinessRules.ServiceOrderRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class Name2MaxLength : MaxLengthRule<ServiceOrderHead>
	{
		public Name2MaxLength()
		{
			Init(d => d.Name2, 180);
		}
	}
}