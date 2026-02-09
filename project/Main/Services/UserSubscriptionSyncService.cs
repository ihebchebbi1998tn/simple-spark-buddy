namespace Main.Services
{
	using System;
	using System.Linq;

	using AutoMapper;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Rest;
	using Crm.Library.Services;

	using Main.Model;

	public class UserSubscriptionSyncService : DefaultSyncService<UserSubscription, Guid>
	{
		public UserSubscriptionSyncService(IRepositoryWithTypedId<UserSubscription, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
		}

		public override UserSubscription Save(UserSubscription entity)
		{
			return repository.SaveOrUpdate(entity);
		}

		public override IQueryable<UserSubscription> GetAll(User user)
		{
			return repository.GetAll().Where(x => x.Username == user.Id);
		}
	}
}
