using Crm.Library.Data.Domain.DataInterfaces;
using Crm.Service.Model;
using Crm.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Crm.Service.Services
{
	public class ServiceObjectService : IServiceObjectService
	{
		private readonly IRepositoryWithTypedId<ServiceObject, Guid> serviceObjectRepository;

		public ServiceObjectService(IRepositoryWithTypedId<ServiceObject, Guid> serviceObjectRepository)
		{
			this.serviceObjectRepository = serviceObjectRepository;
		}

		public virtual IEnumerable<string> GetUsedServiceObjectCategories()
		{
			return serviceObjectRepository.GetAll().Select(c => c.CategoryKey).Distinct();
		}

		public virtual ServiceObject GetServiceObject(Guid serviceObjectId)
		{
			return serviceObjectRepository.Get(serviceObjectId);
		}
	}
}
