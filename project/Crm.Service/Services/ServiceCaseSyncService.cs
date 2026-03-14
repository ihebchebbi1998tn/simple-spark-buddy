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
	using Crm.Model;
	using Crm.Service.Model;
	using Crm.Services.Interfaces;

	using Main.Replication.Services;

	public class ServiceCaseSyncService : DefaultSyncService<ServiceCase, Guid>, IContactSyncService
	{
		private readonly IVisibilityProvider visibilityProvider;
		public ServiceCaseSyncService(IRepositoryWithTypedId<ServiceCase, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, IVisibilityProvider visibilityProvider, IReplicationService<ServiceCase, Guid> replicationService = null)
			: base(repository, restTypeProvider, restSerializer, mapper, replicationService)
		{
			this.visibilityProvider = visibilityProvider;
		}
		public override IQueryable<ServiceCase> GetAll(User user)
		{
			return visibilityProvider.FilterByVisibility(repository.GetAll());
		}
		public override Type[] SyncDependencies => new[]
		{
			typeof(ServiceCaseTemplate),
			typeof(ServiceObject),
			typeof(Company),
			typeof(Person),
			typeof(Installation),
			typeof(ServiceOrderTime)
		};
		public virtual IQueryable<Guid> GetAllContactIds(User user, IDictionary<string, int?> groups, IDictionary<string, Guid> clientIds)
		{
			return clientIds != null ? replicationService.GetReplicatedEntityIds(clientIds.FirstOrDefault(x => x.Key == nameof(ServiceCase)).Value) : GetAll(user).Select(x => x.Id);
		}
	}
}
