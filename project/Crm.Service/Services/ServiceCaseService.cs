using Crm.Library.Data.Domain.DataInterfaces;
using Crm.Service.Model;
using Crm.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Crm.Service.Services
{
	public class ServiceCaseService : IServiceCaseService
	{
		private readonly IRepositoryWithTypedId<ServiceCase, Guid> serviceCaseRepository;

		public ServiceCaseService(IRepositoryWithTypedId<ServiceCase, Guid> serviceCaseRepository)
		{
			this.serviceCaseRepository = serviceCaseRepository;
		}

		public virtual IEnumerable<string> GetUsedErrorCodes()
		{
			return serviceCaseRepository.GetAll().Select(c => c.ErrorCodeKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedServiceCaseCategories()
		{
			return serviceCaseRepository.GetAll().Select(c => c.CategoryKey).Distinct();
		}

		public virtual IEnumerable<int> GetUsedServiceCaseStatuses()
		{
			return serviceCaseRepository.GetAll().Select(c => c.StatusKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedServicePriorities()
		{
			return serviceCaseRepository.GetAll().Select(c => c.PriorityKey).Distinct();
		}
	}
}
