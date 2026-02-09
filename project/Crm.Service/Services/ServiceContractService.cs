namespace Crm.Service.Services
{
  using System;
  using System.Collections.Generic;
	using System.Linq;

  using Crm.Library.Data.Domain.DataInterfaces;
  using Crm.Service.Model;
  using Crm.Service.Model.Relationships;
  using Crm.Service.Services.Interfaces;

	public class ServiceContractService : IServiceContractService
	{
		// Members
		private readonly IRepositoryWithTypedId<ServiceContract, Guid> serviceContractRepository;
		private readonly IRepositoryWithTypedId<ServiceContractAddressRelationship, Guid> serviceContractAddressRelationshipRepository;


		public virtual IEnumerable<string> GetUsedServiceContractLimitTypes()
		{
			return serviceContractRepository.GetAll().Select(c => c.LimitTypeKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedServiceContractStatuses()
		{
			return serviceContractRepository.GetAll().Select(c => c.StatusKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedServiceContractTypes()
		{
			return serviceContractRepository.GetAll().Select(c => c.ContractTypeKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedSparePartsBudgetInvoiceTypes()
		{
			return serviceContractRepository.GetAll().Select(c => c.SparePartsBudgetInvoiceTypeKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedSparePartsBudgetTimeSpanUnits()
		{
			return serviceContractRepository.GetAll().Select(c => c.ServiceProvisionPerTimeSpanUnitKey).Distinct().ToList()
				.Union(serviceContractRepository.GetAll().Select(c => c.SparePartsPerTimeSpanUnitKey).Distinct().ToList());
		}

		public virtual IEnumerable<string> GetUsedPaymentIntervals()
		{
			return serviceContractRepository.GetAll().Select(c => c.PaymentIntervalKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedPaymentTypes()
		{
			return serviceContractRepository.GetAll().Select(c => c.PaymentTypeKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedServiceContractAddressRelationshipTypes()
		{
			return serviceContractAddressRelationshipRepository.GetAll().Select(c => c.RelationshipTypeKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedTimeUnits()
		{
			return serviceContractRepository.GetAll().Select(c => c.ServiceCompletedUnitKey).Distinct().ToList()
				.Union(serviceContractRepository.GetAll().Select(c => c.ServiceProvisionUnitKey).Distinct().ToList());
		}

		public virtual IEnumerable<string> GetUsedCurrencies()
		{
			return serviceContractRepository.GetAll().Select(c => c.IncreasedPriceCurrencyKey).Distinct().ToList()
				.Union(serviceContractRepository.GetAll().Select(c => c.PriceCurrencyKey).Distinct().ToList());
		}

		// Constructor
		public ServiceContractService(IRepositoryWithTypedId<ServiceContract, Guid> serviceContractRepository,
			IRepositoryWithTypedId<ServiceContractAddressRelationship, Guid> serviceContractAddressRelationshipRepository)
		{
			this.serviceContractRepository = serviceContractRepository;
			this.serviceContractAddressRelationshipRepository = serviceContractAddressRelationshipRepository;
		}
	}
}
