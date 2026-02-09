namespace Crm.Service.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.Api.Attributes;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Model;
	using Crm.Model;
	using Crm.Model.Interfaces;
	using Crm.Service.Model.Lookup;

	using Main.Model.Lookups;

	public class ServiceOrderHead : Contact, IEntityWithTags, IEntityWithGeocode
	{
		public virtual bool IsTemplate { get; set; }
		public virtual string OrderNo { get; set; }
		public override string Name
		{
			get { return OrderNo; }
		}
		public virtual Guid? InstallationId { get; set; }
		public virtual string ErrorCodeKey { get; set; }
		public virtual ErrorCode ErrorCode
		{
			get { return ErrorCodeKey != null ? LookupManager.Get<ErrorCode>(ErrorCodeKey) : null; }
		}
		public virtual string ErrorMessage { get; set; }
		public virtual string Component { get; set; }
		public virtual Guid? MaintenancePlanId { get; set; }
		public virtual Guid? MaintenancePlanningRun { get; set; }
		[GroupProperty] public virtual DateTime? Deadline { get; set; }
		[GroupProperty] public virtual DateTime? Reported { get; set; }
		[GroupProperty] public virtual DateTime? Planned { get; set; }
		public virtual TimeSpan? PlannedTime { get; set; }
		public virtual DateTime? PrematureDate { get; set; }
		public virtual TimeSpan? PrematureTime { get; set; }
		public virtual bool PlannedDateFix { get; set; }
		public virtual DateTime? Completed { get; set; }
		public virtual DateTime? Closed { get; set; }
		public virtual string CostingUnit { get; set; }
		public virtual string FromWarehouse { get; set; }
		public virtual string FromLocationNo { get; set; }
		public virtual string ToWarehouse { get; set; }
		public virtual string ToLocationNo { get; set; }
		public virtual string PredecessorOrderNo { get; set; }
		public virtual string PurchaseOrderNo { get; set; }
		public virtual DateTime? PurchaseDate { get; set; }
		public virtual string CommissionNo { get; set; }
		public virtual DateTime? LegacyTransferDate { get; set; }
		public virtual string CommissioningStatusKey { get; set; }
		public virtual CommissioningStatus CommissioningStatus
		{
			get { return CommissioningStatusKey != null ? LookupManager.Get<CommissioningStatus>(CommissioningStatusKey) : null; }
		}
		public virtual bool LegacyTransferFlag { get; set; }
		public virtual bool HasDispatches { get; set; }
		public virtual bool HasNotes { get; set; }
		public virtual int GeocodingRetryCounter { get; set; }
		public virtual string Name1 { get; set; }
		public virtual string Name2 { get; set; }
		public virtual string Name3 { get; set; }
		public virtual string Street { get; set; }
		[GroupProperty] public virtual string City { get; set; }
		[GroupProperty] public virtual string ZipCode { get; set; }
		[GroupProperty]  public virtual string CountryKey { get; set; }
		public virtual string RegionKey { get; set; }
		[GroupProperty] public virtual Region Region
		{
			get { return RegionKey != null ? LookupManager.Get<Region>(RegionKey) : null; }
		}
		public virtual string ServiceLocationPhone { get; set; }
		public virtual string ServiceLocationMobile { get; set; }
		public virtual string ServiceLocationFax { get; set; }
		public virtual string ServiceLocationEmail { get; set; }
		public virtual string ServiceLocationResponsiblePerson { get; set; }
		public virtual string PreferredTechnicianUsername { get; set; }
		public virtual User PreferredTechnician { get; set; }
		public virtual Guid? PreferredTechnicianUsergroupKey { get; set; }
		public virtual Usergroup PreferredTechnicianUsergroup { get; set; }
		public virtual string InvoiceRemark { get; set; }
		public virtual string ReportSendingError { get; set; }
		public virtual bool ReportSent { get; set; }
		public virtual bool ReportSaved { get; set; }
		public virtual string ReportSavingError { get; set; }

		public virtual string StatusKey { get; set; }
		[GroupProperty] public virtual ServiceOrderStatus Status
		{
			get { return LookupManager.Get<ServiceOrderStatus>(StatusKey) ?? LookupManager.Get<ServiceOrderStatus>(ServiceOrderStatus.NewKey); }
		}
		public virtual string TypeKey { get; set; }
		[GroupProperty] public virtual ServiceOrderType Type
		{
			get { return TypeKey != null ? LookupManager.Get<ServiceOrderType>(TypeKey) : null; }
		}
		public virtual string PriorityKey { get; set; }
		[GroupProperty] public virtual ServicePriority Priority
		{
			get { return PriorityKey != null ? LookupManager.Get<ServicePriority>(PriorityKey) : null; }
		}
		public virtual bool IsCostLumpSum { get; set; }
		public virtual bool IsMaterialLumpSum { get; set; }
		public virtual bool IsTimeLumpSum { get; set; }
		public virtual string InvoicingTypeKey { get; set; }                                    
		public virtual InvoicingType InvoicingType
		{
			get { return InvoicingTypeKey != null ? LookupManager.Get<InvoicingType>(InvoicingTypeKey) : null; }
		}
		public virtual string NoInvoiceReasonKey { get; set; }
		public virtual ServiceOrderNoInvoiceReason NoInvoiceReason
		{
			get { return NoInvoiceReasonKey != null ? LookupManager.Get<ServiceOrderNoInvoiceReason>(NoInvoiceReasonKey) : null; }
		}
		public virtual string InvoiceReasonKey { get; set; }
		public virtual ServiceOrderInvoiceReason InvoiceReason
		{
			get { return InvoiceReasonKey != null ? LookupManager.Get<ServiceOrderInvoiceReason>(InvoiceReasonKey) : null; }
		}

		[GroupProperty] public virtual Installation AffectedInstallation { get; set; }
		public virtual Guid? CustomerContactId { get; set; }
		[GroupProperty]  public virtual Company CustomerContact { get; set; }
		public virtual Guid? InitiatorId { get; set; }
		public virtual Company Initiator { get; set; }
		public virtual Guid? InitiatorPersonId { get; set; }
		public virtual Person InitiatorPerson { get; set; }
		public virtual Guid? PayerId { get; set; }
		public virtual Company Payer { get; set; }
		public virtual Guid? PayerAddressId { get; set; }
		public virtual Address PayerAddress { get; set; }
		public virtual Guid? InvoiceRecipientId { get; set; }
		public virtual Company InvoiceRecipient { get; set; }
		public virtual Guid? InvoiceRecipientAddressId { get; set; }
		public virtual Address InvoiceRecipientAddress { get; set; }
		public virtual Guid? UserGroupKey { get; set; }
		public virtual Usergroup UserGroup { get; set; }
		public virtual Guid? ServiceCaseKey { get; set; }
		public virtual ServiceCase ServiceCase { get; set; }
		public virtual Guid? ServiceContractId { get; set; }
		[GroupProperty]  public virtual ServiceContract ServiceContract { get; set; }
		public virtual Guid? ServiceObjectId { get; set; }
		[GroupProperty]  public virtual ServiceObject ServiceObject { get; set; }
		public virtual Guid? ServiceOrderTemplateId { get; set; }
		public virtual ServiceOrderHead ServiceOrderTemplate { get; set; }
		public virtual Guid? StationKey { get; set; }
		[GroupProperty] public virtual Station Station { get; set; }

		public virtual List<string> ReportRecipients { get; set; }
		public virtual ICollection<ServiceOrderDispatch> Dispatches { get; set; }
		public virtual ICollection<ServiceOrderStatisticsKey> ServiceOrderStatisticsKeys { get; set; }
		public virtual ICollection<ServiceOrderTime> ServiceOrderTimes { get; set; }
		public virtual ICollection<ServiceOrderMaterial> ServiceOrderMaterials { get; set; }
		public virtual ICollection<ServiceOrderTimePosting> ServiceOrderTimePostings { get; set; }
		public virtual ICollection<ServiceOrderExpensePosting> ServiceOrderExpensePostings { get; set; }
		public virtual ICollection<ServiceOrderErrorType> ServiceOrderErrorTypes { get; set; }

		public virtual ICollection<string> RequiredSkillKeys { get; set; }
		public virtual List<Skill> RequiredSkills
		{
			get { return RequiredSkillKeys == null ? null : RequiredSkillKeys.Select(key => LookupManager.Get<Skill>(key)).ToList(); }
		}
		public virtual ICollection<string> RequiredAssetKeys { get; set; }
		public virtual List<Asset> RequiredAssets
		{
			get { return RequiredAssetKeys == null ? null : RequiredAssetKeys.Select(key => LookupManager.Get<Asset>(key)).ToList(); }
		}

		public virtual double? Latitude { get; set; }
		public virtual double? Longitude { get; set; }
		public virtual bool IsGeocoded { get; set; }
		public virtual string CloseReason { get; set; }
		[GroupProperty]  public virtual string CurrencyKey { get; set; }

		// Constructor
		public ServiceOrderHead()
		{
			StatusKey = ServiceOrderStatus.NewKey;
			Dispatches = new List<ServiceOrderDispatch>();
			ServiceOrderStatisticsKeys = new List<ServiceOrderStatisticsKey>();
			ServiceOrderTimes = new List<ServiceOrderTime>();
			ServiceOrderMaterials = new List<ServiceOrderMaterial>();
			ServiceOrderTimePostings = new List<ServiceOrderTimePosting>();
			ServiceOrderExpensePostings = new List<ServiceOrderExpensePosting>();
			ServiceOrderErrorTypes = new List<ServiceOrderErrorType>();
			RequiredSkillKeys = new List<string>();
			RequiredAssetKeys = new List<string>();
		}
	}
}
