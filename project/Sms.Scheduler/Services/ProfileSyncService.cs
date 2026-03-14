namespace Sms.Scheduler.Services
{
	using System;

	using AutoMapper;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services;
	using Crm.Library.Services.Interfaces;

	using Newtonsoft.Json;

	using Sms.Scheduler.Model;

	using Profile = Sms.Scheduler.Model.Profile;
	public class ProfileSyncService : DefaultSyncService<Profile, int>
	{
		private readonly IUserService userService;
		private readonly ProfileService profileService;
		public ProfileSyncService(
				IRepositoryWithTypedId<Profile, int> repository,
				RestTypeProvider restTypeProvider,
				IRestSerializer restSerializer,
				IMapper mapper,
				IUserService userService,
				ProfileService profileService)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
			this.userService = userService;
			this.profileService = profileService;
		}
		public override Profile Save(Profile entity)
		{
			entity = repository.SaveOrUpdate(entity);
			if (entity.DefaultProfile)
			{
				profileService.ResetDefaultProfile(entity.Username, entity.Id);
			}
			
			return entity;
		}
		public override void Remove(Profile entity)
		{
			profileService.RemoveRelatedResources(entity);
			repository.Delete(entity);
		} 
	}
}
