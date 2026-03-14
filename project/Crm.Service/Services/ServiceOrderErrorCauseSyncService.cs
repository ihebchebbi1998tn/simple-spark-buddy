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

	public class ServiceOrderErrorCauseSyncService : DefaultSyncService<ServiceOrderErrorCause, ServiceOrderErrorCauseRest, Guid>
	{
		private readonly ISyncService<ServiceOrderErrorType> serviceOrderErrorTypeSyncService;
		public ServiceOrderErrorCauseSyncService(IRepositoryWithTypedId<ServiceOrderErrorCause, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, ISyncService<ServiceOrderErrorType> serviceOrderErrorTypeSyncService)
			: base(repository,
				restTypeProvider,
				restSerializer,
				mapper)
		{
			this.serviceOrderErrorTypeSyncService = serviceOrderErrorTypeSyncService;
		}

		public override Type[] ClientSyncDependencies
		{
			get { return new[] { typeof(ServiceOrderErrorType) }; }
		}
		public override Type[] SyncDependencies
		{
			get { return new[] { typeof(ServiceOrderHead), typeof(ServiceOrderDispatch), typeof(PerDiemReport), typeof(ServiceOrderErrorType) }; }
		}
		public override IQueryable<ServiceOrderErrorCause> GetAll(User user, IDictionary<string, int?> groups, IDictionary<string, Guid> clientIds)
		{
			var serviceOrderErrorTypes = serviceOrderErrorTypeSyncService.GetAll(user,
				groups,
				clientIds);
			return repository.GetAll()
				.Where(x => serviceOrderErrorTypes.Any(y => y.Id == x.ServiceOrderErrorTypeId));
		}
		public override ServiceOrderErrorCause Save(ServiceOrderErrorCause entity)
		{
			var preplannedPositionWasDeleted = entity.ParentServiceOrderErrorCauseId.HasValue && repository.Get(entity.ParentServiceOrderErrorCauseId.Value) == null;
			if (preplannedPositionWasDeleted)
			{
				entity.ParentServiceOrderErrorCauseId = null;
			}
			return repository.SaveOrUpdate(entity);
		}
	}
}
