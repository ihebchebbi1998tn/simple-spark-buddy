using Crm.Library.Data.Domain.DataInterfaces;
using Crm.Service.Model;
using Crm.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Crm.Service.Services
{
	public class ServiceOrderTimeService : IServiceOrderTimeService
	{
		private readonly IRepositoryWithTypedId<ServiceOrderTime, Guid> serviceOrderTimeRepository;

		public ServiceOrderTimeService(IRepositoryWithTypedId<ServiceOrderTime, Guid> serviceOrderTimeRepository)
		{
			this.serviceOrderTimeRepository = serviceOrderTimeRepository;
		}

		public virtual IEnumerable<string> GetUsedNoCausingItemPreviousSerialNoReasons()
		{
			return serviceOrderTimeRepository.GetAll().Select(c => c.NoCausingItemPreviousSerialNoReasonKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedNoCausingItemSerialNoReasons()
		{
			return serviceOrderTimeRepository.GetAll().Select(c => c.NoCausingItemSerialNoReasonKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedServiceOrderTimeCategories()
		{
			return serviceOrderTimeRepository.GetAll().Select(c => c.CategoryKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedServiceOrderTimeLocations()
		{
			return serviceOrderTimeRepository.GetAll().Select(c => c.LocationKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedServiceOrderTimePriorities()
		{
			return serviceOrderTimeRepository.GetAll().Select(c => c.PriorityKey).Distinct();
		}
	}
}
