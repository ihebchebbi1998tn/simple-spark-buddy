namespace Crm.Service.BusinessRules.ServiceOrderDispatchRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Rest.Model;

	public class StatusRequired : RequiredRule<ServiceOrderDispatchRest>
	{
		public StatusRequired()
		{
			Init(x => x.StatusKey, propertyNameReplacementKey: "Status");
		}
	}
}
