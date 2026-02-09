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

	using NHibernate.Linq;

	public class ServiceObjectSyncService : DefaultSyncService<ServiceObject, Guid>, IContactSyncService, IAddressReferenceSyncService
	{
		private readonly IVisibilityProvider visibilityProvider;
		public ServiceObjectSyncService(
			IRepositoryWithTypedId<ServiceObject, Guid> repository,
			RestTypeProvider restTypeProvider,
			IRestSerializer restSerializer,
			IMapper mapper,
			IVisibilityProvider visibilityProvider,
			IReplicationService<ServiceObject, Guid> replicationService = null)
			: base(repository, restTypeProvider, restSerializer, mapper, replicationService)
		{
			this.visibilityProvider = visibilityProvider;
		}
		public override IQueryable<ServiceObject> GetAll(User user)
		{
			return visibilityProvider.FilterByVisibility(repository.GetAll());
		}
		public override IQueryable<ServiceObject> Eager(IQueryable<ServiceObject> entities)
		{
			return entities.Fetch(x => x.StandardAddress);
		}
		public virtual IQueryable<Guid> GetAllContactIds(User user, IDictionary<string, int?> groups, IDictionary<string, Guid> clientIds)
		{
			return clientIds != null ? replicationService.GetReplicatedEntityIds(clientIds.FirstOrDefault(x => x.Key == nameof(ServiceObject)).Value) : GetAll(user).Select(x => x.Id);
		}
	}
}
