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
	using Crm.PerDiem.Model;
	using Crm.Service.Model;
	using Crm.Service.Rest.Model;

	public class ServiceOrderErrorTypeSyncService : DefaultSyncService<ServiceOrderErrorType, ServiceOrderErrorTypeRest, Guid>
	{
		private readonly ISyncService<ServiceOrderHead> serviceOrderSyncService;
		private readonly ISyncService<ServiceCase> serviceCaseSyncService;

		public ServiceOrderErrorTypeSyncService(IRepositoryWithTypedId<ServiceOrderErrorType, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, ISyncService<ServiceOrderHead> serviceOrderSyncService, ISyncService<ServiceCase> serviceCaseSyncService)
			: base(repository,
				restTypeProvider,
				restSerializer,
				mapper)
		{
			this.serviceOrderSyncService = serviceOrderSyncService;
			this.serviceCaseSyncService = serviceCaseSyncService;
		}

		public override Type[] ClientSyncDependencies
		{
			get { return new[] { typeof(ServiceOrderHead) }; }
		}
		public override Type[] SyncDependencies
		{
			get { return new[] { typeof(ServiceOrderHead), typeof(ServiceOrderDispatch), typeof(PerDiemReport) }; }
		}
		public override IQueryable<ServiceOrderErrorType> GetAll(User user, IDictionary<string, int?> groups, IDictionary<string, Guid> clientIds)
		{
			var serviceOrders = serviceOrderSyncService.GetAll(user,
				groups,
				clientIds);
			var serviceCases = serviceCaseSyncService.GetAll(user,
				groups,
				clientIds);
			return repository.GetAll()
				.Where(x => serviceOrders.Any(y => y.Id == x.OrderId) || serviceCases.Any(y => y.Id == x.ServiceCaseId));
		}
		public override ServiceOrderErrorType Save(ServiceOrderErrorType entity)
		{
			var preplannedPositionWasDeleted = entity.ParentServiceOrderErrorTypeId.HasValue && repository.Get(entity.ParentServiceOrderErrorTypeId.Value) == null;
			if (preplannedPositionWasDeleted)
			{
				entity.ParentServiceOrderErrorTypeId = null;
			}

			return repository.SaveOrUpdate(entity);
		}
	}
}
