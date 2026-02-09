namespace Main.Controllers.OData
{
	using System;
	using System.Linq;
	using System.Reflection;

	using Crm.Library.Api.Controller;
	using Crm.Library.AutoFac;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Model;

	using Microsoft.AspNetCore.OData.Query;

	public class ODataQueryUserRoleFilter : IODataQueryFunction, IDependency
	{
		protected static MethodInfo FilterUserByRoleInfo = typeof(ODataQueryUserRoleFilter)
			.GetMethod(nameof(FilterUserByRole), BindingFlags.Instance | BindingFlags.NonPublic);
		protected virtual IQueryable<User> FilterUserByRole(IQueryable<User> query, Guid roleId)
		{
			return query.Where(x => x.Roles.Any(y => y.UId == roleId));
		}
		public virtual IQueryable<T> Apply<T, TRest>(ODataQueryOptions<TRest> options, IQueryable<T> query)
			where T : class, IEntityWithId
			where TRest : class
		{
			if (typeof(User).IsAssignableFrom(typeof(T)))
			{
				const string parameterName = "filterByRoleId";
				var parameters = options.Request.Query;
				if (parameters.Keys.Contains(parameterName))
				{
					var filter = options.Request.GetQueryParameter(parameterName)?.Trim();
					if (Guid.TryParse(filter, out var roleId))
					{
						query = (IQueryable<T>)FilterUserByRoleInfo.Invoke(this, new object[] { query, roleId });
					}
				}
			}
			return query;
		}
	}
}
