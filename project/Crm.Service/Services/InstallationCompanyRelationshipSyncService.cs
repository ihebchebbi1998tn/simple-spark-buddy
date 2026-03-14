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

	public class InstallationCompanyRelationshipSyncService : DefaultSyncService<InstallationCompanyRelationship, Guid>
	{
		private readonly ISyncService<Company> companySyncService;
		private readonly ISyncService<Installation> installationSyncService;
		public InstallationCompanyRelationshipSyncService(IRepositoryWithTypedId<InstallationCompanyRelationship, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, ISyncService<Company> companySyncService, ISyncService<Installation> installationSyncService)
			: base(repository,
				restTypeProvider,
				restSerializer,
				mapper)
		{
			this.companySyncService = companySyncService;
			this.installationSyncService = installationSyncService;
		}
		public override Type[] ClientSyncDependencies => new[] { typeof(Installation), typeof(Company) };
		public override Type[] SyncDependencies => new[] { typeof(Installation), typeof(Company) };
		public override IQueryable<InstallationCompanyRelationship> GetAll(User user, IDictionary<string, int?> groups, IDictionary<string, Guid> clientIds)
		{
			var companies = companySyncService.GetAll(user,
				groups,
				clientIds);
			var installations = installationSyncService.GetAll(user,
				groups,
				clientIds);
			return repository.GetAll()
				.Where(x => installations.Any(y => y.Id == x.ParentId) && companies.Any(y => y.Id == x.ChildId));
		}
	}
}
