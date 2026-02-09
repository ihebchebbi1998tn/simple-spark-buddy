namespace Sms.Scheduler.Services
{
	using AutoMapper;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services;

	using Sms.Scheduler.Model;

	public class ProfileResourceSyncService : DefaultSyncService<ProfileResource, int>
	{
		public ProfileResourceSyncService(
				IRepositoryWithTypedId<ProfileResource, int> repository,
				RestTypeProvider restTypeProvider,
				IRestSerializer restSerializer,
				IMapper mapper)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
		}
		public override ProfileResource Save(ProfileResource entity) => repository.SaveOrUpdate(entity);
		public override void Remove(ProfileResource entity) => repository.Delete(entity);
	}
}
