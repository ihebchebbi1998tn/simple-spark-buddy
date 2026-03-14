using Crm.Service.Model;

namespace Crm.Service.Services.Interfaces
{
	using System;
	using System.Collections.Generic;
	using Crm.Library.AutoFac;

	public interface IMaintenancePlanService : ITransientDependency
	{
		ICollection<ServiceOrderHead> EvaluateMaintenancePlanAndGenerateOrders(MaintenancePlan maintenancePlan, DateTime toDate);
		DateTime? CalculateNextMaintenanceDate(MaintenancePlan maintenancePlan);
		IEnumerable<string> GetClosedStatusKeys();
		IEnumerable<string> GetPendingStatusKeys();
		IEnumerable<string> GetUsedTimeUnits();
	}
}
