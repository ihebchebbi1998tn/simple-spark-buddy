namespace Crm.Service.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using AutoMapper;

	using Crm.Article.Model;
	using Crm.Article.Services.Interfaces;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Rest;
	using Crm.Library.Services;
	using Crm.Library.Services.Interfaces;
	using Crm.Service.Model;
	using Crm.Service.Rest.Model;
	using Crm.Service.Services.Interfaces;

	using NHibernate.Linq;

	public class ServiceOrderMaterialSyncService : DefaultSyncService<ServiceOrderMaterial, ServiceOrderMaterialRest, Guid>
	{
		private readonly IArticleService articleService;
		private readonly IPositionNumberingService positionNumberingService;
		private readonly ISyncService<ServiceOrderHead> serviceOrderHeadSyncService;
		private readonly IRepositoryWithTypedId<ServiceOrderDispatch, Guid> serviceOrderDispatchRepository;
		private readonly IRepositoryWithTypedId<QuantityUnitEntry, Guid> quantityUnitEntryRepository;

		public ServiceOrderMaterialSyncService(IRepositoryWithTypedId<ServiceOrderMaterial, Guid> repository,
			RestTypeProvider restTypeProvider,
			IRestSerializer restSerializer,
			IArticleService articleService,
			IPositionNumberingService positionNumberingService,
			IMapper mapper,
			ISyncService<ServiceOrderHead> serviceOrderHeadSyncService,
			IRepositoryWithTypedId<ServiceOrderDispatch, Guid> serviceOrderDispatchRepository,
			IRepositoryWithTypedId<QuantityUnitEntry, Guid> quantityUnitEntryRepository)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
			this.articleService = articleService;
			this.positionNumberingService = positionNumberingService;
			this.serviceOrderHeadSyncService = serviceOrderHeadSyncService;
			this.serviceOrderDispatchRepository = serviceOrderDispatchRepository;
			this.quantityUnitEntryRepository = quantityUnitEntryRepository;
		}
		public override Type[] SyncDependencies
		{
			get { return new[] { typeof(ServiceOrderHead), typeof(ServiceOrderDispatch), typeof(ServiceOrderTime) }; }
		}
		public override Type[] ClientSyncDependencies => new[] { typeof(ServiceOrderHead) };
		public override IQueryable<ServiceOrderMaterial> GetAll(User user, IDictionary<string, int?> groups, IDictionary<string, Guid> clientIds)
		{
			var serviceOrders = serviceOrderHeadSyncService.GetAll(user, groups, clientIds);
			return repository.GetAll()
				.Where(x => serviceOrders.Any(y => y.Id == x.OrderId));
		}
		public override ServiceOrderMaterial Save(ServiceOrderMaterial entity)
		{
			var article = entity.ArticleId.HasValue ? articleService.GetArticle(entity.ArticleId.Value) : null;
			if (article != null)
			{
				entity.IsSerial = article.IsSerial;
				entity.ArticleTypeKey = article.ArticleTypeKey;
				if (entity.QuantityUnitEntryKey != null)
				{
					var quantityUnitEntry = quantityUnitEntryRepository.Get(entity.QuantityUnitEntryKey.Value);
					entity.QuantityUnitKey = quantityUnitEntry.QuantityUnitKey;
				}
			}
			if (entity.QuantityUnitEntryKey == null && entity.QuantityUnitKey != null && article != null && article.QuantityUnitEntryKey != null)
			{
				var quantityUnitEntry = quantityUnitEntryRepository.GetAll().FirstOrDefault(x => x.QuantityUnitGroupKey == entity.QuantityUnitEntryKey && x.QuantityUnitKey == entity.QuantityUnitKey);
				article.QuantityUnitEntryKey = quantityUnitEntry?.Id;
			}
			if (string.IsNullOrWhiteSpace(entity.PosNo))
			{
				entity.PosNo = positionNumberingService.GetNextPositionNumber(entity.OrderId);
			}
			entity.FromLocation = entity.FromLocation;
			entity.FromWarehouse = entity.FromWarehouse;
			entity.Price = entity.Price.HasValue || article == null ? entity.Price : article.Price;

			var preplannedPositionWasDeleted = entity.ParentServiceOrderMaterialId.HasValue && repository.Get(entity.ParentServiceOrderMaterialId.Value) == null;
			if (preplannedPositionWasDeleted)
			{
				entity.ParentServiceOrderMaterialId = null;
				entity.ParentServiceOrderMaterialVersion = null;
			}

			return base.Save(entity);
		}

		protected override bool IsStale(ServiceOrderMaterialRest restEntity)
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
		public override IQueryable<ServiceOrderMaterial> Eager(IQueryable<ServiceOrderMaterial> entities)
		{
			return entities.Fetch(x => x.ServiceOrderHead);
		}
	}
}
