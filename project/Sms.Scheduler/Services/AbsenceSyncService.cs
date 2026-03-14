namespace Sms.Scheduler.Services
{
	using System;

	using AutoMapper;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services;

	using Sms.Scheduler.Model;

	public class AbsenceSyncService : DefaultSyncService<Absence, Guid>
	{
		public AbsenceSyncService(IRepositoryWithTypedId<Absence, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
		}
	}
}
