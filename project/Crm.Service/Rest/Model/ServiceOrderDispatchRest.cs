namespace Crm.Service.Rest.Model
{
	using System;
	using System.Xml.Serialization;

	using Crm.Library.Api.Attributes;
	using Crm.Library.BaseModel;
	using Crm.Library.Rest;
	using Crm.Library.Rest.Interfaces;
	using Crm.Service.Model;

	using Main.Rest.Model;

	[RestTypeFor(DomainType = typeof(ServiceOrderDispatch))]
	[XmlRoot("service-order-dispatch")]
	public class ServiceOrderDispatchRest : RestEntity, IRestEntityWithExtensionValues
	{
		public virtual SerializableDictionary<string, object> ExtensionValues { get; set; }
		public string DispatchNo { get; set; }
		[XmlElement("id")]
		public Guid Id { get; set; }
		[XmlElement("username")]
		public string Username { get; set; }
		public string TimeZone { get; set; }
		[XmlElement("date")]
		public DateTime Date { get; set; }
		[XmlElement("is-fixed")]
		public bool IsFixed { get; set; }
		[XmlElement("status-key")]
		public string StatusKey { get; set; }
		[XmlElement("remark", IsNullable = true)]
		public string Remark { get; set; }
		public string RequiredOperations { get; set; }
		public string Diagnosis { get; set; }
		[NavigationProperty(nameof(Username))] public UserRest DispatchedUser { get; set; }
		public virtual double? LatitudeOnDispatchStart { get; set; }
		public virtual double? LongitudeOnDispatchStart { get; set; }
		public string SignatureContactName { get; set; }
		public string SignatureJson { get; set; }
		public DateTime? SignatureDate { get; set; }
		public bool SignPrivacyPolicyAccepted { get; set; }
		public string SignatureTechnicianName { get; set; }
		public string SignatureTechnicianJson { get; set; }
		public DateTime? SignatureTechnicianDate { get; set; }
		public string SignatureOriginatorName { get; set; }
		public string SignatureOriginatorJson { get; set; }
		public DateTime? SignatureOriginatorDate { get; set; }
		public bool FollowUpServiceOrder { get; set; }
		public string FollowUpServiceOrderRemark { get; set; }
		public string RejectReasonKey { get; set; }
		public string CancellationReasonKey { get; set; }
		public string RejectRemark { get; set; }
		public string CancellationRemark { get; set; }
		public string ComponentKey { get; set; }
		[XmlElement("order-id")]
		public Guid OrderId { get; set; }
		[RestrictedField]
		[XmlElement("order-no")]
		public string OrderNo { get; set; }
		[NavigationProperty(nameof(OrderId), nameof(ServiceOrderHeadRest.Dispatches))]
		public ServiceOrderHeadRest ServiceOrder { get; set; }
		public DateTime? CloseDate { get; set; }
		public Guid? CurrentServiceOrderTimeId { get; set; }
		public string InfoForTechnician { get; set; }

		[NavigationProperty(nameof(CurrentServiceOrderTimeId))]
		public ServiceOrderTimeRest CurrentServiceOrderTime { get; set; }
		public DateTime EndDate { get; set; }
		public int? NetWorkMinutes { get; set; }

		[NavigationProperty(nameof(ServiceOrderDispatchReportRecipientRest.DispatchId), nameof(ServiceOrderDispatchReportRecipientRest.Dispatch))]
		public ServiceOrderDispatchReportRecipientRest[] ReportRecipients { get; set; }

		[NavigationProperty(nameof(ServiceOrderMaterialRest.DispatchId))]
		public ServiceOrderMaterialRest[] ServiceOrderMaterial { get; set; }

		[NavigationProperty(nameof(ServiceOrderTimeDispatchRest.ServiceOrderDispatchId), nameof(ServiceOrderTimeDispatchRest.ServiceOrderDispatch))]
		public ServiceOrderTimeDispatchRest[] ServiceOrderTimeDispatches { get; set; }

		[NavigationProperty(nameof(ServiceOrderTimePostingRest.DispatchId), nameof(ServiceOrderTimePostingRest.ServiceOrderDispatch))]
		public ServiceOrderTimePostingRest[] ServiceOrderTimePostings { get; set; }

		[NavigationProperty(nameof(ServiceOrderExpensePostingRest.DispatchId), nameof(ServiceOrderExpensePostingRest.ServiceOrderDispatch))]
		public ServiceOrderExpensePostingRest[] ServiceOrderExpensePostings { get; set; }

		[NavigationProperty(nameof(ServiceOrderErrorTypeRest.DispatchId), nameof(ServiceOrderErrorTypeRest.ServiceOrderDispatch))]
		public ServiceOrderErrorTypeRest[] ServiceOrderErrorTypes { get; set; }
	}
}
