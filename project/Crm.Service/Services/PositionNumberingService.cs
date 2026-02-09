namespace Crm.Service.Services
{
	using System;
	using System.Globalization;
	using System.Linq;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Service.Model;
	using Crm.Service.Model.Extensions;
	using Crm.Service.SearchCriteria;
	using Crm.Service.Services.Interfaces;

	public class PositionNumberingService : IPositionNumberingService
	{
		private const int MaxNumber = int.MaxValue;
		private readonly IRepositoryWithTypedId<ServiceOrderMaterial, Guid> serviceOrderMaterialRepository;
		private readonly IRepositoryWithTypedId<ServiceOrderTime, Guid> serviceOrderTimeRepository;

		public PositionNumberingService(IRepositoryWithTypedId<ServiceOrderMaterial, Guid> serviceOrderMaterialRepository, IRepositoryWithTypedId<ServiceOrderTime, Guid> serviceOrderTimeRepository)
		{
			this.serviceOrderMaterialRepository = serviceOrderMaterialRepository;
			this.serviceOrderTimeRepository = serviceOrderTimeRepository;
		}

		public virtual string GetNextPositionNumber(Guid orderId)
		{
			var materialList = serviceOrderMaterialRepository.GetAll().Filter(new ServiceOrderMaterialSearchCriteria { OrderId = orderId, SortBy = "PosNo", SortOrder = "ascending" }).Select(x => x.PosNo).ToList();
			var timeList = serviceOrderTimeRepository.GetAll().Filter(new ServiceOrderTimeSearchCriteria { OrderId = orderId, SortBy = "PosNo", SortOrder = "ascending" }).Select(x => x.PosNo).ToList();
			
			var existingPositionNumbers = materialList.Concat(timeList).ToArray();
			var nextPositionNumber = 1;

			if (existingPositionNumbers.Any())
			{
				var maxNumber = existingPositionNumbers.Max(x => int.TryParse(x.Split('.')[0], out var number) ? number : 0);

				if (maxNumber >= MaxNumber)
				{
					throw new ApplicationException(string.Format("Position Number would exceed maximum of {0}", MaxNumber));
				}

				nextPositionNumber = maxNumber + 1;
			}

			return nextPositionNumber.ToString(CultureInfo.InvariantCulture).PadLeft(4, '0');
		}
	}
}
