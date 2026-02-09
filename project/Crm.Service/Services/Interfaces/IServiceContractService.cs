namespace Crm.Service.Services.Interfaces
{
	using System.Collections.Generic;

	using Crm.Library.AutoFac;

	public interface IServiceContractService : IDependency
	{
		IEnumerable<string> GetUsedServiceContractLimitTypes();
		IEnumerable<string> GetUsedServiceContractStatuses();
		IEnumerable<string> GetUsedServiceContractTypes();
		IEnumerable<string> GetUsedSparePartsBudgetInvoiceTypes();
		IEnumerable<string> GetUsedSparePartsBudgetTimeSpanUnits();
		IEnumerable<string> GetUsedPaymentIntervals();
		IEnumerable<string> GetUsedPaymentTypes();
		IEnumerable<string> GetUsedServiceContractAddressRelationshipTypes();
		IEnumerable<string> GetUsedTimeUnits();
		IEnumerable<string> GetUsedCurrencies();
	}
}
