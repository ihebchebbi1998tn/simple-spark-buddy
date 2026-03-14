using Crm.Library.AutoFac;
using System.Collections.Generic;

namespace Crm.Service.Services.Interfaces
{
	using Crm.Service.Model;

	public interface IServiceOrderDispatchService : IDependency
	{
		IEnumerable<string> GetUsedComponents();
		IEnumerable<string> GetUsedServiceOrderDispatchRejectReasons();
		void SendCancelNotificationEmail(ServiceOrderDispatch dispatch);
	}
}
