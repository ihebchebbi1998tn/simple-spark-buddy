namespace Main.Services
{
	using System;
	using System.Linq;
	using System.Reflection;

	using AutoMapper;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Rest;
	using Crm.Library.Services;
	using Crm.Library.Services.Interfaces;

	using Main.Rest.Model;

	public class UserSyncService : DefaultSyncService<User, string>
	{
		private readonly IUserService userService;

		public UserSyncService(IRepositoryWithTypedId<User, string> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IUserService userService, IMapper mapper)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
			this.userService = userService;
		}
		public override MemberInfo[] Expands => new[] { typeof(UserRest).GetMember(nameof(UserRest.Avatar))[0] };
		public override IQueryable<User> GetAll(User user)
		{
			return userService.GetActiveUsersQuery();
		}
		public override User Save(User entity)
		{
			userService.SaveUser(entity);
			return entity;
		}
		public override void Remove(User entity)
		{
			throw new NotImplementedException();
		}
	}
}
