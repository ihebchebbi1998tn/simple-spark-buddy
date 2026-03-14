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

	public class UserSkillSyncService : DefaultSyncService<UserSkill, Guid>
	{
		private readonly ISyncService<User> userSyncService;
		public UserSkillSyncService(IRepositoryWithTypedId<UserSkill, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, ISyncService<User> userSyncService)
			: base(repository,
				restTypeProvider,
				restSerializer,
				mapper)
		{
			this.userSyncService = userSyncService;
		}
		public override Type[] ClientSyncDependencies => new[] { typeof(User) };
		public override IQueryable<UserSkill> GetAll(User user, IDictionary<string, int?> groups, IDictionary<string, Guid> clientIds)
		{
			var users = userSyncService.GetAll(user,
				groups,
				clientIds);
			return repository.GetAll()
				.Where(x => users.Any(y => y.Id == x.Username))
				.Where(x => x.ValidTo == null || x.ValidTo >= DateTime.Now);
		}
	}
}
