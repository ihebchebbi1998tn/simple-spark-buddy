namespace Crm.Service.Rest.Model
{
	using System;

	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;
	using Crm.Rest.Model;
	using Crm.Service.Model;

	using AddressRest = Crm.Rest.Model.AddressRest;
	using UserRest = Main.Rest.Model.UserRest;

	[RestTypeFor(DomainType = typeof(ServiceContract))]
	public class ServiceContractRest : ContactRest
	{
		public string ExternalReference { get; set; }
		public string ContractNo { get; set; }
		public string FirstAnswerUnitKey { get; set; }
		public short? FirstAnswerValue { get; set; }
		public string InternalInvoiceInformation { get; set; }
		public Guid? InvoiceAddressKey { get; set; }
		public Guid? InvoiceRecipientId { get; set; }
		public string InvoiceSpecialConditions { get; set; }
		public DateTime? NoPaymentsUntil { get; set; }
		public Guid? PayerAddressId { get; set; }
		public Guid? PayerId { get; set; }
		public string PaymentConditionKey { get; set; }
		public string PaymentIntervalKey { get; set; }
		public string PaymentTypeKey { get; set; }
		public decimal? Price { get; set; }
		public string PriceCurrencyKey { get; set; }
		public string ServiceCompletedUnitKey { get; set; }
		public short? ServiceCompletedValue { get; set; }
		public virtual Guid? ServiceObjectId { get; set; }
		public string ServiceProvisionPerTimeSpanUnitKey { get; set; }
		public string ServiceProvisionUnitKey { get; set; }
		public decimal? ServiceProvisionValue { get; set; }
		public string SparePartsBudgetInvoiceTypeKey { get; set; }
		public string SparePartsPerTimeSpanUnitKey { get; set; }
		public string SparePartsUnitKey { get; set; }
		public decimal? SparePartsValue { get; set; }
		public string ContractTypeKey { get; set; }
		public string StatusKey { get; set; }
		public DateTime? ValidFrom { get; set; }
		public DateTime? ValidTo { get; set; }
		public virtual DateTime? PriceGuaranteedUntil { get; set; }
		public DateTime? InvoicedUntil { get; set; }
		public string LastInvoiceNo { get; set; }

		[NavigationProperty(nameof(ServiceContractInstallationRelationshipRest.ParentId), nameof(ServiceContractInstallationRelationshipRest.Parent))]
		public ServiceContractInstallationRelationshipRest[] Installations { get; set; }

		[NavigationProperty(nameof(InvoiceAddressKey))]
		public AddressRest InvoiceAddress { get; set; }

		[NavigationProperty(nameof(InvoiceRecipientId))]
		public CompanyRest InvoiceRecipient { get; set; }

		[NavigationProperty(nameof(MaintenancePlanRest.ServiceContractId), nameof(MaintenancePlanRest.ServiceContract))]
		public MaintenancePlanRest[] MaintenancePlans { get; set; }

		[NavigationProperty(nameof(ParentId))]
		public CompanyRest ParentCompany { get; set; }

		[NavigationProperty(nameof(PayerId))]
		public CompanyRest Payer { get; set; }

		[NavigationProperty(nameof(PayerAddressId))]
		public AddressRest PayerAddress { get; set; }

		[NavigationProperty(nameof(ResponsibleUser))]
		public UserRest ResponsibleUserUser { get; set; }

		[NavigationProperty(nameof(ServiceObjectId))]
		public ServiceObjectRest ServiceObject { get; set; }

		[NavigationProperty(nameof(TagRest.ContactKey))]
		public TagRest[] Tags { get; set; }
	}
}
