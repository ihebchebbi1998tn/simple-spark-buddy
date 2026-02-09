namespace Crm.Service.BusinessRules.ServiceOrderDispatchRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class RemarkMaxLength : MaxLengthRule<ServiceOrderDispatch>
	{
		// Constructor
		public RemarkMaxLength()
		{
			Init(d => d.Remark, 500);
		}
	}
}