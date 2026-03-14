namespace Crm.Service.Model.Extensions
{
	using System.Linq;
	using System.Linq.Dynamic.Core;
	using Crm.Service.SearchCriteria;

	public static class ServiceOrderTimeExtensions
	{
		public static IQueryable<ServiceOrderTime> Filter(this IQueryable<ServiceOrderTime> times, ServiceOrderTimeSearchCriteria criteria)
		{
			if (criteria == null)
				return times;

			// Filter OrderId
			if (criteria.OrderId.HasValue)
				times = times.Where(o => o.OrderId == criteria.OrderId.Value);

			// Filter InstallationPosId
			if (criteria.InstallationPosId != null)
				times = times.Where(t => t.InstallationPosId == criteria.InstallationPosId);

			// IsExported
			if (criteria.IsExported.HasValue)
				times = times.Where(m => m.IsExported == criteria.IsExported);

			if (!string.IsNullOrEmpty(criteria.SortBy))
				times = times.OrderBy(string.Format("{0} {1}", criteria.SortBy, criteria.SortOrder));

			return times;
		}
	}
}