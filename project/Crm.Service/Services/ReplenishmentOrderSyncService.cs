namespace Crm.Service.Services
{
	using System;
	using System.Linq;
	using AutoMapper;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.Interfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services;
	using Crm.Service.Model;

	public class ReplenishmentOrderSyncService : DefaultSyncService<ReplenishmentOrder, Guid>
	{
		private readonly IAppSettingsProvider appSettingsProvider;
		private readonly IAuthorizationManager authorizationManager;
		public ReplenishmentOrderSyncService(IRepositoryWithTypedId<ReplenishmentOrder, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, IAppSettingsProvider appSettingsProvider, IAuthorizationManager authorizationManager)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
			this.appSettingsProvider = appSettingsProvider;
			this.authorizationManager = authorizationManager;
		}
		public override IQueryable<ReplenishmentOrder> GetAll(User user)
		{
			var historySyncPeriod = appSettingsProvider.GetValue(ServicePlugin.Settings.ReplenishmentOrder.ClosedReplenishmentOrderHistorySyncPeriod);
			var historySince = DateTime.UtcNow.AddDays(-1 * historySyncPeriod);
			var query = repository.GetAll();
			query = authorizationManager.IsAuthorizedForAction(user, ServicePlugin.PermissionGroup.ReplenishmentOrder, ServicePlugin.PermissionName.SeeClosedReplenishmentOrders) ? query.Where(x => !x.IsClosed || x.CloseDate >= historySince) : query.Where(x => !x.IsClosed);
			if (!authorizationManager.IsAuthorizedForAction(user, ServicePlugin.PermissionGroup.ReplenishmentOrder, ServicePlugin.PermissionName.ReplenishmentsFromOtherUsersSelectable))
			{
				query = query.Where(x => x.ResponsibleUser == user.Id);
			}

			return query;
		}
		public override ReplenishmentOrder Save(ReplenishmentOrder entity)
		{
			return repository.SaveOrUpdate(entity);
		}
		public override void Remove(ReplenishmentOrder entity)
		{
			repository.Delete(entity);
		}
	}
}