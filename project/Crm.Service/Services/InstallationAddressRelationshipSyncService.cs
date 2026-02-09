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

	public class InstallationAddressRelationshipSyncService : DefaultSyncService<InstallationAddressRelationship, Guid>
	{
		private readonly ISyncService<Address> addressSyncService;
		public readonly ISyncService<Installation> installationSyncService;

		public InstallationAddressRelationshipSyncService(IRepositoryWithTypedId<InstallationAddressRelationship, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, ISyncService<Address> addressSyncService, ISyncService<Installation> installationSyncService)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
			this.addressSyncService = addressSyncService;
			this.installationSyncService = installationSyncService;
		}
		public override Type[] SyncDependencies => new[] { typeof(Address), typeof(Installation) };
		public override Type[] ClientSyncDependencies => new[] { typeof(Address), typeof(Installation) };
		public override InstallationAddressRelationship Save(InstallationAddressRelationship entity)
		{
			return repository.SaveOrUpdate(entity);
		}
		public override IQueryable<InstallationAddressRelationship> GetAll(User user, IDictionary<string, int?> groups, IDictionary<string, Guid> clientIds)
		{
			var addresses = addressSyncService.GetAll(user, groups, clientIds);
			var installations = installationSyncService.GetAll(user, groups, clientIds);
			return repository.GetAll()
				.Where(x => addresses.Any(y => y.Id == x.ChildId) && installations.Any(y => y.Id == x.ParentId));
		}
	}
}
