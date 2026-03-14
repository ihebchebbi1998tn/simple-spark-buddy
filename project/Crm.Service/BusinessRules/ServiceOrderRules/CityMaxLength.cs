namespace Crm.Service.BusinessRules.ServiceOrderRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class CityMaxLength : MaxLengthRule<ServiceOrderHead>
	{
		public CityMaxLength()
		{
			Init(d => d.City, 80);
		}
	}
}