namespace Crm.Service
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Article.Model.Lookups;
	using Crm.Library.Globalization.Lookup;
	using Crm.PerDiem.Model.Lookups;
	using Crm.PerDiem.Services.Interfaces;
	using Crm.Service.Model.Lookup;
	using Crm.Service.Services.Interfaces;

	using Main.Model.Lookups;

	public class ServiceUsedLookupsProvider : IUsedLookupsProvider
	{
		private readonly IExpenseService expenseService;
		private readonly IInstallationService installationService;
		private readonly IServiceCaseService serviceCaseService;
		private readonly IServiceContractService serviceContractService;
		private readonly IServiceObjectService serviceObjectService;
		private readonly IServiceOrderDispatchService serviceOrderDispatchService;
		private readonly IServiceOrderHeadService serviceOrderHeadService;
		private readonly IServiceOrderMaterialService serviceOrderMaterialService;
		private readonly IServiceOrderTimeService serviceOrderTimeService;
		private readonly ITimeEntryService timeEntryService;
		private readonly IMaintenancePlanService maintenancePlanService;
		private readonly IReplenishmentOrderService replenishmentOrderService;
		public ServiceUsedLookupsProvider(IExpenseService expenseService,
			IInstallationService installationService,
			IServiceCaseService serviceCaseService,
			IServiceContractService serviceContractService,
			IServiceObjectService serviceObjectService,
			IServiceOrderDispatchService serviceOrderDispatchService,
			IServiceOrderHeadService serviceOrderHeadService,
			IServiceOrderMaterialService serviceOrderMaterialService,
			IServiceOrderTimeService serviceOrderTimeService,
			ITimeEntryService timeEntryService,
			IMaintenancePlanService maintenancePlanService,
			IReplenishmentOrderService replenishmentOrderService)
		{
			this.expenseService = expenseService;
			this.installationService = installationService;
			this.serviceCaseService = serviceCaseService;
			this.serviceContractService = serviceContractService;
			this.serviceObjectService = serviceObjectService;
			this.serviceOrderDispatchService = serviceOrderDispatchService;
			this.serviceOrderHeadService = serviceOrderHeadService;
			this.serviceOrderMaterialService = serviceOrderMaterialService;
			this.serviceOrderTimeService = serviceOrderTimeService;
			this.timeEntryService = timeEntryService;
			this.maintenancePlanService = maintenancePlanService;
			this.replenishmentOrderService = replenishmentOrderService;
		}
		public virtual IEnumerable<object> GetUsedLookupKeys(Type lookupType)
		{
			//Expense
			if (lookupType == typeof(CostCenter))
			{
				return expenseService.GetUsedCostCenters()
					.Union(timeEntryService.GetUsedCostCenters());
			}

			//Installation
			if (lookupType == typeof(InstallationHeadStatus))
			{
				return installationService.GetUsedInstallationHeadStatuses();
			}

			if (lookupType == typeof(InstallationType))
			{
				return installationService.GetUsedInstallationTypes();
			}

			if (lookupType == typeof(Manufacturer))
			{
				return installationService.GetUsedManufacturers();
			}

			if (lookupType == typeof(InstallationAddressRelationshipType))
			{
				return installationService.GetUsedInstallationAddressRelationshipTypes();
			}

			//ServiceCase
			if (lookupType == typeof(ErrorCode))
			{
				return serviceCaseService.GetUsedErrorCodes()
					.Union(serviceOrderHeadService.GetUsedErrorCodes());
			}

			if (lookupType == typeof(ServiceCaseCategory))
			{
				return serviceCaseService.GetUsedServiceCaseCategories();
			}

			if (lookupType == typeof(ServiceCaseStatus))
			{
				return serviceCaseService.GetUsedServiceCaseStatuses().Cast<object>();
			}

			if (lookupType == typeof(ServicePriority))
			{
				return serviceCaseService.GetUsedServicePriorities()
					.Union(serviceOrderHeadService.GetUsedServicePriorities());
			}

			//ServiceContract
			if (lookupType == typeof(ServiceContractLimitType))
			{
				return serviceContractService.GetUsedServiceContractLimitTypes();
			}

			if (lookupType == typeof(ServiceContractStatus))
			{
				return serviceContractService.GetUsedServiceContractStatuses();
			}

			if (lookupType == typeof(ServiceContractType))
			{
				return serviceContractService.GetUsedServiceContractTypes();
			}

			if (lookupType == typeof(SparePartsBudgetInvoiceType))
			{
				return serviceContractService.GetUsedSparePartsBudgetInvoiceTypes();
			}

			if (lookupType == typeof(SparePartsBudgetTimeSpanUnit))
			{
				return serviceContractService.GetUsedSparePartsBudgetTimeSpanUnits();
			}

			if (lookupType == typeof(PaymentInterval))
			{
				return serviceContractService.GetUsedPaymentIntervals();
			}

			if (lookupType == typeof(PaymentType))
			{
				return serviceContractService.GetUsedPaymentTypes();
			}

			if (lookupType == typeof(ServiceContractAddressRelationshipType))
			{
				return serviceContractService.GetUsedServiceContractAddressRelationshipTypes();
			}

			//ServiceObject
			if(lookupType == typeof(ServiceObjectCategory))
			{
				return serviceObjectService.GetUsedServiceObjectCategories();
			}

			//ServiceOrderDispatch
			if (lookupType == typeof(Component))
			{
				return serviceOrderDispatchService.GetUsedComponents();
			}

			if (lookupType == typeof(ServiceOrderDispatchRejectReason))
			{
				return serviceOrderDispatchService.GetUsedServiceOrderDispatchRejectReasons();
			}

			//ServiceOrderHead
			if (lookupType == typeof(CommissioningStatus))
			{
				return serviceOrderHeadService.GetUsedCommissioningStatuses()
					.Union(serviceOrderMaterialService.GetUsedCommissioningStatuses());
			}

			if (lookupType == typeof(InvoicingType))
			{
				return serviceOrderHeadService.GetUsedInvoicingTypes();
			}

			if (lookupType == typeof(ServiceOrderInvoiceReason))
			{
				return serviceOrderHeadService.GetUsedServiceOrderInvoiceReasons();
			}

			if (lookupType == typeof(ServiceOrderNoInvoiceReason))
			{
				return serviceOrderHeadService.GetUsedServiceOrderNoInvoiceReasons();
			}

			if (lookupType == typeof(ServiceOrderType))
			{
				return serviceOrderHeadService.GetUsedServiceOrderTypes();
			}

			if (lookupType == typeof(Skill))
			{
				return serviceOrderHeadService.GetUsedSkills();
			}

			//ServiceOrderMaterial
			if (lookupType == typeof(NoPreviousSerialNoReason))
			{
				return serviceOrderMaterialService.GetUsedNoPreviousSerialNoReasons();
			}

			//ServiceOrderTime
			if (lookupType == typeof(NoCausingItemPreviousSerialNoReason))
			{
				return serviceOrderTimeService.GetUsedNoCausingItemPreviousSerialNoReasons();
			}

			if (lookupType == typeof(NoCausingItemSerialNoReason))
			{
				return serviceOrderTimeService.GetUsedNoCausingItemSerialNoReasons();
			}

			if (lookupType == typeof(ServiceOrderTimeCategory))
			{
				return serviceOrderTimeService.GetUsedServiceOrderTimeCategories();
			}

			if (lookupType == typeof(ServiceOrderTimeLocation))
			{
				return serviceOrderTimeService.GetUsedServiceOrderTimeLocations();
			}

			if (lookupType == typeof(ServiceOrderTimePriority))
			{
				return serviceOrderTimeService.GetUsedServiceOrderTimePriorities();
			}

			//TimeEntry
			if (lookupType == typeof(TimeEntryType))
			{
				return timeEntryService.GetUsedTimeEntryTypes();
			}

			if (lookupType == typeof(TravelType))
			{
				return timeEntryService.GetUsedTravelTypes();
			}

			if (lookupType == typeof(DriverType))
			{
				return timeEntryService.GetUsedDriverTypes();
			}

			if (lookupType == typeof(DistanceUnit))
			{
				return timeEntryService.GetUsedDistanceUnits();
			}

			if (lookupType == typeof(TimeUnit))
			{
				return maintenancePlanService.GetUsedTimeUnits()
					.Union(serviceContractService.GetUsedTimeUnits());
			}

			if (lookupType == typeof(Currency))
			{
				return serviceContractService.GetUsedCurrencies()
					.Union(expenseService.GetUsedCurrencies());
			}

			if (lookupType == typeof(QuantityUnit))
			{
				return replenishmentOrderService.GetUsedQuantityUnits()
					.Union(serviceOrderMaterialService.GetUsedQuantityUnits())
					.Union(installationService.GetUsedQuantityUnits());
			}

			return new List<object>();
		}
	}
}
