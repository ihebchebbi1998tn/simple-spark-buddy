namespace Crm.Service.Model.Helpers
{
	using Crm.Library.AutoFac;

	public interface IServiceOrderDispatchEditableByUserEvaluator : IDependency
    {
        bool Evaluate(ServiceOrderDispatch dispatch, ServiceOrderHead order, string username);
    }
}