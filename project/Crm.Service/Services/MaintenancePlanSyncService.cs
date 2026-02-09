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
	using Crm.Services.Interfaces;

	using Main.Replication.Services;

	public class MaintenancePlanSyncService : DefaultSyncService<MaintenancePlan, Guid>, IContactSyncService
	{
		private readonly ISyncService<ServiceContract> serviceContractSyncService;

		public override Type[] SyncDependencies => new[] { typeof(ServiceOrderHead) };
		public override Type[] ClientSyncDependencies => new[] { typeof(ServiceContract) };

		public MaintenancePlanSyncService(IRepositoryWithTypedId<MaintenancePlan, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, ISyncService<ServiceContract> serviceContractSyncService, IReplicationService<MaintenancePlan, Guid> replicationService = null)
			: base(repository, restTypeProvider, restSerializer, mapper, replicationService)
		{
			this.serviceContractSyncService = serviceContractSyncService;
		}

		public override IQueryable<MaintenancePlan> GetAll(User user, IDictionary<string, int?> groups, IDictionary<string, Guid> clientIds)
		{
			var serviceContract = serviceContractSyncService.GetAll(user, groups, clientIds);
			return repository.GetAll()
				.Where(x => serviceContract.Any(y => y.Id == x.ServiceContractId));
		}
		public virtual IQueryable<Guid> GetAllContactIds(User user, IDictionary<string, int?> groups, IDictionary<string, Guid> clientIds)
		{
			return clientIds != null ? replicationService.GetReplicatedEntityIds(clientIds.FirstOrDefault(x => x.Key == nameof(MaintenancePlan)).Value) : GetAll(user, groups, null).Select(x => x.Id);
		}
	}
}
