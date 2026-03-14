namespace Main.Services;

using System.Linq;
using System.Linq.Dynamic.Core;

using Crm.Library.Api.Controller;
using Crm.Library.AutoFac;
using Crm.Library.BaseModel.Interfaces;
using Crm.Library.Extensions;

using Microsoft.AspNetCore.OData.Query;

public class ODataQueryOrderByArray : IODataQueryFunction, IDependency
{
	public virtual IQueryable<T> Apply<T, TRest>(ODataQueryOptions<TRest> options, IQueryable<T> query)
		where T : class, IEntityWithId
		where TRest : class
	{
		var parameters = options.Request.Query;
		var parameterName = "orderByArray";
		var properties = parameters.Keys.Where(x => x.StartsWith(parameterName)).Select(x => x.Substring(parameterName.Length)).ToList();

		foreach (var property in properties)
		{
			var values = options.Request.GetQueryParameter(parameterName + property);
			var valueList = values.Split(',').ToList();
			foreach (var value in valueList)
			{
				query = query.OrderBy($"{property} == \"{value}\" descending");
			}
		}

		return query;
	}
}
