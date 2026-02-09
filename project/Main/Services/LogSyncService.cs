namespace Main.Services
{
	using AutoMapper;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services;
	using Crm.Library.Services.Interfaces;

	using Main.Model;

	public class LogSyncService : DefaultSyncService<Log, int>
	{
		public LogSyncService(
				IRepositoryWithTypedId<Log, int> repository,
				RestTypeProvider restTypeProvider,
				IRestSerializer restSerializer,
				IMapper mapper,
				IVisibilityProvider visibilityProvider)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
		}
		public override void Remove(Log entity)
		{
			repository.Delete(entity);
		}
	}
}
