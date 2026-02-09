namespace Crm.Service.BusinessRules.ServiceOrderDispatchRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class SignatureContactNameMaxLength : MaxLengthRule<ServiceOrderDispatch>
	{
		public SignatureContactNameMaxLength()
		{
			Init(d => d.SignatureContactName, 256);
		}
	}
}