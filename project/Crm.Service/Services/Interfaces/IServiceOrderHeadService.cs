using Crm.Library.AutoFac;
using System.Collections.Generic;

namespace Crm.Service.Services.Interfaces
{
	public interface IServiceOrderHeadService : IDependency
	{
		IEnumerable<string> GetUsedCommissioningStatuses();
		IEnumerable<string> GetUsedInvoicingTypes();
		IEnumerable<string> GetUsedServiceOrderInvoiceReasons();
		IEnumerable<string> GetUsedServiceOrderNoInvoiceReasons();
		IEnumerable<string> GetUsedServiceOrderTypes();
		IEnumerable<string> GetUsedServicePriorities();
		IEnumerable<string> GetUsedSkills();
		IEnumerable<string> GetUsedAssets();
		IEnumerable<string> GetUsedErrorCodes();

	}
}
