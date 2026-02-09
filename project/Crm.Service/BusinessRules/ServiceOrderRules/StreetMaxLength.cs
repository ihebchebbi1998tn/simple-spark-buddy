namespace Crm.Service.BusinessRules.ServiceOrderRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class StreetMaxLength : MaxLengthRule<ServiceOrderHead>
	{
		public StreetMaxLength()
		{
			Init(d => d.Street, 4000);
		}
	}
}