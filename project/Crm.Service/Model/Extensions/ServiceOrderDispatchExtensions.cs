namespace Crm.Service.Model.Extensions
{
	using System.Linq;
	using System.Linq.Dynamic.Core;
	
	using Crm.Library.Extensions;
	using Crm.Service.SearchCriteria;

	public static class ServiceOrderDispatchExtensions
	{
		public static IQueryable<ServiceOrderDispatch> Filter(this IQueryable<ServiceOrderDispatch> dispatches, ServiceOrderDispatchSearchCriteria criteria)
		{
			// Filter by OrderId
			if (criteria.OrderId.HasValue)
			{
				dispatches = dispatches.Where(d => d.OrderId == criteria.OrderId);
			}

			// Filter by OrderNo
			if (criteria.OrderNo.IsNotNullOrEmpty())
			{
				dispatches = dispatches.Where(d => d.OrderHead.OrderNo == criteria.OrderNo);
			}

			// Filter by Username
			if (criteria.Username.IsNotNullOrEmpty())
			{
				dispatches = dispatches.Where(d => d.DispatchedUser.Id == criteria.Username);
			}

			if (!string.IsNullOrEmpty(criteria.SortBy))
			{
				dispatches = dispatches.OrderBy(string.Format("{0} {1}", criteria.SortBy, criteria.SortOrder));
			}

			return dispatches;
		}
	}
}