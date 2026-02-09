using Crm.Library.AutoFac;
using System.Collections.Generic;

namespace Crm.Service.Services.Interfaces
{
	public interface IServiceOrderTimeService : IDependency
	{
		IEnumerable<string> GetUsedNoCausingItemPreviousSerialNoReasons();
		IEnumerable<string> GetUsedNoCausingItemSerialNoReasons();
		IEnumerable<string> GetUsedServiceOrderTimeCategories();
		IEnumerable<string> GetUsedServiceOrderTimeLocations();
		IEnumerable<string> GetUsedServiceOrderTimePriorities();
	}
}
