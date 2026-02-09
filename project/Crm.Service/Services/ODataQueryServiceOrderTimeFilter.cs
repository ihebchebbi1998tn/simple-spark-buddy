namespace Crm.Service.Services
{
	using System;
	using System.Linq;
	using System.Reflection;

	using Crm.Library.Api.Controller;
	using Crm.Library.AutoFac;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Service.Model;

	using Microsoft.AspNetCore.OData.Query;

	public class ODataQueryServiceOrderTimeFilter : IODataQueryFunction, IDependency
	{
		private readonly IRepositoryWithTypedId<ServiceOrderTime, Guid> serviceOrderTimeRepository;
		protected static MethodInfo FilterInfo = typeof(ODataQueryServiceOrderTimeFilter)
			.GetMethod(nameof(Filter), BindingFlags.Instance | BindingFlags.NonPublic);
		public ODataQueryServiceOrderTimeFilter(IRepositoryWithTypedId<ServiceOrderTime, Guid> serviceOrderTimeRepository)
		{
			this.serviceOrderTimeRepository = serviceOrderTimeRepository;
		}
		protected virtual IQueryable<Installation> Filter(IQueryable<Installation> query, Guid serviceOrderId)
		{
			var ids = serviceOrderTimeRepository.GetAll().Where(x => x.OrderId == serviceOrderId && x.InstallationId != null).Select(x => x.InstallationId);
			return query.Where(x => ids.Contains(x.Id));
		}
		public virtual IQueryable<T> Apply<T, TRest>(ODataQueryOptions<TRest> options, IQueryable<T> query)
			where T : class, IEntityWithId
			where TRest : class
		{
			if (typeof(Installation).IsAssignableFrom(typeof(T)) == false)
			{
				return query;
			}
			const string parameterName = "filterByServiceOrderTimes";
			var parameters = options.Request.Query;
			if (parameters.Keys.Contains(parameterName))
			{
				var serviceOrderId = Guid.Parse(options.Request.GetQueryParameter(parameterName));
				return (IQueryable<T>)FilterInfo.Invoke(this, new object[] { query, serviceOrderId });
			}
			return query;
		}
	}
}
