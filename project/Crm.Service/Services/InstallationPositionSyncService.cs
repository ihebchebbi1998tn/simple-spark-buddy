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

	public class InstallationPositionSyncService : DefaultSyncService<InstallationPos, Guid>
	{
		public readonly ISyncService<Installation> installationSyncService;

		public InstallationPositionSyncService(IRepositoryWithTypedId<InstallationPos, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, ISyncService<Installation> installationSyncService)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
			this.installationSyncService = installationSyncService;
		}
		public override Type[] SyncDependencies => new[] { typeof(Installation) };
		public override Type[] ClientSyncDependencies => new[] { typeof(Installation) };
		public override InstallationPos Save(InstallationPos entity)
		{
			return repository.SaveOrUpdate(entity);
		}
		public override IQueryable<InstallationPos> GetAll(User user, IDictionary<string, int?> groups, IDictionary<string, Guid> clientIds)
		{
			var installations = installationSyncService.GetAll(user, groups, clientIds);
			return repository.GetAll()
				.Where(x => installations.Any(y => y.Id == x.InstallationId));
		}
	}
}
