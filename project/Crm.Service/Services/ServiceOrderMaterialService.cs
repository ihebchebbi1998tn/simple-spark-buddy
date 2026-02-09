using Crm.Library.Data.Domain.DataInterfaces;
using Crm.Service.Model;
using Crm.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Crm.Service.Services
{
	public class ServiceOrderMaterialService : IServiceOrderMaterialService
	{
		private readonly IRepositoryWithTypedId<ServiceOrderMaterial, Guid> serviceOrderMaterialRepository;

		public ServiceOrderMaterialService(IRepositoryWithTypedId<ServiceOrderMaterial, Guid> serviceOrderMaterialRepository)
		{
			this.serviceOrderMaterialRepository = serviceOrderMaterialRepository;
		}

		public virtual IEnumerable<string> GetUsedCommissioningStatuses()
		{
			return serviceOrderMaterialRepository.GetAll().Select(c => c.CommissioningStatusKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedNoPreviousSerialNoReasons()
		{
			return serviceOrderMaterialRepository.GetAll().Select(c => c.NoPreviousSerialNoReasonKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedQuantityUnits()
		{
			return serviceOrderMaterialRepository.GetAll().Select(c => c.QuantityUnitKey).Distinct();
		}
	}
}
