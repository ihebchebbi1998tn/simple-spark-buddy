namespace Crm.Service.BusinessRules.ServiceOrderRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class ReportedRequired : RequiredRule<ServiceOrderHead>
	{
		public ReportedRequired()
		{
			Init(x => x.Reported);
		}

		protected override bool IsIgnoredFor(ServiceOrderHead entity)
		{
			return entity.IsTemplate;
		}
	}
}