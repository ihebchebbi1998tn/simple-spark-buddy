namespace Main.Services
{
	using System;
	using System.Linq;

	using AutoMapper;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services;
	using Crm.Library.Unicore;

	using LMobile.Unicore;

	using User = Crm.Library.Model.User;

	public class RoleSyncService : DefaultSyncService<PermissionSchemaRole, Guid>
	{
		private readonly IAccessControlManager accessControlManager;
		public RoleSyncService(IRepositoryWithTypedId<PermissionSchemaRole, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, IAccessControlManager accessControlManager)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
			this.accessControlManager = accessControlManager;
		}
		public override IQueryable<PermissionSchemaRole> GetAll(User user)
		{
			var permissionSchema = accessControlManager.FindPermissionSchema(UnicoreDefaults.DefaultPermissionSchema);
			return repository.GetAll().Where(x => x.PermissionSchemaId == permissionSchema.UId);
		}
	}
}
