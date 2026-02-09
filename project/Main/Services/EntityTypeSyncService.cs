namespace Main.Services
{
	using System;
	using System.Linq;

	using AutoMapper;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services;

	using LMobile.Unicore;

	using User = Crm.Library.Model.User;

	public class EntityTypeSyncService : DefaultSyncService<EntityType, Guid>
	{
		public EntityTypeSyncService(IRepositoryWithTypedId<EntityType, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
		}
		public override IQueryable<EntityType> GetAll(User user)
		{
			return repository.GetAll();
		}
		public override EntityType Save(EntityType entity)
		{
			throw new NotImplementedException();
		}
		public override void Remove(EntityType entity)
		{
			throw new NotImplementedException();
		}
	}
}
