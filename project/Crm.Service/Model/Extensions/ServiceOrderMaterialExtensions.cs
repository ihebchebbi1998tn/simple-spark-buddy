namespace Crm.Service.Model.Extensions
{
	using System;
	using System.Linq;
	using System.Linq.Dynamic.Core;

	using Crm.Service.SearchCriteria;

	public static class ServiceOrderMaterialExtensions
	{
		public static IQueryable<ServiceOrderMaterial> Filter(this IQueryable<ServiceOrderMaterial> materials, ServiceOrderMaterialSearchCriteria criteria)
		{
			if (criteria == null)
				return materials;

			// Filter Id
			if (criteria.Id != default(Guid))
			{
				materials = materials.Where(m => m.Id == criteria.Id);
			}

			// Filter OrderId
			if (criteria.OrderId.HasValue)
				materials = materials.Where(m => m.OrderId == criteria.OrderId.Value);

			// IsExported
			if (criteria.IsExported.HasValue)
				materials = materials.Where(m => m.IsExported == criteria.IsExported);

			// Filter DispatchId
			if (criteria.DispatchId != null)
				materials = materials.Where(m => m.DispatchId == criteria.DispatchId);

			// Filter ParentId
			if (criteria.ParentId != default(Guid))
				materials = materials.Where(m => m.ServiceOrderTimeId == criteria.ParentId);

			if (!string.IsNullOrEmpty(criteria.SortBy))
				materials = materials.OrderBy(string.Format("{0} {1}", criteria.SortBy, criteria.SortOrder));

			return materials;
		}
	}
}
