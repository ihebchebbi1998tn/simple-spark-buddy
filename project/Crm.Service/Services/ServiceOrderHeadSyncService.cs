namespace Crm.Service.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using AutoMapper;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Library.Rest;
	using Crm.Library.Services;
	using Crm.Library.Services.Interfaces;
	using Crm.Model;
	using Crm.Service.Model;
	using Crm.Service.Model.Lookup;
	using Crm.Service.Rest.Model;
	using Crm.Services.Interfaces;

	using Main.Replication.Services;

	using Main.Model.Lookups;

	using NHibernate.Linq;
	using ServiceObject = Model.ServiceObject;

	public class ServiceOrderHeadSyncService : DefaultSyncService<ServiceOrderHead, ServiceOrderHeadRest, Guid>, IContactSyncService
	{
		private readonly ILookupManager lookupManager;
		private readonly IVisibilityProvider visibilityProvider;
		private readonly IAppSettingsProvider appSettingsProvider;
		public static string ClosedServiceOrderHistory = "ClosedServiceOrderHistory";
		public ServiceOrderHeadSyncService(
			IRepositoryWithTypedId<ServiceOrderHead, Guid> repository,
			RestTypeProvider restTypeProvider,
			IRestSerializer restSerializer,
			ILookupManager lookupManager,
			IMapper mapper,
			IVisibilityProvider visibilityProvider,
			IAppSettingsProvider appSettingsProvider,
			IReplicationService<ServiceOrderHead, Guid> replicationService = null)
			: base(repository, restTypeProvider, restSerializer, mapper, replicationService)
		{
			this.lookupManager = lookupManager;
			this.visibilityProvider = visibilityProvider;
			this.appSettingsProvider = appSettingsProvider;
		}

		protected virtual string[] SyncedStatusKeys => new[]
		{
			ServiceOrderStatus.NewKey,
			ServiceOrderStatus.ReadyForSchedulingKey,
			ServiceOrderStatus.ScheduledKey,
			ServiceOrderStatus.PartiallyReleasedKey,
			ServiceOrderStatus.ReleasedKey,
			ServiceOrderStatus.InProgressKey,
			ServiceOrderStatus.PartiallyCompletedKey,
			ServiceOrderStatus.CompletedKey,
			ServiceOrderStatus.PostProcessingKey,
			ServiceOrderStatus.ReadyForInvoiceKey,
			ServiceOrderStatus.InvoicedKey
		};

		public override Type[] SyncDependencies => new[] 
		{
			typeof(Company),
			typeof(Person),
			typeof(ServiceObject),
			typeof(Installation)
		};

		public override IQueryable<ServiceOrderHead> GetAll(User user, IDictionary<string, int?> groups, IDictionary<string, Guid> clientIds)
		{
			var historySyncPeriod = groups == null ? 0 : ((groups.ContainsKey(ClosedServiceOrderHistory) ? groups[ClosedServiceOrderHistory] : null) ?? lookupManager.Get<ReplicationGroup>(ClosedServiceOrderHistory)?.DefaultValue ?? 0);
			var historySince = DateTime.UtcNow.AddDays(-1 * historySyncPeriod);
			var query = repository
					.GetAll()
					.Where(x => x.IsTemplate || (!x.Closed.HasValue && SyncedStatusKeys.Contains(x.StatusKey)) || (x.Closed.HasValue && (historySyncPeriod == 0 || x.Closed.Value >= historySince)));
			return visibilityProvider.FilterByVisibility(query);
		}
		protected override bool IsStale(ServiceOrderHeadRest serviceOrderHeadRest)
		{
			var persistedServiceOrderHead = repository.Get(serviceOrderHeadRest.Id);
			if (persistedServiceOrderHead == null)
			{
				return false;
			}
			var serviceOrderHeadRestStatus = lookupManager.Get<ServiceOrderStatus>(serviceOrderHeadRest.StatusKey);
			var persistedServiceOrderStatus = lookupManager.Get<ServiceOrderStatus>(persistedServiceOrderHead.StatusKey);

			return persistedServiceOrderStatus.SortOrder > serviceOrderHeadRestStatus.SortOrder;
		}
		public override IQueryable<ServiceOrderHead> Eager(IQueryable<ServiceOrderHead> entities)
		{
			entities = entities
						.Fetch(x => x.CustomerContact)
						.Fetch(x => x.ServiceCase)
						.Fetch(x => x.ServiceObject)
						.Fetch(x => x.AffectedInstallation)
						.Fetch(x => x.UserGroup)
						.Fetch(x => x.PreferredTechnicianUsergroup);
			return entities;
		}
		public virtual IQueryable<Guid> GetAllContactIds(User user, IDictionary<string, int?> groups, IDictionary<string, Guid> clientIds)
		{
			return clientIds != null ? replicationService.GetReplicatedEntityIds(clientIds.FirstOrDefault(x => x.Key == nameof(ServiceOrderHead)).Value) : GetAll(user, groups, null).Select(x => x.Id);
		}
	}
}
