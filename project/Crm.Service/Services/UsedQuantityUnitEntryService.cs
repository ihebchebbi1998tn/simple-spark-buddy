namespace Crm.Service.Services
{
	using System;
	using System.Linq;
	using Crm.Article.Model;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Services.Interfaces;
	using Crm.Service.Model;

	public class UsedQuantityUnitEntryService : IUsedEntityService<QuantityUnitEntry>
	{
		private readonly IRepositoryWithTypedId<InstallationPos, Guid> installationPosRepository;
		private readonly IRepositoryWithTypedId<ReplenishmentOrderItem, Guid> replenishmentOrderItemRepository;
		private readonly IRepositoryWithTypedId<ServiceOrderMaterial, Guid> serviceOrderMaterialRepository;
		public UsedQuantityUnitEntryService(IRepositoryWithTypedId<InstallationPos, Guid> installationPosRepository, IRepositoryWithTypedId<ReplenishmentOrderItem, Guid> replenishmentOrderItemRepository, IRepositoryWithTypedId<ServiceOrderMaterial, Guid> serviceOrderMaterialRepository)
		{
			this.installationPosRepository = installationPosRepository;
			this.replenishmentOrderItemRepository = replenishmentOrderItemRepository;
			this.serviceOrderMaterialRepository = serviceOrderMaterialRepository;
		}

		public virtual bool IsUsed(QuantityUnitEntry entity)
		{
			return installationPosRepository.GetAll().Any(x => x.QuantityUnitEntryKey == entity.Id) || replenishmentOrderItemRepository.GetAll().Any(x => x.QuantityUnitEntryKey == entity.Id) || serviceOrderMaterialRepository.GetAll().Any(x => x.QuantityUnitEntryKey == entity.Id);
		}
	}
}
