namespace Crm.Service.BusinessRules.ServiceOrderTimePostingRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class ServiceOrderTimePostingTypeRequired : RequiredRule<ServiceOrderTimePosting>
	{
		public ServiceOrderTimePostingTypeRequired()
		{
			Init(x => x.ServiceOrderTimePostingType);
		}
	}
}
