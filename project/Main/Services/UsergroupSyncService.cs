namespace Main.Services
{
	using System;
	using System.Linq;

	using AutoMapper;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Model;
	using Crm.Library.Rest;
	using Crm.Library.Services;

	public class UsergroupSyncService : DefaultSyncService<Usergroup, Guid>
	{
		public UsergroupService usergroupService;
		public UserService userService;

		public UsergroupSyncService(IRepositoryWithTypedId<Usergroup, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, UsergroupService usergroupService, UserService userService)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
			this.usergroupService = usergroupService;
			this.userService = userService;
		}

		public override IQueryable<Usergroup> GetAll(User user)
		{
			return repository.GetAll();
		}
		public override Usergroup Save(Usergroup entity)
		{
			return repository.SaveOrUpdate(entity);
		}
		public override void Remove(Usergroup entity)
		{
			entity.Members.ForEach(
				userUserGroup =>
				{
					userUserGroup.User.RemoveUsergroup(entity);
				});
			entity.Members.Clear();
			repository.Delete(entity);
		}
	}
}
