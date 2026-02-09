namespace Crm.Service.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using AutoMapper;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Rest;
	using Crm.Library.Services;
	using Crm.Library.Services.Interfaces;
	using Crm.Service.Model;
	using Crm.Service.Rest.Model;

	public class ServiceOrderStatisticsKeySyncService : DefaultSyncService<ServiceOrderStatisticsKey, ServiceOrderStatisticsKeyRest, Guid>
	{
		private readonly ISyncService<ServiceOrderHead> serviceOrderHeadSyncService;

		public ServiceOrderStatisticsKeySyncService(IRepositoryWithTypedId<ServiceOrderStatisticsKey, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, ISyncService<ServiceOrderHead> serviceOrderHeadSyncService)
			: base(repository,
				restTypeProvider,
				restSerializer,
				mapper)
		{
			this.serviceOrderHeadSyncService = serviceOrderHeadSyncService;
		}
		public override Type[] SyncDependencies
		{
			get { return [typeof(ServiceOrderHead), typeof(ServiceOrderDispatch)]; }
		}
		public override IQueryable<ServiceOrderStatisticsKey> GetAll(User user, IDictionary<string, int?> groups, IDictionary<string, Guid> clientIds)
		{
			var serviceOrders = serviceOrderHeadSyncService.GetAll(user,
				groups,
				clientIds);
			return repository.GetAll()
				.Where(x => serviceOrders.Any(y => y.Id == x.ServiceOrderId));
		}
	}
}
