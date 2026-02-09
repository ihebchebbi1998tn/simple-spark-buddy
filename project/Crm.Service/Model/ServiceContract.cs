namespace Crm.Service.Model
{
	using System;
	using System.Collections.Generic;

	using Crm.Library.BaseModel.Attributes;
	using Crm.Model;
	using Crm.Model.Interfaces;
	using Crm.Service.Model.Lookup;
	using Crm.Service.Model.Relationships;

	using Main.Model.Lookups;

	public class ServiceContract : Contact, IEntityWithTags
	{
		public virtual string ExternalReference { get; set; }
		public virtual string ContractNo { get; set; }
		public override string Name
		{
			get { return ContractNo; }
			set { ContractNo = value; }
		}
		public virtual Company ParentCompany { get; set; }
		public virtual DateTime? ValidFrom { get; set; }
		public virtual DateTime? ValidTo { get; set; }
		public virtual string InvoiceSpecialConditions { get; set; }
		public virtual string InternalInvoiceInformation { get; set; }
		public virtual decimal? Price { get; set; }
		public virtual DateTime? PriceModificationDate { get; set; }
		public virtual DateTime? NoPaymentsUntil { get; set; }
		public virtual decimal? IncreasedPrice { get; set; }
		public virtual decimal? IncreaseByPercent { get; set; }
		public virtual DateTime? PriceGuaranteedUntil { get; set; }
		public virtual short? FirstAnswerValue { get; set; }
		public virtual short? ServiceCompletedValue { get; set; }
		public virtual decimal? ServiceProvisionValue { get; set; }
		public virtual decimal? SparePartsValue { get; set; }
		public virtual DateTime? InvoicedUntil { get; set; }
		public virtual string LastInvoiceNo { get; set; }

		public virtual Guid? InvoiceRecipientId { get; set; }
		public virtual Company InvoiceRecipient { get; set; }
		public virtual Guid? InvoiceAddressKey { get; set; }
		public virtual Address InvoiceAddress { get; set; }
		public virtual Guid? PayerId { get; set; }
		public virtual Company Payer { get; set; }
		public virtual Guid? PayerAddressId { get; set; }
		public virtual Address PayerAddress { get; set; }
		public virtual Guid? ServiceObjectId { get; set; }
		public virtual ServiceObject ServiceObject { get; set; }

		public virtual string ContractTypeKey { get; set; }
		public virtual ServiceContractType ContractType
		{
			get { return ContractTypeKey != null ? LookupManager.Get<ServiceContractType>(ContractTypeKey) : null; }
		}
		public virtual string StatusKey { get; set; }
		public virtual ServiceContractStatus Status
		{
			get { return StatusKey != null ? LookupManager.Get<ServiceContractStatus>(StatusKey) : null; }
		}
		public virtual string FirstAnswerUnitKey { get; set; }
		public virtual TimeUnit FirstAnswerUnit
		{
			get { return FirstAnswerUnitKey != null ? LookupManager.Get<TimeUnit>(FirstAnswerUnitKey) : null; }
		}
		public virtual string ServiceCompletedUnitKey { get; set; }
		public virtual TimeUnit ServiceCompletedUnit
		{
			get { return ServiceCompletedUnitKey != null ? LookupManager.Get<TimeUnit>(ServiceCompletedUnitKey) : null; }
		}
		public virtual string ServiceProvisionUnitKey { get; set; }
		public virtual TimeUnit ServiceProvisionUnit
		{
			get { return ServiceCompletedUnitKey != null ? LookupManager.Get<TimeUnit>(ServiceProvisionUnitKey) : null; }
		}
		public virtual string ServiceProvisionPerTimeSpanUnitKey { get; set; }
		public virtual SparePartsBudgetTimeSpanUnit ServiceProvisionPerTimeSpanUnit
		{
			get { return ServiceProvisionPerTimeSpanUnitKey != null ? LookupManager.Get<SparePartsBudgetTimeSpanUnit>(ServiceProvisionPerTimeSpanUnitKey) : null; }
		}
		public virtual string SparePartsUnitKey { get; set; }
		public virtual Currency SparePartsUnit
		{
			get { return SparePartsUnitKey != null ? LookupManager.Get<Currency>(SparePartsUnitKey) : null; }
		}
		public virtual string SparePartsPerTimeSpanUnitKey { get; set; }
		public virtual SparePartsBudgetTimeSpanUnit SparePartsPerTimeSpanUnit
		{
			get { return SparePartsPerTimeSpanUnitKey != null ? LookupManager.Get<SparePartsBudgetTimeSpanUnit>(SparePartsPerTimeSpanUnitKey) : null; }
		}
		public virtual string SparePartsBudgetInvoiceTypeKey { get; set; }
		public virtual SparePartsBudgetInvoiceType SparePartsBudgetInvoiceType
		{
			get { return SparePartsBudgetInvoiceTypeKey != null ? LookupManager.Get<SparePartsBudgetInvoiceType>(SparePartsBudgetInvoiceTypeKey) : null; }
		}
		public virtual string LimitTypeKey { get; set; }
		public virtual ServiceContractLimitType LimitType
		{
			get { return LimitTypeKey != null ? LookupManager.Get<ServiceContractLimitType>(LimitTypeKey) : null; }
		}
		public virtual string PaymentTypeKey { get; set; }
		public virtual PaymentType PaymentType
		{
			get { return PaymentTypeKey != null ? LookupManager.Get<PaymentType>(PaymentTypeKey) : null; }
		}
		public virtual string PaymentIntervalKey { get; set; }
		public virtual PaymentInterval PaymentInterval
		{
			get { return PaymentIntervalKey != null ? LookupManager.Get<PaymentInterval>(PaymentIntervalKey) : null; }
		}
		public virtual string PaymentConditionKey { get; set; }
		public virtual PaymentCondition PaymentCondition
		{
			get { return PaymentConditionKey != null ? LookupManager.Get<PaymentCondition>(PaymentConditionKey) : null; }
		}
		public virtual string PriceCurrencyKey { get; set; }
		public virtual Currency PriceCurrency
		{
			get { return PriceCurrencyKey != null ? LookupManager.Get<Currency>(PriceCurrencyKey) : null; }
		}
		public virtual string IncreasedPriceCurrencyKey { get; set; }
		public virtual Currency IncreasedPriceCurrency
		{
			get { return IncreasedPriceCurrencyKey != null ? LookupManager.Get<Currency>(IncreasedPriceCurrencyKey) : null; }
		}

		public virtual ICollection<ServiceContractInstallationRelationship> Installations { get; set; }
		public virtual ICollection<MaintenancePlan> MaintenancePlans { get; set; }
		public virtual ICollection<ServiceContractAddressRelationship> AddressRelationships { get; set; }

		[Database(Ignore = true)]
		public virtual bool HasExpired {
			get
			{
				var today = DateTime.UtcNow.Date;
				if (!ValidFrom.HasValue && ValidTo.HasValue && today > ValidTo.Value)
				{
					return true;
				}
				if (ValidFrom.HasValue && ValidTo.HasValue && ValidFrom.Value <= today && ValidTo.Value < today)
				{
					return true;
				}
				return false;
			}
		}

		[Database(Ignore = true)]
		public virtual bool CanSetToActive
		{
			get
			{
				var today = DateTime.UtcNow.Date;
				if (!ValidFrom.HasValue && !ValidTo.HasValue)
				{
					return true;
				}
				if (!ValidFrom.HasValue && today <= ValidTo)
				{
					return true;
				}
				if (ValidFrom.HasValue && !ValidTo.HasValue && ValidFrom <= today)
				{
					return true;
				}
				if (ValidFrom.HasValue && ValidTo.HasValue && ValidFrom <= today && today <= ValidTo)
				{
					return true;
				}
				return false;
			}
		}

		public ServiceContract()
		{
			Installations = new List<ServiceContractInstallationRelationship>();
			MaintenancePlans = new List<MaintenancePlan>();
			AddressRelationships = new List<ServiceContractAddressRelationship>();
		}
	}
}
