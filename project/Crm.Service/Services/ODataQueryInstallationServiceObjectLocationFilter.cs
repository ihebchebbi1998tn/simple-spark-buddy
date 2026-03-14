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

	public class ODataQueryServiceObjectInstallationLocationFilter : IODataQueryFunction, IDependency
	{
		private readonly IRepositoryWithTypedId<Installation, Guid> installationRepository;
		private readonly IRepositoryWithTypedId<ServiceObject, Guid> serviceObjectRepository;
		protected static MethodInfo FilterInfo = typeof(ODataQueryServiceObjectInstallationLocationFilter)
			.GetMethod(nameof(Filter), BindingFlags.Instance | BindingFlags.NonPublic);

		public ODataQueryServiceObjectInstallationLocationFilter(IRepositoryWithTypedId<Installation, Guid> installationRepository,
			IRepositoryWithTypedId<ServiceObject, Guid> serviceObjectRepository)
		{
			this.installationRepository = installationRepository;
			this.serviceObjectRepository = serviceObjectRepository;
		}
		protected virtual IQueryable<ServiceObject> Filter(IQueryable<ServiceObject> query, Guid locationContactId)
		{
			var serviceObjectIds = installationRepository.GetAll().Where(x => x.LocationContactId == locationContactId).Select(x => x.FolderId);

			return query.Where(x => serviceObjectIds.Contains(x.Id));
		}
		public virtual IQueryable<T> Apply<T, TRest>(ODataQueryOptions<TRest> options, IQueryable<T> query)
			where T : class, IEntityWithId
			where TRest : class
		{
			if (typeof(ServiceObject).IsAssignableFrom(typeof(T)) == false)
			{
				return query;
			}
			const string locationContactIdParameterName = "locationContactId";
			
			var parameters = options.Request.Query;
			if (parameters.Keys.Contains(locationContactIdParameterName))
			{
				var locationContactId = Guid.Empty;
				Guid.TryParse(options.Request.GetQueryParameter(locationContactIdParameterName).Replace("\"", ""), out locationContactId);
				return (IQueryable<T>)FilterInfo.Invoke(this, new object[] { query, locationContactId });
			}
			return query;
		}
	}
}
