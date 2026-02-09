using System;
using AutoMapper;
using Crm.Library.Data.Domain.DataInterfaces;
using Crm.Library.Rest;
using Crm.Library.Services;
using Main.Flow.Model;

namespace Main.Flow.Services
{
	public class FlowRuleSyncService : DefaultSyncService<FlowRule, Guid>
	{
		public FlowRuleSyncService(IRepositoryWithTypedId<FlowRule, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper) : base(repository, restTypeProvider, restSerializer, mapper)
		{
		}
	}
}
