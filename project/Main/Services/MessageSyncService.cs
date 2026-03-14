namespace Main.Services
{
	using System;

	using AutoMapper;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services;

	using Main.Model;

	public class MessageSyncService : DefaultSyncService<Message, Guid>
	{
		public MessageSyncService(IRepositoryWithTypedId<Message, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
		}
	}
}
