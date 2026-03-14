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

	public class ODataQueryServiceContractMaintenancePlanNextDateFilter : IODataQueryFunction, IDependency
	{
		private readonly IRepositoryWithTypedId<ServiceContract, Guid> serviceContractRepository;
		private readonly IRepositoryWithTypedId<MaintenancePlan, Guid> maintenancePlanRepository;
		protected static MethodInfo FilterInfo = typeof(ODataQueryServiceContractMaintenancePlanNextDateFilter)
			.GetMethod(nameof(Filter), BindingFlags.Instance | BindingFlags.NonPublic);

		public ODataQueryServiceContractMaintenancePlanNextDateFilter(IRepositoryWithTypedId<ServiceContract, Guid> serviceContractRepository, IRepositoryWithTypedId<MaintenancePlan, Guid> maintenancePlanRepository)
		{
			this.serviceContractRepository = serviceContractRepository;
			this.maintenancePlanRepository = maintenancePlanRepository;
		}
		protected virtual IQueryable<ServiceContract> Filter(IQueryable<ServiceContract> query, DateTime fromDate, DateTime toDate)
		{
			var serviceContractIds = maintenancePlanRepository.GetAll()
				.Where(x => x.NextDate >= fromDate && x.NextDate <= toDate)
				.Select(x => x.ServiceContractId);

			return query.Where(x => serviceContractIds.Contains(x.Id));
		}
		public virtual IQueryable<T> Apply<T, TRest>(ODataQueryOptions<TRest> options, IQueryable<T> query)
			where T : class, IEntityWithId
			where TRest : class
		{
			if (typeof(ServiceContract).IsAssignableFrom(typeof(T)) == false)
			{
				return query;
			}
			const string fromDateParameterName = "filterByNextFireDateFrom";
			const string toDateParameterName = "filterByNextFireDateTo";
			var parameters = options.Request.Query;
			if (parameters.Keys.Contains(fromDateParameterName) && parameters.Keys.Contains(toDateParameterName))
			{
				var fromDate = DateTime.Now.Date;
				DateTime.TryParse(options.Request.GetQueryParameter(fromDateParameterName).Replace("\"", ""), out fromDate);
				var toDate = DateTime.MaxValue;
				DateTime.TryParse(options.Request.GetQueryParameter(toDateParameterName).Replace("\"", ""), out toDate);
				return (IQueryable<T>)FilterInfo.Invoke(this, new object[] { query, fromDate, toDate });
			}
			return query;
		}
	}
}
