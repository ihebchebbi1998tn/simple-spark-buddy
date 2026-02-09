namespace Crm.Service.Model.Helpers
{
	using Crm.Library.AutoFac;

	public interface IServiceOrderDispatchesAddableEvaluator : ITransientDependency
	{
		bool Evaluate(ServiceOrderHead order, string username);
	}
}
