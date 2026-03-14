namespace Crm.Service.BusinessRules.ServiceOrderRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class PurchaseOrderNoMaxLength : MaxLengthRule<ServiceOrderHead>
	{
		public PurchaseOrderNoMaxLength()
		{
			Init(d => d.PurchaseOrderNo, 256);
		}
	}
}