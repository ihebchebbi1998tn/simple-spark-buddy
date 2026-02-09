using Crm.Library.Data.Domain.DataInterfaces;
using Crm.Service.Model;
using Crm.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Crm.Service.Services
{
	public class ServiceOrderHeadService : IServiceOrderHeadService
	{
		private readonly IRepositoryWithTypedId<ServiceOrderHead, Guid> serviceOrderHeadRepository;

		public ServiceOrderHeadService(IRepositoryWithTypedId<ServiceOrderHead, Guid> serviceOrderHeadRepository)
		{
			this.serviceOrderHeadRepository = serviceOrderHeadRepository;
		}

		public virtual IEnumerable<string> GetUsedCommissioningStatuses()
		{
			return serviceOrderHeadRepository.GetAll().Select(c => c.CommissioningStatusKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedInvoicingTypes()
		{
			return serviceOrderHeadRepository.GetAll().Select(c => c.InvoicingTypeKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedServiceOrderInvoiceReasons()
		{
			return serviceOrderHeadRepository.GetAll().Select(c => c.InvoiceReasonKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedServiceOrderNoInvoiceReasons()
		{
			return serviceOrderHeadRepository.GetAll().Select(c => c.NoInvoiceReasonKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedServiceOrderTypes()
		{
			return serviceOrderHeadRepository.GetAll().Select(c => c.TypeKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedServicePriorities()
		{
			return serviceOrderHeadRepository.GetAll().Select(c => c.PriorityKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedSkills()
		{
			return serviceOrderHeadRepository.GetAll().SelectMany(s => s.RequiredSkillKeys).Distinct();
		}
		public virtual IEnumerable<string> GetUsedAssets()
		{
			return serviceOrderHeadRepository.GetAll().SelectMany(s => s.RequiredAssetKeys).Distinct();
		}

		public virtual IEnumerable<string> GetUsedErrorCodes()
		{
			return serviceOrderHeadRepository.GetAll().Select(c => c.ErrorCodeKey).Distinct();
		}
	}
}
