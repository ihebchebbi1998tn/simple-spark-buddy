using Crm.Library.AutoFac;
using System.Collections.Generic;

namespace Crm.Service.Services.Interfaces
{
	public interface IServiceCaseService : IDependency
	{
		IEnumerable<string> GetUsedErrorCodes();
		IEnumerable<string> GetUsedServiceCaseCategories();
		IEnumerable<int> GetUsedServiceCaseStatuses();
		IEnumerable<string> GetUsedServicePriorities();
	}
}
