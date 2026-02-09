namespace Crm.Service.Services
{
	using System;
	using System.Linq;
	using Crm.Article.Model;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Services.Interfaces;
	using Crm.Service.Model;

	public class UsedArticleService : IUsedEntityService<Article>
	{
		private readonly IRepositoryWithTypedId<InstallationPos, Guid> installationPosRepository;
		private readonly IRepositoryWithTypedId<ReplenishmentOrderItem, Guid> replenishmentOrderItemRepository;
		private readonly IRepositoryWithTypedId<ServiceOrderMaterial, Guid> serviceOrderMaterialRepository;
		private readonly IRepositoryWithTypedId<ServiceOrderTime, Guid> serviceOrderTimeRepository;
		private readonly IRepositoryWithTypedId<ServiceOrderTimePosting, Guid> serviceOrderTimePostingRepository;
		public UsedArticleService(IRepositoryWithTypedId<InstallationPos, Guid> installationPosRepository, IRepositoryWithTypedId<ReplenishmentOrderItem, Guid> replenishmentOrderItemRepository, IRepositoryWithTypedId<ServiceOrderMaterial, Guid> serviceOrderMaterialRepository, IRepositoryWithTypedId<ServiceOrderTime, Guid> serviceOrderTimeRepository, IRepositoryWithTypedId<ServiceOrderTimePosting, Guid> serviceOrderTimePostingRepository)
		{
			this.installationPosRepository = installationPosRepository;
			this.replenishmentOrderItemRepository = replenishmentOrderItemRepository;
			this.serviceOrderMaterialRepository = serviceOrderMaterialRepository;
			this.serviceOrderTimeRepository = serviceOrderTimeRepository;
			this.serviceOrderTimePostingRepository = serviceOrderTimePostingRepository;
		}

		public virtual bool IsUsed(Article entity)
		{
			return installationPosRepository.GetAll().Any(x => x.ArticleId == entity.Id) || serviceOrderTimeRepository.GetAll().Any(x => x.ArticleId == entity.Id) || serviceOrderTimePostingRepository.GetAll().Any(x => x.ArticleId == entity.Id);
		}
	}
}
