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
	using NHibernate.Linq;

	public class ReplenishmentOrderItemSyncService : DefaultSyncService<ReplenishmentOrderItem, Guid>
	{
		private readonly ISyncService<ReplenishmentOrder> replenishmentOrderSyncService;
		public ReplenishmentOrderItemSyncService(IRepositoryWithTypedId<ReplenishmentOrderItem, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, ISyncService<ReplenishmentOrder> replenishmentOrderSyncService)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
			this.replenishmentOrderSyncService = replenishmentOrderSyncService;
		}

		public override Type[] SyncDependencies
		{
			get { return new[] { typeof(ReplenishmentOrder) }; }
		}
		public override Type[] ClientSyncDependencies
		{
			get { return new[] { typeof(ReplenishmentOrder) }; }
		}

		public override IQueryable<ReplenishmentOrderItem> GetAll(User user, IDictionary<string, int?> groups, IDictionary<string, Guid> clientIds)
		{
			var replenishmentOrders = replenishmentOrderSyncService.GetAll(user, groups, clientIds);
			return repository.GetAll()
				.Where(x => replenishmentOrders.Any(y => y.Id == x.ReplenishmentOrderId));
		}
		public override ReplenishmentOrderItem Save(ReplenishmentOrderItem entity)
		{
			return repository.SaveOrUpdate(entity);
		}
		public override void Remove(ReplenishmentOrderItem entity)
		{
			repository.Delete(entity);
		}
		public override IQueryable<ReplenishmentOrderItem> Eager(IQueryable<ReplenishmentOrderItem> entities)
		{
			return entities.Fetch(x => x.ReplenishmentOrder);
		}
	}
}
