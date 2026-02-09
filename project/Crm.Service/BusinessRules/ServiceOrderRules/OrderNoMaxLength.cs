namespace Crm.Service.BusinessRules.ServiceOrderRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class OrderNoMaxLength : MaxLengthRule<ServiceOrderHead>
	{
		public OrderNoMaxLength()
		{
			Init(d => d.OrderNo, 120);
		}
	}
}