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
	using Crm.Service.Model.Relationships;

	public class InstallationPersonRelationshipSyncService : DefaultSyncService<InstallationPersonRelationship, Guid>
	{
		private readonly ISyncService<Installation> installationSyncService;
		private readonly ISyncService<Person> personSyncService;
		public InstallationPersonRelationshipSyncService(IRepositoryWithTypedId<InstallationPersonRelationship, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, ISyncService<Installation> installationSyncService, ISyncService<Person> personSyncService)
			: base(repository,
				restTypeProvider,
				restSerializer,
				mapper)
		{
			this.installationSyncService = installationSyncService;
			this.personSyncService = personSyncService;
		}
		public override Type[] ClientSyncDependencies => new[] { typeof(Installation), typeof(Person) };
		public override Type[] SyncDependencies => new[] { typeof(Installation), typeof(Person) };
		public override IQueryable<InstallationPersonRelationship> GetAll(User user, IDictionary<string, int?> groups, IDictionary<string, Guid> clientIds)
		{
			var installations = installationSyncService.GetAll(user,
				groups,
				clientIds);
			var persons = personSyncService.GetAll(user,
				groups,
				clientIds);
			return repository.GetAll()
				.Where(x => installations.Any(y => y.Id == x.ParentId) && persons.Any(y => y.Id == x.ChildId));
		}
	}
}
