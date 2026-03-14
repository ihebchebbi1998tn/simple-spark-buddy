namespace Crm.Service.BusinessRules.ServiceOrderDispatchRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model.Lookup;
	using Crm.Service.Rest.Model;

	public class RejectReasonRequired : RequiredRule<ServiceOrderDispatchRest>
	{
		public override bool IsSatisfiedBy(ServiceOrderDispatchRest entity)
		{
			return entity.StatusKey != ServiceOrderDispatchStatus.RejectedKey;
		}

		public RejectReasonRequired()
		{
			Init(x => x.RejectReasonKey, "RejectReason");
		}
	}
}
