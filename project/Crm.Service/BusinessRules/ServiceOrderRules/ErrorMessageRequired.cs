namespace Crm.Service.BusinessRules.ServiceOrderRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class ErrorMessageRequired : RequiredRule<ServiceOrderHead>
	{
		public ErrorMessageRequired()
		{
			Init(x => x.ErrorMessage);
		}

		protected override bool IsIgnoredFor(ServiceOrderHead entity)
		{
			return entity.IsTemplate;
		}
	}
}