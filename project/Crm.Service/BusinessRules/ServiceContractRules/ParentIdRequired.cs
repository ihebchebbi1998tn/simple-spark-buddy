namespace Crm.Service.BusinessRules.ServiceContractRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class BusinessPartnerIdRequired : RequiredRule<ServiceContract>
	{
		//Constructor
		public BusinessPartnerIdRequired()
		{
			Init(s => s.ParentId, "BusinessPartner");
		}
	}
}