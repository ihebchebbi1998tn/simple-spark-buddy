namespace Crm.Service.Services.Interfaces
{
	using System;
	using System.Collections.Generic;
	using Crm.Library.AutoFac;
	using Crm.Library.Services.Interfaces;
	using Crm.Service.Model;

	public interface IReplenishmentOrderService : ITransientDependency
	{
		bool SendOrderAsPdf(ReplenishmentOrder replenishmentOrder);
		string[] GetDefaultReplenishmentOrderRecipientEmails(ReplenishmentOrder replenishmentOrder);
		IEnumerable<string> GetUsedQuantityUnits();
		byte[] CreateReportAsPdf(Guid replenishmentOrderId);
	}
}
