namespace Main.Services
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

	using Main.Model;

	public class UserUserGroupSyncService : DefaultSyncService<UserUserGroup, Guid>
	{
		private readonly ISyncService<User> userSyncService;
		private readonly ISyncService<Usergroup> usergroupSyncService;
		public UserUserGroupSyncService(IRepositoryWithTypedId<UserUserGroup, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, ISyncService<User> userSyncService, ISyncService<Usergroup> usergroupSyncService)
			: base(repository,
				restTypeProvider,
				restSerializer,
				mapper)
		{
			this.userSyncService = userSyncService;
			this.usergroupSyncService = usergroupSyncService;
		}
		public override Type[] ClientSyncDependencies => new[] { typeof(User), typeof(Usergroup) };
		public override IQueryable<UserUserGroup> GetAll(User user, IDictionary<string, int?> groups, IDictionary<string, Guid> clientIds)
		{
			var users = userSyncService.GetAll(user,
				groups,
				clientIds);
			var usergroups = usergroupSyncService.GetAll(user,
				groups,
				clientIds);
			return repository.GetAll()
				.Where(x => users.Any(y => y.Id == x.Username))
				.Where(x => usergroups.Any(y => y.Id == x.UserGroupKey));
		}
	}
}
