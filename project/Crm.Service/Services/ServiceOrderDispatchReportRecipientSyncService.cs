namespace Crm.Service.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using AutoMapper;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Rest;
	using Crm.Library.Services;
	using Crm.Library.Services.Interfaces;
	using Crm.Service.Model;

	public class ServiceOrderDispatchReportRecipientSyncService : DefaultSyncService<ServiceOrderDispatchReportRecipient, Guid>
	{
		private readonly ISyncService<ServiceOrderDispatch> dispatchSyncService;
		public ServiceOrderDispatchReportRecipientSyncService(IRepositoryWithTypedId<ServiceOrderDispatchReportRecipient, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, ISyncService<ServiceOrderDispatch> dispatchSyncService)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
			this.dispatchSyncService = dispatchSyncService;
		}
		public override Type[] SyncDependencies
		{
			get { return new[] { typeof(ServiceOrderDispatch) }; }
		}
		public override Type[] ClientSyncDependencies
		{
			get { return new[] { typeof(ServiceOrderDispatch) }; }
		}
		public override IQueryable<ServiceOrderDispatchReportRecipient> GetAll(User user, IDictionary<string, int?> groups, IDictionary<string, Guid> clientIds)
		{
			var dispatches = dispatchSyncService.GetAll(user, groups, clientIds);
			return repository.GetAll().Where(x => dispatches.Any(y => y.Id == x.DispatchId));
		}
	}
}
