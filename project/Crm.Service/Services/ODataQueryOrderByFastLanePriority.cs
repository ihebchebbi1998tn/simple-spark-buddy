namespace Crm.Service.Services
{
	using Crm.Library.Api.Controller;
	using Crm.Library.AutoFac;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Service.Model;
	using Microsoft.AspNetCore.OData.Query;
	using System.Linq;
	using System.Reflection;
	using Crm.Library.Extensions;

	public class ODataQueryOrderByFastLanePriority : IODataQueryFunction, IDependency
	{
		protected static MethodInfo FilterByVisibilityInfo = typeof(ODataQueryOrderByFastLanePriority).GetMethod(nameof(OrderByFastLanePriority), BindingFlags.Instance | BindingFlags.NonPublic);

		protected virtual IQueryable<ServiceOrderHead> OrderByFastLanePriority(IQueryable<ServiceOrderHead> query, string keys)
		{
			var keyList = keys.Split(',').ToList();
			return query.OrderByDescending(o => keyList.Contains(o.PriorityKey));
		}
		public virtual IQueryable<T> Apply<T, TRest>(ODataQueryOptions<TRest> options, IQueryable<T> query)
			where T : class, IEntityWithId
			where TRest : class
		{
			if (typeof(ServiceOrderHead).IsAssignableFrom(typeof(T)) == false)
			{
				return query;
			}
			const string parameterName = "orderByFastLanePriority";
			var parameters = options.Request.Query;
			if (parameters.Keys.Contains(parameterName))
			{
				var keys = options.Request.GetQueryParameter("orderByFastLanePriority");
				return (IQueryable<T>)FilterByVisibilityInfo.Invoke(this, new object[] { query, keys });
			}
			return query;
		}
	}
}
