namespace Crm.Service.BusinessRules.ServiceOrderRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class ZipCodeMaxLength : MaxLengthRule<ServiceOrderHead>
	{
		public ZipCodeMaxLength()
		{
			Init(d => d.ZipCode, 20);
		}
	}
}