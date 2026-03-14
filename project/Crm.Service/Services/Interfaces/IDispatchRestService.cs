namespace Crm.Service.Services.Interfaces
{
	using Crm.Library.AutoFac;
	using Crm.Service.Model;

	public interface IDispatchRestService : ITransientDependency
	{
		void CreateDispatch(ServiceOrderDispatch dispatch);
		void UpdateDispatch(ServiceOrderDispatch dispatch);
		void DeleteDispatch(ServiceOrderDispatch dispatch);
	}
}