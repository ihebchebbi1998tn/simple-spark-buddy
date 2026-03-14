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

	using NHibernate.Linq;

	public class InstallationSyncService : DefaultSyncService<Installation, Guid>, IContactSyncService
	{
		private readonly IVisibilityProvider visibilityProvider;
		public InstallationSyncService(
			IRepositoryWithTypedId<Installation, Guid> repository,
			RestTypeProvider restTypeProvider,
			IRestSerializer restSerializer,
			IMapper mapper,
			IVisibilityProvider visibilityProvider,
			IReplicationService<Installation, Guid> replicationService = null)
			: base(repository, restTypeProvider, restSerializer, mapper, replicationService)
		{
			this.visibilityProvider = visibilityProvider;
		}

		public override Type[] SyncDependencies => new[] { typeof(Company), typeof(ServiceObject), typeof(Person), typeof(Address) };

		public override IQueryable<Installation> GetAll(User user)
		{
			return visibilityProvider.FilterByVisibility(repository.GetAll());
		}
		public override IQueryable<Installation> Eager(IQueryable<Installation> entities)
		{
			return entities
				.Fetch(x => x.ServiceObject)
				.Fetch(x => x.LocationAddress)
				.Fetch(x => x.LocationCompany);
		}
		public virtual IQueryable<Guid> GetAllContactIds(User user, IDictionary<string, int?> groups, IDictionary<string, Guid> clientIds)
		{
			return clientIds != null ? replicationService.GetReplicatedEntityIds(clientIds.FirstOrDefault(x => x.Key == nameof(Installation)).Value) : GetAll(user).Select(x => x.Id);
		}
	}
}
