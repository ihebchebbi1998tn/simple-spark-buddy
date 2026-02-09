namespace Crm.Service.BusinessRules.ServiceOrderDispatchRules
{
	using Crm.Library.Extensions;
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class SignatureContactNameRequired : RequiredRule<ServiceOrderDispatch>
	{
		// Constructor
		public SignatureContactNameRequired()
		{
			Init(x => x.SignatureContactName);
		}

		protected override bool IsIgnoredFor(ServiceOrderDispatch dispatch)
		{
			return dispatch.SignatureJson.IsNullOrEmpty();
		}
	}
}