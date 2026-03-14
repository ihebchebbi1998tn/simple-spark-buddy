namespace Crm.Service.Model.Helpers
{
	using Crm.Library.AutoFac;

	public interface IServiceOrderStatusEvaluator : IDependency
	{
		string Evaluate(ServiceOrderHead serviceOrderHead);
	}
}