namespace Crm.Service.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using AutoMapper;

	using Crm.Article.Services.Interfaces;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Rest;
	using Crm.Library.Services;
	using Crm.Library.Services.Interfaces;
	using Crm.Service.Model;
	using Crm.Service.Services.Interfaces;

	public class ServiceOrderTimeDispatchSyncService : DefaultSyncService<ServiceOrderTimeDispatch, Guid>
	{
		private readonly ISyncService<ServiceOrderDispatch> serviceOrderDispatchSyncService;
		public ServiceOrderTimeDispatchSyncService(IRepositoryWithTypedId<ServiceOrderTimeDispatch, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IPositionNumberingService positionNumberingService, IArticleService articleService, IMapper mapper, ISyncService<ServiceOrderDispatch> serviceOrderDispatchSyncService)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
			this.serviceOrderDispatchSyncService = serviceOrderDispatchSyncService;
		}
		public override Type[] SyncDependencies
		{
			get { return new[] { typeof(ServiceOrderDispatch) }; }
		}
		public override IQueryable<ServiceOrderTimeDispatch> GetAll(User user, IDictionary<string, int?> groups, IDictionary<string, Guid> clientIds)
		{
			var serviceOrderDispatches = serviceOrderDispatchSyncService.GetAll(user, groups, clientIds);
			return repository.GetAll()
				.Where(x => serviceOrderDispatches.Any(y => y.Id == x.ServiceOrderDispatchId));
		}
	}
}
