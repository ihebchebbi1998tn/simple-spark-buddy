namespace Crm.Service.Rest.Model
{
	using System;
	using System.Collections.Generic;

	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;
	using Crm.Library.Rest.Model;
	using Crm.Rest.Model;
	using Crm.Service.Model;

	using Main.Rest.Model;

	using AddressRest = Crm.Rest.Model.AddressRest;
	using DocumentAttributeRest = Crm.Rest.Model.DocumentAttributeRest;
	using UserRest = Main.Rest.Model.UserRest;

	[RestTypeFor(DomainType = typeof(ServiceOrderHead))]
	public class ServiceOrderHeadRest : ContactRest
	{
		public bool IsTemplate { get; set; }
		public string OrderNo { get; set; }
		public DateTime? Planned { get; set; }
		public TimeSpan? PlannedTime { get; set; }
		public DateTime? PrematureDate { get; set; }
		public TimeSpan? PrematureTime { get; set; }
		public bool PlannedDateFix { get; set; }
		public DateTime? Deadline { get; set; }
		public double? Latitude { get; set; }
		public double? Longitude { get; set; }
		public bool IsGeocoded { get; set; }
		public string TypeKey { get; set; }
		[NavigationProperty(nameof(UserGroupKey))] public UsergroupRest UserGroup { get; set; }
		public Guid? UserGroupKey { get; set; }
		public string StatusKey { get; set; }
		public string PriorityKey { get; set; }
		public bool IsCostLumpSum { get; set; }
		public bool IsMaterialLumpSum { get; set; }
		public bool IsTimeLumpSum { get; set; }
		public string InvoicingTypeKey { get; set; }
		public string NoInvoiceReasonKey { get; set; }
		public List<string> ReportRecipients { get; set; }
		public Guid? MaintenancePlanningRun { get; set; }
		public string ErrorMessage { get; set; }
		public string ErrorCodeKey { get; set; }

		public DateTime? Reported { get; set; }
		public DateTime? Closed { get; set; }
		public string PurchaseOrderNo { get; set; }
		public DateTime? PurchaseDate { get; set; }
		public string CommissionNo { get; set; }
		public string CommissioningStatusKey { get; set; }
		public string Name1 { get; set; }
		public string Name2 { get; set; }
		public string Name3 { get; set; }
		public string Street { get; set; }
		public string City { get; set; }
		public string ZipCode { get; set; }
		public string CountryKey { get; set; }
		public string RegionKey { get; set; }
		public string ServiceLocationPhone { get; set; }
		public string ServiceLocationMobile { get; set; }
		public string ServiceLocationFax { get; set; }
		public string ServiceLocationEmail { get; set; }
		public string ServiceLocationResponsiblePerson { get; set; }
		public Guid? PreferredTechnicianUsergroupKey { get; set; }
		[NavigationProperty(nameof(PreferredTechnicianUsergroupKey))] public UsergroupRest PreferredTechnicianUsergroupObject { get; set; }
		public string CloseReason { get; set; }
		public Guid? MaintenancePlanId { get; set; }
		[RestrictedField] public string[] RequiredSkillKeys { get; set; }
		[RestrictedField] public string[] RequiredAssetKeys { get; set; }
		public string PreferredTechnician { get; set; }
		[NavigationProperty(nameof(PreferredTechnician))] public UserRest PreferredTechnicianUser { get; set; }
		[NavigationProperty(nameof(ResponsibleUser))] public UserRest ResponsibleUserUser { get; set; }
		public Guid? StationKey { get; set; }
		[NavigationProperty(nameof(StationKey))] public StationRest Station { get; set; }
		public Guid? CustomerContactId { get; set; }
		[NavigationProperty(nameof(CustomerContactId))] public CompanyRest Company { get; set; }
		public Guid? InitiatorId { get; set; }
		[NavigationProperty(nameof(InitiatorId))] public CompanyRest Initiator { get; set; }
		public Guid? PayerId { get; set; }
		[NavigationProperty(nameof(PayerId))] public CompanyRest Payer { get; set; }
		public Guid? PayerAddressId { get; set; }
		[NavigationProperty(nameof(PayerAddressId))] public AddressRest PayerAddress { get; set; }
		public Guid? InvoiceRecipientId { get; set; }
		[NavigationProperty(nameof(InvoiceRecipientId))] public CompanyRest InvoiceRecipient { get; set; }
		public Guid? InvoiceRecipientAddressId { get; set; }
		[NavigationProperty(nameof(InvoiceRecipientAddressId))] public AddressRest InvoiceRecipientAddress { get; set; }
		public Guid? InitiatorPersonId { get; set; }
		[NavigationProperty(nameof(InitiatorPersonId))] public PersonRest InitiatorPerson { get; set; }
		public Guid? InstallationId { get; set; }
		[NavigationProperty(nameof(InstallationId))] public InstallationRest Installation { get; set; }
		public Guid? ServiceObjectId { get; set; }
		[NavigationProperty(nameof(ServiceObjectId))] public ServiceObjectRest ServiceObject { get; set; }
		[NavigationProperty(nameof(ServiceOrderTemplateId))] public ServiceOrderHeadRest ServiceOrderTemplate { get; set; }
		public Guid? ServiceOrderTemplateId { get; set; }
		public Guid? ServiceCaseKey { get; set; }
		[NotReceived] public string ServiceCaseNo { get; set; }
		[NavigationProperty(nameof(ServiceCaseKey))] public ServiceCaseRest ServiceCase { get; set; }
		public Guid? ServiceContractId { get; set; }
		[NavigationProperty(nameof(ServiceContractId))] public ServiceContractRest ServiceContract { get; set; }

		[NavigationProperty(nameof(ServiceOrderMaterialRest.OrderId), nameof(ServiceOrderMaterialRest.ServiceOrderHead))]
		public ServiceOrderMaterialRest[] ServiceOrderMaterials { get; set; }

		[NavigationProperty(nameof(ServiceOrderStatisticsKeyRest.ServiceOrderId), nameof(ServiceOrderStatisticsKeyRest.ServiceOrder))]
		public ServiceOrderStatisticsKeyRest[] ServiceOrderStatisticsKeys { get; set; }

		[NavigationProperty(nameof(ServiceOrderTimeRest.OrderId))]
		public ServiceOrderTimeRest[] ServiceOrderTimes { get; set; }

		[NavigationProperty(nameof(ServiceOrderExpensePostingRest.OrderId), nameof(ServiceOrderExpensePostingRest.ServiceOrder))]
		public ServiceOrderExpensePostingRest[] ServiceOrderExpensePostings { get; set; }

		[NavigationProperty(nameof(ServiceOrderErrorTypeRest.OrderId), nameof(ServiceOrderErrorTypeRest.ServiceOrder))]
		public ServiceOrderErrorTypeRest[] ServiceOrderErrorTypes { get; set; }

		[NavigationProperty(nameof(ServiceOrderTimePostingRest.OrderId), nameof(ServiceOrderTimePostingRest.ServiceOrder))]
		public ServiceOrderTimePostingRest[] ServiceOrderTimePostings { get; set; }

		[NavigationProperty(nameof(ServiceOrderDispatchRest.OrderId), nameof(ServiceOrderDispatchRest.ServiceOrder))]
		public ServiceOrderDispatchRest[] Dispatches { get; set; }

		[NavigationProperty(nameof(DocumentAttributeRest.ReferenceKey))]
		public DocumentAttributeRest[] DocumentAttributes { get; set; }

		[NotReceived] public override string Name { get; set; }

		[NavigationProperty(nameof(TagRest.ContactKey))]
		public TagRest[] Tags { get; set; }

		public string CurrencyKey { get; set; }
	}
}
