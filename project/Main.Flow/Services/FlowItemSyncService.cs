using System.Linq;
using AutoMapper;
using Crm.Library.Data.Domain.DataInterfaces;
using Crm.Library.Model;
using Crm.Library.Rest;
using Crm.Library.Services;
using Main.Flow.Model;

namespace Main.Flow.Services
{
	public class FlowItemSyncService : DefaultSyncService<FlowItem, int>
	{
		public FlowItemSyncService(IRepositoryWithTypedId<FlowItem, int> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper) : base(repository, restTypeProvider, restSerializer, mapper)
		{
		}

		public override IQueryable<FlowItem> GetAll(User user)
		{
			return repository.GetAll();
		}
	}
}
