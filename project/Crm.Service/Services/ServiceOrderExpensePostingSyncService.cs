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
	using Crm.Services.Interfaces;

	using Main.Replication.Services;

	public class ServiceOrderExpensePostingSyncService : DefaultSyncService<ServiceOrderExpensePosting, ServiceOrderExpensePostingRest, Guid>, IFileResourceReferenceSyncService
	{
		private readonly ISyncService<ServiceOrderHead> serviceOrderSyncService;
		public ServiceOrderExpensePostingSyncService(IRepositoryWithTypedId<ServiceOrderExpensePosting, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, ISyncService<ServiceOrderHead> serviceOrderSyncService, IReplicationService<ServiceOrderExpensePosting, Guid> replicationService = null)
			: base(repository, restTypeProvider, restSerializer, mapper, replicationService)
		{
			this.serviceOrderSyncService = serviceOrderSyncService;
		}

		public override Type[] ClientSyncDependencies
		{
			get { return new[] { typeof(ServiceOrderHead) }; }
		}
		public override Type[] SyncDependencies
		{
			get { return new[] { typeof(ServiceOrderHead), typeof(ServiceOrderDispatch), typeof(PerDiemReport) }; }
		}
		public override IQueryable<ServiceOrderExpensePosting> GetAll(User user, IDictionary<string, int?> groups, IDictionary<string, Guid> clientIds)
		{
			var serviceOrders = serviceOrderSyncService.GetAll(user, groups, clientIds);
			return repository.GetAll()
				.Where(x => serviceOrders.Any(y => y.Id == x.OrderId));
		}
		public override ServiceOrderExpensePosting Save(ServiceOrderExpensePosting entity)
		{
			repository.SaveOrUpdate(entity);
			return entity;
		}
		public virtual IQueryable<Guid> GetAllFileResourceIds(User user, IDictionary<string, int?> groups, IDictionary<string, Guid> clientIds)
		{
			var serviceOrderExpensePostings = clientIds != null ? replicationService.GetReplicatedEntities(repository.GetAll(), clientIds.FirstOrDefault(x => x.Key == nameof(ServiceOrderExpensePosting)).Value) : GetAll(user, groups, null);
			return serviceOrderExpensePostings.Where(x => x.FileResourceKey.HasValue).Select(x => x.FileResourceKey.Value);
		}
	}
}
