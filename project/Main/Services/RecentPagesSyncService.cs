namespace Main.Services
{
	using System;
	using System.Linq;

	using AutoMapper;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Rest;
	using Crm.Library.Services;

	public class RecentPagesSyncService : DefaultSyncService<RecentPage, Guid>
    {
        public RecentPagesSyncService(IRepositoryWithTypedId<RecentPage, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper)
            : base(repository, restTypeProvider, restSerializer, mapper)
        {
        }
        public override IQueryable<RecentPage> GetAll(User user)
        {
            return repository.GetAll().Where(x => x.Username == user.Id);
        }
    }
}
