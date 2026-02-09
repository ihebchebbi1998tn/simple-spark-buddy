namespace Crm.Service.BusinessRules.ServiceOrderTimePostingRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Extensions;
	using Crm.Service.Model;

	public class UserRequiredRule : RequiredRule<ServiceOrderTimePosting>
	{
		public UserRequiredRule()
		{
			Init(x => x.UserUsername);
		}
		protected override bool IsIgnoredFor(ServiceOrderTimePosting entity)
		{
			if (entity.IsPrePlanned() || entity.IsInvoiced())
			{
				return true;
			}
			return base.IsIgnoredFor(entity);
		}
	}
}
