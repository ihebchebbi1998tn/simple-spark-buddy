namespace Crm.Service.Services
{
	using System;
	using System.Linq;

	using Crm.Article.Services.Interfaces;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Helper;
	using Crm.Service.Extensions;
	using Crm.Service.Model;
	using Crm.Service.Services.Interfaces;

	public class ServiceOrderTimePostingService : IServiceOrderTimePostingService
	{
		private readonly IRepositoryWithTypedId<ServiceOrderTime, Guid> serviceOrderTimeRepository;
		private readonly IRepositoryWithTypedId<ServiceOrderHead, Guid> serviceOrderHeadRepository;
		private readonly IArticleService articleService;
		private readonly IAppSettingsProvider appSettingsProvider;
		private readonly Func<ServiceOrderTime> serviceOrderTimeFactory;
		private readonly IPositionNumberingService positionNumberingService;
		public ServiceOrderTimePostingService(
			IRepositoryWithTypedId<ServiceOrderTime, Guid> serviceOrderTimeRepository,
			IArticleService articleService,
			Func<ServiceOrderTime> serviceOrderTimeFactory,
			IRepositoryWithTypedId<ServiceOrderHead, Guid> serviceOrderHeadRepository,
			IAppSettingsProvider appSettingsProvider,
			IPositionNumberingService positionNumberingService)
		{
			this.serviceOrderTimeRepository = serviceOrderTimeRepository;
			this.serviceOrderHeadRepository = serviceOrderHeadRepository;
			this.articleService = articleService;
			this.serviceOrderTimeFactory = serviceOrderTimeFactory;
			this.appSettingsProvider = appSettingsProvider;
			this.positionNumberingService = positionNumberingService;
		}
		public virtual void SetOrderTimesId(ServiceOrderTimePosting serviceOrderTimePosting)
		{
			if (!appSettingsProvider.GetValue(ServicePlugin.Settings.ServiceOrder.GenerateAndAttachJobsToUnattachedTimePostings)
				|| !serviceOrderTimePosting.OrderTimesId.GetValueOrDefault().IsDefault()
				|| serviceOrderTimePosting.IsPrePlanned())
			{
				return;
			}

			var serviceOrderTime = serviceOrderTimeRepository.GetAll().FirstOrDefault(x => x.OrderId == serviceOrderTimePosting.OrderId && x.ArticleId == serviceOrderTimePosting.ArticleId && x.InstallationId == null);
			if (serviceOrderTime == null)
			{
				var order = serviceOrderHeadRepository.Get(serviceOrderTimePosting.OrderId);
				var article = serviceOrderTimePosting.ArticleId.HasValue ? articleService.GetArticle(serviceOrderTimePosting.ArticleId.Value) : null;
				serviceOrderTime = serviceOrderTimeFactory();
				serviceOrderTime.ArticleId = article?.Id;
				serviceOrderTime.ItemNo = serviceOrderTimePosting.ItemNo;
				serviceOrderTime.OrderId = serviceOrderTimePosting.OrderId;
				serviceOrderTime.ServiceOrderHead = order;
				serviceOrderTime.TrySetLumpSumData(order);
				serviceOrderTime.Description = article?.Description;
				serviceOrderTime.PosNo = positionNumberingService.GetNextPositionNumber(serviceOrderTimePosting.OrderId);
				serviceOrderTime.Price = article?.Price;
				serviceOrderTime.Id = Guid.NewGuid();
				serviceOrderTimeRepository.SaveOrUpdate(serviceOrderTime);
			}

			serviceOrderTimePosting.OrderTimesId = serviceOrderTime.Id;
		}
	}
}
