namespace Crm.Service.Model.Helpers
{
	using Crm.Library.AutoFac;

	public interface IServiceOrderDispatchStatusEvaluator : IDependency
	{
		DefaultServiceOrderDispatchStatusEvaluator.CompareResult CompareStatus(string status1, string status2);
		bool IsStatusTransitionAllowed(string fromStatus, string toStatus);
	}
}