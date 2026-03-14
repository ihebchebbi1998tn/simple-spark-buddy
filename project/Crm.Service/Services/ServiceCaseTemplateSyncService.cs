namespace Crm.Service.Services
{
	using System;
	using System.Linq;

	using AutoMapper;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Rest;
	using Crm.Library.Services;
	using Crm.Service.Model;

	public class ServiceCaseTemplateSyncService : DefaultSyncService<ServiceCaseTemplate, Guid>
	{
		public ServiceCaseTemplateSyncService(IRepositoryWithTypedId<ServiceCaseTemplate, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
		}
		public override IQueryable<ServiceCaseTemplate> GetAll(User user)
		{
			return repository.GetAll();
		}
	}
}