namespace Crm.Service.Model.Extensions
{
	using System.Linq;
	using Crm.Service.SearchCriteria;

	public static class MaintenancePlanExtensions
	{
		public static IQueryable<MaintenancePlan> Filter(this IQueryable<MaintenancePlan> maintenancePlans, MaintenancePlanSearchCriteria criteria)
		{
			if (criteria == null)
				return maintenancePlans;

			if (criteria.FromNextDate.HasValue)
			{
				maintenancePlans = maintenancePlans.Where(x => x.NextDate >= criteria.FromNextDate);
			}

			if (criteria.ToNextDate.HasValue)
			{
				maintenancePlans = maintenancePlans.Where(x => x.NextDate <= criteria.ToNextDate);
			}
			
			return maintenancePlans;
		}
	}
}