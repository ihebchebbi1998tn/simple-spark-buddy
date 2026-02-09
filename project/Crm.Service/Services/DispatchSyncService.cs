namespace Crm.Service.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using AutoMapper;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.Interfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services;
	using Crm.Library.Services.Interfaces;
	using Crm.Service.Model;
	using Crm.Service.Model.Helpers;
	using Crm.Service.Model.Lookup;
	using Crm.Service.Rest.Model;

	using Newtonsoft.Json;

	using NHibernate.Linq;

	public class DispatchSyncService : DefaultSyncService<ServiceOrderDispatch, ServiceOrderDispatchRest, Guid>
	{
		private readonly ILookupManager lookupManager;
		private readonly IServiceOrderDispatchStatusEvaluator serviceOrderDispatchStatusEvaluator;
		private readonly ISyncService<ServiceOrderHead> serviceOrderHeadSyncService;
		private readonly IAuthorizationManager authorizationManager;

		public DispatchSyncService(IRepositoryWithTypedId<ServiceOrderDispatch, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, ILookupManager lookupManager, IServiceOrderDispatchStatusEvaluator serviceOrderDispatchStatusEvaluator, IMapper mapper, ISyncService<ServiceOrderHead> serviceOrderHeadSyncService, IAuthorizationManager authorizationManager)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
			this.lookupManager = lookupManager;
			this.serviceOrderDispatchStatusEvaluator = serviceOrderDispatchStatusEvaluator;
			this.serviceOrderHeadSyncService = serviceOrderHeadSyncService;
			this.authorizationManager = authorizationManager;
		}
		public override Type[] SyncDependencies
		{
			get { return new[] { typeof(ServiceOrderHead), typeof(ServiceOrderTime) }; }
		}
		public override Type[] ClientSyncDependencies => new[] { typeof(ServiceOrderHead) };
		public override IQueryable<ServiceOrderDispatch> GetAll(User user, IDictionary<string, int?> groups, IDictionary<string, Guid> clientIds)
		{
			var serviceOrders = serviceOrderHeadSyncService.GetAll(user, groups, clientIds);
			return repository.GetAll()
				.Where(x => (authorizationManager.IsAuthorizedForAction(user, ServicePlugin.PermissionGroup.Dispatch, ServicePlugin.PermissionName.SeeAllUsersDispatches) || x.DispatchedUsername == user.Id) && serviceOrders.Any(y => y.Id == x.OrderId));
		}
		public override ServiceOrderDispatch Save(ServiceOrderDispatch entity)
		{
			if (entity.IsInProgress())
			{
				entity.SignatureContactName = null;
				entity.SignatureJson = null;
				entity.SignatureDate = null;
			}
			repository.SaveOrUpdate(entity);
			return entity;
		}
		public override bool IsRelated(string serializedEntity, string serializedOtherEntity)
		{
			dynamic entity = JsonConvert.DeserializeObject(serializedEntity);
			dynamic otherEntity = JsonConvert.DeserializeObject(serializedOtherEntity);
			return entity.Id == otherEntity.DispatchId;
		}
		protected override bool IsStale(ServiceOrderDispatchRest restEntity)
		{
			var persistedDispatch = repository.Get(restEntity.Id);

			if (persistedDispatch == null)
			{
				return false;
			}

			var isStaleServiceOrder = false;
			if (persistedDispatch.OrderHead != null)
			{
				var serviceOrderStatus = lookupManager.Get<ServiceOrderStatus>(persistedDispatch.OrderHead.StatusKey);
				isStaleServiceOrder = serviceOrderStatus.BelongsToPostProcessing() || serviceOrderStatus.BelongsToClosed();
			}

			if (isStaleServiceOrder)
			{
				return true;
			}

			var isStaleDispatch = !serviceOrderDispatchStatusEvaluator.IsStatusTransitionAllowed(persistedDispatch.StatusKey, restEntity.StatusKey);

			return isStaleDispatch;
		}
		public override IQueryable<ServiceOrderDispatch> Eager(IQueryable<ServiceOrderDispatch> entities)
		{
			return entities.Fetch(x => x.OrderHead);
		}
	}
}
