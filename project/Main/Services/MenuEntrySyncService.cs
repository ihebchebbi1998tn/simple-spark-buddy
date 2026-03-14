namespace Main.Services
{
	using AutoMapper;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services;

	using Main.Model;

	public class MenuEntrySyncService : DefaultSyncService<MenuEntry>
	{
		public MenuEntrySyncService(
			IRepositoryWithTypedId<MenuEntry, int> repository,
			RestTypeProvider restTypeProvider,
			IRestSerializer restSerializer,
			IMapper mapper)
			: base(repository,
				restTypeProvider,
				restSerializer,
				mapper)
		{
		}
	}
}
