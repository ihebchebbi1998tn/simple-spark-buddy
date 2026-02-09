namespace Crm.Service.Services.Interfaces
{

	using Crm.Library.AutoFac;
	using Crm.Service.Model;
	using Crm.Library.Model;
	public interface IAttentionTaskService : IDependency
	{
		void CreateAttentionTaskForServiceOrder(ServiceOrderHead serviceOrder, User user, string text);
		void CreateAttentionTaskForDispatch(ServiceOrderDispatch dispatch, User user, string text);

	}
}
