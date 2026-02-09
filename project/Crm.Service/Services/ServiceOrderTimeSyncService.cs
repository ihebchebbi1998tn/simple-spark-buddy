namespace Crm.Service.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Rest;
	using Crm.Library.Services;
	using Crm.Service.Model;

	using NHibernate.Linq;
	using Interfaces;
	using Article.Services.Interfaces;

	using AutoMapper;

	using Crm.Library.Services.Interfaces;

	public class ServiceOrderTimeSyncService : DefaultSyncService<ServiceOrderTime, Guid>
	{
		private readonly IPositionNumberingService positionNumberingService;
		private readonly IArticleService articleService;
		private readonly ISyncService<ServiceOrderHead> serviceOrderHeadSyncService;
		public ServiceOrderTimeSyncService(IRepositoryWithTypedId<ServiceOrderTime, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IPositionNumberingService positionNumberingService, IArticleService articleService, IMapper mapper, ISyncService<ServiceOrderHead> serviceOrderHeadSyncService)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
			this.positionNumberingService = positionNumberingService;
			this.articleService = articleService;
			this.serviceOrderHeadSyncService = serviceOrderHeadSyncService;
		}
		public override Type[] SyncDependencies
		{
			get { return new[] { typeof(ServiceOrderHead) }; }
		}
		public override Type[] ClientSyncDependencies
		{
			get { return new[] { typeof(ServiceOrderHead) }; }
		}
		public override IQueryable<ServiceOrderTime> GetAll(User user, IDictionary<string, int?> groups, IDictionary<string, Guid> clientIds)
		{
			var serviceOrders = serviceOrderHeadSyncService.GetAll(user, groups, clientIds);
			return repository.GetAll()
				.Where(x => serviceOrders.Any(y => y.Id == x.OrderId));
		}
		public override ServiceOrderTime Save(ServiceOrderTime entity)
		{
			var article = entity.ArticleId.HasValue ? articleService.GetArticle(entity.ArticleId.Value) : null;
			if (string.IsNullOrWhiteSpace(entity.PosNo))
			{
				entity.PosNo = repository.Get(entity.Id)?.PosNo;
			}
			if (string.IsNullOrWhiteSpace(entity.PosNo))
			{
				entity.PosNo = GetNewPosNo(entity);
			}
			entity.Description = entity.Description ?? article?.Description ?? entity.Installation?.FullDescription ?? String.Empty;
			entity.ItemNo = entity.ItemNo ?? article?.ItemNo ?? String.Empty;
			entity.Price = entity.Price ?? article?.Price;
			return base.Save(entity);
		}
		protected virtual string GetNewPosNo(ServiceOrderTime entity)
		{
			var newPosNo = positionNumberingService.GetNextPositionNumber(entity.OrderId);
			return newPosNo;
		}

		public override IQueryable<ServiceOrderTime> Eager(IQueryable<ServiceOrderTime> entities)
		{
			entities = entities
				.Fetch(x => x.Installation)
				.Fetch(x => x.ServiceOrderHead);
			return entities;
		}
	}
}
