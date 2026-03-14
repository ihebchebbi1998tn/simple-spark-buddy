using Crm.Library.AutoFac;
using System.Collections.Generic;

namespace Crm.Service.Services.Interfaces
{
	public interface IServiceOrderMaterialService : IDependency
	{
		IEnumerable<string> GetUsedCommissioningStatuses();
		IEnumerable<string> GetUsedNoPreviousSerialNoReasons();
		IEnumerable<string> GetUsedQuantityUnits();
	}
}
