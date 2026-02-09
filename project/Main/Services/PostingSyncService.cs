namespace Main.Services
{
	using System.Linq;

	using AutoMapper;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Rest;
	using Crm.Library.Services;

	public class PostingSyncService : DefaultSyncService<Posting, int>
	{
		public PostingSyncService(IRepositoryWithTypedId<Posting, int> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper) : base(repository, restTypeProvider, restSerializer, mapper)
		{
		}

		public override IQueryable<Posting> GetAll(User user)
		{
			return repository.GetAll();
		}
	}
}
