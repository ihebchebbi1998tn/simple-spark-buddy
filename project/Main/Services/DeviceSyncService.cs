namespace Main.Services
{
	using System;
	using System.Linq;

	using AutoMapper;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Rest;
	using Crm.Library.Services;

	public class DeviceSyncService : DefaultSyncService<Device, Guid>
	{
		public DeviceSyncService(IRepositoryWithTypedId<Device, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
		}

		public override Device Save(Device entity)
		{
			return repository.SaveOrUpdate(entity);
		}

		public override IQueryable<Device> GetAll(User user)
		{
			return repository.GetAll().Where(x => x.Username == user.Id);
		}
	}
}
