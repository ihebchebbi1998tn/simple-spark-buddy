namespace Crm.Service.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using AutoMapper;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Library.Rest;
	using Crm.Library.Services;
	using Crm.Library.Services.Interfaces;
	using Crm.PerDiem.Model;
	using Crm.Service.Model;
	using Crm.Service.Rest.Model;
	using Crm.Service.Services.Interfaces;

	using NHibernate.Linq;

	public class ServiceOrderTimePostingSyncService : DefaultSyncService<ServiceOrderTimePosting, ServiceOrderTimePostingRest, Guid>
	{
		private readonly IServiceOrderTimePostingService serviceOrderTimePostingService;
		private readonly IAppSettingsProvider appSettingsProvider;
		private readonly ISyncService<ServiceOrderHead> serviceOrderHeadSyncService;
		private readonly ISyncService<PerDiemReport> perDiemReportSyncService;
		private readonly IRepositoryWithTypedId<ServiceOrderDispatch, Guid> serviceOrderDispatchRepository;
		public ServiceOrderTimePostingSyncService(IRepositoryWithTypedId<ServiceOrderTimePosting, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, IServiceOrderTimePostingService serviceOrderTimePostingService, IAppSettingsProvider appSettingsProvider, ISyncService<ServiceOrderHead> serviceOrderHeadSyncService, ISyncService<PerDiemReport> perDiemReportSyncService, IRepositoryWithTypedId<ServiceOrderDispatch, Guid> serviceOrderDispatchRepository)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
			this.serviceOrderTimePostingService = serviceOrderTimePostingService;
			this.appSettingsProvider = appSettingsProvider;
			this.serviceOrderHeadSyncService = serviceOrderHeadSyncService;
			this.perDiemReportSyncService = perDiemReportSyncService;
			this.serviceOrderDispatchRepository = serviceOrderDispatchRepository;
		}

		public override Type[] SyncDependencies
		{
			get { return new[] { typeof(ServiceOrderHead), typeof(ServiceOrderDispatch), typeof(PerDiemReport) }; }
		}
		public override Type[] ClientSyncDependencies
		{
			get { return new[] { typeof(ServiceOrderHead), typeof(PerDiemReport) }; }
		}
		public override IQueryable<ServiceOrderTimePosting> GetAll(User user, IDictionary<string, int?> groups, IDictionary<string, Guid> clientIds)
		{
			var historySyncPeriod = appSettingsProvider.GetValue(ServicePlugin.Settings.ServiceOrderTimePosting.ClosedTimePostingsHistorySyncPeriod);
			var isClosedEntitiesSince = DateTime.UtcNow.AddDays(-1 * historySyncPeriod);
			var serviceOrders = serviceOrderHeadSyncService.GetAll(user, groups, clientIds);
			var perDiemReports = perDiemReportSyncService.GetAll(user, groups, clientIds);
			return repository.GetAll()
				.Where(
					x =>
						serviceOrders.Any(y => y.Id == x.OrderId) && (!x.IsClosed || x.IsClosed && x.To >= isClosedEntitiesSince) ||
						x.PerDiemReportId.HasValue && perDiemReports.Any(y => y.Id == x.PerDiemReportId.Value));
		}
		public override ServiceOrderTimePosting Save(ServiceOrderTimePosting entity)
		{
			var preplannedPositionWasDeleted = entity.ParentServiceOrderTimePostingId.HasValue && repository.Get(entity.ParentServiceOrderTimePostingId.Value) == null;
			if (preplannedPositionWasDeleted)
			{
				entity.ParentServiceOrderTimePostingId = null;
				entity.ParentServiceOrderTimePostingVersion = null;
			}
			serviceOrderTimePostingService.SetOrderTimesId(entity);
			repository.SaveOrUpdate(entity);
			return entity;
		}
		protected override bool IsStale(ServiceOrderTimePostingRest restEntity)
		{
			if (!restEntity.DispatchId.HasValue)
			{
				return false;
			}

			var dispatch = serviceOrderDispatchRepository.Get(restEntity.DispatchId.Value);
			if (dispatch == null)
			{
				return true;
			}

			return false;
		}
		public override IQueryable<ServiceOrderTimePosting> Eager(IQueryable<ServiceOrderTimePosting> entities)
		{
			return entities.Fetch(x => x.ServiceOrderHead);
		}
	}
}
