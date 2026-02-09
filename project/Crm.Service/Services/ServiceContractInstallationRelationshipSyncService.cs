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
	using Crm.Service.Model.Relationships;

	using NHibernate.Linq;

	public class ServiceContractInstallationRelationshipSyncService : DefaultSyncService<ServiceContractInstallationRelationship, Guid>
	{
		private readonly ISyncService<ServiceContract> serviceContractSyncService;
		private readonly ISyncService<Installation> installationSyncService;
		public ServiceContractInstallationRelationshipSyncService(IRepositoryWithTypedId<ServiceContractInstallationRelationship, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, ISyncService<ServiceContract> serviceContractSyncService, ISyncService<Installation> installationSyncService)
			: base(repository,
				restTypeProvider,
				restSerializer,
				mapper)
		{
			this.serviceContractSyncService = serviceContractSyncService;
			this.installationSyncService = installationSyncService;
		}

		public override Type[] SyncDependencies => new[] { typeof(ServiceContract), typeof(Installation) };
		public override Type[] ClientSyncDependencies => new[] { typeof(ServiceContract), typeof(Installation) };

		public override IQueryable<ServiceContractInstallationRelationship> GetAll(User user, IDictionary<string, int?> groups, IDictionary<string, Guid> clientIds)
		{
			var serviceContracts = serviceContractSyncService.GetAll(user,
				groups,
				clientIds);
			var installations = installationSyncService.GetAll(user,
				groups,
				clientIds);
			return repository.GetAll().Where(x => serviceContracts.Any(y => y.Id == x.ParentId) && installations.Any(y => y.Id == x.ChildId));
		}
		public override IQueryable<ServiceContractInstallationRelationship> Eager(IQueryable<ServiceContractInstallationRelationship> entities)
		{
			entities = entities.Fetch(x => x.Parent);

			entities = entities
				.Fetch(x => x.Child)
				.ThenFetch(x => x.ServiceObject);
			entities = entities
				.Fetch(x => x.Child)
				.ThenFetch(x => x.LocationCompany);
			return entities;
		}
	}
}
